extends Sprite2D
@export var speed := 360.0
func _process(delta: float) -> void:
	rotation_degrees += speed * delta
