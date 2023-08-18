using HtmlAgilityPack;
using System;
using System.Windows.Forms;
using System.Web;

namespace HtmlAgilityPack_WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string sUrl = "https://www.jobkorea.co.kr/Search/?stext=" + Uri.EscapeDataString("인도네시아");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string sResult = "";
            textBox1.Text = sResult;
            textBox2.Text = sResult;

            HtmlParser1 parser = new HtmlParser1();
            string sHtml = await parser.getHtml(sUrl);
            //textBox1.Text = sHtml;

            string xExpression1 = "//div[contains(@class, 'list-default')]//li[contains(@class, 'list-post')]";
            HtmlNodeCollection nodes = parser.getNodes(sHtml, xExpression1);
            if (nodes != null)
            {
                foreach (HtmlNode item in nodes)
                {
                    sResult += item.OuterHtml;
                }
            }
            textBox1.Text = sResult;
            sResult = "";
           

            string xExpression2 = "//div[contains(@class, 'tplPagination newVer wide')]";
            HtmlNode node = parser.getNode(sHtml, xExpression2);
            if (node != null)
            {
                sResult = node.OuterHtml;
            }
            textBox2.Text = sResult;
            sResult = "";
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string sResult = "";
            textBox1.Text = sResult;
            textBox2.Text = sResult;

            HtmlParser1 parser = new HtmlParser1();
            string sHtml = await parser.getHtml(sUrl);
            
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(sHtml);

            HtmlNode singlenode = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'list-default')]");
            if (singlenode == null)
                return;

            HtmlNodeCollection nodes = singlenode.SelectNodes(".//li[@class='list-post']");
            if (nodes == null)
                return;

            for (int i = 0; i < nodes.Count; i++)
            {
                HtmlNode item = nodes[i];

                string surl = HttpUtility.HtmlDecode(item.Attributes["data-gavirturl"].Value);
                string sinfo = HttpUtility.HtmlDecode(item.Attributes["data-gainfo"].Value);

                HtmlNode node1 = item.SelectSingleNode(".//div[@class='post-list-corp']");
                HtmlNode node2 = item.SelectSingleNode(".//div[@class='post-list-info']");

                sResult += surl + "\r\n" + sinfo + "\r\n" + node1.InnerText + "\r\n" + node2.InnerText + "\r\n\r\n";
            }
            textBox1.Text = sResult;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string sResult = "";
            textBox1.Text = sResult;
            textBox2.Text = sResult;

            HtmlParser1 parser = new HtmlParser1();
            string sHtml = await parser.getHtml(sUrl);

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(sHtml);

            HtmlNode singlenode = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'tplPagination newVer wide')]");
            if (singlenode == null)
                return;

            HtmlNodeCollection nodes = singlenode.SelectNodes(".//li/a[@href]");
            if (nodes == null)
                return;
                            
            foreach (HtmlNode item in nodes)
            {       
                sResult += item.Attributes["href"].Value + "\r\n"; 
            }
            textBox2.Text = sResult;
        }
    }
}
