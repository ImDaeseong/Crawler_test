// main
package main

import (
	"fmt"
	"log"
	"strconv"
	"strings"
	"sync"

	"github.com/gocolly/colly"
)

var nLastIndex int
var mu sync.Mutex

func resultNum(wg *sync.WaitGroup) {
	defer wg.Done()

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
			mu.Lock()
			nLastIndex = nLastIndexInt
			mu.Unlock()
			fmt.Println("마지막회차:", nLastIndex)
		}
	})

	err := c.Visit(searchURL)
	if err != nil {
		log.Fatal(err)
	}
}

func resultlastNum(wg *sync.WaitGroup) {
	defer wg.Done()

	mu.Lock()
	searchURL := fmt.Sprintf("https://www.dhlottery.co.kr/gameResult.do?method=byWin&drwNo=%d", nLastIndex)
	mu.Unlock()

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

func resultAllNum(wg *sync.WaitGroup) {
	defer wg.Done()

	mu.Lock()
	baseURL := "https://www.dhlottery.co.kr/gameResult.do?method=byWin&drwNo="
	nLastIndexCopy := nLastIndex
	mu.Unlock()

	var sResult strings.Builder

	for i := 1; i <= nLastIndexCopy; i++ {
		sUrlIndex := fmt.Sprintf("%s%d", baseURL, i)

		c := colly.NewCollector()

		c.OnRequest(func(r *colly.Request) {
			//fmt.Println("url:", r.URL)
		})

		c.OnError(func(_ *colly.Response, err error) {
			log.Println("url:", sUrlIndex, " error:", err)
		})

		c.OnHTML(".contentsArticle", func(e *colly.HTMLElement) {

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
	var wg sync.WaitGroup

	// 각 함수의 실행을 고루틴으로 시작
	wg.Add(1)
	go resultNum(&wg)

	wg.Wait()

	wg.Add(1)
	go resultlastNum(&wg)

	wg.Wait()

	wg.Add(1)
	go resultAllNum(&wg)

	// 모든 고루틴이 종료될 때까지 대기
	wg.Wait()
}
