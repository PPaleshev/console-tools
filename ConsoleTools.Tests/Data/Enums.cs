using System;

namespace ConsoleTools.Tests.Data
{
    [Flags]
    public enum FlagEnum
    {
        None = 0,
        Item1 = 1,
        Item2 = 2,
        Item3 = 4
    }

    public enum SimpleEnum
    {
        None,
        Item1,
        Item2,
        Item3
    }
}