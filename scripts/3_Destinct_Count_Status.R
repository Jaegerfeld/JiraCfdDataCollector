
DTOpenDist <- summarise(group_by(KanbanDT,Open),count = n())
DTAnalysisDist <- summarise(group_by(KanbanDT,Analysis),count = n())
DTBacklogDist <- summarise(group_by(KanbanDT,Backlog),count = n())
DTIPDist <- summarise(group_by(KanbanDT,IP),count = n())
DTResolvedDist <- summarise(group_by(KanbanDT,Resolved),count = n())
DTReleasingDist <- summarise(group_by(KanbanDT,Releasing),count = n())
DTClosedDist <- summarise(group_by(KanbanDT,Closed),count = n())
DTDoneDist <- summarise(group_by(KanbanDT,Done),count = n())

