using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace EmbedStock
{
    public partial class _Default : System.Web.UI.Page
    {
        public string strValue = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            strValue = ExtractContent("http://vnexpress.net/GL/Home/", "<div class=\"dlistitem\">((.|\\n)*?)</div>");
            strValue = strValue.Replace("/Images/Stock", "http://vnexpress.net/Images/Stock");



        }
        public string ExtractContent(string url, string pattern)
        {
            string content = LoadHTML(url);
            string extractedContent = "";
            if (content != null)
            {
                Regex regEx = new Regex(pattern, RegexOptions.IgnoreCase);
                Match match = regEx.Match(content);
                if (match != null && match.Success && match.Groups[1].Success)
                {

                    while (match.Groups.Count>1) 
                    {
                        extractedContent += match.Groups[1].Value;
                        match = match.NextMatch();
                    } 

                }
            }
            return extractedContent;
        }
        private string LoadHTML(string link)
        {
            string strreturn = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://vnexpress.net/GL/Home/");

            // Set some reasonable limits on resources used by this request
            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            // Set credentials to use for this request.
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Console.WriteLine("Content length is {0}", response.ContentLength);
            Console.WriteLine("Content type is {0}", response.ContentType);

            // Get the stream associated with the response.
            Stream receiveStream = response.GetResponseStream();

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            strreturn = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
            return strreturn;
        }
    }
}
