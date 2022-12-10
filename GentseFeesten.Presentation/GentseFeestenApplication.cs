using System;
using System.DirectoryServices.ActiveDirectory;

namespace GentseFeesten.Presentation
{
    public class GentseFeestenApplication
    {
        private readonly DomainController _domainController;

        public GentseFeestenApplication(DomainController domainController)
        {
            _domainController = domainController;
        }
    }
}
