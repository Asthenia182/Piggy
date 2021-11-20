namespace Piggy.Core.Entites
{
    public class ConstOutcome
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public double Amount { get; set; }
        public string? Currency { get; set; }
        public Category? Category { get; set; }
    }
}
