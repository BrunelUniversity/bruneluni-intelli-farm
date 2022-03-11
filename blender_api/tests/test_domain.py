from unittest import TestCase
from unittest.mock import Mock, MagicMock, call

from src.core import TempCommsService, SceneAdapter, DateTimeAdapter, MeshEnum
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
        self.__scene_adapter.delete_current_object = MagicMock()
        self.__scene_adapter.cast_ray.side_effect = [True, False, True, True]
        self.__comms_service.read_json = MagicMock(return_value={"subdivisions": 8})
        self.__scene_adapter.add_mesh = MagicMock(return_value=[(0, 0, 1),
                                                                (0, 0, 2),
                                                                (0, 0, 3),
                                                                (0, 0, 4)])

        # act
        self.__sut.get_scene_and_viewpoint_coverage()

        # assert
        self.__scene_adapter.cast_ray.assert_has_calls([call(origin=(0, 0, 0), direction=(0, 0, 1), distance=100),
                                                        call(origin=(0, 0, 0), direction=(0, 0, 2), distance=100),
                                                        call(origin=(0, 0, 0), direction=(0, 0, 3), distance=100),
                                                        call(origin=(0, 0, 0), direction=(0, 0, 4), distance=100)])
        self.__scene_adapter.add_mesh.assert_called_with(subdivisions=8,
                                                         location=(0, 0, 0),
                                                         mesh=MeshEnum.Iscosphere)
        self.__scene_adapter.delete_current_object.assert_called_once()
        assert self.__scene_adapter.cast_ray.call_count == 4
        self.__comms_service.write_json.assert_called_once_with(data={"percentage": 0.75})
