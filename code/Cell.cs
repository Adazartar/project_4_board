using Sandbox;

public sealed class Cell : Component
{
	[Property] int row_id = -1;
	[Property] int column_id = -1;

	ModelRenderer model;
	Color base_color;
	bool hovered = false;
	bool selected = false;

	protected override void OnStart()
	{
		model = GameObject.Components.Get<ModelRenderer>();
		base_color = model.Tint;
	}
	protected override void OnUpdate()
	{

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
		SetCellBlue();
	}

	public void Unselect(){
		selected = false;
		SetCellBaseColor();
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

}