﻿using System.ComponentModel;
using NUnit.Framework;

namespace Api.Common;

[EditorBrowsable(EditorBrowsableState.Never)]
internal class StringExtensionsTest
{
    [Test]
    [Parallelizable]
    public void TestToHexBytesEven()
    {
        var hex = "ffff";
        var bytes = hex.ToBytesFromHex();
        Assert.That(bytes, Is.EqualTo(new byte[] { 255, 255 }));
    }

    [Test]
    [Parallelizable]
    public void TestToHexBytesOdd()
    {
        var hex = "fffff";
        var bytes = hex.ToBytesFromHex();
        Assert.That(bytes, Is.EqualTo(new byte[] { 255, 255, 15 }));
    }

    [Test]
    [Parallelizable]
    public void TestToSnakeCase()
    {
        var snake = "HelloWorld".ToSnakeCase();
        Assert.That(snake, Is.EqualTo("hello_world"));
    }
}