import requests
from bs4 import BeautifulSoup
import urllib.parse


def f1():
    baseURL = "https://www.jobkorea.co.kr"
    searchKeyword = "인도네시아"
    searchURL = baseURL + "/Search/?stext=" + searchKeyword

    response = requests.get(searchURL)
    response.raise_for_status()

    soup = BeautifulSoup(response.content, "html.parser")

    sResultNodes = ""
    sResultPagination = ""

    result_divs = soup.select("div.list-default")
    for result_div in result_divs:
        sResultNodes += result_div.get_text() + "\n"

    pagination_divs = soup.select("div.tplPagination.newVer.wide")
    for pagination_div in pagination_divs:
        sResultPagination = pagination_div.get_text()

    print("Nodes:")
    print(sResultNodes)
    print("Pagination:")
    print(sResultPagination)


def f2():
    baseURL = "https://www.jobkorea.co.kr"
    searchKeyword = "인도네시아"
    searchURL = baseURL + "/Search/?stext=" + searchKeyword

    response = requests.get(searchURL)
    response.raise_for_status()

    soup = BeautifulSoup(response.content, "html.parser")

    sResult = ""

    post_list_items = soup.select(".list-post")
    for item in post_list_items:
        if "data-gavirturl" in item.attrs:
            surl = item["data-gavirturl"]
        else:
            surl = "N/A"

        if "data-gainfo" in item.attrs:
            sinfo = item["data-gainfo"]
        else:
            sinfo = "N/A"

        node1 = item.select_one(".post-list-corp")
        node2 = item.select_one(".post-list-info")

        if node1:
            node1_text = node1.get_text()
        else:
            node1_text = "N/A"

        if node2:
            node2_text = node2.get_text()
        else:
            node2_text = "N/A"

        sResult += surl + "\r\n" + sinfo + "\r\n" + node1_text + "\r\n" + node2_text + "\r\n\r\n"

    print(sResult)


def f3():
    baseURL = "https://www.jobkorea.co.kr"
    searchKeyword = "인도네시아"
    searchURL = baseURL + "/Search/?stext=" + urllib.parse.quote(searchKeyword)

    response = requests.get(searchURL)
    response.raise_for_status()

    soup = BeautifulSoup(response.content, "html.parser")

    sResult = ""

    pagination_links = soup.select("div.tplPagination.newVer.wide li a[href]")
    for link in pagination_links:
        href = link["href"]
        sResult += href + "\r\n"

    print(sResult)


if __name__ == "__main__":
    f1()
    # f2()
    # f3()
