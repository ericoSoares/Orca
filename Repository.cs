using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using tcc.Models;

namespace tcc
{
    class Repository
    {
        public IList<Entity> Entities { get; set; }
        public IList<Relationship> Relationships { get; set; }

        public Repository()
        {
            this.Entities = new List<Entity>();
            this.Relationships = new List<Relationship>();
        }
    }
}
