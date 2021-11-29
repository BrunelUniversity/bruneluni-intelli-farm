import json
import os
import sys
import bpy
import pathlib

FILENAME = f"{pathlib.Path(__file__).parent.resolve()}\\temp\\render.json"

if not os.path.exists(FILENAME):
    try:
        os.makedirs(FILENAME)
    except OSError as exc: # Guard against race condition
        if exc.errno != errno.EEXIST:
            raise

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