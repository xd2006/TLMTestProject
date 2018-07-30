
namespace Tests.Helpers.Api
{
    using global::Tests.ApiRequests;
    using global::Tests.Managers;
    using global::Tests.Models;
    using global::Tests.Models.Temp;

    public class ApiCommonHelper : ApiHelperTemplate
    {
        public ApiCommonHelper(ApplicationManager app)
            : base(app)
        {           
        }

        #region temp
        public Learn GetLearnData()
        {
            var client = CreateClient();
            var result = client.Execute<Learn>(TempRequests.GetLearn());
            Learn instance = result.Data;
            this.App.Logger.Info($"Request was processed");
            return instance;
        }
          #endregion
    }
}
