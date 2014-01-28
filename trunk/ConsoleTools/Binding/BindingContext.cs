using System;
using System.ComponentModel;

namespace ConsoleTools.Binding {
    /// <summary>
    /// –еализаци€ контекста св€зывани€.
    /// </summary>
    internal class BindingContext : ITypeDescriptorContext {
        /// <summary>
        /// Ёкземпл€р объекта, которому принадлежат свойства.
        /// </summary>
        private readonly object instance;

        /// <summary>
        /// ƒескриптор свойства целевого объекта.
        /// </summary>
        private readonly PropertyDescriptor descriptor;

        /// <summary>
        /// —оздаЄт новый экземпл€р контекста св€зывани€.
        /// </summary>
        /// <param name="instance">Ёкземпл€р объекта, свойства которого св€зываютс€.</param>
        /// <param name="metadata">ћетаданные опции, значение которой св€зываетс€.</param>
        public BindingContext(object instance, OptionMetadata metadata) {
            this.instance = instance;
            descriptor = metadata.PropertyDescriptor;
        }

        object IServiceProvider.GetService(Type serviceType) {
            throw new NotSupportedException();
        }

        bool ITypeDescriptorContext.OnComponentChanging() {
            throw new NotSupportedException();
        }

        //----------------------------------------------------------------------[]
        void ITypeDescriptorContext.OnComponentChanged() {
            throw new NotSupportedException();
        }

        //----------------------------------------------------------------------[]
        IContainer ITypeDescriptorContext.Container {
            get { throw new NotSupportedException(); }
        }

        //----------------------------------------------------------------------[]
        object ITypeDescriptorContext.Instance {
            get { return instance; }
        }

        //----------------------------------------------------------------------[]
        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor {
            get { return descriptor; }
        }
    }
}