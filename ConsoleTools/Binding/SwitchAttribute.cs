using System;


namespace ConsoleTools.Binding
{
    /// <summary>
    /// �������, ������� ���������� ���������, �������������� � ���� �����.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SwitchAttribute : Attribute
    {
    }
}