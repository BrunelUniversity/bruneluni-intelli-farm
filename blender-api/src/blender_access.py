import json
import os
from ctypes.wintypes import HANDLE
from datetime import datetime
from typing import Callable, Any

import time
import bmesh
import bpy
import win32file
from bpy.app import handlers

from src.core import RenderEngineEnum, SceneDataDto, VectorType, MeshEnum, OperationEnum, ObjectDto


class BlenderSceneAdapter:

    def set_render_engine(self, engine: RenderEngineEnum) -> None:
        print(f"setting render engine to {engine.value}")
        if engine == RenderEngineEnum.Cycles:
            bpy.context.scene.render.engine = 'CYCLES'

    def get_scene_data(self) -> SceneDataDto:
        data = SceneDataDto(
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
        print(f"get scene data\n{data.__dict__}")
        return data

    def set_scene_data(self, data: SceneDataDto) -> None:
        print(f"set scene data\n{data.__dict__}")
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
        print(f"saving file")
        bpy.ops.wm.save_mainfile()

    def add_render_handlers(self,
                            init_handler: Callable[[Any], None],
                            complete_handler: Callable[[Any], None]) -> None:
        print(f"adding render handles")
        handlers.render_init.append(init_handler)
        handlers.render_complete.append(complete_handler)

    def add_mesh(self,
                 subdivisions: int,
                 location: VectorType,
                 rotation: VectorType,
                 scale: VectorType,
                 mesh: MeshEnum) -> list[VectorType]:
        print(f"adding {mesh} of {subdivisions} subdivisions at {str(location)}")
        if mesh is MeshEnum.Iscosphere:
            bpy.ops.mesh.primitive_ico_sphere_add(subdivisions=subdivisions,
                                                  location=location,
                                                  rotation=rotation,
                                                  scale=scale)
        if mesh is MeshEnum.Plane:
            bpy.ops.mesh.primitive_plane_add(location=location,
                                             rotation=rotation,
                                             scale=scale)
            ob = bpy.context.object
            me = ob.data
            bm = bmesh.new()
            bm.from_mesh(me)
            bmesh.ops.subdivide_edges(bm,
                                      edges=bm.edges,
                                      cuts=subdivisions,
                                      use_grid_fill=True)
            bm.to_mesh(me)
            me.update()
        active_object = bpy.context.active_object
        return [active_object.matrix_world @ _object.co for _object in active_object.data.vertices]

    def transform(self,
                  object: str,
                  vector: VectorType,
                  operation: OperationEnum):
        print(f"{operation} {object} {vector}")
        if object == "":
            this_object = bpy.context.object
        else:
            this_object = bpy.data.scenes[0].objects[object]
        if operation is OperationEnum.Rotate:
            this_object.rotation_euler = vector
        if operation is OperationEnum.Move:
            this_object.location = vector
        if operation is OperationEnum.Scale:
            this_object.scale = vector

    def cast_ray(self,
                 origin: VectorType,
                 direction: VectorType,
                 distance: int) -> bool:
        return bpy.context.scene.ray_cast(depsgraph=bpy.context.evaluated_depsgraph_get(),
                                          origin=origin,
                                          direction=direction,
                                          distance=distance)[0]

    def delete_current_object(self) -> None:
        print("delete current object")
        bpy.ops.object.delete(use_global=False, confirm=False)

    def get_current_object_vertex_vectors(self) -> list[VectorType]:
        active_object = bpy.context.active_object
        return [active_object.matrix_world @ _object.co for _object in active_object.data.vertices]

    def triangulate_object(self, obj: str):
        me = bpy.data.scenes[0].objects[obj].data
        bm = bmesh.new()
        bm.from_mesh(me)
        bmesh.ops.triangulate(bm, faces=bm.faces[:])
        bm.to_mesh(me)
        bm.free()

    def get_all_objects(self) -> list[ObjectDto]:
        objects = bpy.data.scenes[0].objects
        meshes = list(filter(lambda x: x.type == "MESH", objects))
        return list(map(lambda x: ObjectDto(name=x.name, poly_count=len(x.data.polygons)), meshes))


class FileTempCommsService:

    def __init__(self, filename: str):
        self.__filename = filename
        os.makedirs(os.path.dirname(self.__filename), exist_ok=True)

    def read_json(self) -> dict:
        with open(self.__filename) as opened_file:
            read = json.loads(opened_file.read())
            print(f"read from file\n{json.dumps(read, indent=4, sort_keys=True)}")
            return read

    def write_json(self, data: dict):
        with open(self.__filename, 'w') as opened_file:
            print(f"writing to file\n{json.dumps(data, indent=4, sort_keys=True)}")
            opened_file.write(json.dumps(data))


class Win32NamedPipeTempCommsService:

    def __init__(self, filename_read: str, filename_write: str):
        self.__filename_write = filename_write
        self.__filename_read = filename_read

    def read_json(self) -> dict:
        print("reading from named pipe client")
        handle = self.__connect_to_pipe(filename=self.__filename_write)
        _, data = win32file.ReadFile(handle, 4096)
        message_string = data.decode('utf-8')
        win32file.CloseHandle(handle)
        print("read from named pipe client")
        return json.loads(message_string)

    def write_json(self, data: dict):
        print("writing from named pipe client")
        handle = self.__connect_to_pipe(filename=self.__filename_read)
        message_bytes = json.dumps(data).encode('utf-8')
        win32file.WriteFile(handle, message_bytes)
        win32file.CloseHandle(handle)
        print("written from named pipe client")

    def __connect_to_pipe(self, filename: str) -> HANDLE:
        print(f"connecting to named pipe at {filename}")
        return win32file.CreateFile(
            f"\\\\.\\pipe\\{filename}",
            win32file.GENERIC_READ | win32file.GENERIC_WRITE,
            0,
            None,
            win32file.OPEN_EXISTING,
            0,
            None)

class InBuiltDateTimeAdapter:

    def now_utc(self) -> datetime:
        return datetime.utcnow()
