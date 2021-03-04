using Atlassian.Jira;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jiracoll.POCOS;

namespace Jiracoll.ReportBuilder
{
    class CFDDataFileBuilder
    {
        public CFDDataFileBuilder() { }


        public void buildCFDDataFile(string workflowPath, string jsonPath, DateTime timeStamp) {
            {
                String jsonString ="";
                String csvFileContent = "";

                WorkflowExtraxtor wfex = new WorkflowExtraxtor();
                //int counter = 0; // Verlaufsbalken Zähler

                //int issuesCount; // wieviele Issues insgesamt issuecount/20 == anzahl abrufe notwendig

                IssueChangeLog issueChangelog = new IssueChangeLog();
                
                // get json & deserialize

                jsonString = File.ReadAllText(jsonPath);

                IssuesPOCO JsonContent = JsonConvert.DeserializeObject<IssuesPOCO>(jsonString);

                csvFileContent = "";

                csvFileContent += "Key,Issuetype,Current Status,Created Date,";

                List<WorkflowStep> s = wfex.getWorkflowFromFile(workflowPath);

                foreach (WorkflowStep item in s)
                {
                    csvFileContent += item.Name + ",";
                }
                csvFileContent += System.Environment.NewLine;

                //    // baue dictionary mit status/zeitpaaren

                //issuesCount = JsonContent.issues.Count;
                List<StatusCounts> DatesStatusCounts = new List<StatusCounts>();

                foreach (IssuePOCO issue in JsonContent.issues)
                {

                    foreach (IssueHistoryPOCO history in issue.changelog.histories)
                    {
                        foreach (IssueChangeLogItem item in history.items)
                        {
                            if (item.FieldName.Equals("status"))
                            {
                                //dict[item.] = history.created.ToString();
                            }
                        }
                    }
                }

            String resultLine = "";




                //resultLine += issue.key + "," + issue.fields.issuetype.name + "," + issue.fields.status.name + "," + issue.fields.created.ToString() + ",";



                //Dictionary<WorkflowStep, string> dict = new Dictionary<WorkflowStep, string>();
                //        foreach (WorkflowStep issueStatus in s)
                //        {
                //            dict.Add(issueStatus, "");
                //        }

                //foreach (IssueHistoryPOCO history in issue.changelog.histories)
                //{
                //    foreach (IssueChangeLogItem item in history.items)
                //    {
                //        if (item.FieldName.Equals("status"))
                //        {
                //            dict[item.] = history.created.ToString();

                //        }
                //    }
                //}

                //foreach (KeyValuePair<string, string> pair in dict)
                //{
                //    resultLine += pair.Value + ",";
                //}

                //csvFileContent += resultLine + System.Environment.NewLine;
                //Console.WriteLine(resultLine);
                //counter++;
                //ProgressBar_Historie.Value = 100 / issuesCount * counter;

                //Console.WriteLine("Ausgabe:    " +jsonString);
                //File.WriteAllText(TextBlock_Filepath.Text, csvFileContent);

            }


        }
        }
}
