
namespace Tests.Models.ProjectManager.DbModels.Postgres
{
    using System;

    using global::Tests.Models.ProjectManager.DbModels.Postgres.Interfaces;

    [PetaPoco.TableName("Tasks")]
    [PetaPoco.PrimaryKey("Id")]
    public class Task : ICreatable, IEditable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int DurationPerWorkpiece { get; set; }

        public int DurationPerTotal { get; set; }

        public int MachineId { get; set; }

        public int WorkplanId { get; set; }

        public string EditedBy { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime EditedDate { get; set; }



    }
}
