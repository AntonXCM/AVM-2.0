@tool
extends SubViewport

@export var camera : Camera2D
@export var insidecamera : Camera2D

func _process(_d):
	var viewport_size = camera.get_viewport_rect().size / camera.zoom
	size = viewport_size
	insidecamera.position = camera.position
