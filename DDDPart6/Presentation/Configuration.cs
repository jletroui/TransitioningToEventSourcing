using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure.Impl;
using log4net.Config;
using Infrastructure;
using System.Data.SqlClient;
using Infrastructure.Web;
using System.Web.Mvc;
using System.Reflection;
using Domain;
using Infrastructure.Reflection;
using Domain.ViewModel;
using System.Configuration;
using Domain.Commands;
using Domain.CommandHandlers;
using Domain.PersistenceHandlers;
using System.IO;

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

            ReCreateDatabaseSchema();

            // Configure container
            CurrentContainer.Container = new WindsorObjectBuilder();
            CurrentContainer.Container.RegisterSingleton<IContainer>(CurrentContainer.Container);
            CurrentContainer.Container.Configure<ContainerControllerFactory>(ComponentInstanciationPolicy.Singleton);
            CurrentContainer.Container.Configure<ContainerCommandBus>(ComponentInstanciationPolicy.Singleton);
            CurrentContainer.Container.Configure<ContainerEventBus>(ComponentInstanciationPolicy.Singleton);
            CurrentContainer.Container.Configure<WebContext>(ComponentInstanciationPolicy.Singleton);
            // add repositories to container for each type of aggregate roots
            foreach (Type agg in from t in typeof(Student).Assembly.GetTypes()
                                 where typeof(IAggregateRoot).IsAssignableFrom(t)
                                 select t)
            {
                var rep = typeof(EventSourcedRepository<>).MakeGenericType(agg);
                CurrentContainer.Container.Configure(rep, ComponentInstanciationPolicy.Singleton);
            }
            // add DTO queries
            foreach (Type queries in from t in typeof(StudentDTO).Assembly.GetTypes()
                                     where typeof(DTOQueries).IsAssignableFrom(t)
                                     select t)
            {
                CurrentContainer.Container.Configure(queries, ComponentInstanciationPolicy.Singleton);
            }
            // add command handlers
            foreach (Type handlers in from t in typeof(CreateClassCommandHandler).Assembly.GetTypes()
                                      where t.ImplementsGenericDefinition(typeof(IHandleCommand<>))
                                      select t)
            {
                CurrentContainer.Container.Configure(handlers, ComponentInstanciationPolicy.Singleton);
            }
            // add event handlers
            foreach (Type handlers in from t in typeof(ClassCreatedEventHandler).Assembly.GetTypes()
                                      where t.ImplementsGenericDefinition(typeof(IHandleEvent<>))
                                      select t)
            {
                CurrentContainer.Container.Configure(handlers, ComponentInstanciationPolicy.Singleton);
            }
            // add controllers to container
            foreach (Type ctl in from t in Assembly.GetExecutingAssembly().GetTypes()
                                 where typeof(IController).IsAssignableFrom(t)
                                 select t)
            {
                CurrentContainer.Container.Configure(ctl, ComponentInstanciationPolicy.NewInstance);
            }

            // Configure aggregate roots
            AggregateRoot.CreateDelegatesForAggregatesIn(typeof(Student).Assembly);

            var persistenceManager = new SqlServerPersistenceManager(ConfigurationManager.ConnectionStrings["maindb"].ConnectionString,
                CurrentContainer.Container.Build<IContext>(),
                CurrentContainer.Container.Build<IEventBus>(),
                CurrentContainer.Container);
            CurrentContainer.Container.RegisterSingleton<IPersistenceManager>(persistenceManager);
            CurrentContainer.Container.Configure<SqlServerEventStore>(ComponentInstanciationPolicy.Singleton);

            // Configure ASP.Net MVC controller factory
            ControllerBuilder.Current.SetControllerFactory(CurrentContainer.Container.Build<ContainerControllerFactory>());
        }

        // Create database schema
        private static void ReCreateDatabaseSchema()
        {
            if (ConfigurationManager.AppSettings["ReCreateSchemaAtStartup"] == "true")
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["maindb"].ConnectionString))
                {
                    conn.Open();

                    using (var stream = typeof(Configuration).Assembly.GetManifestResourceStream("Presentation.schema.sql"))
                    using (var reader = new StreamReader(stream))
                    {
                        var cmdText = reader.ReadLine();
                        while (cmdText != null)
                        {
                            if (cmdText.Trim().Length > 0)
                            {
                                using (var cmd = new SqlCommand(cmdText, conn))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            cmdText = reader.ReadLine();
                        }
                    }
                }
            }
        }
    }
}