using System;
using System.Collections.Generic;
using System.Text;
using tcc.Models;

namespace tcc
{
    public class DesignPatternRepository
    {
        // Criacionais
        public DesignPattern Factory => new DesignPattern("Factory", "Factory DP", "www.google.com");
        public DesignPattern Builder => new DesignPattern("Builder", "Builer DP", "www.google.com");
        public DesignPattern Singleton => new DesignPattern("Singleton", "Singleton DP", "www.google.com");

        // Estruturais
        public DesignPattern Composite => new DesignPattern("Composite", "Composite DP", "www.google.com");
        public DesignPattern Bridge => new DesignPattern("Bridge", "Bridge DP", "www.google.com");
        public DesignPattern Decorator => new DesignPattern("Decorator", "Composite DP", "www.google.com");
        public DesignPattern Facade => new DesignPattern("Facade", "Composite DP", "www.google.com");
        public DesignPattern Proxy => new DesignPattern("Proxy", "Composite DP", "www.google.com");

        // Comportamentais
    }
}
