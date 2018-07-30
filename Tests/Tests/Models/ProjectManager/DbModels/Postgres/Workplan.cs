namespace Tests.Models.ProjectManager.DbModels.Postgres
{
    using System;

    using global::Tests.Models.ProjectManager.DbModels.Postgres.Interfaces;

    [PetaPoco.TableName("Workplans")]
    [PetaPoco.PrimaryKey("Id")]
    public class Workplan : ICreatable, IEditable
    {
        public int Id { get; set; }

        public int WorkpieceId { get; set; }

        public string CreatedBy { get; set; }

        public string EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime EditedDate { get; set; }

    }
}
