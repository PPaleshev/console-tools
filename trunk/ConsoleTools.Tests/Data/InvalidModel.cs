using System.Collections.Generic;
using ConsoleTools.Binding;

namespace ConsoleTools.Tests.Data
{
    /// <summary>
    /// Пример некорректной модели с несколькими полями, отмеченными атрибутом <see cref="UnboundAttribute"/>.
    /// </summary>
    public class InvalidModel
    {
        [Unbound]
        public List<string> Unbound1 { get; set; }

        [Unbound]
        public List<int> Unbound2 { get; set; }
    }
}