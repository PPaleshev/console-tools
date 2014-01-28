using System;

namespace ConsoleTools.Binding
{
    /// <summary>
    /// јтрибут, которым помечаетс€ свойство, в котором будет содержатьс€ список аргументов, которые не удалось св€зать в процессе прив€зки.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UnboundOptionsAttribute : OptionBindingAttribute
    {
        /// <summary>
        /// —оздаЄт новый экземпл€р атрибута.
        /// </summary>
        public UnboundOptionsAttribute()
            : base(false)
        {
        }

        public override void FillMetadata(OptionMetadata metadata)
        {
            metadata.OptionType = OptionType.Unbound;
        }
    }
}