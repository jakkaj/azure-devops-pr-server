using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PR.Helpers.Contract;

namespace PR.Helpers.AzRest
{
    public class TemplateBuilder : ITemplateBuilder
    {
        public string Build(string templateName, string arm)
        {
            var template = File.ReadAllText($"Templates\\{templateName}");

            var repl = template.Replace("%%template%%", arm);

            return repl;
        }
    }
}
