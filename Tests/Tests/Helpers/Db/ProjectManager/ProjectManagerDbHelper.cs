
namespace Tests.Helpers.Db.ProjectManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Tests.Managers;
    using global::Tests.Models.ProjectManager.DbModels;
    using global::Tests.Models.ProjectManager.DbModels.Postgres;

    using PetaPoco;

    public class ProjectManagerDbHelper : DbHelperTemplate
    {

        public ProjectManagerDbHelper(ApplicationManager app)
            : base(app)
        {

            Db = DatabaseConfiguration.Build().UsingConnectionString(Parameters.Parameters.ProjectManagerDbConnectionString)
                .UsingProvider(new PostgreSQLDatabaseProvider()).Create();
        }

        public Task GetTask(int id)
        {
            return Db.Single<Task>(id);
        }

        public List<Task> GetTasks()
        {
            return this.GetTasks(new Sql());
        }

        public List<Task> GetTasks(Sql query)
        {
            return Db.Fetch<Task>(query);
        }

        public List<Task> GetActiveTasksForMachine(int machineId)
        {
            return GetTasks(new Sql($"where \"MachineId\" = {machineId} and \"EndDate\" >= '{DateTime.Now:yyyy-MM-dd 00:00:00}'"));           
        }

        public List<Order> GetOrders()
        {
            return this.GetOrders(new Sql());
        }

        public Order GetOrder(int id)
        {
            return Db.Single<Order>(id);
        }

        public List<Order> GetOrders(Sql query)
        {
            return GetEntities<Order>(query);
        }
       
        public Workpiece GetWorkpiece(int workpieceId)
        {
            return Db.Single<Workpiece>(workpieceId);
        }

        public Workplan GetWorkplan(int workplanId)
        {
            return Db.Single<Workplan>(workplanId);
        }

        public List<Workpiece> GetWorkpieces()
        {
            return Db.Fetch<Workpiece>(string.Empty);
        }

        public List<Workplan> GetWorkplans(Sql query)
        {
            return Db.Fetch<Workplan>(query);
        }
       
        public List<Workplan> GetWorkplans()
        {
            return Db.Fetch<Workplan>(string.Empty);
        }

        public List<Task> GeTasks()
        {
            return Db.Fetch<Task>(string.Empty);
        }

        public List<Task> GeTasks(Sql query)
        {
            return Db.Fetch<Task>(query);
        }
        
        public void CleanDb()
        {
            Db.Delete<Workplan>("*");
            Db.Delete<Workpiece>("*");
            Db.Delete<Order>("*");
            Db.Delete<Task>("*");
        }
    }
}
