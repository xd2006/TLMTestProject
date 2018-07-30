
namespace Tests.Models.ProjectManager.DbModels.Postgres.Interfaces
{
    using System;

    interface IEditable
    {
        string EditedBy { get; set; }

        DateTime EditedDate { get; set; }
    }
}
