﻿partial class InnerVerifier
{
    static IEnumerable<Target> emptyTargets = Enumerable.Empty<Target>();

    public Task<VerifyResult> VerifyJson(string? target)
    {
        if (target is null)
        {
            return VerifyInner(target, null, emptyTargets, true);
        }

        return VerifyJson(JToken.Parse(target));
    }

    public async Task<VerifyResult> VerifyJson(Stream? target)
    {
        if (target is null)
        {
            return await VerifyInner(target, null, emptyTargets, true);
        }

        using var reader = new StreamReader(target);
        using var textReader = new JsonTextReader(reader);
        var json = await JToken.LoadAsync(textReader);
        return await VerifyJson(json);
    }

    public Task<VerifyResult> VerifyJson(JToken target) =>
        VerifyInner(target, null, emptyTargets, true);

    public async Task<VerifyResult> Verify(object? target)
    {
        if (target is byte[])
        {
            throw new("Use Verify(byte[] target, string extension)");
        }

        if (target is string)
        {
            throw new("Use Verify(string target, string extension)");
        }

        if (target is Stream)
        {
            throw new("Use Verify(Stream target, string extension)");
        }

        if (target is null)
        {
            return await VerifyInner(target, null, emptyTargets, true);
        }

        if (target.GetType().ImplementsStreamEnumerable())
        {
            throw new("Use Verify(IEnumerable<T> targets, string extension)");
        }

        if (VerifierSettings.TryGetToString(target, out var toString))
        {
            var stringResult = toString(target, settings.Context);
            if (stringResult.Extension is null)
            {
                return await VerifyString(stringResult.Value);
            }

            return await VerifyString(stringResult.Value, stringResult.Extension);
        }

        if (VerifierSettings.TryGetTypedConverter(target, settings, out var converter))
        {
            var result = await converter.Conversion(target, settings.Context);
            return await VerifyInner(result.Info, result.Cleanup, result.Targets, true);
        }

        return await VerifyInner(target, null, emptyTargets, true);
    }
}