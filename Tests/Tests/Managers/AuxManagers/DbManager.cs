
namespace Tests.Managers.AuxManagers
{
    using global::Tests.Helpers.Db.ProjectManager;
    using global::Tests.Managers.AuxManagers.Templates;

    public class DbManager : AuxManagerTemplate
    {

        private ProjectManagerDbHelper projectManager;

        public DbManager(ApplicationManager app)
            : base(app)
        {
        }


        #region helpers initialization

        public ProjectManagerDbHelper ProjectManager => this.projectManager ?? (this.projectManager = new ProjectManagerDbHelper(App));

        #endregion


    }
}
