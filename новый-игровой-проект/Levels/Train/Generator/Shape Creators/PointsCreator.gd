class_name PointsCreator extends UseTileType
@export var spacing_range : Vector4i
@export var emptyTile : TileType

func create(region: LevelRegion) -> Array:
	var size = region.Bounds.size
	var spacing := Vector2i(randi_range(spacing_range.x, spacing_range.y), randi_range(spacing_range.z, spacing_range.w))
	var offset := Vector2i(randi_range(0, spacing.x), randi_range(0, spacing.y))
	if region.IsEnd:
		size.x -= 1
	if (size.y == 3 and offset.y == 1) or size.y == 2:
		return []
	
	var regionPos = region.Bounds.position + Vector2i.ONE
	var transitions = region.Transitions.map(
		func(transition): 
			var bounds : Rect2i = transition.Bounds
			bounds.position -= regionPos
			bounds.size += Vector2i.ONE * 2
			return bounds
			)
	var result = []
	for x in size.x:
		result.append([])
		for y in size.y:
			result[x].append(
				TileType.FILLED if
				(x - offset.x) % (spacing.x + 1) == 0 and (y - offset.y) % (spacing.y + 1) == 0
				and transitions.all(func(transition): return not transition.has_point(Vector2i(x,y)))
				else emptyTile)
	return result
