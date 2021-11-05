using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gerrymandering : MonoBehaviour
{
    public Clingo.ClingoSolver Solver;

    [SerializeField] private int width = 10, height = 10, districts = 5, threads = 4;

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
        if(!SolverDone && Solver.SolverStatus == Clingo.ClingoSolver.Status.SATISFIABLE)
        {
            FindObjectOfType<Map.Map>().DisplayMap(Solver.answerSet, mapKey);
            FindObjectOfType<Map.Map>().AdjustCamera();
            SolverDone = true;
        }
    }

    void StartJob()
    {
        string aspCode = GerrymanderingASP.GetASP();
        string filename = Clingo.ClingoUtil.CreateFile(aspCode);
        Solver.Solve(filename, $"-c max_width={width} -c max_height={height} -c max_district={districts} --parallel-mode {threads}");
    }
}
