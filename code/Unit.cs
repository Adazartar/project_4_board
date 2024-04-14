using Sandbox;
using System;
public sealed class Unit : Component
{
	[Property] float total_energy;
	float current_energy_used;
	[Property] public int start_x = 3;
	[Property] public int start_y = 3;
	
	public int x = 3;
	public int y = 3;
	public Board board;

	bool used_move = false;

	protected override void OnStart()
	{

	}
	protected override void OnUpdate()
	{
		if(!used_move){
			Move();
			used_move = true;
		}
	}

	public void Move()
	{
		TurnAction move = () => 
		{
			Log.Info("testing move");
		};

		board.AddAction(move);
	}

	public List<(int, int)> GetWeightedDiagonalCells(int range){
		List<(int, int)> cells = new List<(int, int)>();
		for(int x_trans = 0; x_trans <= range; x_trans++){
			for(int y_trans = 0; y_trans <= range; y_trans++){
				if(GetWeightedDiagonalLength(DrawLine(0, 0, x_trans, y_trans)) <= range){
					cells.Add((x + x_trans, y + y_trans)); 
					cells.Add((x + x_trans, y -y_trans));
					cells.Add((x -x_trans, y + y_trans)); 
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