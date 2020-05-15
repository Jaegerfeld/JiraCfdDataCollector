library(readr)
KanbanDT <- read_delim("test9.csv", ";", escape_double = FALSE, 
                    col_types = cols(Analysis = col_date(format = "%d.%m.%Y"), 
                                     Backlog = col_date(format = "%d.%m.%Y"), 
                                     Closed = col_date(format = "%d.%m.%Y"), 
                                     Done = col_date(format = "%d.%m.%Y"), 
                                     `In Progress` = col_date(format = "%d.%m.%Y"), 
                                     Open = col_date(format = "%d.%m.%Y"), 
                                     Releasing = col_date(format = "%d.%m.%Y"), 
                                     Resolved = col_date(format = "%d.%m.%Y")), 
                    trim_ws = TRUE)
