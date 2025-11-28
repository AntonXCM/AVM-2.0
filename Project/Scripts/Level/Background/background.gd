@tool
extends SubViewport

@export var camera : Camera2D
@export var insidecamera : Camera2D

func _process(_d):
	size = (camera.get_viewport_rect().size / camera.zoom).ceil()
	insidecamera.global_position = camera.global_position
