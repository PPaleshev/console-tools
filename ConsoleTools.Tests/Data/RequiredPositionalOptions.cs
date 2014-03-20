using ConsoleTools.Binding;


namespace ConsoleTools.Tests.Data {
    public class RequiredPositionalOptions
    {
        [Positional("value", 0, IsRequired = true)]
        public string Value { get; set; }
    }
}