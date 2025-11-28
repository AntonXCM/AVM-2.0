class_name ColumnCreator extends UseTileType

@export var obstacle_spacing := Vector2i(1,3)
@export var max_height := Vector2i(3,4)

func create(region: LevelRegion) -> Array:
	var size = region.Bounds.size
	var bottom_transitions = region.GetBottomTransitions
	var result = []
	
	var column_height = max(3, size.y - randi_range(max_height.x, max_height.y))
	var spacing = randi_range(obstacle_spacing.x, obstacle_spacing.y) + 1
	var offset = randi_range(0,spacing)
	var empty_last_column = region.IsEnd or region.HasRightTransitions
	if empty_last_column:
		size.x-=1
	for x in size.x:
		result.append([])
		var force_empty = x == 0 and (region.IsStart or region.HasLeftTransitions)
		var is_column = (x + offset) % spacing == 0
		var has_bottom_transition := false
		if not is_column:
			for t in bottom_transitions:
				if x >= t.Bounds.position.x and x < t.Bounds.end.x:
					has_bottom_transition = true
					break
		for y in size.y:
			result[x].append(
				TileType.EMPTY if force_empty or y < column_height
				else (TileType.FILLED if is_column 
				else (TileType.UNDEFINED if has_bottom_transition else TileType.EMPTY))
			)
	if empty_last_column:
		var empty_column = []
		for y in size.y:
			empty_column.append(TileType.EMPTY)
		result.append(empty_column)
	
	return result
