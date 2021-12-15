namespace TestApplication.Models
{
    class DependentType
    {
        public int DependentProperty { get; }

        public int Value { get; set; }

        public int Action(int input)
        {
            return input + 1;
        }

    }
}
