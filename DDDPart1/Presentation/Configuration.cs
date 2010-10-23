using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure.Impl;
using log4net.Config;
using Infrastructure;
using Infrastrucure;
using System.Data.SqlClient;
using NHibernate.Dialect;
using Domain.Repositories;
using Infrastructure.Web;
using System.Web.Mvc;
using System.Reflection;
using Domain;
using Infrastructure.Reflection;

namespace Presentation
{
    /// <summary>
    /// Handles the configuration of the application.
    /// You can usually also do that in .config and xml files, but you would not have refactoring support.
    /// </summary>
    public class Configuration
    {
        public static void Configure()
        {
            // Configure log4net
            XmlConfigurator.Configure();

            // Configure container
            CurrentContainer.Container = new WindsorObjectBuilder();
            CurrentContainer.Container.RegisterSingleton<IContainer>(CurrentContainer.Container);
            CurrentContainer.Container.Configure<ContainerControllerFactory>(ComponentInstanciationPolicy.Singleton);
            // add repositories to container
            foreach (Type rep in from t in typeof(Student).Assembly.GetTypes()
                                 where t.ImplementsGenericDefinition(typeof(NHibernateRepository<>))
                                 select t)
            {
                CurrentContainer.Container.Configure(rep, ComponentInstanciationPolicy.Singleton);
            }
            // add controllers to container
            foreach (Type ctl in from t in Assembly.GetExecutingAssembly().GetTypes()
                                 where typeof(IController).IsAssignableFrom(t)
                                 select t)
            {
                CurrentContainer.Container.Configure(ctl, ComponentInstanciationPolicy.NewInstance);
            }

            // Configure NHibernate
            var nhibernateCfg = new NHibernate.Cfg.Configuration();
            nhibernateCfg.AddProperties(new Dictionary<string, string>()
            {
                {NHibernate.Cfg.Environment.Dialect, "NHibernate.Dialect.MsSql2005Dialect"},
                {NHibernate.Cfg.Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider"},
                {NHibernate.Cfg.Environment.ConnectionDriver, "NHibernate.Driver.SqlClientDriver"},
                {NHibernate.Cfg.Environment.ConnectionString, "Data Source=localhost\\SQLEXPRESS;Database=DDDPart1;Integrated Security=SSPI;"},
                {NHibernate.Cfg.Environment.QueryTranslator, "NHibernate.Hql.Classic.ClassicQueryTranslatorFactory"},
                {NHibernate.Cfg.Environment.Isolation, "ReadCommitted"},
                {NHibernate.Cfg.Environment.DefaultSchema, "dbo"},
                {NHibernate.Cfg.Environment.ProxyFactoryFactoryClass, "NHibernate.ByteCode.Spring.ProxyFactoryFactory, NHibernate.ByteCode.Spring"},
                {NHibernate.Cfg.Environment.CurrentSessionContextClass, "NHibernate.Context.WebSessionContext"},
            });
            nhibernateCfg.AddAssembly("Domain");
            nhibernateCfg.AddAssembly("Infrastructure.Impl");
            
            CreateSchema(nhibernateCfg);

            var sessionFactory = nhibernateCfg.BuildSessionFactory();
            var persistenceManager = new NHibernatePersistenceManager(sessionFactory);
            CurrentContainer.Container.RegisterSingleton<NHibernatePersistenceManager>(persistenceManager);

            // Configure ASP.Net MVC controller factory
            ControllerBuilder.Current.SetControllerFactory(CurrentContainer.Container.Build<ContainerControllerFactory>());
        }

        // Create database schema
        private static void CreateSchema(NHibernate.Cfg.Configuration cfg)
        {
            Dialect dialect = Dialect.GetDialect(cfg.Properties);

            using (var conn = new SqlConnection(cfg.GetProperty(NHibernate.Cfg.Environment.ConnectionString)))
            {
                conn.Open();

                foreach (string cmdText in cfg.GenerateDropSchemaScript(dialect).Union(cfg.GenerateSchemaCreationScript(dialect)))
                {
                    using (var cmd = new SqlCommand(cmdText, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}