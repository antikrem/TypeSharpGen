using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeSharpGenLauncher.Core.Model;

namespace TypeSharpGenLauncher.Core.Synthesiser
{
    public record DeclarationFile(
        string Location,
        IEnumerable<ITypeModel> Types
    )
    {
        IEnumerable<DeclarationFile> ImportedFiles(ISet<DeclarationFile> files)
        {
            foreach (var file in files)
            {
                if (ContainsDependentType(file))
                    yield return file;
            }
        }

        private bool ContainsDependentType(DeclarationFile file)
        {
            var fileTypes = file.Types.Select(type => type.Type);
            return Types.SelectMany(type => type.DependentTypes).Intersect(fileTypes).Count() > 0;
        }

    }
}
