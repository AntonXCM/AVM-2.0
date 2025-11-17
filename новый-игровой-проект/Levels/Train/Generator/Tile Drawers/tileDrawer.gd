extends UseTileType

class_name TileDrawer
signal draw_finished(id)
signal draw_process(id, percentage : float)
func draw(tiles : Array, tilemap : TileMapLayer, id : String):
	var tileset_data := TilesetData.new(tilemap.tile_set)
	var has_right := len(tiles) - 1
	var tree = tilemap.get_tree()
	tilemap.collision_enabled = false
	
	var top : bool
	var bottom : bool
	var right : bool
	var left_col = null
	var current_col = []
	var get_horizontal_median := func(x: int, y: int) -> TileType:
		var center = tiles[x][y]
		if not right or left_col == null:
			return center
		var right_tile = tiles[x + 1][y]
		if center == right_tile:
			return center
		var left_tile = tiles[x - 1][y]
		
		return left_tile if left_tile == center or left_tile == right_tile else center
	for x in len(tiles):
		var col : Array = tiles[x]
		var has_bottom := len(col) - 1
		
		var get_vertical_median := func(y: int) -> TileType:
			var center = col[y]
			if not top or not bottom:
				return center
			var top_tile = col[y - 1]
			if center == top_tile:
				return center
			var bottom_tile = col[y + 1]
			return bottom_tile if bottom_tile == center or bottom_tile == top_tile else center
			
		right = x < has_right
		for y in len(col):
			var tile = Tile.new() #Удалять не надо, в будущем он будет использован для тайлов с отрисовкой в несколько проходов
			current_col.append(tile)
			var vertical_median = null
			tile.type = col[y]
			if tile.type == TileType.EMPTY or tile.type == TileType.UNDEFINED:
				tilemap.erase_cell(Vector2i(x,y))
			else:
				top = y > 0
				bottom = y < has_bottom
				if top:
					tile.top_tile = define(col[y - 1])
					if right:
						tile.topright_tile = define(tiles[x + 1][y - 1])
					else:
						vertical_median = get_vertical_median.call(y)
						tile.topright_tile = vertical_median
					if left_col != null:
						tile.topleft_tile = define(tiles[x - 1][y - 1])
					else:
						vertical_median = get_vertical_median.call(y)
						tile.topleft_tile = vertical_median
				else:
					var median = define(get_horizontal_median.call(x, y))
					tile.top_tile = median
					tile.topleft_tile = median
					tile.topright_tile = median
				if bottom: 
					tile.bottom_tile = define(col[y + 1])
					if right:
						tile.bottomright_tile = define(tiles[x + 1][y + 1])
					else:
						vertical_median = get_vertical_median.call(y)
						tile.bottomright_tile = vertical_median
					if left_col != null:
						tile.bottomleft_tile = define(tiles[x - 1][y + 1])
					else:
						vertical_median = get_vertical_median.call(y)
						tile.bottomleft_tile = vertical_median
				else:
					var median = define(get_horizontal_median.call(x, y))
					tile.bottom_tile = median
					tile.bottomright_tile = median
					tile.bottomleft_tile = median
				if left_col != null: 
					tile.left_tile = define(tiles[x - 1][y])
				else:
					if vertical_median != null:
						tile.left_tile = vertical_median
					else:
						tile.left_tile = define(get_vertical_median.call(y))
				if right:
					tile.right_tile = define(tiles[x + 1][y])
				else:
					if vertical_median != null:
						tile.right_tile = vertical_median
					else:
						tile.right_tile = define(get_vertical_median.call(y))
				tilemap.set_cell(Vector2i(x,y),0,tile.get_atlas_coords(tileset_data))
			left_col = current_col
			await tree.physics_frame
		draw_process.emit(id, float(float(x) / len(tiles)))
	tilemap.collision_enabled = true
	draw_finished.emit(id)
func define(tile : TileType) -> TileType:
	return TileType.EMPTY if tile == TileType.UNDEFINED else tile

class TilesetData:
	var top_connection : Dictionary[int, PackedByteArray] = {}
	var bottom_connection : Dictionary[int, PackedByteArray] = {}
	var topright_connection : Dictionary[int, PackedByteArray] = {}
	var bottomright_connection : Dictionary[int, PackedByteArray] = {}
	var topleft_connection : Dictionary[int, PackedByteArray] = {}
	var bottomleft_connection : Dictionary[int, PackedByteArray] = {}
	var left_connection : Dictionary[int, PackedByteArray] = {}
	var right_connection : Dictionary[int, PackedByteArray] = {}
	var vector : Dictionary[int, Vector2i] = {}
	var count := 0
	
	func _init(tileset : TileSet = null) -> void:
		if tileset == null:
			return
		var tiles := tileset.get_source(0) as TileSetAtlasSource
		count = tiles.get_tiles_count()
		var top_connection_id = tileset.get_custom_data_layer_by_name("top_connection")
		var bottom_connection_id = tileset.get_custom_data_layer_by_name("bottom_connection")
		var left_connection_id = tileset.get_custom_data_layer_by_name("left_connection")
		var right_connection_id = tileset.get_custom_data_layer_by_name("right_connection")
		var topright_connection_id = tileset.get_custom_data_layer_by_name("topright_connection")
		var bottomright_connection_id = tileset.get_custom_data_layer_by_name("bottomright_connection")
		var topleft_connection_id = tileset.get_custom_data_layer_by_name("topleft_connection")
		var bottomleft_connection_id = tileset.get_custom_data_layer_by_name("bottomleft_connection")
		for tile in count:
			var tile_vector = tiles.get_tile_id(tile)
			vector.set(tile, tile_vector)
			var tiledata = tiles.get_tile_data(tile_vector,0)
			top_connection.set(tile, tiledata.get_custom_data_by_layer_id(top_connection_id))
			bottom_connection.set(tile, tiledata.get_custom_data_by_layer_id(bottom_connection_id))
			left_connection.set(tile, tiledata.get_custom_data_by_layer_id(left_connection_id))
			right_connection.set(tile, tiledata.get_custom_data_by_layer_id(right_connection_id))
			topright_connection.set(tile, tiledata.get_custom_data_by_layer_id(topright_connection_id))
			bottomright_connection.set(tile, tiledata.get_custom_data_by_layer_id(bottomright_connection_id))
			topleft_connection.set(tile, tiledata.get_custom_data_by_layer_id(topleft_connection_id))
			bottomleft_connection.set(tile, tiledata.get_custom_data_by_layer_id(bottomleft_connection_id))

class Tile:
	var type 		:= TileType.UNDEFINED
	var top_tile	= TileType.UNDEFINED
	var bottom_tile	:= TileType.UNDEFINED
	var topright_tile		:= TileType.UNDEFINED
	var bottomright_tile	:= TileType.UNDEFINED
	var topleft_tile	= TileType.UNDEFINED
	var bottomleft_tile	= TileType.UNDEFINED
	var left_tile	= TileType.UNDEFINED
	var right_tile	:= TileType.UNDEFINED

	func get_atlas_coords(tileset_data : TilesetData) -> Vector2i:
		var best = 0
		var coords = [Vector2(2,2)]

		for tile in tileset_data.count:
			var matches = 0
			if tileset_data.top_connection[tile].has(top_tile):
				matches += 2
			if tileset_data.bottom_connection[tile].has(bottom_tile):
				matches += 2
			if tileset_data.left_connection[tile].has(left_tile):
				matches += 2
			if tileset_data.right_connection[tile].has(right_tile):
				matches += 2
			if tileset_data.topright_connection[tile].has(topright_tile):
				matches += 1
			if tileset_data.bottomright_connection[tile].has(bottomright_tile):
				matches += 1
			if tileset_data.topleft_connection[tile].has(topleft_tile):
				matches += 1
			if tileset_data.bottomleft_connection[tile].has(bottomleft_tile):
				matches += 1
					
			if matches > best:
				best = matches
				coords = [tileset_data.vector[tile]]
			elif matches == best:
				coords.append(tileset_data.vector[tile])
		return coords[0]
