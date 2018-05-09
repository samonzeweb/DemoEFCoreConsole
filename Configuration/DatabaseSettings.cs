using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DemoEFCoreConsole.Configuration
{
    public class DatabaseSettings
    {
        public string ConnectionString  { get; set; }
    }
}