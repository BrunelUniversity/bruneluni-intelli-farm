import errno
import json
import os
from typing import Callable, Any
from bpy.app import handlers
from bruneluni_Intellifarm_data_scripts.src.core import RenderEngineEnum, SceneDataDto, VectorType
import bpy


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

    def add_iscosphere(self, subdivisions: int, location: VectorType) -> list[VectorType]:
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
                 origin: VectorType,
                 direction: VectorType,
                 distance: int) -> bool:
        return bpy.context.scene.ray_cast(view_layer=bpy.context.view_layer,
                                          origin=origin,
                                          direction=direction,
                                          distance=distance)[0]

    def delete_current_object(self) -> None:
        bpy.ops.object.delete(use_global=False, confirm=False)


class FileTempCommsService:

    def __init__(self, filename: str):
        self.__filename = filename
        if not os.path.exists(self.__filename):
            try:
                os.makedirs(self.__filename)
            except OSError as exc:
                if exc.errno != errno.EEXIST:
                    raise

    def read_json(self) -> dict:
        with open(self.__filename) as opened_file:
            return json.loads(opened_file.read())

    def write_json(self, data: dict):
        with open(self.__filename, 'w') as opened_file:
            opened_file.write(json.dumps(data))
