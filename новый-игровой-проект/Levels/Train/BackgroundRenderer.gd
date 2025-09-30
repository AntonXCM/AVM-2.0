extends TextureRect
var camera : Camera2D

func _init() -> void:
	camera = get_parent()
	
func _process(_delta: float) -> void:
	size = texture.get_size() 
	position = size * -0.5
