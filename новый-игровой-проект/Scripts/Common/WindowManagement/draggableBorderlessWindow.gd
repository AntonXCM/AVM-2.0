extends Control

var dragging: bool = false
var drag_offset: Vector2

func _input(event):
	if event is InputEventMouseButton and event.button_index == MOUSE_BUTTON_LEFT:
		if event.pressed:
			dragging = true
			drag_offset = event.position
		else:
			dragging = false
	elif event is InputEventMouseMotion and dragging:
		get_parent().position += (Vector2i)((event.position - drag_offset).round())
