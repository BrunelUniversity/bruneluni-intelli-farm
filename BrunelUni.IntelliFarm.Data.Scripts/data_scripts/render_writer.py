import json
import os
import sys
import bpy


f = open("temp/render.json")
data = json.load(f)
bpy.data.scenes[0].cycles.samples = data["samples"]
bpy.data.scenes[0].cycles.max_bounces = data["max_bounces"]
f.close()
bpy.ops.wm.save_mainfile()