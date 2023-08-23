// main
package main

import (
	"fmt"
	"log"
	"strconv"
	"strings"

	"github.com/gocolly/colly"
)

var nLastIndex int

func resultNum() {
	baseURL := "https://www.dhlottery.co.kr/gameResult.do?method=byWin"
	searchURL := baseURL

	c := colly.NewCollector()

	c.OnRequest(func(r *colly.Request) {
		//fmt.Println("Visiting", r.URL)
	})

	c.OnError(func(_ *colly.Response, err error) {
		log.Println("Request URL:", searchURL, "failed with response:", err)
	})

	c.OnHTML("div.contentsArticle", func(e *colly.HTMLElement) {

		singlenode2 := e.ChildText("select#dwrNoList")
		if singlenode2 == "" {
			fmt.Println("singlenode2")
			return
		}

		//첫번째 값만 가져온다.
		optionNodes := e.ChildAttrs("select#dwrNoList option", "value")
		if len(optionNodes) > 0 {
			nLastIndexStr := optionNodes[0]
			nLastIndexInt, err := strconv.Atoi(nLastIndexStr)
			if err != nil {
				log.Fatal("Error converting nLastIndexStr to int:", err)
			}
			nLastIndex = nLastIndexInt
			fmt.Println("마지막회차:", nLastIndex)
		}

		//전체 값을 가져온다.
		/*
			for _, value := range optionNodes {
				fmt.Println("Value:", value)
			}
		*/
	})

	err := c.Visit(searchURL)
	if err != nil {
		log.Fatal(err)
	}
}

func resultlastNum() {
	baseURL := "https://www.dhlottery.co.kr/gameResult.do?method=byWin&drwNo="
	searchURL := baseURL + fmt.Sprintf("%d", nLastIndex)

	c := colly.NewCollector()

	var sResult strings.Builder

	c.OnRequest(func(r *colly.Request) {
		fmt.Println("url:", r.URL)
	})

	c.OnError(func(r *colly.Response, err error) {
		log.Println("url:", r.Request.URL, " response:", r, "\nError:", err)
	})

	c.OnHTML(".contentsArticle", func(e *colly.HTMLElement) {

		numberNode := e.ChildText(".win_result h4 strong")
		descNode := e.ChildText(".win_result p.desc")

		sResult.WriteString(numberNode + "\n")
		sResult.WriteString(descNode + "\n")

		e.ForEach(".win_result .nums", func(_ int, item *colly.HTMLElement) {

			node1 := item.ChildText(".num.win")
			if node1 != "" {
				spanNodes := item.ChildTexts("span")
				sResult.WriteString(strings.Join(spanNodes, "|") + "\n")
			}

			node2 := item.ChildText(".num.bonus p span")
			if node2 != "" {
				sResult.WriteString(node2 + "\n")
			}
		})

		e.ForEach(".contentsArticle tbody tr", func(_ int, item *colly.HTMLElement) {
			tdNodes := item.ChildTexts("td")
			sResult.WriteString(strings.Join(tdNodes, "|") + "\n")
		})
	})

	err := c.Visit(searchURL)
	if err != nil {
		log.Fatal("Error:", err)
	}

	fmt.Println(sResult.String())
}

func resultAllNum() {

	var baseURL = "https://www.dhlottery.co.kr/gameResult.do?method=byWin&drwNo="
	var sResult strings.Builder

	for i := 1; i <= nLastIndex; i++ {

		sUrlIndex := fmt.Sprintf("%s%d", baseURL, i)

		c := colly.NewCollector()

		c.OnRequest(func(r *colly.Request) {
			//fmt.Println("url:", r.URL)
		})

		c.OnError(func(_ *colly.Response, err error) {
			log.Println("url:", sUrlIndex, " error:", err)
		})

		c.OnHTML(".contentsArticle", func(e *colly.HTMLElement) {

			//numberNode := e.ChildText(".win_result h4 strong")
			//descNode := e.ChildText(".win_result p.desc")

			e.ForEach(".win_result .nums", func(_ int, item *colly.HTMLElement) {

				node1 := item.ChildText(".num.win")
				if node1 != "" {
					spanNodes := item.ChildTexts("span")
					sResult.WriteString(strings.Join(spanNodes, "|") + "\n")
				}
			})
		})

		err := c.Visit(sUrlIndex)
		if err != nil {
			fmt.Println("Error:", err)
			continue
		}
	}
	fmt.Println(sResult.String())
}

func main() {
	resultNum()
	//resultlastNum()
	resultAllNum()
}
