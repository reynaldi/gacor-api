namespace GacorAPI.Infra
{
    public class ConfigHelper
    {
        public static Config GetConfig()
        {
            var config = new Config();    
            config.ConnectionString = Environment.GetEnvironmentVariable("ConnectionString");
            return config;        
        }
    }

    public struct Config
    {
        public string ConnectionString { get; set; }
    }
}