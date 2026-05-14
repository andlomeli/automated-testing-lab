namespace TestProject1;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using Microsoft.VisualStudio.CodeCoverage;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;



[TestFixture]
public class TestCases : TestBasics
{
    //Test 1: Returns the total of women's clothes categories inside the drop down menu
    [Test, Order(1)]
    public void ClothesCategories() 
    {
        //Checking if exceptions are null
        ArgumentNullException.ThrowIfNull(driver);
        driver.Navigate().GoToUrl("https://automationexercise.com/");
        Test.Log(Status.Info, "Navigating to automationexercise.com");


        //Find the women's category in the website by using .Name (by text)
        Test.Log(Status.Info, "Clicking WOMEN category to expand dropdown");
        var womenCategory = driver.FindElement(By.LinkText("WOMEN"));
        this.JsClick(womenCategory);  //Clicks the menu to expand drop down
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);

        //Get subcategories of women's clothes
        var subCategories = this.findElementSByXPath("//div[@id='Women']//a");


        //Log each category name found
        var categoryNames = new System.Text.StringBuilder();
        foreach(var item in subCategories) {

            categoryNames.Append($"{item.Text}");
            Console.WriteLine(item.Text);

        }

        var catString = categoryNames.ToString();
        Test.Log(Status.Info, $"Subcategories found under Women's section: {subCategories.Count}, {catString}");


        //Assert and lof the expected result and the actual result
        int expected = 3;
        int actual = subCategories.Count();
        Test.Log(Status.Info, $"Expected subcategory count: {expected} | Actual categories found: {actual}");

        Assert.That(expected, Is.EqualTo(actual));

     
    }

    //Test case 8: Verify all products and product detial page
    [Test, Order(2)]
    public void ProductDetailsPage() 
    {
        //Checking if exceptions are null
        ArgumentNullException.ThrowIfNull(driver);
        Test.Log(Status.Info, "Navigating to automationexercise.com");
        driver.Navigate().GoToUrl("https://automationexercise.com/");
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

        //Validate home page is visible by checking URL
        Assert.That(driver.Url, Is.EqualTo("https://automationexercise.com/"));
        Test.Log(Status.Pass, $"Home page confirmed: {driver.Url}");

        //Navigates to the 'Products' list
        Test.Log(Status.Info, "Clicking products navigation link");
        IWebElement productList = driver.FindElement(By.XPath("/html/body/header/div/div/div/div[2]/div/ul/li[2]/a"));
        this.JsClick(productList);

        //Waits frot he products page to load, and checks 'All products' title
        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h2[contains(text(),'All Products')]")));
        IWebElement heading = driver.FindElement(By.XPath("//h2[contains(text(),'All Products')]"));
        Assert.That(heading.Text, Does.Contain("ALL PRODUCTS"));
        Test.Log(Status.Pass, $"Products page loaded, header displayed: {heading.Text}");

        //Selects first item from list
        Test.Log(Status.Info, "Selecting first item from list");
        var productItem = driver.FindElement(By.LinkText("View Product"));
        this.JsClick(productItem);

        //Wait for product detail page to load
        wait.Until(d => d.Url.Contains("https://automationexercise.com/product_details/1"));
        Test.Log(Status.Pass, $"Products deatail page loaded, header displayed: {driver.Url}");

        //find page elements: product name, category, price, availability, condition and brand.
        var productName = this.findElementByCSS(".product-information h2");
        var productCategory = this.findElementByXPath("//div[@class='product-information']//p[contains(text(), 'Category')]");
        var productPrice = this.findElementByCSS( ".product-information span span");
        var availability = this.findElementByXPath("/html/body/section/div/div/div[2]/div[2]/div[2]/div/p[2]");
        var condition = this.findElementByXPath("/html/body/section/div/div/div[2]/div[2]/div[2]/div/p[2]");
        var brand = this.findElementByXPath("/html/body/section/div/div/div[2]/div[2]/div[2]/div/p[3]");


        // Log all scraped values before asserting so they appear in the report
        // whether the test passes or fails
        Test.Log(Status.Info,
            $"Product details found on page:<br/>" +
            $"<b>Name:</b> {productName.Text}<br/>" +
            $"<b>Category:</b> {productCategory.Text}<br/>" +
            $"<b>Price:</b> {productPrice.Text}<br/>" +
            $"<b>Availability:</b> {availability.Text}<br/>" +
            $"<b>Condition:</b> {condition.Text}<br/>" +
            $"<b>Brand:</b> {brand.Text}");


        //Assert agaisnt element text
        Assert.That(productName.Text, Is.EqualTo("Blue Top"));
        Test.Log(Status.Pass, $"Name assertion passed: {productName.Text}");

        Assert.That(productCategory.Text, Is.EqualTo("Category: Women > Tops"));
        Test.Log(Status.Pass, $"Categroy assertion passed: {productCategory.Text}");

        Assert.That(productPrice.Text, Is.EqualTo("Rs. 500"));
        Test.Log(Status.Pass, $"Price assertion passed: {productPrice.Text}");

        Assert.That(availability.Text, Is.EqualTo("Availability: In Stock"));
        Test.Log(Status.Pass, $"Availability assertion passed: {availability.Text}");

        Assert.That(condition.Text, Is.EqualTo("Condition: New"));
        Test.Log(Status.Pass, $"Condition assertion passed: {condition.Text}");

        Assert.That(brand.Text.Contains("Polo"), Is.True);
        Test.Log(Status.Pass, $"Brand assertion passed: {brand.Text}");

        //Testing commit
    }

    //Test case 9: Using the search functionality
    [Test, Order(3)]
    public void SearchProduct()
    {
        //Checking if exceptions are null
        ArgumentNullException.ThrowIfNull(driver);
        Test.Log(Status.Info, "Navigating to automationexercise.com");
        driver.Navigate().GoToUrl("https://automationexercise.com/");
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

        Assert.That(driver.Url, Is.EqualTo("https://automationexercise.com/"));
        Test.Log(Status.Pass, $"Home page confirmed: {driver.Url}");

        Test.Log(Status.Info, "Clicking products navigation link");
        IWebElement productList = driver.FindElement(By.XPath("/html/body/header/div/div/div/div[2]/div/ul/li[2]/a"));
        this.JsClick(productList);

        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h2[contains(text(),'All Products')]")));
        IWebElement heading = driver.FindElement(By.XPath("//h2[contains(text(),'All Products')]"));
        Assert.That(heading.Text, Does.Contain("ALL PRODUCTS"));
        Test.Log(Status.Pass, $"Products page loaded, header displayed: {heading.Text}");

        //Finding search bar element by Id
        Test.Log(Status.Info, "Finding search bar element");
        IWebElement searchBar = driver.FindElement(By.Id("search_product"));
        
        //Click search bar and type in product name
        searchBar.Click();
        searchBar.SendKeys("Green Side Placket Detail T - Shirt" + Keys.Enter);
        Test.Log(Status.Info, "Searching for item: Green Side Placket Detail T-Shirt ");

        //Validating Green shirt is visible in search results
        //pending

        //Clicks on item presented after item search
        Test.Log(Status.Info, "Selecting green t-shirt item from search results");
        var productItem = driver.FindElement(By.LinkText("View Product"));
        this.JsClick(productItem);

    }

}
