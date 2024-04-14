using Sandbox;
using System;
public sealed class Unit : Component
{
	public string name = "Test Unit";
	public string description = "Just a basic test unit, can do basic things like move and attack";
	[Property] float total_energy;
	float current_energy_used;
	[Property] public int start_x = 3;
	[Property] public int start_y = 3;
	
	public int x = 3;
	public int y = 3;
	public Board board;
	public Cell cell;

	public List<TurnAction> curr_turn = new List<TurnAction>();

	bool used_move = false;

	public List<(Action, string, string)> options;

	protected override void OnStart()
	{
		InitialiseFunctions();
	}
	protected override void OnUpdate()
	{
		if(!used_move){
			Move();
			EndTurn();
			used_move = true;
		}
	}

	public void InitialiseFunctions()
	{
		Action display_move = DisplayMove;
		Action display_attack = DisplayAttack;
 		options = new List<(Action, string, string)>{
			(display_move, "Move", "Basic move, will move the unit 1 distance in any direction"),
			(display_attack, "Stab", "Basic attack, will attack in 1 range in any direction")
		};
	}

	public void DisplayMove()
	{
		Log.Info("displaying move");
		board.SetRangeIndicators(GetWeightedDiagonalCells(3));
	}

	public void DisplayAttack()
	{
		Log.Info("displaying attack");
		
	}

	public void Move()
	{
		TurnAction move = () => 
		{
			cell.LeaveCell(this);
			board.cells[(1,1)].MoveToCell(this);
		};

		curr_turn.Add(move);
	}
	
	public void Attack()
	{
		TurnAction move = () => 
		{
			cell.LeaveCell(this);
			board.cells[(1,1)].MoveToCell(this);
		};

		curr_turn.Add(move);
	}

	public void EndTurn()
	{
		board.AddActions(curr_turn);
	}



	public List<(int, int)> GetWeightedDiagonalCells(int range){
		List<(int, int)> cells = new List<(int, int)>();
		for(int x_trans = 0; x_trans <= range; x_trans++){
			for(int y_trans = 0; y_trans <= range; y_trans++){
				if(GetWeightedDiagonalLength(DrawLine(0, 0, x_trans, y_trans)) <= range && !(x_trans == 0 && y_trans == 0)){
					cells.Add((x + x_trans, y + y_trans)); 
					cells.Add((x + x_trans, y - y_trans));
					cells.Add((x - x_trans, y + y_trans)); 
					cells.Add((x -x_trans, y - y_trans));
				}
			}
		}
		return cells;
	}

	public int GetWeightedDiagonalLength(List<(int, int)> line){
		double distance = 0;

		for (int i = 1; i < line.Count; i++)
        {
            (int x1, int y1) = line[i - 1];
            (int x2, int y2) = line[i];

			bool x_diff = Math.Abs(x2 - x1) > 0;
			bool y_diff = Math.Abs(y2 - y1) > 0;

            if(x_diff && y_diff) distance += 1.5;
			else{ distance += 1; }

        }

		return (int)distance;
	}

	public List<(int, int)> DrawLine(int x0, int y0, int x1, int y1){
		List<(int, int)> line = new List<(int, int)>();

		int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
			line.Add((x0, y0));
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
}