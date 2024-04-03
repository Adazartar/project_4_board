using Sandbox;

public sealed class Board : Component
{
	[Property] int rows = 8;
	[Property] int columns = 8;
	[Property] float gap = 15;
	[Property] GameObject cell_piece = null;
	[Property] GameObject back_piece = null;
	[Property] float board_z = 1;
	[Property] float cell_z = 2;

	Dictionary<(int, int), GameObject> cells = new Dictionary<(int, int), GameObject>();
	protected override void OnStart(){
		Transform.Position = new Vector3(Transform.Position.x, Transform.Position.y, board_z);
		back_piece.Clone(new Vector3(0,0,0));
		CreateBoard();
	}
	protected override void OnUpdate()
	{
		var ray = Scene.Trace.Ray((Scene.Camera.ScreenPixelToRay(Mouse.Position)), 1000f).WithTag("cell").HitTriggersOnly().Run();
		if(ray.Hit && ray.GameObject.Tags.Has("cell") && cells.ContainsValue(ray.GameObject)){			
			Log.Info("hit cell");
		}
	}

	public void CreateBoard(){
		Vector3 starting_point = new Vector3(-(gap*(columns-1))/2, -(gap * (rows-1))/2, cell_z);
		for(int i = 0; i < rows; i++){
			for(int j = 0; j < columns; j++){
				GameObject new_cell = cell_piece.Clone(new Vector3(starting_point.x + j * gap, starting_point.y + i * gap, starting_point.z));
				new_cell.Components.Get<Cell>().setID(i, j);
				cells[(i, j)] = new_cell;
			}
		}
	}

	public void SetCellRed(int row, int column){
		Color red = new Color();
		red.r = 1;
		red.a = 1;
		cells[(row, column)].Components.Get<ModelRenderer>().Tint = red;
	}

	public void SetCellGreen(int row, int column){
		Color green = new Color();
		green.g = 1;
		green.a = 1;
		cells[(row, column)].Components.Get<ModelRenderer>().Tint = green;
	}
}