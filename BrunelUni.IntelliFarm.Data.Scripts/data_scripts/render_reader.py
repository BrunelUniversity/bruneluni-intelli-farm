import json
import os
import sys
import bpy
import pathlib

FILENAME = f"{pathlib.Path(__file__).parent.resolve()}\\temp\\render.json"

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
