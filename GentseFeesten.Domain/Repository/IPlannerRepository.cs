using GentseFeesten.Domain.Model;

namespace GentseFeesten.Domain.Repository
{
    public interface IPlannerRepository
    {
        public List<Evenement> GetAllEventsOnPlanner();
        public int GetCurrentTotalPrice();
        public void AddEventToPlanner(Evenement evenement);
        public void RemoveEventFromPlanner(Evenement evenement);

    }
}
