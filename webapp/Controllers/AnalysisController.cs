using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tcc;
using tcc.Models;

namespace webapp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly ILogger<AnalysisController> _logger;

        public AnalysisController(ILogger<AnalysisController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public AnalysisResultDto Get(string slnPath = "", string excluded = "")
        {
            var project1 = @"C:\Users\erico\source\repos\clean-architecture-manga\Clean-Architecture-Manga.sln";
            var project2 = @"C:\Users\erico\source\repos\TestProject\TestProject.sln";
            var excludedList = excluded.Split(";");
            var extractor = new Extractor(slnPath, excludedList);
            extractor.Run();
            var ruleResults = new RuleDriver().ExecuteRules(extractor.Repository);
            var analysisResult = new AnalysisResultDto();

            var ruleResultGroups = ruleResults
                .GroupBy(r => r.FilePath)
                .Select(r => new RuleResultsGroupDto
                {
                    FilePath = r.Key,
                    FileName = Path.GetFileName(r.Key),
                    RuleResults = r.Select(x => new RuleResultDto
                    {
                        LineNumber = x.LineNumber,
                        DPName = x.Rule.DesignPattern.Name,
                        DPExtraInfo = x.Rule.DesignPattern.MoreInfoUrl,
                        RuleDescription = x.Rule.Description,
                        RuleName = x.Rule.Name,
                        SeverityLevel = x.Rule.SeverityLevel
                    }).ToList()
                })
                .ToList();

            var overview = new OverviewDto();
            overview.OpportunitiesBlocker = ruleResults.Where(r => r.Rule.SeverityLevel == ESeverityLevel.BLOCKER).Count();
            overview.OpportunitiesCritical = ruleResults.Where(r => r.Rule.SeverityLevel == ESeverityLevel.CRITICAL).Count();
            overview.OpportunitiesMajor = ruleResults.Where(r => r.Rule.SeverityLevel == ESeverityLevel.MAJOR).Count();
            overview.OpportunitiesMinor = ruleResults.Where(r => r.Rule.SeverityLevel == ESeverityLevel.MINOR).Count();
            overview.OpportunitiesInfo = ruleResults.Where(r => r.Rule.SeverityLevel == ESeverityLevel.INFO).Count();
            overview.FilesAnalysed = extractor.Repository.Entities.GroupBy(r => r.FilePath).Count();
            overview.RuleNames = new RuleDriver().GetRuleNames();
            overview.DPNames = new RuleDriver().GetDPNames();

            overview.Entities = extractor.Repository.Entities.Select(r => new EntityDto
            {
                Name = r.Name,
                Type = (r.Type == EEntityType.CLASS ? "Class" : "Interface"),
                Project = r.ProjectName,
                File = r.FilePath
            }).OrderBy(r => r.Project).ThenBy(r => r.Type).ToList();

            overview.Rules = new RuleDriver().GetRules().Select(r => new RuleDto
            {
                Name = r.Name,
                Description = r.Description,
                SeverityLevel = (int)(r.SeverityLevel),
                DPName = r.DesignPattern.Name
            }).OrderBy(r => r.Name).ToList();

            overview.DesignPatterns = new RuleDriver().GetDPs().Select(r => new DPDto
            {
                Name = r.Name,
                Description = r.Description,
                MoreInfoUrl = r.MoreInfoUrl
            }).GroupBy(r => r.Name).Select(r => r.First()).OrderBy(r => r.Name).ToList();

            overview.Relationships = new RelationshipsDto()
            {
                Implementations = extractor.Repository.Relationships.Where(r => r.Type == ERelationshipType.IMPLEMENTATION).Count(),
                Inheritances = extractor.Repository.Relationships.Where(r => r.Type == ERelationshipType.INHERITANCE).Count(),
                Dependencies = extractor.Repository.Relationships.Where(r => r.Type == ERelationshipType.DEPENDENCY).Count(),
                Instantiations = extractor.Repository.Relationships
                    .Where(r => r.Type == ERelationshipType.INSTANTIATION_IN_CLASS
                        || r.Type == ERelationshipType.INSTANTIATION_IN_CONSTRUCTOR
                        || r.Type == ERelationshipType.INSTANTIATION_IN_METHOD).Count(),
                Receptions = extractor.Repository.Relationships
                    .Where(r => r.Type == ERelationshipType.RECEPTION_IN_CONSTRUCTOR
                        || r.Type == ERelationshipType.RECEPTION_IN_METHOD).Count()
            };

            analysisResult.Overview = overview;
            analysisResult.RuleResultGroups = ruleResultGroups;
            return analysisResult;
        }
    }
}