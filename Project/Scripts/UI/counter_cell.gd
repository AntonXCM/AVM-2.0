extends Node

var sprite : Sprite2D
var isEnabled : bool
func _notification(what: int) -> void:
	match(what):
		NOTIFICATION_PARENTED:
			sprite=get_child(0);
			UpdateSprite();

func Disable() -> void:
	isEnabled = false;
	UpdateSprite();
	return;

func Enable() -> void:
	isEnabled = true;
	UpdateSprite();
	return;

func UpdateSprite() -> void:
	if(sprite == null):
		print("kakoe-to govno sluchilos");
		return;
	sprite.visible = isEnabled;
