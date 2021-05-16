using System;
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
        public IEnumerable<RuleResultDto> Get(string slnPath = "", string excluded = "")
        {
            var project1 = @"C:\Users\erico\source\repos\clean-architecture-manga\Clean-Architecture-Manga.sln";
            var project2 = @"C:\Users\erico\source\repos\TestProject\TestProject.sln";
            var excludedList = excluded.Split(";");
            var extractor = new Extractor(slnPath, excludedList);
            extractor.Run();
            var ruleResults = new FactoryRule1(extractor.Repository).Execute();
            return ruleResults.Select(r => new RuleResultDto()
            {
                RuleName = r.Rule.Name,
                RuleDescription = r.Rule.Description,
                FilePath = r.FilePath,
                LineNumber = r.LineNumber
            }).ToArray();
        }
    }
}