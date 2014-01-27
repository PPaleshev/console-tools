using System;


namespace ConsoleTools.Binding
{
    /// <summary>
    /// Атрибут, которым помечаются аргументы, представленные в виде флага.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SwitchAttribute : Attribute
    {
    }
}