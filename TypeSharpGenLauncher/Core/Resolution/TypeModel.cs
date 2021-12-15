using System.Collections.Generic;
using System.Linq;

using EphemeralEx.Extensions;


namespace TypeSharpGenLauncher.Core.Resolution
{

    public interface ITypeModel : IEmmitableSymbol
    {
        public string OutputLocation { get; }

        IEnumerable<PropertyModel> Properties { get; }
        IEnumerable<MethodModel> Methods { get; }   

        IEnumerable<ITypeModel> DownstreamDependencies { get; }
    }

    public class TypeModel : ITypeModel
    {
        public string Symbol { get; }

        public string OutputLocation { get; }

        public List<PropertyModel> Properties { private get; set; } = new();
        IEnumerable<PropertyModel> ITypeModel.Properties => Properties;
        
        public List<MethodModel> Methods { private get; set; } = new();
        IEnumerable<MethodModel> ITypeModel.Methods => Methods;

        public IEnumerable<ITypeModel> Dependencies => this.ToEnumerable();

        public IEnumerable<ITypeModel> DownstreamDependencies
            => Sequence.From(
                Properties.SelectMany(property => property.Dependencies),
                Methods.SelectMany(property => property.Dependencies)
            );

        public TypeModel(string outputLocation, string name)
        {
            OutputLocation = outputLocation;
            Symbol = name;
        }
    }
}
