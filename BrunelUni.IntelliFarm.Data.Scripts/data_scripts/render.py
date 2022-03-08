import json
import os
from datetime import datetime
from bpy.app import handlers
import pathlib
import sys
import bpy

RENDER_START_TIME: datetime
FILENAME = f"{pathlib.Path(__file__).parent.resolve()}\\temp\\render.json"
argv = sys.argv
try:
    argv = argv[argv.index("--") + 1:]
except:
    argv = ["render_logger"]

if not os.path.exists(FILENAME):
    try:
        os.makedirs(FILENAME)
    except OSError as exc:
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

def render_logger():
    bpy.context.scene.render.engine = 'CYCLES'
    handlers.render_init.append(render_init)
    handlers.render_complete.append(complete)
    
def render_reader():
    f = open(FILENAME, "w")
    f.write(json.dumps({
        "samples": bpy.data.scenes[0].cycles.samples,
        "max_bounces": bpy.data.scenes[0].cycles.max_bounces,
        "diffuse_bounces": bpy.data.scenes[0].cycles.diffuse_bounces,
        "glossy_bounces": bpy.data.scenes[0].cycles.glossy_bounces,
        "transparent_max_bounces": bpy.data.scenes[0].cycles.transparent_max_bounces,
        "transmission_bounces": bpy.data.scenes[0].cycles.transmission_bounces,
        "volume_bounces": bpy.data.scenes[0].cycles.volume_bounces,
        "start_frame": bpy.data.scenes[0].frame_start,
        "end_frame": bpy.data.scenes[0].frame_end
    }))
    f.close()

def render_writer():
    f = open(FILENAME)
    data = json.load(f)
    bpy.data.scenes[0].cycles.samples = data["samples"]
    bpy.data.scenes[0].cycles.max_bounces = data["max_bounces"]
    bpy.data.scenes[0].cycles.diffuse_bounces = data["diffuse_bounces"]
    bpy.data.scenes[0].cycles.glossy_bounces = data["glossy_bounces"]
    bpy.data.scenes[0].cycles.transparent_max_bounces = data["transparent_max_bounces"]
    bpy.data.scenes[0].cycles.transmission_bounces = data["transmission_bounces"]
    bpy.data.scenes[0].cycles.volume_bounces = data["volume_bounces"]
    bpy.data.scenes[0].frame_start = data["start_frame"]
    bpy.data.scenes[0].frame_end = data["end_frame"]
    f.close()
    bpy.ops.wm.save_mainfile()
if argv[0] is "render_reader":
    print("running render_reader")
    render_reader()
elif argv[0] is "render_writer":
    print("running render_writer")
    render_writer()
elif argv[0] is "render_logger":
    print("running render_logger")
    render_logger()
else:
    print("ERROR: render function not found")