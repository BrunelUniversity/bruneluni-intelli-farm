import errno
import json
import os
from dataclasses import dataclass
from datetime import datetime
from enum import Enum
from typing import Protocol, Callable, Any, OrderedDict

from bpy.app import handlers
import pathlib
import sys
import bpy
from mathutils import Vector

RENDER_START_TIME: datetime
FILENAME = f"{pathlib.Path(__file__).parent.resolve()}\\temp\\render.json"


@dataclass
class SceneDataDto:
    samples: int
    max_bounces: int
    diffuse_bounces: int
    glossy_bounces: int
    transparent_max_bounces: int
    transmission_bounces: int
    volume_bounces: int
    start_frame: int
    end_frame: int


class RenderEngineEnum(Enum):
    Cycles = 0,
    BlenderInternal = 1


class TempCommsService(Protocol):
    def read_json(self) -> dict:
        ...

    def write_json(self, data: dict):
        ...


class DateTimeAdapter(Protocol):
    def now_utc(self) -> datetime:
        ...


class SceneAdapter(Protocol):
    def set_render_engine(self, engine: RenderEngineEnum) -> None:
        ...

    def get_scene_data(self) -> SceneDataDto:
        ...

    def set_scene_data(self, data: SceneDataDto) -> None:
        ...

    def save_file(self) -> None:
        ...

    def add_render_handlers(self,
                            init_handler: Callable[[Any], None],
                            complete_handler: Callable[[Any], None]) -> None:
        ...

    def add_iscosphere(self, subdivisions: int, location: Vector) -> list[Vector]:
        '''
        add iscosphere and get real vectors multiplied by world matrix
        '''
        ...

    def cast_ray(self,
                 origin: Vector,
                 direction: Vector,
                 distance: int) -> bool:
        ...

    def delete_current_object(self) -> None:
        ...


class RenderCommands:

    def __init__(self,
                 comms_service: TempCommsService,
                 scene_adapter: SceneAdapter,
                 date_time_adapter: DateTimeAdapter):
        self.__scene_adapter = scene_adapter
        self.__comms_service = comms_service
        self.__date_time_adapter = date_time_adapter

    def get_scene_data(self):
        ...

    def set_scene_data(self):
        ...

    def render_frame(self):
        ...

    def get_scene_and_viewpoint_coverage(self):
        ...


class BlenderSceneAdapter:

    def set_render_engine(self, engine: RenderEngineEnum) -> None:
        if engine == RenderEngineEnum.Cycles:
            bpy.context.scene.render.engine = 'CYCLES'

    def get_scene_data(self) -> SceneDataDto:
        return SceneDataDto(
            samples=bpy.data.scenes[0].cycles.samples,
            max_bounces=bpy.data.scenes[0].cycles.max_bounces,
            diffuse_bounces=bpy.data.scenes[0].cycles.diffuse_bounces,
            glossy_bounces=bpy.data.scenes[0].cycles.glossy_bounces,
            transparent_max_bounces=bpy.data.scenes[0].cycles.transparent_max_bounces,
            transmission_bounces=bpy.data.scenes[0].cycles.transmission_bounces,
            volume_bounces=bpy.data.scenes[0].cycles.volume_bounces,
            start_frame=bpy.data.scenes[0].frame_start,
            end_frame=bpy.data.scenes[0].frame_end
        )

    def set_scene_data(self, data: SceneDataDto) -> None:
        bpy.data.scenes[0].cycles.samples = data.samples
        bpy.data.scenes[0].cycles.max_bounces = data.max_bounces
        bpy.data.scenes[0].cycles.diffuse_bounces = data.diffuse_bounces
        bpy.data.scenes[0].cycles.glossy_bounces = data.glossy_bounces
        bpy.data.scenes[0].cycles.transparent_max_bounces = data.transparent_max_bounces
        bpy.data.scenes[0].cycles.transmission_bounces = data.transmission_bounces
        bpy.data.scenes[0].cycles.volume_bounces = data.volume_bounces
        bpy.data.scenes[0].frame_start = data.start_frame
        bpy.data.scenes[0].frame_end = data.end_frame
        bpy.context.scene.frame_set(data.end_frame)

    def save_file(self) -> None:
        bpy.ops.wm.save_mainfile()

    def add_render_handlers(self,
                            init_handler: Callable[[Any], None],
                            complete_handler: Callable[[Any], None]) -> None:
        handlers.render_init.append(init_handler)
        handlers.render_complete.append(complete_handler)

    def add_iscosphere(self, subdivisions: int, location: Vector) -> list[Vector]:
        '''
        add iscosphere and get real vectors multiplied by world matrix
        '''
        bpy.ops.mesh.primitive_ico_sphere_add(subdivisions=subdivisions,
                                              radius=1,
                                              enter_editmode=False,
                                              align='WORLD',
                                              location=location,
                                              scale=[10, 10, 10])
        active_object = bpy.context.active_object
        return [active_object.matrix_world @ _object.co for _object in active_object.data.vertices]

    def cast_ray(self,
                 origin: Vector,
                 direction: Vector,
                 distance: int) -> bool:
        return bpy.context.scene.ray_cast(view_layer=bpy.context.view_layer,
                                   origin=origin,
                                   direction=direction,
                                   distance=distance)[0]

    def delete_current_object(self) -> None:
        bpy.ops.object.delete(use_global=False, confirm=False)


class FileTempCommsService:
    def read_json(self) -> dict:
        raise NotImplemented

    def write_json(self, data: dict):
        raise NotImplemented


class InBuiltDateTimeAdapter:
    def now_utc(self) -> datetime:
        ...


def render_init(scene):
    global RENDER_START_TIME
    RENDER_START_TIME = datetime.now()


def complete(scene):
    print(os.getcwd())
    f = open(FILENAME, "w")
    totalRenderTime = float((datetime.now() - RENDER_START_TIME).total_seconds())
    f.write(json.dumps({"render_time": totalRenderTime}))
    f.close()


def render_logger():
    bpy.context.scene.render.engine = 'CYCLES'
    handlers.render_init.append(render_init)
    handlers.render_complete.append(complete)


def render_reader():
    f = open(FILENAME, "w")
    f.write(json.dumps({
        "samples": bpy.data.scenes[0].cycles.samples,
        "max_bounces": bpy.data.scenes[0].cycles.max_bounces,
        "diffuse_bounces": bpy.data.scenes[0].cycles.diffuse_bounces,
        "glossy_bounces": bpy.data.scenes[0].cycles.glossy_bounces,
        "transparent_max_bounces": bpy.data.scenes[0].cycles.transparent_max_bounces,
        "transmission_bounces": bpy.data.scenes[0].cycles.transmission_bounces,
        "volume_bounces": bpy.data.scenes[0].cycles.volume_bounces,
        "start_frame": bpy.data.scenes[0].frame_start,
        "end_frame": bpy.data.scenes[0].frame_end
    }))
    f.close()


def render_writer(render_service: BlenderSceneAdapter) -> None:
    f = open(FILENAME)
    data = json.load(f)
    bpy.data.scenes[0].cycles.samples = data["samples"]
    bpy.data.scenes[0].cycles.max_bounces = data["max_bounces"]
    bpy.data.scenes[0].cycles.diffuse_bounces = data["diffuse_bounces"]
    bpy.data.scenes[0].cycles.glossy_bounces = data["glossy_bounces"]
    bpy.data.scenes[0].cycles.transparent_max_bounces = data["transparent_max_bounces"]
    bpy.data.scenes[0].cycles.transmission_bounces = data["transmission_bounces"]
    bpy.data.scenes[0].cycles.volume_bounces = data["volume_bounces"]
    bpy.data.scenes[0].frame_start = data["start_frame"]
    bpy.data.scenes[0].frame_end = data["end_frame"]
    bpy.context.scene.frame_set(data["end_frame"])
    f.close()
    bpy.ops.wm.save_mainfile()


def cast_ray(direction):
    cast_result = bpy.context.scene.ray_cast(view_layer=bpy.context.view_layer,
                                             origin=Vector((0, 0, 0)),
                                             direction=direction,
                                             distance=100)
    return cast_result[0]


def blender_ray_trace(vectors):
    start_time = datetime.now()
    ray_hits = 0
    ray_did_hits = 0
    for vector in vectors:
        did_ray_hit = cast_ray(direction=vector)
        ray_hits = ray_hits + 1
        if did_ray_hit:
            ray_did_hits = ray_did_hits + 1
    print(f"ray cast took {datetime.now() - start_time}")
    return ray_did_hits / ray_hits


# todo: always casting from origin
def render_ray_cast():
    f = open(FILENAME)
    data = json.load(f)
    subdivisions = data["subdivisions"]
    print(f"subdivisions in temp {subdivisions}")
    f.close()
    bpy.ops.mesh.primitive_ico_sphere_add(subdivisions=subdivisions,
                                          radius=1,
                                          enter_editmode=False,
                                          align='WORLD',
                                          location=[0, 0, 0],
                                          scale=[10, 10, 10])
    active_object = bpy.context.active_object
    vertexes = [active_object.matrix_world @ _object.co for _object in active_object.data.vertices]
    bpy.ops.object.delete(use_global=False, confirm=False)
    vectors = [Vector((vertex[0], vertex[1], vertex[2])) for vertex in vertexes]
    print(f"processing {len(vectors)} vectors for ray cast")
    ray_coverage = blender_ray_trace(vectors=vectors)
    print(f"ray coverage of {ray_coverage}")
    f = open(FILENAME, "w")
    f.write(json.dumps({"percentage": ray_coverage}))
    f.close()


class Routes:

    def __init__(self):
        self.__render_commands = RenderCommands(comms_service=FileTempCommsService(),
                                                scene_adapter=BlenderSceneAdapter(),
                                                date_time_adapter=InBuiltDateTimeAdapter())
        self.__routes = dict({
            "render_frame": self.__render_commands.render_frame,
            "get_scene_data": self.__render_commands.get_scene_data,
            "set_scene_data": self.__render_commands.set_scene_data,
            "get_scene_and_viewpoint_coverage": self.__render_commands.get_scene_and_viewpoint_coverage
        })

    def __do_route_func(self, func: Callable, index: str):
        print(f"starting: {index}")
        func()
        print(f"ending: {index}")

    def get_route(self, index: str) -> Callable:
        try:
            return lambda: self.__do_route_func(func=self.__routes[index], index=index)
        except Exception as e:
            return lambda: print(str(e))


argv = sys.argv
try:
    argv = argv[argv.index("--") + 1:]
except:
    argv = ["render_frame"]

if not os.path.exists(FILENAME):
    try:
        os.makedirs(FILENAME)
    except OSError as exc:
        if exc.errno != errno.EEXIST:
            raise


routes = Routes()
routes.get_route(argv[0])()
