// main
package main

import (
	"fmt"
	"log"
	"net/url"
	"sync"

	"github.com/gocolly/colly"
)

func f1(wg *sync.WaitGroup) {
	defer wg.Done()

	baseURL := "https://www.jobkorea.co.kr"
	searchKeyword := "인도네시아"
	searchURL := baseURL + "/Search/?stext=" + searchKeyword

	c := colly.NewCollector(
		colly.AllowedDomains("www.jobkorea.co.kr"),
	)

	var sResultNodes, sResultPagination string

	c.OnHTML("div.list-default", func(e *colly.HTMLElement) {
		e.ForEach(".list-post", func(i int, item *colly.HTMLElement) {
			sResultNodes += item.Text
			sResultNodes += "\n"
		})
	})

	c.OnHTML("div.tplPagination.newVer.wide", func(e *colly.HTMLElement) {
		sResultPagination = e.Text
	})

	c.OnError(func(r *colly.Response, err error) {
		log.Println("Request URL:", r.Request.URL, "failed with response:", r, "\nError:", err)
	})

	err := c.Visit(searchURL)
	if err != nil {
		log.Fatal(err)
	}

	fmt.Println("Nodes:")
	fmt.Println(sResultNodes)
	fmt.Println("Pagination:")
	fmt.Println(sResultPagination)
}

func f2(wg *sync.WaitGroup) {
	defer wg.Done()

	baseURL := "https://www.jobkorea.co.kr"
	searchKeyword := "인도네시아"
	searchURL := baseURL + "/Search/?stext=" + searchKeyword

	c := colly.NewCollector(
		colly.AllowedDomains("www.jobkorea.co.kr"),
	)

	var sResult string

	c.OnHTML("div.list-default", func(e *colly.HTMLElement) {
		e.ForEach(".list-post", func(i int, item *colly.HTMLElement) {
			surl := item.Attr("data-gavirturl")
			sinfo := item.Attr("data-gainfo")

			node1 := item.DOM.Find(".post-list-corp")
			node2 := item.DOM.Find(".post-list-info")

			sResult += surl + "\r\n" + sinfo + "\r\n" + node1.Text() + "\r\n" + node2.Text() + "\r\n\r\n"
		})
	})

	c.OnError(func(r *colly.Response, err error) {
		log.Println("Request URL:", r.Request.URL, "failed with response:", r, "\nError:", err)
	})

	err := c.Visit(searchURL)
	if err != nil {
		log.Fatal(err)
	}

	fmt.Println(sResult)
}

func f3(wg *sync.WaitGroup) {
	defer wg.Done()

	baseURL := "https://www.jobkorea.co.kr"
	searchKeyword := "인도네시아"
	searchURL := baseURL + "/Search/?stext=" + url.QueryEscape(searchKeyword)

	c := colly.NewCollector() // colly.New() 대신 colly.NewCollector() 사용

	var sResult string

	c.OnHTML("div.tplPagination.newVer.wide", func(e *colly.HTMLElement) {
		e.ForEach("li a[href]", func(_ int, item *colly.HTMLElement) {
			href := item.Attr("href")
			sResult += href + "\r\n"
		})
	})

	c.OnError(func(r *colly.Response, err error) {
		log.Println("Request URL:", r.Request.URL, "failed with response:", r, "\nError:", err)
	})

	err := c.Visit(searchURL)
	if err != nil {
		log.Fatal(err)
	}

	fmt.Println(sResult)
}

func main() {
	var wg sync.WaitGroup

	// 각 함수의 실행을 고루틴으로 시작
	wg.Add(3)
	go f1(&wg)
	go f2(&wg)
	go f3(&wg)

	// 모든 고루틴이 종료될 때까지 대기
	wg.Wait()
}
