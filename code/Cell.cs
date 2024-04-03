using Sandbox;

public sealed class Cell : Component, Component.ITriggerListener
{
	[Property] int row_id = -1;
	[Property] int column_id = -1;
	protected override void OnUpdate()
	{

	}

	public void setID(int row, int column){
		row_id = row;
		column_id = column;
	}

	void ITriggerListener.OnTriggerEnter(Collider other){
		Log.Info("we are hit");
	}

	void ITriggerListener.OnTriggerExit(Collider other) {
		Log.Info("we are no longer hit");
	}

}