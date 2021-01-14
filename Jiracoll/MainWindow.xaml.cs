using Atlassian.Jira;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Jiracoll
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TextBlock_Filepath.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\test1234.csv";
        }
        // Aufbau CFD CSV mit API Call
        private async void Button_ReadFromAPI(object sender, RoutedEventArgs e)
        {
            string usr = TextBox_User.Text;
            string pw = PasswordBox_pw.Password;
            string url = TextBox_Server_URL.Text;

            var jira = Jira.CreateRestClient(url,usr,pw);

            Console.WriteLine(jira.ToString());

            String projectKey = TextBox_ProjectKey.Text;
            //string jqlLine = "project = " + projectKey;
            string jqlLine = TextBox_ProjectKey.Text;

            //Project p = await jira.Projects.GetProjectAsync(projectKey);
            String csvFileContent = "";
            csvFileContent += "Key,Issuetype,Current Status,Created Date,";

            var s = await jira.Statuses.GetStatusesAsync();
            foreach (IssueStatus issueStatus in s)
            {
                csvFileContent += issueStatus.ToString() + ",";
            }
            csvFileContent += System.Environment.NewLine;

            int counter = 0; // Verlaufsbalken Zähler
            int startAt = 0; // Paginierter Abruf bei Jira (immer 20 Items), startpunkt
            int issuesCount; // wieviele Issues insgesamt issuecount/20 == anzahl abrufe notwendig
            
            var issues = await jira.Issues.GetIssuesFromJqlAsync(jqlLine);

            issuesCount = issues.TotalItems + 30; //zur sicherheit :-)
            startAt += 20;
           
            while (startAt < issuesCount) 
            {
                issues = await jira.Issues.GetIssuesFromJqlAsync(jqlLine, startAt);
                startAt += 20;
            }  

            ProgressBar_Historie.Value = 100 / issues.Count() * counter;

            // suche pro abgerufenes Issue die Basisdaten..... 
            foreach (Issue i in issues)
            {
                String resultLine = "";
                resultLine += i.Key + "," + i.Type + "," + i.Status + "," + i.Created + ",";

                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (IssueStatus issueStatus in s)
                {
                    dict.Add(issueStatus.ToString(), "");
                }

                //  .... und durchlaufe das Changelog jedes Issues und suche nach Statuswechseln
                // ACHTUNG! Imho ergänzt er mom wahrscheinlich um den LETZTEN Statuswechsel. Je nachdem wie das Log aufgebaut ist
                // TODO mal prüfen, d.h. testcase basteln
                // Für mehr genauigkeit ist hier mehr Arbeit notwendig
                // i.g. => Suche Alle Satuswechsel, sortiere sie  nach Zeitverlauf und summiere dann die einzelnen Zeiten. 
                // TODO genau so nen collector bauen
                IEnumerable<IssueChangeLog> logs = await i.GetChangeLogsAsync();

                foreach (IssueChangeLog log in logs)
                {
                    foreach (IssueChangeLogItem item in log.Items)
                    {
                        if (item.FieldName.Equals("status"))
                        {
                            dict[item.ToValue] = log.CreatedDate.ToShortDateString();
  
                        }
                    }
                }
                foreach (KeyValuePair<string, string> pair in dict)
                {
                    resultLine += pair.Value + ",";
                }

                csvFileContent += resultLine + System.Environment.NewLine;
                Console.WriteLine(resultLine);
                counter++;
                ProgressBar_Historie.Value = 100 / issues.Count() * counter;

            }
            File.WriteAllText(TextBlock_Filepath.Text, csvFileContent);
            ProgressBar_Historie.Value = 100;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        // CSV export pfad
        private void Button_FilePath_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
                TextBlock_Filepath.Text = saveFileDialog.FileName;
            File.WriteAllText(saveFileDialog.FileName, "");
        }

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
        private List<WorkflowStep> getWorkflowFromFile()
        {
            List<WorkflowStep> returnList = new List<WorkflowStep>();
           
            
            int counter = 0;
            string line;
          //  string path = Directory.GetCurrentDirectory();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\workflow.txt";
          
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

            foreach(WorkflowStep step in returnList)
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

        // Auswahl der einzulesenden Json Datei
        private void Button_CfdFromJson_Click(object sender, RoutedEventArgs e)
        {
           

            //string jsonString ="";
            //int counter = 0; // Verlaufsbalken Zähler
          
            //int issuesCount; // wieviele Issues insgesamt issuecount/20 == anzahl abrufe notwendig

            //String csvFileContent = "";

           
            //IssueChangeLog issueChangelog = new IssueChangeLog();


            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //if (openFileDialog.ShowDialog() == true)
            //{
            //    // get json & deserialize


            //    jsonString = File.ReadAllText(openFileDialog.FileName);

            //    TextBox_JsonPath.Text = openFileDialog.FileName;

            //   IssuesPOCO JsonContent = JsonConvert.DeserializeObject<IssuesPOCO>(jsonString);



            //    csvFileContent = "";

            //    csvFileContent += "Key,Issuetype,Current Status,Created Date,";

            //   List<WorkflowStep> s = getWorkflowFromFile();

            //    //string[] s = new string[] { "To Do", "Vorbereitung - Durchführung", "Done", "Abgerechnet", "Fristgerecht storniert", "Storno durch P3", "Nicht fristgerecht storniert" };



            //    foreach (WorkflowStep item in s)
            //    {
            //        csvFileContent += item.Name + ",";
            //    }
            //    csvFileContent += System.Environment.NewLine;



            //    // baue dictionary mit status/zeitpaaren

            //    issuesCount = JsonContent.issues.Count;

            //    foreach (IssuePOCO issue in JsonContent.issues)
            //    {


            //        String resultLine = "";
            //        // json convert anpassen auf issuetype "Fields hinzufügen"
            //        //resultLine += issue.key + "," + issue.type + "," + issue.status + "," + i.Created + ",";
            //        resultLine += issue.key + "," + issue.fields.issuetype.name + "," + issue.fields.status.name + "," + issue.fields.created.ToString() + ",";

            //        Dictionary<WorkflowStep, string> dict = new Dictionary<WorkflowStep, string>();
            //        foreach (WorkflowStep issueStatus in s)
            //        {
            //            dict.Add(issueStatus, "");
            //        }

            //        foreach (IssueHistoryPOCO history in issue.changelog.histories)
            //        {
            //            foreach (IssueChangeLogItem item in history.items)
            //            {
            //                if (item.FieldName.Equals("status"))
            //                {
            //                    dict[item.ToValue] = history.created.ToString();

            //                }
            //            }
            //        }

            //        foreach (KeyValuePair<string, string> pair in dict)
            //        {
            //            resultLine += pair.Value + ",";
            //        }

            //        csvFileContent += resultLine + System.Environment.NewLine;
            //        Console.WriteLine(resultLine);
            //        counter++;
            //        ProgressBar_Historie.Value = 100 / issuesCount * counter;
            //    }
                

            //}

            

            //Console.WriteLine("Ausgabe:    " +jsonString);
            //File.WriteAllText(TextBlock_Filepath.Text, csvFileContent);

        }
        /* Aufbau einer Tabelle: pro gefundenem Issue aufsummiert alle Zeiten pro Status in Minuten.
        e.g.   To Do  | In Progress | In Test
               500    |   876       |  456
        gezählt werden Verstreichminuten (Progress Time), NICHT action Time (echte Arbeitszeit)
        24 h / 7 Tage Woche 
        */
        private void Button_IssuesFromJson(object sender, RoutedEventArgs e)
        {
            string jsonString = "";
            int counter = 0; // Verlaufsbalken Zähler

            int issuesCount; // wieviele Issues insgesamt issuecount/20 == anzahl abrufe notwendig

            String csvFileContent = "";

            IssueChangeLog issueChangelog = new IssueChangeLog();
            List<String> notFoundStep = new List<String>();

            if (true)
            {
                // get json & deserialize

                jsonString = File.ReadAllText(TextBlock_FilePathJson.Text);

                TextBox_JsonPath.Text = TextBlock_FilePathJson.Text;

                IssuesPOCO JsonContent = JsonConvert.DeserializeObject<IssuesPOCO>(jsonString);

                csvFileContent = "";

                csvFileContent += "Key,Issuetype,Current Status,Created Date,Component,";
                                
               List<WorkflowStep> statuses = getWorkflowFromFile();

                List<string> doneStatesList = new List<string>();

                // 
                foreach (WorkflowStep status in statuses)
                {              
                        csvFileContent += status.Name + ",";
                    if (status.DoneState)
                    {
                        doneStatesList.Add(status.Name);
                    }        
                }

                csvFileContent += "First Date,Closed Date,";

                csvFileContent += System.Environment.NewLine;

                // baue dictionary mit status/zeitpaaren
                issuesCount = JsonContent.issues.Count;
                
                foreach (IssuePOCO issue in JsonContent.issues)
                {
                    String resultLine = "";
                    string lastName = "";
                    string firstName = "";
                    Boolean foundDate = false;
                    // json convert anpassen auf issuetype "Fields hinzufügen"

                    resultLine += issue.key + "," + issue.fields.issuetype.name + "," + issue.fields.status.name + "," + issue.fields.created.ToString() + ",";

                    if(issue.fields.components != null)
                    {
                        foreach (IssueComponentsItemPOCO item in issue.fields.components)
                        {
                            resultLine += item.name + "|";
                        }
                        resultLine += ",";
                    }
                   
                    Dictionary<string, int> dict = new Dictionary<string, int>();
                    foreach(WorkflowStep status in statuses)
                    {
                        dict[status.Name] = 0;
                        if (status.Last)
                        {
                             lastName = status.Name;
                        }
                        if (status.First)
                        {
                             firstName = status.Name;
                        }
                    }

                    List<StatusRich> statusRichList = new List<StatusRich>();               

                    foreach (IssueHistoryPOCO history in issue.changelog.histories)
                    {
                        foreach (IssueChangeLogItem item in history.items)
                        {
                            if (item.FieldName.Equals("status"))
                            {
                                StatusRich statusTransformation = new StatusRich(item.ToValue, DateTime.Parse(history.created.ToString()));

                                statusRichList.Add(statusTransformation);
                            }
                        }
                    }
                    DateTime CloseDate = new DateTime();
                    DateTime FirstDate = new DateTime();
                    DateTime DoneDate = new DateTime();
                    // umsortieren letzter zuerst, desc
                    statusRichList.Sort((x, y) => y.TimeStamp.CompareTo(x.TimeStamp));

                    // kein Statuswechsel in History ==> immer noch im initialen Status
                     if (statusRichList.Count < 1)
                    {                                                
                        DateTime currentDate = new DateTime(2020, 12, 17, 12, 29, 00);
                        TimeSpan ts = currentDate - issue.fields.created;
                        int minutes = (int)ts.TotalMinutes;
                                               
                        resultLine += minutes + ",";
                    }
                    // sonst Status gefunden, wenn  nicht: immer noch open
                    else
                    {
                        DateTime last;

                        // wenn es einen Donestatus gibt ist der letzte das Ende Date
                        //if (statusRichList.Any(p => p.Name == "Done") || statusRichList.Any(p => p.Name == "Abgebrochen"))
                        if (statusRichList.Any(p => p.Name.Equals(lastName)))
                        {
                            CloseDate = statusRichList.Max(obj => obj.TimeStamp);
                        }

                        if (statusRichList.Any(p => p.Name.Equals(firstName)))
                        {
                            FirstDate = statusRichList.Min(obj => obj.TimeStamp);
                        }
                        
                        // Erster Zeitpunkt: Erstelldatum des Datenabzugs (aka "heute")                                                
                        last = new DateTime(2020, 12, 17, 12, 29, 00);
                        // Dauer eines statusverbleibs: Startdate des nachfolgers - Startdate des betrachteten Status
                        foreach (StatusRich statusTrans in statusRichList)
                        {                           
                            TimeSpan ts = last - statusTrans.TimeStamp ;
                            statusTrans.Minutes = (int)ts.TotalMinutes;
                            last = statusTrans.TimeStamp;
                            string statusName = "";
                            if (!(dict.ContainsKey(statusTrans.Name)))
                            {
                                statusName = statusTrans.Name;
                                foreach(WorkflowStep step in statuses)
                                {
                                    if (step.Aliases.Contains(statusTrans.Name))
                                    {
                                        statusName = step.Name;
                                    }
                                }
                            }
                            else
                            {
                                statusName = statusTrans.Name;
                            }
                            if (doneStatesList.Contains(statusName))
                            {
                                DoneDate = statusTrans.TimeStamp;
                                foundDate = true;
                            }
                            if (!dict.ContainsKey(statusName))
                            {
                                dict.Add(statusName, 0);
                                if (!notFoundStep.Contains(statusName))
                                {
                                    notFoundStep.Add(statusName);
                                }
                            }
                            dict[statusName] += statusTrans.Minutes;
                        }                      
                    }
                                                  
                    foreach (KeyValuePair<string, int> pair in dict)
                    {
                        resultLine += pair.Value + ",";
                    }

                    if (FirstDate.Equals(new DateTime()))
                    {                     
                            resultLine += ",";
                    }
                    else
                    {
                        resultLine += FirstDate.ToString() + ",";
                    }

                    if (CloseDate.Equals(new DateTime()))
                    {
                        if (foundDate)
                        {
                            resultLine += DoneDate.ToString() + ",";
                        }
                        resultLine += ",";
                    }
                    else
                    {
                        resultLine += CloseDate.ToString() + ","; 
                    }
                   
                    csvFileContent += resultLine + System.Environment.NewLine;
                    
                    Console.WriteLine(resultLine);
                    counter++;
                    ProgressBar_Historie.Value = 100 / issuesCount * counter;
                }

            }
            
            TextBox_Errors.Text += "\n<< additional Statuses found >> \n";
            foreach (String s in notFoundStep)
            {                
                TextBox_Errors.Text += s + " \n";
            }
            
            File.WriteAllText(TextBlock_Filepath.Text, csvFileContent);
        }

        private void ProgressBar_Historie_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Button_JsonFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "json File (*.json|*.json|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
                    TextBlock_FilePathJson.Text = openFileDialog.FileName;
        }
    }
}
