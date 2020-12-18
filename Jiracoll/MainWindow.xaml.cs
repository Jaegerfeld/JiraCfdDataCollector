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
        private async void Button_Click(object sender, RoutedEventArgs e)
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

            // suche pro abgerufgenes Issue die Basisdaten..... 
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

        private string[] getWorkflowFromCsv()
        {
            string[] returnArray;
            List<String> strings = new List<string>();

            
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
                strings.Add(line);
                counter++;
            }

           

            file.Close();

            returnArray = strings.ToArray();
            System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  
            System.Console.ReadLine();


            return returnArray;
        }

        private void Button_SelectJson_Click(object sender, RoutedEventArgs e)
        {
           

            string jsonString ="";
            int counter = 0; // Verlaufsbalken Zähler
          
            int issuesCount; // wieviele Issues insgesamt issuecount/20 == anzahl abrufe notwendig

            String csvFileContent = "";

           
            IssueChangeLog issueChangelog = new IssueChangeLog();


            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                // get json & deserialize


                jsonString = File.ReadAllText(openFileDialog.FileName);

                TextBox_JsonPath.Text = openFileDialog.FileName;

               IssuesPOCO JsonContent = JsonConvert.DeserializeObject<IssuesPOCO>(jsonString);



                csvFileContent = "";

                csvFileContent += "Key,Issuetype,Current Status,Created Date,";

                string[] s = getWorkflowFromCsv();

                //string[] s = new string[] { "To Do", "Vorbereitung - Durchführung", "Done", "Abgerechnet", "Fristgerecht storniert", "Storno durch P3", "Nicht fristgerecht storniert" };



                foreach (string item in s)
                {
                    csvFileContent += item + ",";
                }
                csvFileContent += System.Environment.NewLine;



                // baue dictionary mit status/zeitpaaren

                issuesCount = JsonContent.issues.Count;

                foreach (IssuePOCO issue in JsonContent.issues)
                {


                    String resultLine = "";
                    // json convert anpassen auf issuetype "Fields hinzufügen"
                    //resultLine += issue.key + "," + issue.type + "," + issue.status + "," + i.Created + ",";
                    resultLine += issue.key + "," + issue.fields.issuetype.name + "," + issue.fields.status.name + "," + issue.fields.created.ToString() + ",";

                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    foreach (IssueStatus issueStatus in s)
                    {
                        dict.Add(issueStatus.ToString(), "");
                    }

                    foreach (IssueHistoryPOCO history in issue.changelog.histories)
                    {
                        foreach (IssueChangeLogItem item in history.items)
                        {
                            if (item.FieldName.Equals("status"))
                            {
                                dict[item.ToValue] = history.created.ToString();

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
                    ProgressBar_Historie.Value = 100 / issuesCount * counter;
                }
                

            }

            

            Console.WriteLine("Ausgabe:    " +jsonString);
            File.WriteAllText(TextBlock_Filepath.Text, csvFileContent);

        }

        private void Button_IssuesFromJson(object sender, RoutedEventArgs e)
        {
            string jsonString = "";
            int counter = 0; // Verlaufsbalken Zähler

            int issuesCount; // wieviele Issues insgesamt issuecount/20 == anzahl abrufe notwendig

            String csvFileContent = "";


            IssueChangeLog issueChangelog = new IssueChangeLog();


            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                // get json & deserialize


                jsonString = File.ReadAllText(openFileDialog.FileName);

                TextBox_JsonPath.Text = openFileDialog.FileName;

                IssuesPOCO JsonContent = JsonConvert.DeserializeObject<IssuesPOCO>(jsonString);



                csvFileContent = "";

                csvFileContent += "Key,Issuetype,Current Status,Created Date,";

                string[] statuses = getWorkflowFromCsv();

                //string[] s = new string[] { "To Do", "Vorbereitung - Durchführung", "Done", "Abgerechnet", "Fristgerecht storniert", "Storno durch P3", "Nicht fristgerecht storniert" };



                foreach (string item in statuses)
                {
                    csvFileContent += item + ",";
                }
                csvFileContent += "Closed,";
                csvFileContent += System.Environment.NewLine;



                // baue dictionary mit status/zeitpaaren

                issuesCount = JsonContent.issues.Count;

                foreach (IssuePOCO issue in JsonContent.issues)
                {


                    String resultLine = "";
                    // json convert anpassen auf issuetype "Fields hinzufügen"
                    //resultLine += issue.key + "," + issue.type + "," + issue.status + "," + i.Created + ",";
                    resultLine += issue.key + "," + issue.fields.issuetype.name + "," + issue.fields.status.name + "," + issue.fields.created.ToString() + ",";

                    Dictionary<string, int> dict = new Dictionary<string, int>();
                    foreach(string item in statuses)
                    {
                        dict[item] = 0;
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
                    // umsortieren letzter zuerst, desc
                    statusRichList.Sort((x, y) => y.TimeStamp.CompareTo(x.TimeStamp));

                    
                    // wenn Status gefunden, wenn  nicht: immer noch open
                    if (statusRichList.Count > 0)
                    {
                        DateTime last;
                        // wenn es einen Donestatus gibt ist der letzte das Ende Date
                        if (statusRichList.Any(p => p.Name == "Done") || statusRichList.Any(p => p.Name == "Abgebrochen"))
                        {
                            CloseDate = statusRichList.Max(obj => obj.TimeStamp);
                        }

                        last = statusRichList.Max(obj => obj.TimeStamp);


                        // Dauer eines statusverbleibs: Startdate des nachfolgers - Startdate des betrachteten Status
                        foreach (StatusRich statusTrans in statusRichList)
                        {
                            TimeSpan ts = last - statusTrans.TimeStamp ;
                            statusTrans.Minutes = (int)ts.TotalMinutes;
                            last = statusTrans.TimeStamp;
                            dict[statusTrans.Name] += statusTrans.Minutes;
                        }
                        
                    }

                                
                    foreach (KeyValuePair<string, int> pair in dict)
                    {
                        resultLine += pair.Value + ",";
                    }

                    if(CloseDate.Equals(new DateTime()))
                    {
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



            Console.WriteLine("Ausgabe:    " + jsonString);
            File.WriteAllText(TextBlock_Filepath.Text, csvFileContent);
        }
    }
}
