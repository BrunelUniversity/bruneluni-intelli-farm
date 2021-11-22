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
f.close()
bpy.ops.wm.save_mainfile()