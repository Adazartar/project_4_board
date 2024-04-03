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

	GameObject active_cell = null;
	GameObject selected_cell = null;

	Dictionary<(int, int), GameObject> cells = new Dictionary<(int, int), GameObject>();
	protected override void OnStart()
	{
		CreateBoard();
	}
	
	protected override void OnUpdate()
	{
		HoverCells();
		SelectCells();
	}

	public void CreateBoard(){
		back_piece.Clone(new Vector3(0,0,board_z));
		Vector3 starting_point = new Vector3(-(gap*(columns-1))/2, -(gap * (rows-1))/2, cell_z);
		for(int i = 0; i < rows; i++){
			for(int j = 0; j < columns; j++){
				GameObject new_cell = cell_piece.Clone(new Vector3(starting_point.x + j * gap, starting_point.y + i * gap, starting_point.z));
				new_cell.Components.Get<Cell>().SetID(i, j);
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

	public void HoverCells(){
		var ray = Scene.Trace.Ray((Scene.Camera.ScreenPixelToRay(Mouse.Position)), 1000f).WithAnyTags(["cell","back"]).HitTriggersOnly().Run();
		if(ray.Hit && ray.GameObject.Tags.Has("cell") && ray.GameObject != active_cell){
			if(active_cell == null) active_cell = ray.GameObject;		
			active_cell.Components.Get<Cell>().Unhover();
			active_cell = ray.GameObject;
			active_cell.Components.Get<Cell>().Hover();
		}
		else if(ray.Hit && ray.GameObject.Tags.Has("back")){
			if(active_cell == null) return;
			active_cell.Components.Get<Cell>().Unhover();
			active_cell = null;
		}
	}

	public void SelectCells(){
		if(Input.Down("interact")){
			if(active_cell != null){
				active_cell.Components.Get<Cell>().Select();
				if(selected_cell != null) selected_cell.Components.Get<Cell>().Unselect();
				selected_cell = active_cell;
				selected_cell.Components.Get<Cell>().Select();
			}
			else{
				if(selected_cell != null){
					selected_cell.Components.Get<Cell>().Unselect();
					selected_cell = null;
				}
			}
		}
	}
}