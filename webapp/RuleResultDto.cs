using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tcc.Models;

namespace webapp
{
    public class AnalysisResultDto
    {
        public List<RuleResultsGroupDto> RuleResultGroups { get; set; }
        public OverviewDto Overview { get; set; }
        public AnalysisResultDto()
        {
            this.Overview = new OverviewDto();
            this.RuleResultGroups = new List<RuleResultsGroupDto>();
        }
    }

    public class RuleResultsGroupDto
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public List<RuleResultDto> RuleResults { get; set; }
        public RuleResultsGroupDto()
        {
            this.FilePath = "";
            this.FileName = "";
            this.RuleResults = new List<RuleResultDto>();
        }
    }

    public class RuleResultDto
    {
        public string RuleName { get; set; }
        public string RuleDescription { get; set; }
        public int LineNumber { get; set; }
        public string DPName { get; set; }
        public string DPExtraInfo { get; set; }
        public ESeverityLevel SeverityLevel { get; set; }
    }

    public class OverviewDto
    {
        public int OpportunitiesBlocker { get; set; } = 0;
        public int OpportunitiesCritical { get; set; } = 0;
        public int OpportunitiesMajor { get; set; } = 0;
        public int OpportunitiesMinor { get; set; } = 0;
        public int OpportunitiesInfo { get; set; } = 0;
        public int FilesAnalysed{ get; set; } = 0;
        public List<string> RuleNames { get; set; } = new List<string>();
        public List<string> DPNames { get; set; } = new List<string>();
        public EntitiesDto Entities { get; set; } = new EntitiesDto();
        public RelationshipsDto Relationships { get; set; } = new RelationshipsDto();
        
    }

    public class EntitiesDto
    {
        public int Classes { get; set; }
        public int Interfaces { get; set; }
    }

    public class RelationshipsDto
    {
        public int Inheritances { get; set; }
        public int Implementations { get; set; }
        public int Receptions { get; set; }
        public int Instantiations { get; set; }
        public int Dependencies { get; set; }
    }
}
