
namespace Tests.Models.Machines.UiModels
{
    // Is used for both 'machines and tasks allocation' grid and for 'machines details tasks' grid
    public class TaskAllocationRecord
    {
        public string Machine { get; set; }

        public string Workpiece { get; set; }

        public string Qty { get; set; }

        public string CurrentTask { get; set; }

        public string ActualStart { get; set; }

        public string EstimatedDuration { get; set; }

        public string EstimatedEnd { get; set; }

        // 'Next task' for machine details page
        public string UpcomingTask { get; set; }

        public string DeliveryDate { get; set; }

    }
}
