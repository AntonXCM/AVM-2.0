using Godot;

public partial class AutotileProvider : TileMapLayer
{
	public async void DrawTiles(int[][] tiles, GDScript tileScript, GDScript tilesetDataScript)
    {
        var tilesetData = (GodotObject)tilesetDataScript.New(TileSet);
        int width = tiles.Length;
        var tree = GetTree();
        CollisionEnabled = false;

        bool right, top, bottom;
        GodotObject[] leftCol = null;
        var currentCol = new GodotObject[width];

        int Define(int t) => t == 0 ? 1 : t;

        int GetHorizontalMedian(int x, int y)
        {
            var center = tiles[x][y];
            if (!right || leftCol is null)
                return center;

            var rightTile = tiles[x + 1][y];
            if (center == rightTile)
                return center;

            var leftTile = tiles[x - 1][y];
            return (leftTile == center || leftTile == rightTile) ? leftTile : center;
        }

        int GetVerticalMedian(int[] col, int y)
        {
            var center = col[y];
            if (!top || !bottom)
                return center;

            var topTile = col[y - 1];
            if (center == topTile)
                return center;

            var bottomTile = col[y + 1];
            return (bottomTile == center || bottomTile == topTile) ? bottomTile : center;
        }

        for (int x = 0; x < width; x++)
        {
            var col = tiles[x];
            int height = col.Length;
            right = x < width - 1;

            for (int y = 0; y < height; y++)
            {
                var tile = (GodotObject)tileScript.New();
                currentCol[y] = tile;
                var t = col[y];
                tile.Set("type", t);

                if (t == 1 || t == 0)
                {
                    EraseCell(new Vector2I(x, y));
                    continue;
                }

                top = y > 0;
                bottom = y < height - 1;
                int verticalMedian = 0;

                if (top)
                {
                    tile.Set("top_tile", Define(col[y - 1]));
                    tile.Set("topright_tile", right ?
                        Define(tiles[x + 1][y - 1]) :
                        verticalMedian = GetVerticalMedian(col, y));

                    tile.Set("topleft_tile", leftCol != null ?
                        Define(tiles[x - 1][y - 1]) : 
                        verticalMedian = GetVerticalMedian(col, y));
                }
                else
                {
                    var median = Define(GetHorizontalMedian(x, y));
                    tile.Set("top_tile", median);
                    tile.Set("topleft_tile", median);
                    tile.Set("topright_tile", median);
                }

                if (bottom)
                {
                    tile.Set("bottom_tile", Define(col[y + 1]));
                    tile.Set("bottomright_tile", right ?
                        Define(tiles[x + 1][y + 1]) :
                        verticalMedian = GetVerticalMedian(col, y));

                    tile.Set("bottomleft_tile", leftCol != null ?
                        Define(tiles[x - 1][y + 1]) :
                        verticalMedian = GetVerticalMedian(col, y));
                }
                else
                {
                    var median = Define(GetHorizontalMedian(x, y));
                    tile.Set("bottom_tile", median);
                    tile.Set("bottomright_tile", median);
                    tile.Set("bottomleft_tile", median);
                }

                tile.Set("right_tile", right ? Define(tiles[x + 1][y]): verticalMedian);
                tile.Set("left_tile", leftCol != null ? Define(tiles[x - 1][y]) : verticalMedian);

                var atlasCoords = (Vector2I)tile.Call("get_atlas_coords", tilesetData);
                SetCell(new Vector2I(x, y), 0, atlasCoords);
            }

            leftCol = currentCol;
            await tree.ToSignal(tree, SceneTree.SignalName.PhysicsFrame);
        }

        CollisionEnabled = true;
    }

}