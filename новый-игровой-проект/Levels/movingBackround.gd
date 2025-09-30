extends ParallaxLayer

@export var speed : Vector2
var motion_offset_floating : Vector2

func _process(delta: float) -> void:
	motion_offset_floating += speed * delta
	if(motion_mirroring.x != 0 && abs(motion_offset_floating.x)>=motion_mirroring.x): # Этот код делает движение паралакса по пикселям
		motion_offset_floating.x = -motion_mirroring.x + fmod(abs(motion_offset.x),motion_mirroring.x);
	if(motion_mirroring.y != 0 && abs(motion_offset_floating.y)>=motion_mirroring.y):
		motion_offset_floating.y = -motion_mirroring.y + fmod(abs(motion_offset.y),motion_mirroring.y);
	motion_offset = motion_offset_floating as Vector2i;
