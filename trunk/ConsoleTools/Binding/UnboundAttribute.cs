using System;

namespace ConsoleTools.Binding
{
    /// <summary>
    /// јтрибут, которым помечаетс€ свойство, в котором будет содержатьс€ список аргументов, которые не удалось св€зать в процессе прив€зки.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UnboundAttribute : ModelBindingAttribute
    {
        /// <summary>
        /// —оздаЄт новый экземпл€р атрибута.
        /// </summary>
        public UnboundAttribute()
            : base(false)
        {
        }

        public override void FillMetadata(PropertyMetadata metadata)
        {
            metadata.PropertyKind = Kind.Unbound;
        }
    }
}