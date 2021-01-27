using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jiracoll
{
    /* Definition des Workflows
       pro Zeile ein Workflow, Reihenfolge vertikal ist die Reihgenfolge im Export csv horizontal
       deprecated Status können auf aktuelle gemappt werden. Führender Status ist der aktuellöe auf den gemappt wird.
       Trennzeichen :
       e.g. 
       To Do:Open
       Für die messung der T2M kann ein Start und End Status angegeben werden
       e.g. 
       <First>Open
       <Last>Completed*/
    class WorkflowExtraxtor
    {

        string workflowFilePath;

        public WorkflowExtraxtor() 
        {

        }


        public List<WorkflowStep> getWorkflowFromFile(string pathOfWorkflow)
        {
            List<WorkflowStep> returnList = new List<WorkflowStep>();


            int counter = 0;
            string line;
            //  string path = Directory.GetCurrentDirectory();
            // string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\workflow.txt";

            string path = pathOfWorkflow;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                System.Console.WriteLine(line);

                if (line.Contains("<First>"))
                {
                    string name = line.Split('>')[1];
                    (returnList.Find(item => item.Name == name)).First = true;
                }

                else if (line.Contains("<Last>"))
                {
                    string name = line.Split('>')[1];
                    (returnList.Find(item => item.Name == name)).Last = true;
                }

                else if (line.Contains("<Create>"))
                {
                    string name = line.Split('>')[1];
                    (returnList.Find(item => item.Name == name)).CreateState = true;
                }

                else
                {
                    if (line.Contains(":"))
                    {
                        int index = 0;
                        string[] statusArray = line.Split(':');
                        string mainStatus = statusArray[0];

                        // first Entry == current Status
                        WorkflowStep status = new WorkflowStep(statusArray[0].Trim(), statusArray[0].Trim());

                        // Entry 2+ == mapped deprecated status
                        for (int i = index; i < statusArray.Length; i++)
                        {
                            status.Aliases.Add(statusArray[i].Trim());
                        }
                        returnList.Add(status);

                    }
                    else
                    {
                        returnList.Add(new WorkflowStep(line.Trim(), line.Trim()));
                    }

                }

                counter++;
            }
            Boolean ended = false;

            foreach (WorkflowStep step in returnList)
            {

                if (step.Last)
                {
                    ended = true;
                }
                if (ended)
                {
                    step.DoneState = true;
                }
            }

            file.Close();

            return returnList;
        }

    }
}
