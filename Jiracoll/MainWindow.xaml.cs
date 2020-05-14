﻿using Atlassian.Jira;
using Microsoft.Win32;
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
        }

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

        private void Button_FilePath_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
                TextBlock_Filepath.Text = saveFileDialog.FileName;
            File.WriteAllText(saveFileDialog.FileName, "");
        }
    }
}