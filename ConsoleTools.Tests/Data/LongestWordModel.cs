using ConsoleTools.Binding;

namespace ConsoleTools.Tests.Data
{
    public class LongestWordModel
    {
        [Unbound("words", IsRequired = true)]
        public string[] Words { get; set; }
    }
}