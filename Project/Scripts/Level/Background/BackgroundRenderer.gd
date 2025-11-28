extends TextureRect
@export var camera : Camera2D
@export var round_thing = 0.5
func _process(_delta: float) -> void:
	size = camera.get_viewport_rect().size * camera.zoom
	global_position = - texture.get_size() / 2 + camera.global_position
