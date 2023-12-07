using HtmlAgilityPack;
using System;
using System.Windows;
using System.Web;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        string sUrl = "https://www.jobkorea.co.kr/Search/?stext=" + Uri.EscapeDataString("인도네시아");

        public MainWindow()
        {
            InitializeComponent();
        }
                
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            string sResult = "";
            TextBox1.Text = sResult;
            TextBox2.Text = sResult;

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
            TextBox1.Text = sResult;
            sResult = "";


            string xExpression2 = "//div[contains(@class, 'tplPagination newVer wide')]";
            HtmlNode node = parser.getNode(sHtml, xExpression2);
            if (node != null)
            {
                sResult = node.OuterHtml;
            }
            TextBox2.Text = sResult;
            sResult = "";
        }

        private async void Button2_Click(object sender, RoutedEventArgs e)
        {
            string sResult = "";
            TextBox1.Text = sResult;
            TextBox2.Text = sResult;

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
            TextBox1.Text = sResult;
        }

        private async void Button3_Click(object sender, RoutedEventArgs e)
        {
            string sResult = "";
            TextBox1.Text = sResult;
            TextBox2.Text = sResult;

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
            TextBox2.Text = sResult;
        }
                
    }
}
