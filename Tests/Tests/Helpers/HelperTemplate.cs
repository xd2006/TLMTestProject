namespace Tests.Helpers
{
    using global::Tests.Managers;

    public class HelperTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelperTemplate"/> class.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        protected HelperTemplate(ApplicationManager app)
        {
            this.App = app;
        }

        /// <summary>
        /// Gets app. manager
        /// </summary>
        protected ApplicationManager App { get; }
    }
}
