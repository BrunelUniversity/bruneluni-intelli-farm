import json
import os
from datetime import datetime
from bpy.app import handlers
import pathlib

RENDER_START_TIME: datetime

FILENAME = f"{pathlib.Path(__file__).parent.resolve()}\\temp\\render.json"

if not os.path.exists(FILENAME):
    try:
        os.makedirs(FILENAME)
    except OSError as exc: # Guard against race condition
        if exc.errno != errno.EEXIST:
            raise

def render_init(scene):
    global RENDER_START_TIME
    RENDER_START_TIME = datetime.now()


def complete(scene):
    print(os.getcwd())
    f = open(FILENAME, "w")
    totalRenderTime = float((datetime.now() - RENDER_START_TIME).total_seconds())
    f.write(json.dumps({"render_time": totalRenderTime}))
    f.close()


handlers.render_init.append(render_init)
handlers.render_complete.append(complete)