using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AngleSharp.Dom.Html;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using StudentExercises.IntegrationTests.Helpers;
using AngleSharp.Dom;

namespace StudentExercises.IntegrationTests
{
    public class StudentControllerTests :
        IClassFixture<WebApplicationFactory<StudentExerciseMVC.Startup>>
    {
        private readonly HttpClient _client;

        public StudentControllerTests(WebApplicationFactory<StudentExerciseMVC.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task IndexDisplaysStudents()
        {
            // Arrange
            string url = "/students";

            // Act
            HttpResponseMessage response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            IHtmlDocument indexPage = await HtmlHelpers.GetDocumentAsync(response);
            IHtmlCollection<IElement> tds = indexPage.QuerySelectorAll("td");
            Assert.Contains(tds, td => td.TextContent.Trim() == "Ryan");
        }

        [Fact]
        public async Task Post_CreateStudent()
        {
            string url = "/students/create";
            HttpResponseMessage createPageResponse = await _client.GetAsync(url);
            IHtmlDocument createPage = await HtmlHelpers.GetDocumentAsync(createPageResponse);

            string firstName = "Groucho";
            string lastName = "Marx";
            string slack = "@groucho";
            string cohortId = "4";

            // Act
            HttpResponseMessage response = await _client.SendAsync(
                createPage,
                new Dictionary<string, string>
                {
                    {"student_FirstName", firstName},
                    {"student_LastName", lastName},
                    {"student_SlackHandle", slack },
                    {"student_CohortId", cohortId }
                });

            // Assert
            response.EnsureSuccessStatusCode();

            IHtmlDocument indexPage = await HtmlHelpers.GetDocumentAsync(response);
            Assert.Contains(
                indexPage.QuerySelectorAll("td"),
                td => td.TextContent.Contains(firstName));
            Assert.Contains(
                indexPage.QuerySelectorAll("td"),
                td => td.TextContent.Contains(lastName));
            Assert.Contains(
                indexPage.QuerySelectorAll("td"),
                td => td.TextContent.Contains(slack));
        }
    }
}
