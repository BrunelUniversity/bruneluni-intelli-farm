import pathlib
from typing import Callable

try:
    from src.blender_access import Win32NamedPipeTempCommsService

# TODO: fix won't work first time, this crash will mean the software has to be restarted

except ImportError:
    import pip

    pip.main(['install', 'pywin32'])
    from src.blender_access import Win32NamedPipeTempCommsService
from src.blender_access import BlenderSceneAdapter, InBuiltDateTimeAdapter
from src.core import TempCommsService
from src.domain import RenderCommands, StateHelper

FILENAME = f"{pathlib.Path(__file__).parent.parent.resolve()}\\temp\\render.json"


class CommandsDispatcher:

    def __init__(self, temp_comms_service: TempCommsService):
        self.__temp_comms_service = temp_comms_service
        self.__state = {}
        self.__state_helper = StateHelper(state=self.__state)
        self.__render_commands = RenderCommands(scene_adapter=BlenderSceneAdapter(),
                                                date_time_adapter=InBuiltDateTimeAdapter(),
                                                state_helper=self.__state_helper,
                                                temp_comms_service=temp_comms_service)
        self.__commands = dict({
            "render_frame": self.__render_commands.render_frame,
            "get_scene_data": self.__render_commands.get_scene_data,
            "set_scene_data": self.__render_commands.set_scene_data,
            "get_scene_and_viewport_coverage": self.__render_commands.get_scene_and_viewpoint_coverage,
            "get_triangles_count": self.__render_commands.get_triangle_count
        })

    @staticmethod
    def __do_command_func(func: Callable[[], None], index: str):
        try:
            print(f"starting: {index}")
            func()
        except Exception as e:
            print(str(e))
        finally:
            print(f"ending: {index}")

    def get_command(self) -> None:
        self.__state["data_in"] = self.__temp_comms_service.read_json()
        command = self.__state_helper.read_command()
        self.__do_command_func(func=self.__commands[command], index=command)
        if "data_out" in self.__state:
            self.__temp_comms_service.write_json(data=self.__state["data_out"])


temp_comms_service = Win32NamedPipeTempCommsService(filename="blender-api-pipe")
commands = CommandsDispatcher(temp_comms_service=temp_comms_service)
commands.get_command()
