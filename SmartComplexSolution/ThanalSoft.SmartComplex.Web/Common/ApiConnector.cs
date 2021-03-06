﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using ThanalSoft.SmartComplex.Web.Security;

namespace ThanalSoft.SmartComplex.Web.Common
{
    public class ApiConnector<TResponse> where TResponse : new()
    {
        private SmartComplexPrincipal User => HttpContext.Current.User as SmartComplexPrincipal;

        private string ApiBaseURL => ConfigurationManager.AppSettings["API_URL"];

        //public async Task<TResponse> GetAsync(string pController, string pAction, params string[] pParameters)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(ApiBaseURL);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        HttpResponseMessage response;
        //        if (pParameters != null && pParameters.Any())
        //        {
        //            var formattedParams = string.Join("/", pParameters);
        //            response = await client.GetAsync($"api/{pController}/{pAction}/{formattedParams}");
        //        }
        //        else
        //            response = await client.GetAsync($"api/{pController}/{pAction}");

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var result = await response.Content.ReadAsAsync<TResponse>();
        //            return result;
        //        }
        //        throw new Exception();
        //    }
        //}

        public async Task<TResponse> PostAsync<TParameter>(string pController, string pAction, TParameter pParameter)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBaseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsJsonAsync($"api/{pController}/{pAction}", pParameter);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<TResponse>();
                    return result;
                }

                return new TResponse();
            }
        }
        
        public async Task<TResponse> SecureGetAsync(string pController, string pAction, params string[] pParameters)
        {
            if (Thread.CurrentPrincipal == null)
                return default(TResponse);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.UserIdentity);
                client.BaseAddress = new Uri(ApiBaseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                if (pParameters != null && pParameters.Any())
                {
                    var formattedParams = string.Join("/", pParameters);
                    response = await client.GetAsync($"api/{pController}/{pAction}/{formattedParams}");
                }
                else
                    response = await client.GetAsync($"api/{pController}/{pAction}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<TResponse>();
                    return result;
                }

                return new TResponse();
            }
        }

        public async Task<TResponse> SecurePostAsync<TParameter>(string pController, string pAction, TParameter pParameter)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.UserIdentity);
                client.BaseAddress = new Uri(ApiBaseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsJsonAsync($"api/{pController}/{pAction}", pParameter);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<TResponse>();
                    return result;
                }

                return new TResponse();
            }
        }

        internal async Task<string> GetApiToken(string pUserName, string pPassword)
        {
            using (var client = new HttpClient())
            {
                //setup client
                client.BaseAddress = new Uri(ApiBaseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //setup login data
                var formContent = new FormUrlEncodedContent(new[]
                {
                     new KeyValuePair<string, string>("grant_type", "password"),
                     new KeyValuePair<string, string>("username", pUserName),
                     new KeyValuePair<string, string>("password", pPassword)
                     });

                //send request
                var responseMessage = await client.PostAsync("secureaccess", formContent);

                //get access token from response body
                var responseJson = await responseMessage.Content.ReadAsStringAsync();
                var jObject = JObject.Parse(responseJson);
                if(jObject.GetValue("access_token") != null)
                    return jObject.GetValue("access_token").ToString();

                return string.Empty;
            }
        }
    }
}