using HtmlAgilityPack;
using System;
using System.Windows.Forms;
using System.Web;
using System.Xml.Linq;
using System.Security.Policy;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;

namespace HtmlAgilityPack_WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        string sUrlIndex = "https://www.dhlottery.co.kr/gameResult.do?method=byWin";
        string sUrl = "https://www.dhlottery.co.kr/gameResult.do?method=byWin&drwNo=";

        int nLastIndex = 0;


        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        //당첨마지막회차 번호
        private async void button1_Click(object sender, EventArgs e)
        {
            string sResult = "";
            textBox1.Text = sResult;
            textBox2.Text = sResult;

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
            textBox1.Text = sResult;

            /*
            //전체 값을 가져온다.
            HtmlNodeCollection optionNodes = singlenode2.SelectNodes(".//option");
            if (optionNodes != null)
            {
                foreach (HtmlNode optionNode in optionNodes)
                {
                    string value = optionNode.GetAttributeValue("value", "");
                    string text = optionNode.InnerText;
                    sResult += value + " : " + text + "\r\n";
                }
            }
            textBox1.Text = sResult;
            */
        }

        //마지막회차 당첨 정보
        private async void button2_Click(object sender, EventArgs e)
        {
            string sResult = "";
            string s1, s2, s3, s4, s5, s6;
            textBox1.Text = sResult;
            textBox2.Text = sResult;

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
                                sResult += s1  + "\r\n";
                            }
                        }
                    }
                }
            }
            textBox1.Text = sResult;
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
            textBox2.Text = sResult;
        }

        //전체 당첨 정보
        private async void button3_Click(object sender, EventArgs e)
        {
            string sResult = "";
            string s1, s2, s3, s4, s5, s6;
            textBox1.Text = sResult;
            textBox2.Text = sResult;

            for (int i = 1; i <= nLastIndex; i++)
            {
                string sUrlIndex = sUrl + i.ToString();
                //Console.WriteLine(sUrlIndex);

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

            textBox1.Text = sResult;
        }
    }
}
