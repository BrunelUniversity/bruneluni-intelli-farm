import json
import os
import sys
import bpy


f = open("temp/render.json", "w")
f.write(json.dumps({"samples": bpy.data.scenes[0].cycles.samples, "max_bounces": bpy.data.scenes[0].cycles.max_bounces}))
f.close()
