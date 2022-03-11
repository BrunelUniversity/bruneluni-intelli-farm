from unittest import TestCase
from unittest.mock import Mock, MagicMock, call

from src.core import TempCommsService, SceneAdapter, DateTimeAdapter, MeshEnum, OperationEnum, ObjectDto
from src.domain import RenderCommands


class TestRenderCommands(TestCase):

    def setUp(self) -> None:
        self.__comms_service: TempCommsService = Mock()
        self.__scene_adapter: SceneAdapter = Mock()
        self.__date_time_adapter: DateTimeAdapter = Mock()
        self.__sut = RenderCommands(comms_service=self.__comms_service,
                                    scene_adapter=self.__scene_adapter,
                                    date_time_adapter=self.__date_time_adapter)

    def test_when_get_coverage_called(self):
        # arrange
        self.__comms_service.write_json = MagicMock()
        self.__scene_adapter.cast_ray = MagicMock()
        self.__scene_adapter.transform = MagicMock()
        self.__scene_adapter.delete_current_object = MagicMock()
        self.__scene_adapter.cast_ray.side_effect = [True, False, True, True, True, True, False]
        self.__comms_service.read_json = MagicMock(return_value={"subdivisions": 8})
        self.__scene_adapter.add_mesh = MagicMock()
        self.__scene_adapter.add_mesh.side_effect = [[(0, 0, 1),
                                                      (0, 0, 2),
                                                      (0, 0, 3),
                                                      (0, 0, 4)],
                                                     [(1, 0, 1),
                                                      (1, 0, 2),
                                                      (1, 0, 3)]]

        # act
        self.__sut.get_scene_and_viewpoint_coverage()

        # assert
        self.__scene_adapter.cast_ray.assert_has_calls([call(origin=(0, 0, 0), direction=(0, 0, 1), distance=100),
                                                        call(origin=(0, 0, 0), direction=(0, 0, 2), distance=100),
                                                        call(origin=(0, 0, 0), direction=(0, 0, 3), distance=100),
                                                        call(origin=(0, 0, 0), direction=(0, 0, 4), distance=100),
                                                        call(origin=(0, 0, 0), direction=(1, 0, 1), distance=100),
                                                        call(origin=(0, 0, 0), direction=(1, 0, 2), distance=100),
                                                        call(origin=(0, 0, 0), direction=(1, 0, 3), distance=100)])
        self.__scene_adapter.add_mesh.assert_has_calls([
            call(subdivisions=8, location=(0, 0, 0), rotation=(0, 0, 0), scale=(10, 10, 10), mesh=MeshEnum.Iscosphere),
            call(subdivisions=100, location=(-4.87318, 0, 0), rotation=(0, 1.5708, 0), scale=(1, 1.771, 1),
                 mesh=MeshEnum.Plane)])
        assert self.__scene_adapter.delete_current_object.call_count == 2
        assert self.__scene_adapter.cast_ray.call_count == 7
        self.__comms_service.write_json.assert_called_once_with(data={"scene": 0.75, "viewport": float(2 / 3)})

    def test_get_triangle_count(self):
        # arrange
        self.__scene_adapter.get_all_objects = MagicMock()
        self.__scene_adapter.get_all_objects.side_effect = [
            [ObjectDto(name="Cube.001", poly_count=0), ObjectDto(name="Cube.002", poly_count=0),
             ObjectDto(name="Cube.003", poly_count=0)],
            [ObjectDto(name="Cube.001", poly_count=5), ObjectDto(name="Cube.002", poly_count=8),
             ObjectDto(name="Cube.003", poly_count=3)]]
        self.__comms_service.write_json = MagicMock()
        self.__scene_adapter.triangulate_object = MagicMock()

        # act
        actual_count = self.__sut.get_triangle_count()

        # assert
        assert 16 == actual_count
        self.__scene_adapter.triangulate_object.assert_has_calls([
            call(obj="Cube.001"),
            call(obj="Cube.002"),
            call(obj="Cube.003")
        ])
        assert 2 == self.__scene_adapter.get_all_objects.call_count
