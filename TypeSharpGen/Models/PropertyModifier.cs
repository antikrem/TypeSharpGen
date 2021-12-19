using System;


namespace TypeSharpGen.Builder
{
    public interface IPropertyModifier
    {
        PropertyDefinition Apply(PropertyDefinition property);
    }

    public class OverideName : IPropertyModifier
    {
        private string _name;

        public OverideName(string name)
        {
            _name = name;
        }

        public PropertyDefinition Apply(PropertyDefinition property)
        {
            property.OverrideName(_name);
            return property;
        }
    }
}
