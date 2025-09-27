extends CharacterBody2D

@export var x_velocity_cap: float
@export var y_velocity_cap: float
@export var gravity_scale = 1.0
@export var mass = 100.0

func _physics_process(delta: float) -> void:
	if not is_on_floor():
		velocity += get_gravity() * delta * gravity_scale
		
	velocity.x = clamp(velocity.x,-x_velocity_cap,x_velocity_cap)
	velocity.y = clamp(velocity.y,-y_velocity_cap,y_velocity_cap)
	
	if not move_and_slide():
		pass
	for id in get_slide_collision_count():
		var collision = get_slide_collision(id)
		var collider = collision.get_collider()
		if collider is RigidBody2D:
			collider.apply_force(-collision.get_normal() * mass, collision.get_position())
