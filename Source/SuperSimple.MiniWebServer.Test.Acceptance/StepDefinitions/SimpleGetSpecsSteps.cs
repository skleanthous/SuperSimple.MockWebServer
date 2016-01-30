﻿using FluentAssertions;
using System;
using System.Net.Http;
using TechTalk.SpecFlow;

namespace SuperSimple.MiniWebServer.Test.Acceptance.StepDefinitions
{
    [Binding]
    public class SimpleGetSpecsSteps
    {
        private HttpClient client = new HttpClient();
        private string payload;

        private HttpResponseMessage response;

        public SimpleGetSpecsSteps()
        {
            payload = Guid.NewGuid().ToString();
            client.BaseAddress = new Uri("http://localhost:8182");
        }

        [Given(@"I post to resource (.*) with header (.*):(.*)")]
        public void GivenIPostToResourceWithHeader(string resource, string header, string headerValue)
        {
            var content = new StringContent(payload);
            content.Headers.Add(header, headerValue);
            client.PostAsync(resource, content).Wait();
        }

        [Given(@"I set resource (.*) with header (.*):(.*)")]
        public void GivenISetResourceWithHeader(string resource, string header, string headerValue)
        {
            var content = new StringContent(payload);
            content.Headers.Add(header, headerValue);
            content.Headers.Add("Set-Reply", "true");
            client.PostAsync(resource, content).Wait();
        }

        [When(@"I attempt a (.*) on resource (.*)")]
        public void WhenIAttemptAGetOnResourceMyResourceResourceId(string method, string resource)
        {
            var requestMessage = new HttpRequestMessage(new HttpMethod(method), resource);
            response = client.SendAsync(requestMessage).Result;
        }
        
        [Then(@"I should get back exactly what I set up")]
        public void ThenIShouldGetBackExactlyWhatISetUp()
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            response.Content.ReadAsStringAsync().Result.Trim().Should().Be(payload);
        }
    }
}
