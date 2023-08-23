import requests
from bs4 import BeautifulSoup

nLastIndex = 0  # 전역 변수로 선언


def resultNum():
    global nLastIndex
    baseURL = "https://www.dhlottery.co.kr/gameResult.do?method=byWin"
    searchURL = baseURL

    try:
        response = requests.get(searchURL)
        response.raise_for_status()
    except requests.exceptions.RequestException as e:
        print("Error:", e)
        return

    soup = BeautifulSoup(response.content, "html.parser")

    singlenode1 = soup.find('div', class_='contentsArticle')
    if not singlenode1:
        print("singlenode1")
        return

    singlenode2 = singlenode1.find('select', id='dwrNoList')
    if not singlenode2:
        print("singlenode2")
        return

    # 첫번째 값만 가져온다.
    optionNode = singlenode2.find('option')
    if optionNode:
        value = optionNode.get('value', '')
        text = optionNode.get_text()
        sResult = f"{value} : {text}\r\n"
        # print(sResult)
        nLastIndex = int(text)
    else:
        print("No option found")
        return

    '''
    # 전체 값을 가져온다.
    optionNodes = singlenode2.find_all('option')
    sResult = ""
    if optionNodes:
        for optionNode in optionNodes:
            value = optionNode.get('value', '')
            text = optionNode.get_text()
            sResult += f"{value} : {text}\r\n"
            #print(sResult)
    else:
        print("No options found")        
    '''


def resultlastNum():
    global nLastIndex
    baseURL = "https://www.dhlottery.co.kr/gameResult.do?method=byWin&drwNo="
    searchURL = baseURL + str(nLastIndex)
    # print(searchURL)

    try:
        response = requests.get(searchURL)
        response.raise_for_status()
    except requests.exceptions.RequestException as e:
        print("Error:", e)
        return

    soup = BeautifulSoup(response.content, "html.parser")

    singlenode1 = soup.find('div', class_='contentsArticle')
    if not singlenode1:
        print("singlenode1")
        return

    sResult = ""

    # 당첨번호
    numbernode = singlenode1.find('div', class_='win_result')
    if numbernode:
        h4Node = numbernode.find('h4').find('strong')
        sh = h4Node.get_text().strip()
        # print(sh)

        descNode = numbernode.find('p', class_='desc')
        sDesc = descNode.get_text().strip()
        # print(sDesc)

        winNodes = numbernode.find_all('div', class_='nums')
        for item in winNodes:
            node1 = item.find('div', class_='num win')
            if node1:
                spanNodes = item.find_all('span')
                if spanNodes:
                    s1 = spanNodes[0].get_text().strip()
                    s2 = spanNodes[1].get_text().strip()
                    s3 = spanNodes[2].get_text().strip()
                    s4 = spanNodes[3].get_text().strip()
                    s5 = spanNodes[4].get_text().strip()
                    s6 = spanNodes[5].get_text().strip()
                    sResult += f"{s1}|{s2}|{s3}|{s4}|{s5}|{s6}\r\n"
                    # print(sResult)

            node2 = item.find('div', class_='num bonus')
            if node2:
                spanNode = node2.find('p').find('span')
                if spanNode:
                    s1 = spanNode.get_text().strip()
                    sResult += f"{s1}\r\n"
                    # print(sResult)

    # 1등부터 5등까지 당첨정보
    bodyNode = singlenode1.find('tbody')
    if bodyNode:
        trNodes = bodyNode.find_all('tr')
        for item in trNodes:
            tdNodes = item.find_all('td')
            if tdNodes:
                s1 = tdNodes[0].get_text().strip()
                s2 = tdNodes[1].get_text().strip()
                s3 = tdNodes[2].get_text().strip()
                s4 = tdNodes[3].get_text().strip()
                s5 = tdNodes[4].get_text().strip()
                sResult += f"{s1}|{s2}|{s3}|{s4}|{s5}\r\n"
                # print(sResult)
    print(sResult)


def resultAllNum():
    global nLastIndex
    baseURL = "https://www.dhlottery.co.kr/gameResult.do?method=byWin&drwNo="
    sResult = ""
    s1, s2, s3, s4, s5, s6 = "", "", "", "", "", ""

    for i in range(1, nLastIndex + 1):
        sUrlIndex = f"{baseURL}{i}"
        # print(sUrlIndex)

        try:
            response = requests.get(sUrlIndex)
            response.raise_for_status()

            soup = BeautifulSoup(response.content, "html.parser")

            singlenode1 = soup.find('div', class_='contentsArticle')
            if singlenode1 is None:
                continue

            # 당첨번호
            numbernode = singlenode1.find('div', class_='win_result')
            if numbernode:
                winNodes = numbernode.find_all('div', class_='nums')
                if winNodes:
                    for item in winNodes:
                        node1 = item.find('div', class_='num win')
                        if node1:
                            spanNodes = item.find_all('span')
                            if spanNodes:
                                s1 = spanNodes[0].get_text().strip()
                                s2 = spanNodes[1].get_text().strip()
                                s3 = spanNodes[2].get_text().strip()
                                s4 = spanNodes[3].get_text().strip()
                                s5 = spanNodes[4].get_text().strip()
                                s6 = spanNodes[5].get_text().strip()
                                sResult += f"{s1}|{s2}|{s3}|{s4}|{s5}|{s6}\r\n"
                                #print(sResult)
        except Exception as e:
            print("Error:", e)
            continue
    print(sResult)


if __name__ == "__main__":
    resultNum()
    # resultlastNum()
    resultAllNum()
