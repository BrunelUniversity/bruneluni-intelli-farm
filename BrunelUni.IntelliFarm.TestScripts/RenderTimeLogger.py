from datetime import datetime
from bpy.app import handlers

RENDER_START_TIME: datetime


def render_init(scene):
    global RENDER_START_TIME
    RENDER_START_TIME = datetime.now()


def complete(scene):
    f = open("render_times.dump", "a")
    totalRenderTime = str((datetime.now() - RENDER_START_TIME))+"\n"
    f.write(totalRenderTime)
    f.close()


handlers.render_init.append(render_init)
handlers.render_complete.append(complete)
