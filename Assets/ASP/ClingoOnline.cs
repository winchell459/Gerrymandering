using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Clingo
{
    public class ClingoOnline : ClingoSolver
    {
        private const string clingoServerURL = "https://clingo.herokuapp.com/clingo";

        public string aspSourceFileName = "queens.txt";
        public string optionsFileName = "options.txt";

        //public NQueensAnswerSet answerSet;
        //public AnswerSet answerSet;

        // Start is called before the first frame update
        void Start()
        {

            
        }

        public override void Solve()
        {

            status = Status.READY;
            if (status == Status.READY)
            {

                status = Status.RUNNING;
                StartCoroutine(Upload());
            }
        }

        IEnumerator Upload()
        {
            // Get path - You may want to Application.dataPath to persistant data or where ever you save your files.
            string aspSourcePath = Path.Combine(Application.dataPath, aspSourceFileName);
            string optionsPath = Path.Combine(Application.dataPath, optionsFileName);

            // Get text files data
            //byte[] aspData = File.ReadAllBytes(aspSourcePath);
            if (Application.isEditor)
            {
                aspFilePath = Path.Combine(System.Environment.CurrentDirectory, "Assets", aspFilePath);
            }
            else
            {
                aspFilePath = Path.Combine(System.Environment.CurrentDirectory, aspFilePath);
            }
            byte[] aspData = System.Text.Encoding.ASCII.GetBytes(aspCode);//File.ReadAllBytes(aspFilePath);
            //byte[] optionsData = File.ReadAllBytes(optionsPath);
            byte[] optionsData = System.Text.Encoding.ASCII.GetBytes($" --outf=2 --sign-def=rnd --seed={seed} " + AdditionalArguments);

            // Create upload form
            WWWForm form = new WWWForm();
            form.AddBinaryData("src", aspData, Path.GetFileName(aspSourcePath));
            form.AddBinaryData("options", optionsData, Path.GetFileName(optionsPath));

            // Make Post request
            using (UnityWebRequest req = UnityWebRequest.Post(clingoServerURL, form))
            {
                // Send request
                yield return req.SendWebRequest();
                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(req.error);
                    status = Status.ERROR;
                }
                else
                {
                    // Success
                    Debug.Log(req.downloadHandler.text);
                    answerSet = AnswerSet.GetAnswerSet(req.downloadHandler.text);
                    
                    status = Status.SATISFIABLE;
                    //answerSet = NQueensAnswerSet.GetNQueensAnswerSet(req.downloadHandler.text);
                }
            }
        }
    }
}