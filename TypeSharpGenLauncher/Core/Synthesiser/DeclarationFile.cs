using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using TypeSharpGenLauncher.Core.Resolution;


namespace TypeSharpGenLauncher.Core.Synthesiser
{
    public record DeclarationFile(
        string Location,
        IEnumerable<ITypeModel> Types
    )
    {
        public IEnumerable<ITypeModel> Dependencies 
            => Types
                .SelectMany(type => type.DownstreamDependencies)
                .Distinct();
    }

    public class DeclarationFileComparer : IEqualityComparer<DeclarationFile>
    {
        public bool Equals(DeclarationFile? x, DeclarationFile? y)
        {
            return x != null && y != null && x.Location == y.Location;
        }

        public int GetHashCode([DisallowNull] DeclarationFile obj)
        {
            return obj.Location.GetHashCode();
        }
    }
}
