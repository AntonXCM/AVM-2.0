extends CharacterBody2D

@export var x_velocity_cap: float
@export var y_velocity_cap: float
@export var gravity_scale = 1.0

func _physics_process(delta: float) -> void:
	if not is_on_floor():
		velocity += get_gravity() * delta * gravity_scale
		
	velocity.x = clamp(velocity.x,-x_velocity_cap,x_velocity_cap)
	velocity.y = clamp(velocity.y,-y_velocity_cap,y_velocity_cap)
	
	move_and_slide()
