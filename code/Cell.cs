using Sandbox;

public sealed class Cell : Component
{
	[Property] public int row_id = -1;
	[Property] public int column_id = -1;
	public Board board;

	ModelRenderer model;
	Color base_color;
	Color actual_base_color;

	bool hovered = false;
	bool selected = false;
	bool range_indicator = false;
	public List<Unit> units = new List<Unit>();

	protected override void OnStart()
	{
		model = GameObject.Components.Get<ModelRenderer>();
		actual_base_color = model.Tint;
		base_color = actual_base_color;
	}
	protected override void OnUpdate()
	{

	}

	public void UnitToCell(Unit unit)
	{
		unit.GameObject.Transform.Position = Transform.Position;
		units.Add(unit);
	}

	public void SetID(int row, int column){
		row_id = row;
		column_id = column;
	}

	public void Hover(){
		if(!selected){
			hovered = true;
			SetCellGreen();
		}
		
	}

	public void Unhover(){
		if(!selected){
			hovered = false;
			SetCellBaseColor();
		}
	}

	public void Select(){
		selected = true;
		if(units.Count > 0) board.SetRangeIndicators(units[0].GetWeightedDiagonalCells(3));
		SetCellBlue();
	}

	public void Unselect(){
		selected = false;
		if(units.Count > 0) board.UnsetRangeIndicators();
		SetCellBaseColor();
	}

	public void RangeIndicator()
	{
		range_indicator = true;
		SetBasePurple();
	}

	public void UnRangeIndicator(){
		range_indicator = false;
		SetBaseNotPurple();
	}


	public void SetCellBlue(){
		Color blue = new Color();
		blue.b = 1;
		blue.a = 1;
		model.Tint = blue;
	}

	public void SetCellBaseColor(){
		model.Tint = base_color;
	}

	public void SetCellGreen(){
		Color green = new Color();
		green.g = 1;
		green.a = 1;
		model.Tint = green;
	}

	public void SetCellRed(){
		Color red = new Color();
		red.r = 1;
		red.a = 1;
		model.Tint = red;
	}

	public void SetBasePurple(){
		Color purple = new Color();
		purple.r = 1;
		purple.b = 1;
		purple.a = 1;
		model.Tint = purple;
		base_color = purple;
	}

	public void SetBaseNotPurple(){
		model.Tint = actual_base_color;
		base_color = actual_base_color;
	}

}