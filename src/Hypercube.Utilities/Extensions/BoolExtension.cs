﻿namespace Hypercube.Utilities.Extensions;

public static class BoolExtension
{
    public static int ToInt(this bool value)
    {
        return value ? 1 : 0;
    }
}