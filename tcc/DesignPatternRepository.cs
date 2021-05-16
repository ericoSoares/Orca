using System;
using System.Collections.Generic;
using System.Text;
using tcc.Models;

namespace tcc
{
    public class DesignPatternRepository
    {
        public DesignPattern Factory => new DesignPattern("Factory", "Factory DP", "www.google.com");
        public DesignPattern Composite => new DesignPattern("Composite", "Composite DP", "www.google.com");
    }
}
