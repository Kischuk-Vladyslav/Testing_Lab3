using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Testing_Lab3
{
    public class Tests
    {
        private ChromeDriver _driver;
        private WebDriverWait _wait;
        private const string Url = "https://the-internet.herokuapp.com/";
        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        /// Verifies that Delete button appears after clicking Add Element button
        /// </summary>
        [Test]
        public void AddButton_Click_ShouldAddNewElement()
        {
            _driver.Navigate().GoToUrl(Url + "add_remove_elements/");
            IWebElement newElement = _driver.FindElement(By.XPath("//button[text()='Add Element']"));

            newElement.Click();

            IWebElement deleteButton = _wait.Until(driver => driver.FindElement(By.XPath("//button[text()='Delete']")));
            Assert.That(deleteButton.Displayed, Is.True);
        }

        /// <summary>
        /// Verifies that checkbox is checked upon being clicked
        /// </summary>
        [Test]
        public void CheckBox_Click_ShouldCheckTheBox()
        {
            _driver.Navigate().GoToUrl(Url + "checkboxes");
            IWebElement checkbox = _driver.FindElement(By.XPath("//input[@type='checkbox'][1]"));

            checkbox.Click();

            Assert.That(checkbox.Selected, Is.True);
        }

        /// <summary>
        /// Verifies that correct option is selected after picking it on dropdown
        /// </summary>
        [Test]
        public void Dropdown_SelectOption_ShouldSelectOption()
        {
            _driver.Navigate().GoToUrl(Url + "dropdown");
            IWebElement dropdown = _driver.FindElement(By.Id("dropdown"));
            var selectElement = new SelectElement(dropdown);

            selectElement.SelectByText("Option 2");

            Assert.That(selectElement.SelectedOption.Text, Is.EqualTo("Option 2"));
        }

        /// <summary>
        /// Verifies whether value in InputField increments correctly
        /// </summary>
        [Test]
        public void Input_ArrowUp_ShouldIncrementValueCorrectly()
        {
            _driver.Navigate().GoToUrl(Url + "inputs");
            IWebElement inputField = _driver.FindElement(By.XPath("//input[@type='number']"));
            inputField.SendKeys("0");

            inputField.SendKeys(Keys.ArrowUp);

            Assert.That(inputField.GetAttribute("value"), Is.EqualTo("1"));
        }

        /// <summary>
        /// Verifies whether 404 HTTP code gets returned when clicking on 404 status code page
        /// </summary>
        [Test]
        public void StatusCodes_Click_ShouldDisplayCorrectStatusCode()
        {
            _driver.Navigate().GoToUrl(Url + "status_codes");
            IWebElement statusCodeLink = _driver.FindElement(By.XPath("//a[text()='404']"));

            statusCodeLink.Click();

            IWebElement statusCodeMessage = _wait.Until(driver => driver.FindElement(By.XPath("//p[contains(text(),'This page returned a 404 status code.')]")));
            Assert.That(statusCodeMessage.Displayed, Is.True);
        }

        /// <summary>
        /// Verifies whether checkbox dissapears after Remove button is clicked
        /// </summary>
        [Test]
        public void DynamicControls_Click_ShouldDisableCheckbox()
        {
            _driver.Navigate().GoToUrl(Url + "dynamic_controls");
            var button = _driver.FindElement(By.XPath("//button[@onclick='swapCheckbox()']"));
            var checkbox = _driver.FindElement(By.XPath("//input[@type='checkbox']"));

            button.Click();

            var message = _wait.Until(driver => driver.FindElement(By.Id("message")));
            var checkBoxes = _driver.FindElements(By.XPath("//input[@type='checkbox']"));
            Assert.That(checkBoxes, Is.Empty);
        }

        /// <summary>
        /// Verifies that file uploads successfully after clicking the Submit button
        /// </summary>
        [Test]
        public void FileUpload_ShouldUploadFileSuccessfully()
        {
            _driver.Navigate().GoToUrl(Url + "upload");
            var fileInput = _driver.FindElement(By.Id("file-upload"));
            var uploadButton = _driver.FindElement(By.Id("file-submit"));
            string filePath = @"C:\Users\kishu\source\repos\Testing_Lab3\Testing_Lab3\testfile.txt";
            if (!File.Exists(filePath))
                File.Create(filePath);

            fileInput.SendKeys(filePath);
            uploadButton.Click();

            var uploadedFiles = _wait.Until(driver => driver.FindElement(By.Id("uploaded-files")));
            Assert.That(uploadedFiles.Text, Is.EqualTo("testfile.txt"));
        }

        /// <summary>
        /// Verifies that JS alert displays successfully after clicking JS alert button
        /// </summary>
        [Test]
        public void JsAlerts_Click_ShouldDisplayAlert()
        {
            _driver.Navigate().GoToUrl(Url + "javascript_alerts");
            var jsAlertButton = _driver.FindElement(By.XPath("//button[text()='Click for JS Alert']"));

            jsAlertButton.Click();

            IAlert alert = _wait.Until(driver => driver.SwitchTo().Alert());
            Assert.That(alert.Text, Is.EqualTo("I am a JS Alert"));
            alert.Accept();
        }

        /// <summary>
        /// Verifies that new window with "new window" texxt appears after clicking the link
        /// </summary>
        [Test]
        public void MultipleWindows_ShouldOpenNewWindow()
        {
            _driver.Navigate().GoToUrl(Url + "windows");
            var element = _driver.FindElement(By.XPath("//a[text()='Click Here']"));

            element.Click();

            _wait.Until(driver => driver.WindowHandles.Count > 1);
            Assert.That(_driver.WindowHandles.Count.Equals(2), "An exactly two windows should be open");

            _driver.SwitchTo().Window(_driver.WindowHandles[1]);
            Assert.That(_driver.Title, Is.EqualTo("New Window"), "The new window should have 'New Window' title");
        }

        /// <summary>
        /// Verifies that mesage is displayed after clicking the link
        /// </summary>
        [Test]
        public void NotificationMessages_ShouldDisplayMessage()
        {
            _driver.Navigate().GoToUrl(Url + "notification_message_rendered");
            var link = _driver.FindElement(By.XPath("//a[text()='Click here']"));

            link.Click();

            var message = _wait.Until(driver => driver.FindElement(By.Id("flash")));
            Assert.That(message.Displayed, Is.True, "Notification message should be displayed");
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
