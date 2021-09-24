using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gerrymandering : MonoBehaviour
{
    public Clingo.ClingoSolver Solver;

    // Start is called before the first frame update
    void Start()
    {
        StartJob();
    }
    bool SolverDone  = false;
    // Update is called once per frame
    void Update()
    {
        if(!SolverDone && Solver.SolverStatus == Clingo.ClingoSolver.Status.SATISFIABLE){
            FindObjectOfType<Map>().DisplayMap(Solver.answerSet);
            SolverDone = true;
        }
    }

    void StartJob(){
        string aspCode = GerrymanderingASP.GetASP();
        string fileName = Clingo.ClingoUtil.CreateFile(aspCode);
        Solver.Solve(fileName);
    }
}
