// This code is under Copyright (C) 2021 of Arkia Consulting SARL all right reserved

using Microsoft.AspNetCore.Http;

namespace Duende.IdentityServer.Akc.Admin.Api
{
    internal static class FrameworkExtensions
    {
        public static bool IsGet(this HttpRequest httpRequest) =>
            httpRequest.Method == HttpMethod.Get.Method;

        public static bool IsPut(this HttpRequest httpRequest) =>
            httpRequest.Method == HttpMethod.Put.Method;

        public static bool IsPost(this HttpRequest httpRequest) =>
            httpRequest.Method == HttpMethod.Post.Method;

        public static bool IsDelete(this HttpRequest httpRequest) =>
            httpRequest.Method == HttpMethod.Delete.Method;
    }
}
