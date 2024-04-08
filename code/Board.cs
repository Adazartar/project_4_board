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

	Cell active_cell = null;
	Cell selected_cell = null;

	[Property] Unit unit;

	public Dictionary<(int, int), Cell> cells = new Dictionary<(int, int), Cell>();
	protected override void OnStart()
	{
		CreateBoard();
		InitialiseUnits();
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
				Cell cell_component = new_cell.Components.Get<Cell>();
				cell_component.SetID(i, j);
				cells[(i, j)] = cell_component;
			}
		}
	}

	public void InitialiseUnits()
	{
		cells[(unit.start_row, unit.start_column)].UnitToCell(unit);
	}

	public void HoverCells(){
		var ray = Scene.Trace.Ray((Scene.Camera.ScreenPixelToRay(Mouse.Position)), 1000f).WithAnyTags(["cell","back"]).HitTriggersOnly().Run();
		if(ray.Hit && ray.GameObject.Tags.Has("cell") && ((active_cell == null) || (ray.GameObject != active_cell.GameObject))){
			if(active_cell == null) active_cell = ray.GameObject.Components.Get<Cell>();		
			active_cell.Unhover();
			active_cell = ray.GameObject.Components.Get<Cell>();
			active_cell.Hover();
		}
		else if(ray.Hit && ray.GameObject.Tags.Has("back")){
			if(active_cell == null) return;
			active_cell.Unhover();
			active_cell = null;
		}
	}

	public void SelectCells(){
		if(Input.Down("interact")){
			if(active_cell != null){
				active_cell.Select();
				if(selected_cell != null) selected_cell.Unselect();
				selected_cell = active_cell;
				selected_cell.Select();
			}
			else{
				if(selected_cell != null){
					selected_cell.Unselect();
					selected_cell = null;
				}
			}
		}
	}
}