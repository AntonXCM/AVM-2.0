class_name Rect2Extensions
enum TouchSide { NONE, LEFT, RIGHT, TOP, BOTTOM }

# CutBy
static func cut_by(rect: Rect2i, avoid: Rect2i) -> Rect2i:
	if not rect.intersects(avoid):
		return rect

	var options : Array[Rect2i] = [
		Rect2i(rect.position, Vector2i(rect.size.x, avoid.position.y - rect.position.y)), # верхний
		Rect2i(Vector2i(rect.position.x, avoid.end.y), Vector2i(rect.size.x, rect.end.y - avoid.end.y)), # нижний
		Rect2i(rect.position, Vector2i(avoid.position.x - rect.position.x, rect.size.y)), # левый
		Rect2i(Vector2i(avoid.end.x, rect.position.y), Vector2i(rect.end.x - avoid.end.x, rect.size.y)) # правый
	]

	options = options.filter(func(r): return r.size.x > 0 and r.size.y > 0)
	if options.is_empty():
		return Rect2i()

	var best := options[0]
	for r in options:
		if r.size.x * r.size.y < best.size.x * best.size.y:
			best = r
	return best

static func iter_points(rect: Rect2i):
	var result: Array[Vector2i] = []
	for x in range(rect.position.x, rect.end.x):
		for y in range(rect.position.y, rect.end.y):
			await Vector2i(x, y);

static func split_vertically(rect: Rect2i, count: int) -> Array[Rect2i]:
	var height : int = rect.size.y / count
	var remainder = rect.size.y % count
	var y = rect.position.y
	var result: Array[Rect2i] = []

	for i in count:
		var extra = 1 if i < remainder else 0
		result.append(Rect2i(rect.position.x, y, rect.size.x, height + extra))
		y += height + extra
	return result

static func split_horizontally(rect: Rect2i, count: int) -> Array[Rect2i]:
	var width : int = rect.size.x / count
	var remainder = rect.size.x % count
	var x = rect.position.x
	var result: Array[Rect2i] = []

	for i in count:
		var extra = 1 if i < remainder else 0
		result.append(Rect2i(Vector2i(x, rect.position.y), Vector2i(width + extra, rect.size.y)))
		x += width + extra
	return result

static func get_touch_side(a: Rect2i, b: Rect2i) -> TouchSide:
	if a.position.y < b.end.y and a.end.y > b.position.y:
		if a.end.x == b.position.x:
			return TouchSide.RIGHT
		if a.position.x == b.end.x:
			return TouchSide.LEFT
	if a.position.x < b.end.x and a.end.x > b.position.x:
		if a.end.y == b.position.y:
			return TouchSide.BOTTOM
		if a.position.y == b.end.y:
			return TouchSide.TOP
	return TouchSide.NONE
