
namespace Tests.Managers.AuxManagers
{
    using global::Tests.Helpers.GraphApi;
    using global::Tests.Helpers.GraphApi.ProjectManager;
    using global::Tests.Helpers.GraphApi.ToolManager;
    using global::Tests.Managers.AuxManagers.Templates;

    public class GraphApiManager : AuxManagerTemplate
    {
        public GraphApiManager(ApplicationManager app)
            : base(app)
        {
        }

        private GraphApiToolManagerHelper toolManagerHelper;

        private GraphApiProjectManagerHelper projectManagerHelper;

        #region helpers initialization

        public GraphApiToolManagerHelper ToolManager => this.toolManagerHelper ?? (this.toolManagerHelper = new GraphApiToolManagerHelper(App));

        public GraphApiProjectManagerHelper ProjectManager => this.projectManagerHelper ?? (this.projectManagerHelper = new GraphApiProjectManagerHelper(App));
        #endregion
    }
}


