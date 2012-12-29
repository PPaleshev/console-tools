using System;


namespace ConsoleTools.Binding {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FlagAttribute : Attribute {
    }
}