namespace _Data
{
    public interface IRoleInteractionData
    {
        Role.InteractionType Type { get; }
        string Name { get; }
        string Description { get; }
        double Value { get; }
    }

    public record RoleInteractionData(string Name,Role.InteractionType Type ,string Description, double Value) : IRoleInteractionData
    {
        public string Name { get; } = Name;
        public Role.InteractionType Type { get; }= Type;
        public string Description { get; } = Description;
        public double Value { get; } = Value;
    }
}