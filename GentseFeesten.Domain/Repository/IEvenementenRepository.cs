using GentseFeesten.Domain.Model;

namespace GentseFeesten.Domain.Repository
{
    public interface IEvenementenRepository
    {
        public List<Evenement> GetMainEvents();
        public void GetChildEvents(Evenement evenement);
        public List<DateTime?> GetMissingStartData(Evenement evenement);
        public List<DateTime?> GetMissingEndData(Evenement evenement);
        public int GetMissingPriceData(Evenement evenement);
        
    }
}
