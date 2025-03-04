extends Node

@export var vector : Vector2;
var rb : CharacterBody2D;
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	rb = get_child(0)
func _process(delta: float) -> void:
	rb.velocity = vector*delta
	
