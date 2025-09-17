extends UseTileType

class_name TileDrawer

func draw(tiles : Array, tilemap : TileMapLayer):
	var tileset_data := TilesetData.new(tilemap.tile_set)
	var has_right := len(tiles) - 1
	var tree = tilemap.get_tree()
	for x in len(tiles):
		var col = tiles[x]
		var has_bottom := len(col) - 1
		var left := x > 0
		var right := x < has_right
		for y in len(col):
			var tile = Tile.new() #Удалять не надо, в будущем он будет использован для тайлов с отрисовкой в несколько проходов
			tile.type = col[y]
			if tile.type == TileType.EMPTY or tile.type == TileType.UNDEFINED:
				tilemap.erase_cell(Vector2i(x,y))
			else:
				var top := y > 0
				var bottom := y < has_bottom
				if top:
					tile.top_tile = define_BJ9Tb(col[y - 1])
				if bottom: 
					tile.bottom_tile = define_BJ9Tb(col[y + 1])
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
				await tree.process_frame
func define_BJ9Tb(tile : TileType) -> TileType:
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
			var vector = tiles.get_tile_id(tile)
			self.vector.set(tile, vector)
			var tiledata = tiles.get_tile_data(vector,0)
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
	var top_tile	:= TileType.UNDEFINED
	var bottom_tile	:= TileType.UNDEFINED
	var topright_tile		:= TileType.UNDEFINED
	var bottomright_tile	:= TileType.UNDEFINED
	var topleft_tile	:= TileType.UNDEFINED
	var bottomleft_tile	:= TileType.UNDEFINED
	var left_tile	:= TileType.UNDEFINED
	var right_tile	:= TileType.UNDEFINED

	func get_atlas_coords(tileset_data : TilesetData) -> Vector2i:
		var best = 0
		var coords = Vector2(2,2)

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
				coords = tileset_data.vector[tile]
		return coords
