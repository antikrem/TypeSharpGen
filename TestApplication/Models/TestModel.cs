namespace TestApplication.Models
{
    class TestModel
    {
        public string Name { get; set; }

        public TestSubModel Child { get; set; }

        public DefinedTestSubModel Child2 { get; set; }

        public DependentType Dependent { get; }

        public DependentType Something(DefinedTestSubModel subModel)
        {
            return new DependentType()
            {
                Value = subModel.Value2
            };
        }
    }
}
