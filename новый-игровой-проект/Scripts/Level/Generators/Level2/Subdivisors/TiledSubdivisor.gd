extends RefCounted

class_name TiledSubdivisor

func subdivide(rect: Rect2i):
	var result = LevelSubdivision.new()
	for part in Rect2iExtentions.split_horizontally(rect, randi_range(3, 6)):
		for sub_part in Rect2iExtentions.split_vertically(part, randi_range(1, 3)):
			result.Add(sub_part)

	for region in result:
		var rand_value = randf()
		if rand_value < 0.2 and region.bounds.position.y > rect.position.y:
			region.expand_up(1)
		elif rand_value < 0.4 and region.bounds.end.y < rect.end.y:
			region.expand_down(1)
		elif rand_value > 0.9 and region.bounds.position.x > rect.position.x:
			region.expand_left(1)
		elif rand_value > 0.8 and region.bounds.end.x < rect.end.x:
			region.expand_right(1)

	return result
