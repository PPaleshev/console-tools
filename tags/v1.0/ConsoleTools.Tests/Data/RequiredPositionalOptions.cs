using ConsoleTools.Binding;


namespace ConsoleTools.Tests.Data {
    public class RequiredPositionalOptions
    {
        [Positional("value", 0, Required = true)]
        public string Value { get; set; }
    }
}