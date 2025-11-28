class_name FullRectangleCreator extends UseTileType

func create(region: LevelRegion) -> Array:
	var tile = (TileType.EMPTY if
		region.IsStart or not region.Transitions.is_empty()
		else TileType.FILLED)
	
	var size = region.Bounds.size
	var result = []
	for x in size.x:
		result.append([])
		for y in size.y:
			result[x].append(tile)
	return result
