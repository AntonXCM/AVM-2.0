class_name Rect2iExtentions
static var cs_class = preload('res://Scripts/Common/MathAVM/Rect2Extentions.cs')

static func split_horizontally(rect : Rect2i, count : int) -> Array:
	return cs_class.SplitHorizontally(rect, count);

static func split_vertically(rect : Rect2i, count : int) -> Array:
	return cs_class.SplitVertically(rect, count);
