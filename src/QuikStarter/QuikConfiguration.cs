using System.Configuration;

namespace QuikSharp.Starter
{

    public class QuikConfiguration : ConfigurationSection
    {
        public QuikConfiguration()
        {
        }

        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("quikPath")]
        public string QuikPath
        {
            get { return (string)this["quikPath"]; }
            set { this["quikPath"] = value; }
        }

        [ConfigurationProperty("user")]
        public string User
        {
            get { return (string)this["user"]; }
            set { this["user"] = value; }
        }

        [ConfigurationProperty("password")]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }

    }
}
