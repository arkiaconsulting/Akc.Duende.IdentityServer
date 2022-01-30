// This code is under Copyright (C) 2022 of Arkia Consulting SARL all right reserved

using CSharpFunctionalExtensions;

namespace Akc.Duende.IdentityServer.Management.Api
{
    internal static class TaskExtensions
    {
        public static Task<T> AsTask<T>(this T source) =>
            Task.FromResult(source);

        public static Task<Result<T>> AsTask<T>(this Result<T> source) =>
            Task.FromResult(source);

        public static Task<Result> ForgetValue<T>(this Task<Result<T>> source) =>
            source.Bind(source => Result.Success());

        public static Task<Result> ForgetValue<T>(this Result<T> source) =>
            source.Bind(source => Result.Success())
            .AsTask();
    }
}
