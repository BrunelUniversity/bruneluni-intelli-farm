from datetime import datetime

from src.core import RenderEngineEnum, SceneDataDto, SceneAdapter, DateTimeAdapter, \
    MeshEnum, TempCommsService


class StateHelper:

    def __init__(self, state: dict):
        self.__state = state

    def read_command(self) -> str:
        return self.__state["data_in"]["command"]

    def read_data_in(self) -> dict:
        return self.__state["data_in"]["data"]

    def set_data_out(self, data: dict):
        print(f"output data being set to {data}")
        self.__state["data_out"] = data


class RenderCommands:

    def __init__(self,
                 scene_adapter: SceneAdapter,
                 date_time_adapter: DateTimeAdapter,
                 state_helper: StateHelper,
                 temp_comms_service: TempCommsService):
        self.__temp_comms_service = temp_comms_service
        self.__state_helper = state_helper
        self.__scene_adapter = scene_adapter
        self.__date_time_adapter = date_time_adapter
        self.__render_time_start: datetime = self.__date_time_adapter.now_utc()

    def get_scene_data(self) -> None:
        self.__state_helper.set_data_out(data=self.__scene_adapter.get_scene_data().__dict__)

    def set_scene_data(self) -> None:
        data = self.__state_helper.read_data_in()
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
        self.__scene_adapter.save_file()
        self.__state_helper.set_data_out(data={})

    def init_handler(self):
        print(f"init handler")
        self.__render_time_start = self.__date_time_adapter.now_utc()

    def complete_handler(self):
        total_render_time = float((self.__date_time_adapter.now_utc() - self.__render_time_start).total_seconds())
        print(f"render time was {total_render_time}")
        self.__temp_comms_service.write_json(data={"render_time": total_render_time})

    def render_frame(self) -> None:
        self.__scene_adapter.set_render_engine(engine=RenderEngineEnum.Cycles)
        self.__scene_adapter.add_render_handlers(init_handler=lambda scene: self.init_handler(),
                                                 complete_handler=lambda scene: self.complete_handler())

    def get_scene_and_viewpoint_coverage(self) -> None:
        data = self.__state_helper.read_data_in()
        subdivisions = data["subdivisions"]
        origin_vector = (0, 0, 0)
        scene_vectors = self.__scene_adapter.add_mesh(subdivisions=subdivisions,
                                                      location=origin_vector,
                                                      rotation=(0, 0, 0),
                                                      scale=(10, 10, 10),
                                                      mesh=MeshEnum.Iscosphere)
        self.__scene_adapter.delete_current_object()
        viewport_vectors = self.__scene_adapter.add_mesh(subdivisions=100,
                                                         location=(-4.87318, 0, 0),
                                                         rotation=(0, 1.5708, 0),
                                                         scale=(1, 1.771, 1),
                                                         mesh=MeshEnum.Plane)
        self.__scene_adapter.delete_current_object()
        scene_hit_count = 0
        for vector in scene_vectors:
            if self.__scene_adapter.cast_ray(origin=origin_vector, direction=vector, distance=100):
                scene_hit_count = scene_hit_count + 1
        viewport_hit_count = 0
        for vector in viewport_vectors:
            if self.__scene_adapter.cast_ray(origin=origin_vector, direction=vector, distance=100):
                viewport_hit_count = viewport_hit_count + 1
        self.__state_helper.set_data_out(data={
            "scene": scene_hit_count / len(scene_vectors),
            "viewport": viewport_hit_count / len(viewport_vectors)
        })

    def get_triangle_count(self) -> None:
        for object in self.__scene_adapter.get_all_objects():
            self.__scene_adapter.triangulate_object(obj=object.name)
        self.__state_helper.set_data_out(data={
            "count": sum(list(map(lambda x: x.poly_count, self.__scene_adapter.get_all_objects())))
        })
