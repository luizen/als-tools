﻿using AlsTools.Core.Entities;
using AlsTools.Core.ValueObjects.ResultSets;
using Raven.Client.Documents.Indexes;

namespace AlsTools.Infrastructure.Indexes;

public class LiveProjects_TracksCount : AbstractIndexCreationTask<LiveProject, ItemsCountPerProjectResult>
{
    public LiveProjects_TracksCount()
    {
        Map = projects => from project in projects
                          from track in project.Tracks
                          select new ItemsCountPerProjectResult()
                          {
                              ProjectName = project.Name,
                              ProjectPath = project.Path,
                              ItemsCount = 1
                          };

        Reduce = results => from result in results
                            group result by new { result.ProjectPath, result.ProjectName } into g
                            select new ItemsCountPerProjectResult()
                            {
                                ProjectName = g.Key.ProjectName,
                                ProjectPath = g.Key.ProjectPath,
                                ItemsCount = g.Sum(x => x.ItemsCount)
                            };
    }
}
