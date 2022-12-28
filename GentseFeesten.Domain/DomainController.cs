using GentseFeesten.Domain.Model;
using GentseFeesten.Domain.Repository;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace GentseFeesten.Domain
{
    public class DomainController
    {
        private readonly IEvenementenRepository _evenementenRepository;
        private readonly IPlannerRepository _plannerRepository;

        public DomainController(IEvenementenRepository evenementenRepository, IPlannerRepository plannerRepository)
        {
            _evenementenRepository = evenementenRepository;
            _plannerRepository = plannerRepository;
        }

        public List<Evenement> GetAllMainEvents()
        {
            List<Evenement> mainEvents = _evenementenRepository.GetMainEvents();
            return mainEvents;
        }

        public void GetEventDetails(Evenement evenement)
        {
            FillInMissingDate(evenement);
        }

        public List<Evenement> GetChildsFromEvent (Evenement evenement)
        {
            _evenementenRepository.GetChildEvents(evenement);
            List<Evenement> childEvents = evenement.GetChilds();
            foreach (Evenement childEvent in childEvents)
            {
                FillInMissingDate(childEvent);
            }
            return childEvents;
        }

        public List<Evenement> GetEventsFromPlanner()
        {
            return _plannerRepository.GetAllEventsOnPlanner();
        }

        public string GetPlannerSummary()
        {
            string summary = "";
            _plannerRepository.GetAllEventsOnPlanner().ForEach(e => summary += $"{e.Name} - €{e.Price}\n");
            int totalPrice = _plannerRepository.GetCurrentTotalPrice();
            summary += $"Totale kostprijs = {totalPrice}";

            return summary;
        }

        public void AddEventToPlanner(Evenement evenement)
        {
            _plannerRepository.AddEventToPlanner(evenement);
        }

        public void RemoveEventFromPlanner(Evenement evenement)
        {
            _plannerRepository.RemoveEventFromPlanner(evenement);
        }

        private void FillInMissingDate(Evenement evenement)
        {
            if (evenement.Start == null)
            {
                List<DateTime?> startDates = _evenementenRepository.GetMissingStartData(evenement).OrderBy(x => x).ToList();
                evenement.Start = startDates.First();
            }
            
            if (evenement.End == null)
            {
                List<DateTime?> endDates = _evenementenRepository.GetMissingEndData(evenement).OrderByDescending(x => x).ToList();
                evenement.End = endDates.First();
            }
            
            if (evenement.Price == null)
            {
                evenement.Price = _evenementenRepository.GetMissingPriceData(evenement);
            }
        }

        //private Evenement GetEventById(string id)
        //{
        //    Evenement evenement = _evenementenRepository.GetEventById(id);
        //    return evenement;
        //}
    }
}