using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RogueFlashNetSelenium
{
    class Program
    {
        private static string GetAppUrl()
        {
            //return "http://localhost:8080/RogueFlashJspHB/";
            //return "http://localhost:8080/RogueFlashJspEL/";
            return "http://localhost:51409/";
        }

        private static string GetPageSuffix()
        {
            //return ".go";
            return "";
        }


        private static IWebDriver getChromeDriver()
        {
            // See:
            // https://github.com/SeleniumHQ/selenium/wiki/ChromeDriver

            return new ChromeDriver("C:\\Temp\\");
        }


        static void Main(string[] args)
        {
            IWebDriver driver = getChromeDriver();

            // TODO:
            // http://stackoverflow.com/questions/6201425/wait-for-an-ajax-call-to-complete-with-selenium-2-web-driver

            try
            {
                TestDeleteDecks(driver);

                TestNewDeck(driver);
                TestNewCard(driver);
                TestReviewCard(driver);

                TestDeleteDecks(driver);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                driver.Quit();
            }
        }


        private static void TestDeleteDecks(IWebDriver driver)
        {
            Console.WriteLine("");
            Console.WriteLine("*** TestDeleteDecks");

            GoToPage(driver, "Decks", "Decks");
            Console.WriteLine("- Decks loaded");

            IWebElement filterTextElement = driver.FindElement(By.Id("filterText"));
            filterTextElement.Click();
            filterTextElement.SendKeys("Selenium Test Deck");
            WaitMillis(500);
            Console.WriteLine("- Decks filter, ok");

            IWebElement executeFilterElement = driver.FindElement(By.Id("executeFilter"));
            executeFilterElement.Click();
            WaitMillis(500);
            Console.WriteLine("- Decks filtered, ok");

            IReadOnlyCollection<IWebElement> itemContainerWrapperList = driver.FindElements(By.ClassName("itemContainerWrapper"));
            Console.WriteLine("- Decks found: " + itemContainerWrapperList.Count());

            if (itemContainerWrapperList.Count() == 0)
            {
                return;
            }

            foreach (IWebElement itemContainerWrapperElement in itemContainerWrapperList)
            {
                IWebElement itemSelectorElement = itemContainerWrapperElement.FindElement(By.ClassName("itemSelector"));
                itemSelectorElement.Click();
            }
            Console.WriteLine("- Decks selected, ok");

            ExecuteMenu(driver, "openActionMenu", "delete");
            WaitMillis(500);
            Console.WriteLine("- Decks deleted, ok");

            executeFilterElement.Click();
            Console.WriteLine("- Decks filtered, ok");

            itemContainerWrapperList = driver.FindElements(By.ClassName("itemContainerWrapper"));
            Console.WriteLine("- Decks found: " + itemContainerWrapperList.Count());

            if (itemContainerWrapperList.Count() > 0)
            {
                throw new Exception("Too many items to delete");
            }
        }

        private static void TestNewCard(IWebDriver driver)
        {
            Console.WriteLine("");
            Console.WriteLine("*** TestNewCard");

            GoToPage(driver, "Decks", "Decks");
            Console.WriteLine("- Decks loaded");

            IWebElement filterTextElement = driver.FindElement(By.Id("filterText"));
            filterTextElement.Click();
            filterTextElement.SendKeys("Selenium Test Deck");
            WaitMillis(500);
            Console.WriteLine("- Decks filter, ok");

            IWebElement executeFilterElement = driver.FindElement(By.Id("executeFilter"));
            executeFilterElement.Click();
            WaitMillis(500);
            Console.WriteLine("- Decks filtered, ok");

            IReadOnlyCollection<IWebElement> itemContainerWrapperList = driver.FindElements(By.ClassName("itemContainerWrapper"));
            Console.WriteLine("- Decks found: " + itemContainerWrapperList.Count());

            if (itemContainerWrapperList.Count() != 1)
            {
                throw new Exception("Decks expected 1");
            }

            IWebElement itemSelectorElement = itemContainerWrapperList.ElementAt(0);
            itemSelectorElement.Click();
            WaitMillis(500);
            Console.WriteLine("- Deck loaded");

            ExecuteMenu(driver, "openAppMenu", "newCard");
            WaitMillis(500);
            Console.WriteLine("- Card loaded");

            IWebElement sideAElement = driver.FindElement(By.Id("sideA"));
            sideAElement.Click();
            sideAElement.SendKeys("Side A");
            WaitMillis(100);
            Console.WriteLine("- Side A, ok");

            IWebElement sideBElement = driver.FindElement(By.Id("sideB"));
            sideBElement.Click();
            sideBElement.SendKeys("Side B");
            WaitMillis(100);
            Console.WriteLine("- Side B, ok");

            IWebElement notesElement = driver.FindElement(By.Id("notes"));
            notesElement.Click();
            notesElement.SendKeys("Notes");
            WaitMillis(100);
            Console.WriteLine("- Notes, ok");

            IWebElement tagsElement = driver.FindElement(By.Id("tags"));
            tagsElement.Click();
            tagsElement.SendKeys("Tags");
            WaitMillis(100);
            Console.WriteLine("- Tags, ok");

            ExecuteMenu(driver, "openAppMenu", "cards");
            WaitMillis(500);
            Console.WriteLine("- Cards loaded");

            itemContainerWrapperList = driver.FindElements(By.ClassName("itemContainerWrapper"));
            Console.WriteLine("- Cards found: " + itemContainerWrapperList.Count());

            if (itemContainerWrapperList.Count() != 1)
            {
                throw new Exception("Cards expected 1");
            }

            itemSelectorElement = itemContainerWrapperList.ElementAt(0);
            itemSelectorElement.Click();
            WaitMillis(500);
            Console.WriteLine("- Card loaded");

            sideAElement = driver.FindElement(By.Id("sideA"));
            if (!sideAElement.GetAttribute("value").Equals("Side A"))
            {
                throw new Exception("Side A different");
            }

            sideBElement = driver.FindElement(By.Id("sideB"));
            if (!sideBElement.GetAttribute("value").Equals("Side B"))
            {
                throw new Exception("Side B different");
            }

            notesElement = driver.FindElement(By.Id("notes"));
            if (!notesElement.GetAttribute("value").Equals("Notes"))
            {
                throw new Exception("Notes different");
            }

            tagsElement = driver.FindElement(By.Id("tags"));
            if (!tagsElement.GetAttribute("value").Equals("Tags"))
            {
                throw new Exception("Tags different");
            }
        }

        private static void TestNewDeck(IWebDriver driver)
        {
            Console.WriteLine("");
            Console.WriteLine("*** TestNewDeck");

            GoToPage(driver, "Decks", "Decks");
            Console.WriteLine("- Decks loaded");

            ExecuteMenu(driver, "openAppMenu", "newDeck");
            WaitMillis(500);
            Console.WriteLine("- Deck loaded");

            string deckDescription = "Selenium Test Deck " + DateTimeOffset.Now;
            IWebElement descriptionElement = driver.FindElement(By.Id("description"));
            descriptionElement.Click();
            descriptionElement.SendKeys(deckDescription);
            WaitMillis(100);
            Console.WriteLine("- Description, ok");

            string deckNotes = "Selenium Test Deck - Notes";
            IWebElement notesElement = driver.FindElement(By.Id("notes"));
            notesElement.Click();
            notesElement.SendKeys(deckNotes);
            WaitMillis(100);
            Console.WriteLine("- Notes, ok");

            ExecuteMenu(driver, "openAppMenu", "decks");
            WaitMillis(500);
            Console.WriteLine("- Decks loaded");

            IWebElement filterTextElement = driver.FindElement(By.Id("filterText"));
            filterTextElement.Click();
            filterTextElement.SendKeys("Selenium Test Deck");
            WaitMillis(500);
            Console.WriteLine("- Decks filter, ok");

            IWebElement executeFilterElement = driver.FindElement(By.Id("executeFilter"));
            executeFilterElement.Click();
            WaitMillis(500);
            Console.WriteLine("- Decks filtered, ok");

            IReadOnlyCollection<IWebElement> itemContainerWrapperList = driver.FindElements(By.ClassName("itemContainerWrapper"));
            Console.WriteLine("- Decks found: " + itemContainerWrapperList.Count());

            if (itemContainerWrapperList.Count() != 1)
            {
                throw new Exception("Decks expected 1");
            }

            IWebElement itemSelectorElement = itemContainerWrapperList.ElementAt(0);
            itemSelectorElement.Click();
            WaitMillis(500);
            Console.WriteLine("- Deck loaded");

            descriptionElement = driver.FindElement(By.Id("description"));
            if (!descriptionElement.GetAttribute("value").Equals(deckDescription))
            {
                throw new Exception("Description different");
            }

            notesElement = driver.FindElement(By.Id("notes"));
            if (!notesElement.GetAttribute("value").Equals(deckNotes))
            {
                throw new Exception("Notes different");
            }
        }

        private static void TestReviewCard(IWebDriver driver)
        {
            Console.WriteLine("");
            Console.WriteLine("*** TestReviewCard");

            GoToPage(driver, "Decks", "Decks");
            Console.WriteLine("- Decks loaded");

            IWebElement filterTextElement = driver.FindElement(By.Id("filterText"));
            filterTextElement.Click();
            filterTextElement.SendKeys("Selenium Test Deck");
            WaitMillis(500);
            Console.WriteLine("- Decks filter, ok");

            IWebElement executeFilterElement = driver.FindElement(By.Id("executeFilter"));
            executeFilterElement.Click();
            WaitMillis(500);
            Console.WriteLine("- Decks filtered, ok");

            IReadOnlyCollection<IWebElement> itemContainerWrapperList = driver.FindElements(By.ClassName("itemContainerWrapper"));
            Console.WriteLine("- Decks found: " + itemContainerWrapperList.Count());

            if (itemContainerWrapperList.Count() != 1)
            {
                throw new Exception("Decks expected 1");
            }

            IWebElement itemSelectorElement = itemContainerWrapperList.ElementAt(0);
            itemSelectorElement.Click();
            WaitMillis(500);
            Console.WriteLine("- Review loaded");

            if (driver.Title.IndexOf("Review") < 0)
            {
                throw new Exception("Review page expected");
            }

            IWebElement showAnswerElement = driver.FindElement(By.Id("showAnswer"));
            showAnswerElement.Click();
            IWebElement sideAElement = driver.FindElement(By.Id("sideA"));
            string sideAText = sideAElement.Text;
            IWebElement sideBElement = driver.FindElement(By.Id("sideB"));
            string sideBText = sideBElement.Text;
            IWebElement value3Element = driver.FindElement(By.Id("value3"));
            value3Element.Click();
            WaitMillis(500);
            Console.WriteLine("- Card answered");

            showAnswerElement = driver.FindElement(By.Id("showAnswer"));
            showAnswerElement.Click();

            sideAElement = driver.FindElement(By.Id("sideA"));
            if (!sideAElement.Text.Equals(sideBText))
            {
                throw new Exception("Card side unexpected");
            }

            sideBElement = driver.FindElement(By.Id("sideB"));
            if (!sideBElement.Text.Equals(sideAText))
            {
                throw new Exception("Card side unexpected");
            }
        }


        private static void ExecuteMenu(
                IWebDriver driver,
                string openMenuButtonId,
                string menuButtonId)
        {
            IWebElement openMenuButtonElement = driver.FindElement(By.Id(openMenuButtonId));
            openMenuButtonElement.Click();
            IWebElement buttonElement = driver.FindElement(By.Id(menuButtonId));
            buttonElement.Click();
        }


        private static void GoToPage(
                IWebDriver driver,
                string page,
                string title)
        {
            driver.Navigate().GoToUrl(GetAppUrl() + page + GetPageSuffix());
            
            var driverWait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            var until = driverWait.Until(ExpectedConditions.TitleIs("RogueFlash - " + title));
            if (!until)
            {
                throw new Exception("Page doesn't open");
            }
        }

        private static void WaitMillis(int millis)
        {
            Thread.Sleep(millis);
        }
    }
}
