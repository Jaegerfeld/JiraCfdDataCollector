# JiraDataCollector
from Jira Project to Scatterplot/Boxplot/CFD


small leadTime reporting tool.

Collects the "Time in Status" for every single Issue in a given json / from Jira Rest API

1. Prepares Data for Boxplots etc. 
  one line per issue with the time per status
  exported as csv
2. Prepares data for CFDs
  one line per Date with count of issues per status
  exported as csv.


Tool is written in C# and WPF GUI. 
NO automated Testing!
NO bughandling!

It is just a proof of concept and a way to collect data in an automated way. 
Feel free to commit and/or build your own tool

There will be  a second tool for using the data in diagramms, soon
