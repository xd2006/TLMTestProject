
namespace Tests.GraphApiRequests.RequestsManagers
{
    using global::Tests.GraphApiRequests.Orders;
    using global::Tests.Models.ProjectManager.DbModels;

    public class ProjectManagerRequestManager
    {

        private OrderRequests order;

        public OrderRequests Order => this.order ?? (this.order = new OrderRequests());

    }
}
