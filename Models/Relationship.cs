using System;
using System.Collections.Generic;
using System.Text;

namespace tcc.Models
{
    class Relationship
    {
        public int Id { get; set; }
        public Entity Source { get; set; }
        public Entity Target { get; set; }
        public ERelationshipType Type { get; set; }
        public int LineNumber { get; set; }
        public string MethodName { get; set; }
        public bool IsMethodConstructor { get; set; }
    }
}
