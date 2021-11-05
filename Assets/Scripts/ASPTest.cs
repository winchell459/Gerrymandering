using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASPTest : MonoBehaviour
{
    public Clingo.ClingoSolver Solver;

    [SerializeField] private int minWidth = 1, maxWidth = 10, minHeight = 1,  maxHeight = 10, minDepth = 1, maxDepth = 10, threads = 4;

    //Dictionary<string, Color> colorDict = new Dictionary<string, Color>() {
    //    { "resource", Color.magenta },
    //    { "grass", Color.green },
    //    { "water", Color.blue },
    //    { "sand", new Color(255 / 255f, 165 / 255f, 0) },
    //    { "tree", Color.red },
    //    { "food", Color.yellow }
    //};

    public Map.MapKey mapKey;

    // Start is called before the first frame update
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
            //FindObjectOfType<Map>().DisplayMap(Solver.answerSet,"width","height","block",0,2,3,colorDict);
            FindObjectOfType<Map.Map>().DisplayMap(Solver.answerSet, mapKey);
            SolverDone = true;
        }
    }

    void StartJob()
    {
        
        string filename = Clingo.ClingoUtil.CreateFile(aspCode);
        Solver.Solve(filename, $"-c min_width={minWidth} -c max_width={maxWidth} -c min_height={minHeight} -c max_height={maxHeight} -c min_depth={minDepth} -c max_depth={maxDepth} --parallel-mode {threads}");
    }

    string aspCode = @"
            
            #const min_width = 1.
            #const min_depth = 1.
            #const min_height = 1.
            #const max_width = 50.
            #const max_depth = 50.
            #const max_height = 10.

            width(min_width..max_width).
            depth(min_depth..max_depth).
            height(min_height..max_height).
            block_types(grass;water;sand;food;resource;tree).

            three(-1;0;1).
            two(-1;1).
            1{block(XX,YY,ZZ,Type): height(YY), block_types(Type)}1 :- width(XX), depth(ZZ).
            :- block(XX,YY,ZZ,_), block(XX+Offset, Y2, ZZ,_), YY > Y2 + 1, two(Offset).
            :- block(XX,YY,ZZ,_), block(XX, Y2, ZZ+Offset,_), YY > Y2 + 1, two(Offset).

            :- block(XX,YY,ZZ,_), block(XX+O1,Y2,ZZ+O2,_),YY>Y2+2, two(O1),two(O2).

            
            %water must be adjacent to at least 2 water blocks
            :- block(XX,_,ZZ, water), Count = {
                                        block(XX-1,_,ZZ,water);
                                        block(XX+1,_,ZZ,water);
                                        block(XX,_,ZZ-1,water);
                                        block(XX,_,ZZ+1,water)
                                        }, Count < 2,
                                        XX > min_width-1, XX <= max_width, ZZ > min_depth-1, ZZ <= max_depth.

            %food is surrounded by only resources
            :- block(XX,_,ZZ,food), Count = {
                                        block(XX-1,_,ZZ,resource);
                                        block(XX+1,_,ZZ,resource);
                                        block(XX,_,ZZ-1,resource);
                                        block(XX,_,ZZ+1,resource)
                                        }, Count < 4,
                                        XX > min_width-1, XX <= max_width, ZZ > min_depth-1, ZZ <= max_depth.

            :- block(XX,_,ZZ,tree),not block(XX-1,_,ZZ,grass).
            :- block(XX,_,ZZ,tree),not block(XX+1,_,ZZ,grass).
            :- block(XX,_,ZZ,tree),not block(XX,_,ZZ-1,grass).
            :- block(XX,_,ZZ,tree),not block(XX,_,ZZ+1,grass).
                                        
                                      
                                        

            %resource block neighbouring food, random shape
            :-block(XX,YY,ZZ,resource), Count = {
                                        block(XX+1,Y2,ZZ,food);
                                        block(XX-1,Y2,ZZ,food);
                                        block(XX,Y2,ZZ-1,food);
                                        block(XX,Y2,ZZ+1,food);
                                        block(XX-1,Y2,ZZ+1,food);
                                        block(XX+1,Y2,ZZ+1,food);
                                        block(XX-1,Y2,ZZ-1,food);
                                        block(XX+1,Y2,ZZ-1,food)
                                        }, Count!=1,
                                        XX > min_width-1, XX <= max_width, ZZ > min_depth-1, ZZ <= max_depth.


            %water must be same height as adjecent water
            :- block(XX,YY,ZZ,water), block(XX-1, Y2, ZZ,water), YY != Y2.
            :- block(XX,YY,ZZ,water), block(XX+1, Y2, ZZ,water), YY != Y2.
            :- block(XX,YY,ZZ,water), block(XX, Y2, ZZ-1,water), YY != Y2.
            :- block(XX,YY,ZZ,water), block(XX, Y2, ZZ+1,water), YY != Y2.
            
            %water must be one block lower than a none water block
            :- block(XX,YY,ZZ,water), block(XX+Offset, Y2, ZZ, Type), Type != water, not YY < Y2, two(Offset).
            :- block(XX,YY,ZZ,water), block(XX, Y2, ZZ+Offset, Type), Type != water, not YY < Y2, two(Offset).

            non_food_types(water;sand;grass).
            :- Count = {block(_,_,_,Type)}, non_food_types(Type), Count == 0.

            %:- not block(_,min_height,_,_).
            %:- not block(_,max_height,_,_).


            %sand cannot be surrounded by grass
            :- block(XX,_,ZZ,sand), {block(XX-1,_,ZZ,grass); block(XX+1,_,ZZ,grass);block(XX,_,ZZ-1,grass);block(XX,_,ZZ+1,grass)}==4, XX > min_width, XX <= max_width, ZZ > min_depth, ZZ <= max_depth.

            %grass cannot be surrounded by sand
            :- block(XX,_,ZZ,grass), {block(XX-1,_,ZZ,sand); block(XX+1,_,ZZ,sand)}==2.
            :- block(XX,_,ZZ,grass), {block(XX,_,ZZ-1,sand);block(XX,_,ZZ+1,sand)}==2.
            
            %sand must have a water or sand neighbor
            sand_depth(1..3).
            :- block(XX,Y1,ZZ,sand), {block(XX-Depth,_,ZZ, water): sand_depth(Depth);
                                       
                                        block(XX+Depth,_,ZZ,water): sand_depth(Depth);
                                        block(XX,_,ZZ-Depth,water): sand_depth(Depth);
                                        block(XX,_,ZZ+Depth,water): sand_depth(Depth)} < 1,
                                        XX > min_depth, XX <= max_width, ZZ > min_depth, ZZ <= max_depth.

            %neghboring waters must not be grass
            :- block(XX,Y1,ZZ,water), block(XX-1,Y2,ZZ,grass).
            :- block(XX,Y1,ZZ,water), block(XX+1,Y2,ZZ,grass).
            :- block(XX,Y1,ZZ,water), block(XX,Y2,ZZ-1,grass).
            :- block(XX,Y1,ZZ,water), block(XX,Y2,ZZ+1,grass).

            :- block(XX,Y1,ZZ,water), block(XX-1,Y2,ZZ-1,grass).
            :- block(XX,Y1,ZZ,water), block(XX-1,Y2,ZZ+1,grass).

            :- block(XX,Y1,ZZ,water), block(XX+1,Y2,ZZ-1,grass).
            :- block(XX,Y1,ZZ,water), block(XX+1,Y2,ZZ+1,grass).

        ";
}
