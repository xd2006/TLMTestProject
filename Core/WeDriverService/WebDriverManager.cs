
namespace Core.WeDriverService
{
    using global::WebDriverManager;

    using global::WebDriverManager.DriverConfigs;

    using global::WebDriverManager.DriverConfigs.Impl;

    using global::WebDriverManager.Helpers;

    public class WebDriverManager
    {
        public void SetupDriver(string browserName)
        {
            //Default value
            IDriverConfig config = new ChromeConfig();
            switch (browserName.ToLower())
            {
                case "firefox":
                    config = new FirefoxConfig();
                    break;
            }

            SetupConfig(config);
        }


        private void SetupConfig(IDriverConfig config)
        {
            new DriverManager().SetUpDriver(config, "Latest", Architecture.X32);
        } 

    }
}
