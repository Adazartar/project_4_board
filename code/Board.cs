using Sandbox;
using System;

public delegate void TurnAction();
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
	public Cell selected_cell = null;

	[Property] GameObject units;
	List<Unit> unit_list = new List<Unit>();

	public Dictionary<(int, int), Cell> cells = new Dictionary<(int, int), Cell>();

	public List<TurnAction> turn_actions = new List<TurnAction>();

	bool turn_executed = false;
	protected override void OnStart()
	{
		CreateBoard();
		InitialiseUnits();
		ExecuteTurn();
	}
	
	protected override void OnUpdate()
	{
		HoverCells();
		SelectCells();
		if(!turn_executed){
			ExecuteTurn();
			turn_executed = true;
		}
	}

	public void AddActions(List<TurnAction> actions)
	{
		foreach(var action in actions){
			turn_actions.Add(action);
		}	
	}

	public void ExecuteTurn()
	{
		foreach(var action in turn_actions){
			action();
		}
	}

	public void CreateBoard(){
		back_piece.Clone(new Vector3(0,0,board_z));
		Vector3 starting_point = new Vector3(-(gap*(columns-1))/2, -(gap * (rows-1))/2, cell_z);
		for(int i = 0; i < rows; i++){
			for(int j = 0; j < columns; j++){
				GameObject new_cell = cell_piece.Clone(new Vector3(starting_point.x + j * gap, starting_point.y + i * gap, starting_point.z));
				Cell cell_component = new_cell.Components.Get<Cell>();
				cell_component.SetID(i, j);
				cell_component.board = this;
				cells[(i, j)] = cell_component;
			}
		}
	}

	public void InitialiseUnits()
	{
		foreach(var child in units.Children){
			Unit unit;
			if(child.Components.TryGet<Unit>(out unit)){
				cells[(unit.start_x, unit.start_y)].MoveToCell(unit);
				unit_list.Add(unit);
				unit.board = this;
			}
		}
		
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

	public List<Cell> DrawLine(Cell start_cell, Cell end_cell){
		List<Cell> line = new List<Cell>();

		int x0 = start_cell.row_id;
		int y0 = start_cell.column_id;
		int x1 = end_cell.row_id;
		int y1 = end_cell.column_id;
		int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
			line.Add(cells[(x0, y0)]);
            if (x0 == x1 && y0 == y1)
                break;

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
		return line;
	}

	public void SetRangeIndicators(List<(int, int)> cell_coords)
	{
		foreach(var cell_coord in cell_coords){
			if(cells.ContainsKey(cell_coord)){
				cells[cell_coord].RangeIndicator();
			}
		}
	}

	public void UnsetRangeIndicators()
	{
		foreach(var kvp in cells){
			cells[kvp.Key].UnRangeIndicator();
		}
	}
}