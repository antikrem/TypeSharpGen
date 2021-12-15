using System.Collections.Generic;

namespace TestApplication.Models
{
    class TestModel
    {
        public string Name { get; set; }

        public IEnumerable<string> Name2 { get; set; }

        public TestSubModel Child { get; set; }

        public DefinedTestSubModel Child2 { get; set; }

        public DependentType Dependent { get; }

        public IEnumerable<DependentType> Something(DefinedTestSubModel subModel)
        {
            return new List<DependentType> {
                new DependentType { Value = subModel.Value2 }
            };
        }
    }
}
