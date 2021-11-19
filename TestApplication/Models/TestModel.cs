namespace TestApplication.Models
{
    class TestModel
    {
        public string Name { get; set; }

        public TestSubModel Child { get; set; }

        public DefinedTestSubModel Child2 { get; set; }

        public DependentType Dependent { get; }
    }
}
