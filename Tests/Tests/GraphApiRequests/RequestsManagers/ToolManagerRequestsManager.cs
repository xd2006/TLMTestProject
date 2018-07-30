
namespace Tests.GraphApiRequests.RequestsManagers
{
    using global::Tests.GraphApiRequests.Inventory;

    public class ToolManagerRequestsManager
    {
        private SearchRequests search;

        private ServiceRequests service;

        private DetailsRequests details;

        public SearchRequests Search => this.search ?? (this.search = new SearchRequests());

        public ServiceRequests Service => this.service ?? (this.service = new ServiceRequests());

        public DetailsRequests Details => this.details ?? (this.details = new DetailsRequests());



    }
}
