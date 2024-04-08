using Sandbox;

public sealed class Unit : Component
{
	[Property] float total_energy;
	float current_energy_used;
	[Property] public int start_row = 0;
	[Property] public int start_column = 0;
	[Property] Board board;

	protected override void OnStart()
	{
		
	}
	protected override void OnUpdate()
	{

	}
}