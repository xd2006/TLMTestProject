
namespace Tests.Models.ProjectManager.DbModels.Postgres.Interfaces
{
    using System;

    interface ICreatable
    {
        string CreatedBy { get; set; }

        DateTime CreatedDate { get; set; }
    }
}
