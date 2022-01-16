// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

namespace Duende.IdentityServer.Akc.Management.Api
{
    internal static class TaskExtensions
    {
        public static Task<T> AsTask<T>(this T source) => Task.FromResult(source);
    }
}
