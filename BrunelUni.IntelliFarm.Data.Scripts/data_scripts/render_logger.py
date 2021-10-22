import json
import os
from datetime import datetime
from bpy.app import handlers

RENDER_START_TIME: datetime


def render_init(scene):
    global RENDER_START_TIME
    RENDER_START_TIME = datetime.now()


def complete(scene):
    print(os.getcwd())
    f = open("temp/render.json", "w")
    totalRenderTime = float((datetime.now() - RENDER_START_TIME).total_seconds())
    f.write(json.dumps({"render_time": totalRenderTime}))
    f.close()


handlers.render_init.append(render_init)
handlers.render_complete.append(complete)