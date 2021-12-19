using System;


namespace TypeSharpGen.Builder
{
    public interface IMethodModifier
    {
        MethodDefinition Apply(MethodDefinition method);
    }

    public class OverideReturnType : IMethodModifier
    {
        private Type _type;

        public OverideReturnType(Type type)
        {
            _type = type;
        }

        public MethodDefinition Apply(MethodDefinition method)
        {
            method.OverrideReturnType(_type);
            return method;
        }
    }
}
