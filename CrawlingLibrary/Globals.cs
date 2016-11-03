using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrawlingLibrary
{
    public static class Globals
    {
        public const string RegexFull = "(?<=<h4 class=\"hcolor\"><a href=)(.+?)(</address>)";
        public const string RegexAddress = "(?<=<address>)(.+?)(?=</address>)";
    }
}
