using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class OpenSessionNHibernate
    {
        public static ISession OpenSession()
        {
            var configuration = new Configuration();

            var configurationPath = HttpContext.Current.Server.MapPath("~/DAL/Nh.configuration.xml");

            configuration.Configure(configurationPath);

            var userConfigurationFile = HttpContext.Current.Server.MapPath("~/DAL/User.mapping.xml");

            configuration.AddFile(userConfigurationFile);

            ISessionFactory sessionFactory = configuration.BuildSessionFactory();

            return sessionFactory.OpenSession();
        }
    }
}