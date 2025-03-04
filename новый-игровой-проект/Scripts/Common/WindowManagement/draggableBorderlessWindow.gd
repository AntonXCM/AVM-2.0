extends Control

var dragging: bool = false
var drag_offset: Vector2

func _input(event):
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT:
			if event.pressed:
				dragging = true
				drag_offset = event.position
			else:
				dragging = false
	elif event is InputEventMouseMotion and dragging:
		var window_node = get_parent()
		window_node.position += (Vector2i)((event.position - drag_offset).round())
