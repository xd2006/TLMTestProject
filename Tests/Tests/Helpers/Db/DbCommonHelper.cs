
namespace Tests.Helpers.Db
{
    using global::Tests.Managers;

    public class DbCommonHelper : DbHelperTemplate
    {
        protected DbCommonHelper(ApplicationManager app)
            : base(app)
        {
        }
    }
}
