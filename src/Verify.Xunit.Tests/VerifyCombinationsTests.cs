﻿public class VerifyCombinationsTests
{
    [Fact]
    public Task One()
    {
        string[] list = ["A", "b", "C"];
        return VerifyCombinations(
            _ => _.ToLower(),
            list);
    }

    [Fact]
    public Task KeysWithInvalidPathChars()
    {
        string[] list = ["/", "\\"];
        return VerifyCombinations(
            _ => _.ToLower(),
            list);
    }

    [Fact]
    public Task Two()
    {
        string[] a = ["A", "b", "C"];
        int[] b = [1, 2, 3];
        return VerifyCombinations(
            (a, b) => a.ToLower() + b,
            a, b);
    }

    [Fact]
    public Task WithScrubbed()
    {
        int[] years = [2020, 2022];
        int[] months = [2, 3];
        int[] dates = [12, 15];
        return VerifyCombinations(
            (year, month, date) => new DateTime(year, month, date),
            years, months, dates);
    }

    [Fact]
    public Task WithDontScrub()
    {
        int[] years = [2020, 2022];
        int[] months = [2, 3];
        int[] dates = [12, 15];
        return VerifyCombinations(
            (year, month, date) => new DateTime(year, month, date),
            years, months, dates)
            .DontScrubDateTimes();
    }

    [Fact]
    public Task Three()
    {
        string[] a = ["A", "b", "C"];
        int[] b = [1, 2, 3];
        bool[] c = [true, false];
        return VerifyCombinations(
            (a, b, c) => a.ToLower() + b + c,
            a, b, c);
    }

    [Fact]
    public Task MixedLengths()
    {
        string[] a = ["A", "bcc", "sssssC"];
        int[] b = [100, 2, 30];
        bool[] c = [true, false];
        return VerifyCombinations(
            (a, b, c) => a.ToLower() + b + c,
            a, b, c);
    }

    [Fact]
    public Task WithException()
    {
        string[] a = ["A", "b", "C"];
        int[] b = [1, 2, 3];
        bool[] c = [true, false];
        return VerifyCombinations(
            (a, b, c) =>
            {
                if (a == "b")
                {
                    throw new ArgumentException("B is not allowed");
                }

                return a.ToLower() + b + c;
            },
            a, b, c,
            captureExceptions: true);
    }

    [Fact]
    public Task UnBound()
    {
        string[] a = ["A", "b", "C"];
        int[] b = [1, 2, 3];
        bool[] c = [true, false];
        object[] d = [true, 4, false];
        var list = new List<IEnumerable<object?>>
        {
            a.Cast<object?>(),
            b.Cast<object?>(),
            c.Cast<object?>(),
            d.Cast<object?>()
        };
        return VerifyCombinations(
            _ =>
            {
                var a = (string)_[0]!;
                var b = (int)_[1]!;
                var c = (bool)_[2]!;
                var d = (bool)_[2]!;
                return a.ToLower() + b + c + d;
            },
            list);
    }
}