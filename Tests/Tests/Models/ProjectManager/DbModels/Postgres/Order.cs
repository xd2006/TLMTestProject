
namespace Tests.Models.ProjectManager.DbModels.Postgres
{
    using System;

    using global::Tests.Models.ProjectManager.DbModels.Postgres.Interfaces;

    [PetaPoco.TableName("Orders")]
    [PetaPoco.PrimaryKey("Id")]
    public class Order : ICreatable, IEditable
    {
        public int Id { get; set; }

        public string ExternalOrderId { get; set; }

        public int? CustomerId { get; set; }

        public string Comments { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string CreatedBy { get; set; }

        public string EditedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime EditedDate { get; set; }


    }
}
