from dataclasses import dataclass
from datetime import datetime
from enum import Enum
from typing import Callable, Any, Protocol


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


class ObjectDto:
    name: str
    poly_count: int

class MeshEnum(Enum):
    Plane = "Plane",
    Iscosphere = "Iscosphere"


class OperationEnum:
    Move = "Move",
    Rotate = "Rotate",
    Scale = "Scale"


class RenderEngineEnum(Enum):
    Cycles = "Cycles",
    BlenderInternal = "Blender Internal"


VectorType = tuple[float, float, float]


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

    def add_mesh(self,
                 subdivisions: int,
                 location: VectorType,
                 rotation: VectorType,
                 scale: VectorType,
                 mesh: MeshEnum) -> list[VectorType]:
        '''
        add mesh and get real vectors multiplied by world matrix
        '''
        ...

    def transform(self,
                  object: str,
                  vector: VectorType,
                  operation: OperationEnum):
        '''
        transform mesh and get real vectors multiplied by world matrix
        '''
        ...

    def cast_ray(self,
                 origin: VectorType,
                 direction: VectorType,
                 distance: int) -> bool:
        ...

    def delete_current_object(self) -> None:
        ...

    def get_current_object_vertex_vectors(self) -> list[VectorType]:
        ...

    def triangulate_object(self, obj: str) -> ObjectDto:
        ...

    def get_all_objects(self) -> list[ObjectDto]:
        ...