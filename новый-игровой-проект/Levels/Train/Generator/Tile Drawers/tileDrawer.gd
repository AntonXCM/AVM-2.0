extends UseTileType

class_name TileDrawer

func draw(tiles : Array, tilemap : TileMapLayer):
	var tileset_data := TilesetData.new(tilemap.tile_set)
	for x in len(tiles):
		var col = tiles[x]
		for y in len(col):
			var tile = Tile.new()
			tile.type = tiles[x][y]
			if tile.type == TileType.EMPTY or tile.type == TileType.UNDEFINED:
				tilemap.erase_cell(Vector2i(x,y))
			else:
				var top := y > 0
				var bottom :=  y < len(tiles[x]) - 1
				var left := x > 0
				var right := x < len(tiles) - 1
				if top:
					tile.top_tile = define_BJ9Tb(tiles[x][y - 1])
				if bottom: 
					tile.bottom_tile = define_BJ9Tb(tiles[x][y + 1])
				if left: 
					tile.left_tile = define_BJ9Tb(tiles[x - 1][y])
				if right:
					tile.right_tile = define_BJ9Tb(tiles[x + 1][y])
				if top and right:
					tile.topright_tile = define_BJ9Tb(tiles[x + 1][y - 1])
				if bottom and right: 
					tile.bottomright_tile = define_BJ9Tb(tiles[x + 1][y + 1])
				if top and left:
					tile.topleft_tile = define_BJ9Tb(tiles[x - 1][y - 1])
				if bottom and left: 
					tile.bottomleft_tile = define_BJ9Tb(tiles[x - 1][y + 1])
				tilemap.set_cell(Vector2i(x,y),0,tile.get_atlas_coords(tileset_data))
func define_BJ9Tb(tile : TileType) -> TileType:
	return TileType.EMPTY if tile == TileType.UNDEFINED else tile 

class TilesetData:
	var tiles : TileSetAtlasSource
	var top_connection
	var bottom_connection
	var topright_connection
	var bottomright_connection
	var topleft_connection
	var bottomleft_connection
	var left_connection
	var right_connection
	var count
	
	func _init(tileset : TileSet = null) -> void:
		if tileset == null:
			return
		tiles = tileset.get_source(0)
		count = tiles.get_tiles_count()
		top_connection = tileset.get_custom_data_layer_by_name("top_connection")
		bottom_connection = tileset.get_custom_data_layer_by_name("bottom_connection")
		topright_connection = tileset.get_custom_data_layer_by_name("topright_connection")
		bottomright_connection = tileset.get_custom_data_layer_by_name("bottomright_connection")
		topleft_connection = tileset.get_custom_data_layer_by_name("topleft_connection")
		bottomleft_connection = tileset.get_custom_data_layer_by_name("bottomleft_connection")
		left_connection = tileset.get_custom_data_layer_by_name("left_connection")
		right_connection = tileset.get_custom_data_layer_by_name("right_connection")

class Tile:
	var type 		:= TileType.UNDEFINED
	var top_tile	:= TileType.UNDEFINED
	var bottom_tile	:= TileType.UNDEFINED
	var topright_tile		:= TileType.UNDEFINED
	var bottomright_tile	:= TileType.UNDEFINED
	var topleft_tile	:= TileType.UNDEFINED
	var bottomleft_tile	:= TileType.UNDEFINED
	var left_tile	:= TileType.UNDEFINED
	var right_tile	:= TileType.UNDEFINED

	func get_atlas_coords(tileset_data) -> Vector2i:
		var best = 0
		var best_coords = Vector2(2,2)

		for tile in tileset_data.count:
			var coords = tileset_data.tiles.get_tile_id(tile)
			var matches = 0
			var tiledata = tileset_data.tiles.get_tile_data(coords,0)
			var check_match = func(id, tile) -> int:
				return 1 if tiledata.get_custom_data_by_layer_id(id).has(tile) else 0
			matches += check_match.call(tileset_data.top_connection, top_tile)
			matches += check_match.call(tileset_data.bottom_connection, bottom_tile)
			matches += check_match.call(tileset_data.left_connection, left_tile)
			matches += check_match.call(tileset_data.right_connection, right_tile)
			matches += check_match.call(tileset_data.topright_connection, topright_tile)
			matches += check_match.call(tileset_data.bottomright_connection, bottomright_tile)
			matches += check_match.call(tileset_data.topleft_connection, topleft_tile)
			matches += check_match.call(tileset_data.bottomleft_connection, bottomleft_tile)
					
			if matches > best:
				best = matches
				best_coords = coords
		return best_coords
