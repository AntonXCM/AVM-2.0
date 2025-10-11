extends TextureRect
@export var camera : Camera2D
@export var round_thing = 0.5
func _process(_delta: float) -> void:
	size = texture.get_size()
	global_position = - size / 2 + camera.global_position.floor()
