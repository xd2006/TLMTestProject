
namespace Tests.Managers.AuxManagers
{
    using global::Tests.Helpers.Api;
    using global::Tests.Managers.AuxManagers.Templates;

    public class ApiManager : AuxManagerTemplate
    {

        private ApiCommonHelper apiHelp;

        private ApiSearchHelper apiSearch;

        public ApiManager(ApplicationManager app)
            : base(app)
        {
        }
        
        #region helpers initialization

        public ApiCommonHelper ApiHelp => this.apiHelp ?? (this.apiHelp = new ApiCommonHelper(App));

        public ApiSearchHelper Search => this.apiSearch ?? (this.apiSearch = new ApiSearchHelper(App));

        #endregion        
    }
}
