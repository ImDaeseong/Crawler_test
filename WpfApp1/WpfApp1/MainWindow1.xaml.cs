using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow1 : Window
    {
        string sUrlIndex = "https://www.dhlottery.co.kr/gameResult.do?method=byWin";
        string sUrl = "https://www.dhlottery.co.kr/gameResult.do?method=byWin&drwNo=";

        int nLastIndex = 0;

        public MainWindow1()
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
            string sHtml = await parser.getHtml(sUrlIndex);

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(sHtml);

            HtmlNode singlenode1 = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'contentsArticle')]");
            if (singlenode1 == null)
                return;

            HtmlNode singlenode2 = singlenode1.SelectSingleNode(".//select[@id='dwrNoList']");
            if (singlenode2 == null)
                return;

            //첫번째 값만 가져온다.
            HtmlNode optionNode = singlenode2.SelectSingleNode(".//option");
            if (optionNode != null)
            {
                string value = optionNode.GetAttributeValue("value", "");
                string text = optionNode.InnerText;
                sResult = value + " : " + text + "\r\n";
                nLastIndex = int.Parse(text);
            }
            TextBox1.Text = sResult;

            /*
            //전체 값을 가져온다.
            HtmlNodeCollection optionNodes = singlenode2.SelectNodes(".//option");
            if (optionNodes != null)
            {
                foreach (HtmlNode opt in optionNodes)
                {
                    string value = opt.GetAttributeValue("value", "");
                    string text = opt.InnerText;
                    sResult += value + " : " + text + "\r\n";
                }
            }
            TextBox1.Text = sResult;
            */

        }

        private async void Button2_Click(object sender, RoutedEventArgs e)
        {
            string sResult = "";
            string s1, s2, s3, s4, s5, s6;
            TextBox1.Text = sResult;
            TextBox2.Text = sResult;

            string sUrlIndex = sUrl + nLastIndex.ToString();

            HtmlParser1 parser = new HtmlParser1();
            string sHtml = await parser.getHtml(sUrlIndex);

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(sHtml);

            HtmlNode singlenode1 = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'contentsArticle')]");
            if (singlenode1 == null)
                return;

            //당첨번호
            HtmlNode numbernode = singlenode1.SelectSingleNode("//div[contains(@class, 'win_result')]");
            if (numbernode != null)
            {
                HtmlNode h4Node = numbernode.SelectSingleNode(".//h4/strong");
                string sh = h4Node.InnerText.Trim();

                HtmlNode descNode = numbernode.SelectSingleNode(".//p[@class='desc']");
                string sDesc = descNode.InnerText.Trim();

                HtmlNodeCollection winNodes = numbernode.SelectNodes("//div[@class='nums']");
                if (winNodes != null)
                {
                    foreach (HtmlNode item in winNodes)
                    {
                        HtmlNode node1 = item.SelectSingleNode("//div[@class='num win']");
                        if (node1 != null)
                        {
                            HtmlNodeCollection spanNodes = item.SelectNodes(".//span");
                            if (spanNodes != null)
                            {
                                s1 = spanNodes[0].InnerText.Trim();
                                s2 = spanNodes[1].InnerText.Trim();
                                s3 = spanNodes[2].InnerText.Trim();
                                s4 = spanNodes[3].InnerText.Trim();
                                s5 = spanNodes[4].InnerText.Trim();
                                s6 = spanNodes[5].InnerText.Trim();
                                sResult += s1 + "|" + s2 + "|" + s3 + "|" + s4 + "|" + s5 + "|" + s6 + "\r\n";
                            }
                        }

                        HtmlNode node2 = item.SelectSingleNode("//div[@class='num bonus']");
                        if (node2 != null)
                        {
                            HtmlNode spanNode = node2.SelectSingleNode(".//p/span");
                            if (spanNode != null)
                            {
                                s1 = spanNode.InnerText.Trim();
                                sResult += s1 + "\r\n";
                            }
                        }
                    }
                }
            }
            TextBox1.Text = sResult;
            sResult = "";


            //1등부터 5등까지 당첨정보
            HtmlNode bodyNode = singlenode1.SelectSingleNode(".//tbody");
            if (bodyNode != null)
            {
                HtmlNodeCollection trNodes = bodyNode.SelectNodes(".//tr");
                if (trNodes != null)
                {
                    foreach (HtmlNode item in trNodes)
                    {
                        HtmlNodeCollection tdNodes = item.SelectNodes(".//td");
                        if (tdNodes != null)
                        {
                            s1 = tdNodes[0].InnerText.Trim();
                            s2 = tdNodes[1].InnerText.Trim();
                            s3 = tdNodes[2].InnerText.Trim();
                            s4 = tdNodes[3].InnerText.Trim();
                            s5 = tdNodes[4].InnerText.Trim();
                            sResult += s1 + "|" + s2 + "|" + s3 + "|" + s4 + "|" + s5 + "\r\n";

                            /*
                            foreach (HtmlNode item1 in tdNodes)
                            {
                                Console.WriteLine(item1.InnerHtml);
                            }
                            */
                        }
                    }
                }
            }
            TextBox2.Text = sResult;
        }

        private async void Button3_Click(object sender, RoutedEventArgs e)
        {
            string sResult = "";
            string s1, s2, s3, s4, s5, s6;
            TextBox1.Text = sResult;
            TextBox2.Text = sResult;

            for (int i = 1; i <= nLastIndex; i++)
            {
                string sUrlIndex = sUrl + i.ToString();
                Console.WriteLine(sUrlIndex);

                HtmlParser1 parser = new HtmlParser1();
                string sHtml = await parser.getHtml(sUrlIndex);

                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(sHtml);

                HtmlNode singlenode1 = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'contentsArticle')]");
                if (singlenode1 == null)
                    continue;

                //당첨번호
                HtmlNode numbernode = singlenode1.SelectSingleNode("//div[contains(@class, 'win_result')]");
                if (numbernode != null)
                {
                    HtmlNodeCollection winNodes = numbernode.SelectNodes("//div[@class='nums']");
                    if (winNodes != null)
                    {
                        foreach (HtmlNode item in winNodes)
                        {
                            HtmlNode node1 = item.SelectSingleNode("//div[@class='num win']");
                            if (node1 != null)
                            {
                                HtmlNodeCollection spanNodes = item.SelectNodes(".//span");
                                if (spanNodes != null)
                                {
                                    s1 = spanNodes[0].InnerText.Trim();
                                    s2 = spanNodes[1].InnerText.Trim();
                                    s3 = spanNodes[2].InnerText.Trim();
                                    s4 = spanNodes[3].InnerText.Trim();
                                    s5 = spanNodes[4].InnerText.Trim();
                                    s6 = spanNodes[5].InnerText.Trim();
                                    sResult += s1 + "|" + s2 + "|" + s3 + "|" + s4 + "|" + s5 + "|" + s6 + "\r\n";
                                }
                            }
                        }
                    }
                }
            }

            TextBox1.Text = sResult;
        }

    }
   
}
