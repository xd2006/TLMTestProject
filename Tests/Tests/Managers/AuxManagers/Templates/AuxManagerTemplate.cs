
namespace Tests.Managers.AuxManagers.Templates
{
    public class AuxManagerTemplate
    {
        
        public AuxManagerTemplate(ApplicationManager app)
        {
            this.App = app;
        }

        protected ApplicationManager App { get; set; }
    }
}
