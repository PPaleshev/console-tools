using ConsoleTools.Tests.Data;
using NUnit.Framework;

namespace ConsoleTools.Tests
{
    /// <summary>
    /// Тесты частей модели.
    /// </summary>
    [TestFixture]
    public class PartTests
    {
        /// <summary>
        /// Парсер для аргументов командной строки.
        /// </summary>
        static readonly ArgumentParser PARSER = new ArgumentParser(new[] {"/"}, new[] {'='});

        /// <summary>
        /// Простой тест для проверки связывания частей модели.
        /// </summary>
        [Test]
        public void StaticTest()
        {
            var model = ModelBinder.BindTo<StaticPartModel>(PARSER.Parse("/type=item", "/1=hello", "/2=7"));
            Assert.NotNull(model);
            Assert.AreEqual("item", model.Type);

            Assert.NotNull(model.Part);
            Assert.AreEqual("hello", model.Part.Property1);
            Assert.AreEqual(7, model.Part.Property2);
            Assert.NotNull(model.Part2);
            Assert.AreEqual("hello", model.Part2.Property1);
            Assert.AreEqual(7, model.Part2.Property2);
            Assert.NotNull(model.Part2.Nested);
            Assert.AreEqual("hello", model.Part2.Nested.Property1);
            Assert.AreEqual(7, model.Part2.Nested.Property2);
        }

        /// <summary>
        /// Тест динамической модели.
        /// Модель параметров зависит от типа команды.
        /// </summary>
        [TestCase("/command=create", "/name=Пиотр", "/value=Зелёнкин")]
        [TestCase("/value=Зелёнкин", "/command=create", "/name=Пиотр")]
        [TestCase("/name=Пиотр", "/value=Зелёнкин", "/command=create")]
        public void DynamicTest(params string[] args)
        {
            var model = ModelBinder.BindTo<DynamicPartModel>(PARSER.Parse(args));
            Assert.NotNull(model);
            Assert.AreEqual("create", model.Command);
            Assert.IsInstanceOf<CreateParams>(model.Params);
            var p = model.Params as CreateParams;
            Assert.AreEqual("Пиотр", p.Name);
            Assert.AreEqual("Зелёнкин", p.Value);
        }

        /// <summary>
        /// Тест динамической модели.
        /// Модель параметров зависит от типа команды.
        /// </summary>
        [TestCase("/command=remove", "/id=2")]
        [TestCase("/id=2", "/command=remove")]
        [TestCase("/id=2", "/command=remove")]
        public void YetAnoterDynamicTest(params string[] args)
        {
            var model = ModelBinder.BindTo<DynamicPartModel>(PARSER.Parse(args));
            Assert.NotNull(model);
            Assert.AreEqual("remove", model.Command);
            Assert.IsInstanceOf<RemoveParams>(model.Params);
            var p = model.Params as RemoveParams;
            Assert.AreEqual("2", p.Id);
        }
    }
}