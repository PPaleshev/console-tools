using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using ConsoleTools.Conversion;

namespace ConsoleTools.Binding {
    /// <summary>
    /// �����, �������������� ����� ����������, ����������� � �������� ���������� ��������.
    /// </summary>
    public class PropertyMetadata
    {
        /// <summary>
        /// ������ ��������.
        /// </summary>
        public PropertyInfo Property { get; private set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ��� ��������, ������������ ������� � ����������.
        /// </summary>
        public Kind PropertyKind { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������� ��������.
        /// </summary>
        public string Meaning { get; private set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ����, ������ true, ���� �������� ����� ����������� ��� ��������, ����� false.
        /// </summary>
        public bool IsRequired { get; set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// �������� �����.
        /// </summary>
        public string Description { get; private set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// �������� �� ���������.
        /// </summary>
        public object DefaultValue { get; private set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������� ��������.
        /// </summary>
        public IConverter Converter { get; private set; }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// �������� �������, ����������� ��� ����������.
        /// </summary>
        public PropertySpecification Specification { get; private set; }

        /// <summary>
        /// ������ ����� ������ ���������� ��������.
        /// </summary>
        /// <param name="property">������ �������������� ��������.</param>
        PropertyMetadata(PropertyInfo property)
        {
            Property = property;
            Specification = new PropertySpecification();
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ������ ���������� �� ��������.
        /// </summary>
        /// <param name="property">�������������� ��������.</param>
        /// <param name="attr">�������, ����������� �����������.</param>
        public static PropertyMetadata ReadFromProperty(PropertyInfo property, ModelBindingAttribute attr)
        {
            var meta = new PropertyMetadata(property);
            meta.PropertyKind = attr.GetPropertyKind();
            meta.IsRequired = attr.IsRequired;
            meta.DefaultValue = attr.DefaultValue;
            meta.Meaning = attr.Meaning;
            meta.Description = attr.Description;
            meta.Converter = GetPropertyConverter(property);
            meta.Specification.IsCollection = typeof(IList).IsAssignableFrom(property.PropertyType);
            UpdateSpecifications(property, meta.Specification);
            return meta;
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ���������� ���������, ������������ ��� �������������� ��������� �������� � ������� ���� ��������.
        /// </summary>
        static IConverter GetPropertyConverter(PropertyInfo property)
        {
            var attributes = Attribute.GetCustomAttributes(property);
            var provider = (IConverterProvider) attributes.FirstOrDefault(attribute => attribute is IConverterProvider);
            if (provider != null)
                return provider.GetConverter(property.PropertyType);
            var converter = (IConverter) attributes.FirstOrDefault(a => a is IConverter);
            return converter ?? Converters.GetConverterForType(property.PropertyType);
        }

        //----------------------------------------------------------------------[]
        /// <summary>
        /// ������ �������������� ������������
        /// </summary>
        /// <param name="property">������ ��������.</param>
        /// <param name="spec">������ ��� ��������� ����������� �������.</param>
        static void UpdateSpecifications(PropertyInfo property, PropertySpecification spec)
        {
            var attributes = (SpecificationAttribute[]) Attribute.GetCustomAttributes(property, typeof (SpecificationAttribute));
            foreach (SpecificationAttribute attribute in attributes)
                attribute.UpdateSpecification(spec);
        }
    }
}
