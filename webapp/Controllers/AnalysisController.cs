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

            analysisResult.RuleResultGroups = ruleResultGroups;
            return analysisResult;
        }
    }
}