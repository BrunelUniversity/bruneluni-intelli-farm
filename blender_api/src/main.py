import os
import pathlib
import sys
from typing import Callable

import bpy

from blender_api.src.blender_access import FileTempCommsService, BlenderSceneAdapter
from blender_api.src.core import InBuiltDateTimeAdapter
from blender_api.src.domain import RenderCommands

dir = os.path.dirname(bpy.data.filepath)
if not dir in sys.path:
    sys.path.append(dir )

FILENAME = f"{pathlib.Path(__file__).parent.resolve()}\\temp\\render.json"


class Routes:

    def __init__(self):
        self.__render_commands = RenderCommands(comms_service=FileTempCommsService(filename=FILENAME),
                                                scene_adapter=BlenderSceneAdapter(),
                                                date_time_adapter=InBuiltDateTimeAdapter())
        self.__routes = dict({
            "render_frame": self.__render_commands.render_frame,
            "get_scene_data": self.__render_commands.get_scene_data,
            "set_scene_data": self.__render_commands.set_scene_data,
            "get_scene_and_viewpoint_coverage": self.__render_commands.get_scene_and_viewpoint_coverage
        })

    @staticmethod
    def __do_route_func(func: Callable, index: str):
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

routes = Routes()
routes.get_route(argv[0])()
