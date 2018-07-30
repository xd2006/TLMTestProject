
namespace Tests.Models.ProjectManager.DbModels.Postgres
{
    using System;

    using global::Tests.Models.ProjectManager.DbModels.Postgres.Interfaces;

    [PetaPoco.TableName("Workpieces")]
    [PetaPoco.PrimaryKey("Id")]
    public class Workpiece : ICreatable, IEditable
    {
        public int? Id { get; set; }

        public string ExternalWorkpieceId { get; set; }

        public string Name { get; set; }

        public int? Quantity { get; set; }

        public DateTime DeliveryDate { get; set; }

        public int OrderId { get; set; }

        public string CreatedBy { get; set; }

        public string EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime EditedDate { get; set; }
    }
}
