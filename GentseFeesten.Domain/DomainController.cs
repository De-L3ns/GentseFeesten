using GentseFeesten.Domain.Model;
using GentseFeesten.Domain.Repository;
using System.Diagnostics;

namespace GentseFeesten.Domain
{
    public class DomainController
    {
        private readonly IEvenementenRepository _evenementenRepository;

        public DomainController(IEvenementenRepository evenementenRepository)
        {
            _evenementenRepository = evenementenRepository;
        }


        public List<Evenement> GetAllMainEvents()
        {
            List<Evenement> mainEvents = _evenementenRepository.GetAllParentEvents();
            return mainEvents;

        }

        public string ShowEventDetails(string id)
        {
            Evenement eventToShowDetailsFrom = GetEventById(id);
            FillInMissingDate(eventToShowDetailsFrom);
            return $"{eventToShowDetailsFrom}";

        }

        public void ShowAllChildsFromEvent(string id)
        {
            Evenement evenement = GetEventById(id);
            _evenementenRepository.GetChilds(evenement).ForEach(e => { Trace.WriteLine(e.ToString()); });
            
        }

        private void FillInMissingDate(Evenement evenement)
        {
            List<DateTime?> startDates = _evenementenRepository.GetMissingStartData(evenement).OrderBy(x => x).ToList();
            List<DateTime?> endDates = _evenementenRepository.GetMissingEndData(evenement).OrderByDescending(x => x).ToList();

            evenement.Start = startDates.First();
            evenement.Einde = endDates.First();
        }

        private Evenement GetEventById(string id)
        {
            Evenement evenement = _evenementenRepository.GetAllParentEvents().Where(e => e.Id == id).FirstOrDefault();
            return evenement;
        }
    }
}