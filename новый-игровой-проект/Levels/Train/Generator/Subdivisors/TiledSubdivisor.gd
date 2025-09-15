extends Resource

class_name TiledSubdivisor

@export var count : Vector4i

func subdivide(rect: Rect2i) -> LevelSubdivision:
	var result = LevelSubdivision.new()
	for part in Rect2Extensions.split_horizontally(rect, randi_range(count.x, count.y)):
		for sub_part in Rect2Extensions.split_vertically(part, randi_range(count.z, count.w)):
			result.Add(sub_part)

	for region in result.get_regions():
		var rand_value = randf()
		if rand_value < 0.2 and region.Bounds.position.y > rect.position.y:
			region.ExpandUp(1)
		elif rand_value < 0.4 and region.Bounds.end.y < rect.end.y:
			region.ExpandDown(1)
		elif rand_value > 0.8 and region.Bounds.position.x > rect.position.x:
			region.ExpandLeft(1)
		elif rand_value > 0.6 and region.Bounds.end.x < rect.end.x:
			region.ExpandRight(1)

	return result
