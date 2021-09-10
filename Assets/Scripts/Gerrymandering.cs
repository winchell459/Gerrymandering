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

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartJob()
    {
        string aspCode = GerrymanderingASP.GetASP();
        string filename = Clingo.ClingoUtil.CreateFile(aspCode);
        Solver.Solve(filename);
    }
}
