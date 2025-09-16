class_name RectangleCreator extends UseTileType
@export var tileType : Array[TileType]

func create(region: LevelRegion) -> Array:
	var size = region.Bounds.size
	var result = []
	var tile = tileType.pick_random()
	var isStartOrEnd = region.IsStart or region.IsEnd
	var neighborsWrapper
	if not isStartOrEnd:
		neighborsWrapper = region.GetTransitionsBounding
		neighborsWrapper.position -= region.Bounds.position + Vector2i.ONE
		neighborsWrapper.size += Vector2i.ONE * 2
	for x in size.x:
		result.append([])
		for y in size.y:
			result[x].append(
				TileType.EMPTY if
				isStartOrEnd or neighborsWrapper.has_point(Vector2i(x,y))
				else tile)
	return result
