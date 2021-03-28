using System;
using System.Collections.Generic;
using System.Text;

namespace tcc.Models
{
    class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EEntityType Type { get; set; }
        public string AccessModifier { get; set; }
        public string SemanticType { get; set; }
        public int LineNumber { get; set; }
    }
}
