using System;


namespace ConsoleTools {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FlagAttribute : Attribute {
    }
}