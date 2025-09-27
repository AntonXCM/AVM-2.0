extends Sprite2D

@export var mater : ShaderMaterial
@export var camera : Node2D
@export var speed = 1.0
func _physics_process(_delta: float) -> void:
	var uv = Vector2(0.5,(global_position.y - camera.global_position.y) / get_viewport_rect().size.y * speed)
	mater.set_shader_parameter("light_pos", uv)
