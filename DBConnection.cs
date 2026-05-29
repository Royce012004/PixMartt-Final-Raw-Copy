using System.Configuration;

namespace PixMartt
{
    public static class DBConnection
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["PixMarttDB"].ConnectionString;
            }
        }
    }
}