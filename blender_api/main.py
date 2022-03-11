import pathlib
import sys
from typing import Callable

from src.blender_access import FileTempCommsService, BlenderSceneAdapter, InBuiltDateTimeAdapter
from src.domain import RenderCommands

FILENAME = f"{pathlib.Path(__file__).parent.parent.resolve()}\\temp\\render.json"


class CommandsDispatcher:

    def __init__(self):
        self.__render_commands = RenderCommands(comms_service=FileTempCommsService(filename=FILENAME),
                                                scene_adapter=BlenderSceneAdapter(),
                                                date_time_adapter=InBuiltDateTimeAdapter())
        self.__commands = dict({
            "render_frame": self.__render_commands.render_frame,
            "get_scene_data": self.__render_commands.get_scene_data,
            "set_scene_data": self.__render_commands.set_scene_data,
            "get_scene_and_viewport_coverage": self.__render_commands.get_scene_and_viewpoint_coverage
        })

    @staticmethod
    def __do_command_func(func: Callable, index: str):
        try:
            print(f"starting: {index}")
            func()
        except Exception as e:
            print(str(e))
        finally:
            print(f"ending: {index}")

    def get_command(self, index: str) -> Callable:
        try:
            return lambda: self.__do_command_func(func=self.__commands[index], index=index)
        except Exception as e:
            return lambda: print(str(e))


argv = sys.argv
try:
    argv = argv[argv.index("--") + 1:]
except:
    argv = ["render_frame"]

commands = CommandsDispatcher()
commands.get_command(argv[0])()
