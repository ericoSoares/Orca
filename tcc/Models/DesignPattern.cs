using System;
using System.Collections.Generic;
using System.Text;

namespace tcc.Models
{
    public class DesignPattern
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string MoreInfoUrl { get; set; }
        public DesignPattern(string name, string description, string url)
        {
            this.Name = name;
            this.Description = description;
            this.MoreInfoUrl = url;
        }
    }
}
