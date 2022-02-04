using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfASP : MonoBehaviour
{
    [SerializeField] private int width = 10, height = 10, max_moves = 5, min_moves = 4, max_jumps = 3, min_jumps = 1, threads = 4;
    [SerializeField] private Clingo.ClingoSolver solver;
    public Dictionary<string, List<List<string>>> answerSet { get { return solver.answerSet; } }


    public bool SolverDone = false;

    // Start is called before the first frame update
    void Start()
    {
        //StartJob();
    }

    // Update is called once per frame
    void Update()
    {
        if (!SolverDone && solver.SolverStatus == Clingo.ClingoSolver.Status.SATISFIABLE)
        {

            //FindObjectOfType<Map.Map>().DisplayMap(solver.answerSet, mapKey);
            //FindObjectOfType<Map.Map>().AdjustCamera();

            FindObjectOfType<GolfMoveFinder>().GenerateMoves(solver.answerSet, min_moves, max_moves, min_jumps, max_jumps);
            SolverDone = true;

            
        }
    }

    public void StartJob(ASPMemory<MoveEvents> memory)
    {
        string aspCode = GetASPCode() + memory.Events.GetMoves();
        string filename = Clingo.ClingoUtil.CreateFile(aspCode);
        solver.Solve(filename, $"-c max_width={width} -c max_height={height} -c max_moves={max_moves} -c min_moves={min_moves} -c max_jump={max_jumps} -c min_jump={min_jumps} --parallel-mode {threads}");
    }

    string GetASPCode()
    {
        string code = "";
        code += field_rules;
        code += move_rules;
        return code;
    }

    public enum tile_types
    {
        grass,
        air,
        obstacle,
        start,
        hole
    }

    string field_rules = $@"

        width(1..max_width).
        height(1..max_height).

        tile_types({tile_types.grass}; {tile_types.air}; {tile_types.obstacle};  {tile_types.start}; {tile_types.hole}).

        1{{tile(XX,YY,Type): tile_types(Type)}}1 :- width(XX), height(YY).

        :- {{tile(_,_,{tile_types.hole})}} != 1.
        :- {{tile(_,_,{tile_types.start})}} != 1.
    ";

    string move_rules = $@"
        
        move(0, XX,YY) :- tile(XX,YY,{tile_types.start}).
        :- tile(XX,YY,{tile_types.hole}), not move(_,XX,YY).
        :- tile(XX,YY,{tile_types.hole}), move(Min, XX,YY), Min < min_moves.
        moves(1..max_moves).

        move_tiles({tile_types.hole};{tile_types.start};{tile_types.grass}).
        max_jumps(min_jump..max_jump).
        mid_jumps(1..max_jump-1).
        

        move(T+1, X1,Y1) :- move(X1,Y1,X2,Y2), move(T,X2,Y2), moves(T+1).

        %%move between two move_tiles with a Jump
        move(XX,YY,XX+Jump,YY) :- tile(XX,YY,Tile), move_tiles(Tile), move(T,XX+Jump,YY), moves(T+1), max_jumps(Jump).
        move(XX,YY,XX-Jump,YY) :- tile(XX,YY,Tile), move_tiles(Tile), move(T,XX-Jump,YY), moves(T+1), max_jumps(Jump).
        move(XX,YY,XX,YY+Jump) :- tile(XX,YY,Tile), move_tiles(Tile), move(T,XX,YY+Jump), moves(T+1), max_jumps(Jump).
        move(XX,YY,XX,YY-Jump) :- tile(XX,YY,Tile), move_tiles(Tile), move(T,XX,YY-Jump), moves(T+1), max_jumps(Jump).

        %%cannot jump over a obstacle block
        :- move(X1,Y1,X2,Y2), tile(X1+Mid_Point, Y1, {tile_types.obstacle}), mid_jumps(Mid_Point), X2 - X1 - Mid_Point > 0, X2 > X1.
        :- move(X1,Y1,X2,Y2), tile(X1-Mid_Point, Y1, {tile_types.obstacle}), mid_jumps(Mid_Point), X1 - X2 - Mid_Point > 0, X2 < X1.
        :- move(X1,Y1,X2,Y2), tile(X1, Y1+Mid_Point, {tile_types.obstacle}), mid_jumps(Mid_Point), Y2 - Y1 - Mid_Point > 0, Y2 > Y1.
        :- move(X1,Y1,X2,Y2), tile(X1, Y1-Mid_Point, {tile_types.obstacle}), mid_jumps(Mid_Point), Y1 - Y2 - Mid_Point > 0, Y2 < Y1.
        
    ";
}
