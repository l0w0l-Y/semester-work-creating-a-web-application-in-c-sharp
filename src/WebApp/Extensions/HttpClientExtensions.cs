﻿using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApp.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
            where T : class
        {
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (string.IsNullOrEmpty(dataAsString))
                return null;
            
            return JsonSerializer.Deserialize<T>(dataAsString,
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
        }
    }
}