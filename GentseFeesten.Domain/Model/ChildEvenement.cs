namespace GentseFeesten.Domain.Model
{
    public class ChildEvenement : Evenement
    {
        private readonly Evenement _mainEvent;

        public ChildEvenement(string id, string naam, DateTime? start, DateTime? einde, string beschrijving, int? prijs, List<string?> childEventIds, string? parentId, Evenement mainEvent) : base(id, naam, childEventIds, start, einde, beschrijving, prijs)
        {
            ParentId = parentId;
            _mainEvent = mainEvent;
        }

        public string? ParentId { get; init; }

        public override string? ToString()
        {
            return base.ToString();
        }

        public override string GetInformation()
        {
            return $"Dit evenement is deel van het overkoepelde evenement: {_mainEvent.Name}\n" + base.GetInformation();
        }

    }
}
