namespace GentseFeesten.Domain.Model
{
    public class PlannerEvenement : Evenement
    {
        private const List<string?> ChildEventIds = null;

        public PlannerEvenement(string id, string name, DateTime? start, DateTime? end, string description, int? price, List<string?> childEventIds = ChildEventIds) : base(id, name, childEventIds, start, end, description, price)
        {

        }

        public override string? ToString()
        {
            return $"{this.Name} - €{this.Price}\n";
        }
    }
}
