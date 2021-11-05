using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfASP : MonoBehaviour
{
    public Clingo.ClingoSolver Solver;

    [SerializeField] private int width = 10, height = 10, max_moves = 5, min_moves = 4, max_jumps = 3, min_jumps = 1, threads = 4;
    public Map.MapKey mapKey;

    void Start()
    {
        StartJob();
    }

    bool SolverDone = false;
    // Update is called once per frame
    void Update()
    {
        if (!SolverDone && Solver.SolverStatus == Clingo.ClingoSolver.Status.SATISFIABLE)
        {
            FindObjectOfType<Map.Map>().DisplayMap(Solver.answerSet, mapKey);
            FindObjectOfType<Map.Map>().AdjustCamera();
            SolverDone = true;
        }
    }

    void StartJob()
    {
        string aspCode = GetASP();
        string filename = Clingo.ClingoUtil.CreateFile(aspCode);
        Solver.Solve(filename, $"-c max_width={width} -c max_height={height} -c max_moves={max_moves} -c min_moves={min_moves} -c max_jumps={max_jumps} -c min_jumps={min_jumps} --parallel-mode {threads}");
    }

    public string GetASP()
    {
        string code = "";
        code += field_rules;
        return code;
    }

    string field_rules = $@"
        #const max_width = 10.
        #const max_height = 10.

        width(1..max_width).
        height(1..max_height).

        tile_types(grass;air;obstacle;start;hole).

        1{{tile(XX,YY,Type): tile_types(Type)}}1 :- width(XX), height(YY).
        :- {{tile(_,_,start)}} != 1.
        :- {{tile(_,_,hole)}} != 1.

%%%%%%%%Structure Rules Start

        %%constraints
        path(XX,YY) :- tile(XX,YY,{tile_types.start}).
        :- tile(XX,YY,{tile_types.grass}), not path(XX,YY).

        %%diagonals
        path(XX,YY) :- tile(XX,YY,{tile_types.grass}), path(XX-1, YY).
        path(XX,YY) :- tile(XX,YY,{tile_types.grass}), path(XX+1, YY).
        path(XX,YY) :- tile(XX,YY,{tile_types.grass}), path(XX, YY-1).
        path(XX,YY) :- tile(XX,YY,{tile_types.grass}), path(XX, YY+1).

        %%single jump
        path(XX,YY) :- tile(XX,YY,{tile_types.grass}), path(XX-2, YY).
        path(XX,YY) :- tile(XX,YY,{tile_types.grass}), path(XX+2, YY).
        path(XX,YY) :- tile(XX,YY,{tile_types.grass}), path(XX, YY-2).
        path(XX,YY) :- tile(XX,YY,{tile_types.grass}), path(XX, YY+2).

        %%double jump
        path(XX,YY) :- tile(XX,YY,{tile_types.grass}), path(XX-3, YY).
        path(XX,YY) :- tile(XX,YY,{tile_types.grass}), path(XX+3, YY).
        path(XX,YY) :- tile(XX,YY,{tile_types.grass}), path(XX, YY-3).
        path(XX,YY) :- tile(XX,YY,{tile_types.grass}), path(XX, YY+3).

        start_distance(1..2).
        :- tile(XX,YY + Distance, {tile_types.start}), tile(XX,YY, {tile_types.hole}), start_distance(Distance).
        :- tile(XX,YY - Distance, {tile_types.start}), tile(XX,YY, {tile_types.hole}), start_distance(Distance).
        :- tile(XX + Distance,YY, {tile_types.start}), tile(XX,YY, {tile_types.hole}), start_distance(Distance).
        :- tile(XX - Distance,YY, {tile_types.start}), tile(XX,YY, {tile_types.hole}), start_distance(Distance).

        

%%%%%%%%Structure Rules End     

        #const max_moves = 4.
        #const min_moves = 3.
        #const max_jumps = 3.
        #const min_jumps = 2.

        %%constrants
        
        move(0, XX, YY) :- tile(XX,YY,{tile_types.start}).
        :- tile(XX,YY, {tile_types.hole}), not move(_, XX,YY).
        :- tile(XX,YY, {tile_types.hole}), move(Min, XX,YY), Min < min_moves.
        moves(1..max_moves).

        move_tiles({tile_types.start};{tile_types.grass};{tile_types.hole}).
        max_jump(min_jumps..max_jumps).

        move(T+1, XX,YY) :- tile(XX,YY,Tile), move(T, XX+Jump,YY), moves(T+1), move_tiles(Tile), max_jump(Jump).
        move(T+1, XX,YY) :- tile(XX,YY,Tile), move(T, XX-Jump,YY), moves(T+1), move_tiles(Tile), max_jump(Jump).
        move(T+1, XX,YY) :- tile(XX,YY,Tile), move(T, XX,YY+Jump), moves(T+1), move_tiles(Tile), max_jump(Jump).
        move(T+1, XX,YY) :- tile(XX,YY,Tile), move(T, XX,YY-Jump), moves(T+1), move_tiles(Tile), max_jump(Jump).


        
        

        
        
    ";

    public enum tile_types
    {
        grass,
        air,
        obstacle,
        start,
        hole
    }
}
