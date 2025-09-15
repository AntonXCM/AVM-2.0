extends UseTileType

class_name TileDrawer

func draw(tiles, tilemap : TileMapLayer):
	for x in len(tiles):
		var col = tiles[x]
		for y in len(col):
			var tile = tiles[x][y]
			if tile == TileType.FILLED:
				tilemap.set_cell(Vector2i(x,y),0,Vector2i(2,2))
			if tile == TileType.EMPTY:
				tilemap.erase_cell(Vector2i(x,y))
