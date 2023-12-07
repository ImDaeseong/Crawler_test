using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class HtmlParser1
    {
        public async Task<string> getHtml(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                string sHtml = await client.GetStringAsync(url);
                return sHtml;
            }
        }

        public HtmlNodeCollection getNodes(string sHtml, string xExpression)
        {
            HtmlDocument document = new HtmlDocument();

            try
            {
                document.LoadHtml(sHtml);
                return document.DocumentNode.SelectNodes(xExpression);
            }
            catch
            {
                return null;
            }
        }

        public HtmlNode getNode(string sHtml, string xExpression)
        {
            HtmlDocument document = new HtmlDocument();

            try
            {
                document.LoadHtml(sHtml);
                return document.DocumentNode.SelectSingleNode(xExpression);
            }
            catch
            {
                return null;
            }
        }

    }
}
