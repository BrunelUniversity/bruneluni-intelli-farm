from src.core import RenderEngineEnum, SceneDataDto, TempCommsService, SceneAdapter, DateTimeAdapter, \
    MeshEnum, OperationEnum
from datetime import datetime


class RenderCommands:

    def __init__(self,
                 comms_service: TempCommsService,
                 scene_adapter: SceneAdapter,
                 date_time_adapter: DateTimeAdapter):
        self.__scene_adapter = scene_adapter
        self.__comms_service = comms_service
        self.__date_time_adapter = date_time_adapter
        self.__render_time_start: datetime = self.__date_time_adapter.now_utc()

    def get_scene_data(self):
        self.__comms_service.write_json(data=self.__scene_adapter.get_scene_data().__dict__)

    def set_scene_data(self):
        data = self.__comms_service.read_json()
        self.__scene_adapter.set_scene_data(data=SceneDataDto(
            samples=data["samples"],
            max_bounces=data["max_bounces"],
            diffuse_bounces=data["diffuse_bounces"],
            glossy_bounces=data["glossy_bounces"],
            transmission_bounces=data["transmission_bounces"],
            volume_bounces=data["volume_bounces"],
            start_frame=data["start_frame"],
            end_frame=data["end_frame"],
            transparent_max_bounces=data["transparent_max_bounces"]
        ))

    def init_handler(self):
        self.__render_time_start = self.__date_time_adapter.now_utc()

    def complete_handler(self):
        total_render_time = float((self.__date_time_adapter.now_utc() - self.__render_time_start).total_seconds())
        self.__comms_service.write_json(data={"render_time": total_render_time})

    def render_frame(self):
        self.__scene_adapter.set_render_engine(engine=RenderEngineEnum.Cycles)
        self.__scene_adapter.add_render_handlers(init_handler=lambda scene: self.init_handler(),
                                                 complete_handler=lambda scene: self.complete_handler())

    def get_scene_and_viewpoint_coverage(self):
        subdivisions = self.__comms_service.read_json()["subdivisions"]
        origin_vector = (0, 0, 0)
        scene_vectors = self.__scene_adapter.add_mesh(subdivisions=subdivisions,
                                                      location=origin_vector,
                                                      mesh=MeshEnum.Iscosphere)
        self.__scene_adapter.delete_current_object()
        viewport_vectors = self.__scene_adapter.add_mesh(subdivisions=subdivisions,
                                                         location=origin_vector,
                                                         mesh=MeshEnum.Plane)
        self.__scene_adapter.delete_current_object()
        self.__scene_adapter.transform(object="",
                                       vector=(-23, 0, 0),
                                       operation=OperationEnum.Move)
        self.__scene_adapter.transform(object="",
                                       vector=(4.698, 8.352, 4.35),
                                       operation=OperationEnum.Scale)
        self.__scene_adapter.transform(object="",
                                       vector=(0, 1.5708, 0),
                                       operation=OperationEnum.Rotate)
        scene_hit_count = 0
        for vector in scene_vectors:
            if self.__scene_adapter.cast_ray(origin=origin_vector, direction=vector, distance=100):
                scene_hit_count = scene_hit_count + 1
        viewport_hit_count = 0
        for vector in viewport_vectors:
            if self.__scene_adapter.cast_ray(origin=origin_vector, direction=vector, distance=100):
                viewport_hit_count = viewport_hit_count + 1
        self.__comms_service.write_json(
            data={"scene": scene_hit_count / len(scene_vectors), "viewport": viewport_hit_count / len(viewport_vectors)})
