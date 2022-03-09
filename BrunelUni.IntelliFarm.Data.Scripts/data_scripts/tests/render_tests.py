from typing import Union
from unittest import TestCase
from unittest.mock import Mock, MagicMock
from mathutils import Vector
from callee import Captor

from render import RenderCommands, TempCommsService, SceneAdapter, RenderEngineEnum


class RenderCommandTests(TestCase):

    def setUp(self) -> None:
        self.__comms_service: TempCommsService = Mock()
        self.__scene_adapter: SceneAdapter = Mock()
        self.__date_time_adapter = Mock()
        self.__sut = RenderCommands(comms_service=self.__comms_service,
                                    scene_adapter=self.__scene_adapter,
                                    date_time_adapter=self.__date_time_adapter)

    def when_get_coverage_called(self):
        self.__scene_adapter.cast_ray = MagicMock()
        self.__scene_adapter.cast_ray.side_effect = [True, False, True, True]
        self.__comms_service.read_json = MagicMock(return_value={"subdivisions": 8})
        self.__scene_adapter.add_iscosphere = MagicMock(return_value=[Vector({0, 0, 1}),
                                                                      Vector({0, 0, 2}),
                                                                      Vector({0, 0, 3}),
                                                                      Vector({0, 0, 4})])
        result = self.__sut.get_scene_and_viewpoint_coverage()
        self.__scene_adapter.cast_ray.assert_called_with(origin=Vector({0, 0, 0}),
                                                         direction=Vector({0, 0, 1}),
                                                         distance=100)
        self.__scene_adapter.cast_ray.assert_called_with(origin=Vector({0, 0, 0}),
                                                         direction=Vector({0, 0, 2}),
                                                         distance=100)
        self.__scene_adapter.cast_ray.assert_called_with(origin=Vector({0, 0, 0}),
                                                         direction=Vector({0, 0, 3}),
                                                         distance=100)
        self.__scene_adapter.cast_ray.assert_called_with(origin=Vector({0, 0, 0}),
                                                         direction=Vector({0, 0, 4}),
                                                         distance=100)
        assert result == 0.75
