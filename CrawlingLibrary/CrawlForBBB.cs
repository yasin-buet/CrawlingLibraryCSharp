using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace CrawlingLibrary
{
    public class CrawlingLibrary:ICrawl
    {
        //declare varibales
        private string _name;
        private string _location;
        private string _rootUrl;
        private List<BbbResult> _result;

        public CrawlingLibrary(string rootUrl, string name, string location)
        {
            _name = name;
            _location = location;
            _rootUrl = rootUrl;
        }

        #region [Private Methods] 

        public virtual string ConstructUrl(string name, string location)
        {
            return _rootUrl + name + "&location=" + location;
        }

        private string GetHtml()
        {
            using (var webClient = new WebClient())
            {
                var html = webClient.DownloadString(ConstructUrl(this._name, this._location));
                html = html.Replace(System.Environment.NewLine, " ");
                return html;
            }

        }

        private string RemoveTags(string str)
        {
            if (str.Contains("<em>"))
            {
                str = str.Remove(str.IndexOf("<em>", StringComparison.Ordinal), 4);
            }
            if (str.Contains("</em>"))
            {
                str = str.Remove(str.IndexOf("<em>", StringComparison.Ordinal), 5);
            }
            if (str.Contains("<br>"))
            {
                str = str.Remove(str.IndexOf("<br>", StringComparison.Ordinal), 4);
            }
            if (str.Contains("<br />"))
            {
                str = str.Remove(str.IndexOf("<br />", StringComparison.Ordinal), 6);
            }
            if (str.Contains("<strong>"))
            {
                str = str.Remove(str.IndexOf("<strong>", StringComparison.Ordinal), 8);
            }
            if (str.Contains("</strong>"))
            {
                str = str.Remove(str.IndexOf("</strong>", StringComparison.Ordinal), 9);
            }
            return str;
        }

        private string AddressFinder(string address, string regexAddressFull)
        {
            string cleanAddress = Regex.Replace(address, @"\s+", " ").Trim();
            var rxAddress = new Regex(Globals.RegexAddress);
            var matchAddress = rxAddress.Match(cleanAddress);
            return matchAddress.Success ? rxAddress.Match(address).ToString() : string.Empty;

        }
        //get names from here
        private string NameFinder(string match, string regexName)
        {
            return null;
        }

        private string ZipFinder(string m)
        {
            string cleanZipCode = Regex.Replace(m, @"\s+", " ").Trim();
            return cleanZipCode.Split(' ').Last();
        }

        #endregion

        #region [Public Methods]

        public virtual List<BbbResult> Show()
        {        
            _result = new List<BbbResult>();
            var regex = new Regex(Globals.RegexFull, RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(GetHtml());
            foreach (var m in matches)
            {
                var restaurentName = m.ToString().Substring((m.ToString().IndexOf("\">", StringComparison.Ordinal) + 2), 10) + _name;
                string addressOnly = AddressFinder(m.ToString(), Globals.RegexAddress);
                string zipCode = ZipFinder(addressOnly);
                _result.Add(new BbbResult
                {
                    Name = RemoveTags(restaurentName),
                    Address = RemoveTags(addressOnly),
                    Location = _location,
                    ZipCode = RemoveTags(zipCode)
                });                
            }
            return _result;
        }

        #endregion
    }
}