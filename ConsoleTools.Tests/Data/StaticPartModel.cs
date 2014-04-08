using ConsoleTools.Binding;

namespace ConsoleTools.Tests.Data
{
    /// <summary>
    /// Пример модели с одной динамической частью.
    /// </summary>
    public class StaticPartModel
    {
        readonly Part1 part = new Part1();
        readonly Part2 part2 = new Part2();

        [Named("type")]
        public string Type { get; set; }

        [Part]
        public Part1 Part
        {
            get { return part; }
        }

        [Part]
        public Part2 Part2
        {
            get { return part2; }
        }
    }

    /// <summary>
    /// Часть модели.
    /// </summary>
    public class Part1
    {
        [Named("1")]
        public string Property1 { get; set; }

        [Named("2")]
        public int Property2 { get; set; }
    }

    public class Part2 : Part1
    {
        readonly NestedPart nested = new NestedPart();

        [Part]
        public NestedPart Nested
        {
            get { return nested; }
        }
    }

    public class NestedPart
    {
        [Named("1")]
        public string Property1 { get; set; }

        [Named("2")]
        public int Property2 { get; set; }
    }
}