namespace Bars.Gkh.Quartz.Scheduler
{
    using System;
    using System.Collections.Specialized;
    using System.IO;

    using Bars.B4.DataAccess;

    using Castle.Windsor;

    using global::Quartz.Impl;
    using global::Quartz.Util;

    /// <summary>
    /// Фабрика планирощиков с хранилищем данных в БД
    /// </summary>
    public class TaskSchedulerFactory : StdSchedulerFactory
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Конструктор фабрики планировщиков
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public TaskSchedulerFactory(IWindsorContainer container)
        {
            this.Container = container;

            string propFileName = this.GetPropertiesFileName();

            NameValueCollection properties;

            if (!string.IsNullOrEmpty(propFileName))
            {
                properties = this.ReadProperties(propFileName);
            }
            else
            {
                properties = this.GetDefaultProperties();
            }

            properties = this.OverrideWithRequiredProperties(properties);

            base.Initialize(properties);
        }

        private string GetPropertiesFileName()
        {
            var result = FileUtil.ResolveFile("~/quartz.user.config");

            if (File.Exists(result))
            {
                return result;
            }
            
            result = FileUtil.ResolveFile("~/quartz.config");

            if (File.Exists(result))
            {
                return result;
            }

            return string.Empty;
        }

        private NameValueCollection ReadProperties(string propFileName)
        {
            try
            {
                PropertiesParser parser = PropertiesParser.ReadFromFileResource(propFileName);

                this.Log.Info(string.Format("Quartz.NET properties loaded from configuration file '{0}'", propFileName));
                return parser.UnderlyingProperties;
            }
            catch (Exception exception)
            {
                var errorMessage =
                    "Could not load properties for Quartz from file {0}: {1}".FormatInvariant(
                        propFileName,
                        exception.Message);
                this.Log.Error(errorMessage, exception);

                throw;
            }
        }

        private NameValueCollection GetDefaultProperties()
        {
            NameValueCollection result = new NameValueCollection();

            //The maximum number of triggers that a scheduler node is allowed to acquire (for firing) at once. 
            //Default value is 1. The larger the number, the more efficient firing is (in situations where there are very many triggers needing to be fired all at once) 
            result.Set("quartz.scheduler.batchTriggerAcquisitionMaxCount", "15");

            result.Set("quartz.threadPool.type", "Quartz.Simpl.SimpleThreadPool, Quartz");
            result.Set("quartz.threadPool.threadCount", "25");
            result.Set("quartz.threadPool.threadPriority", "Normal");

            //Is the amount of time in milliseconds that the scheduler will wait between re-tries when it has detected a loss of connectivity within the JobStore(e.g.to the database).
            result.Set("quartz.scheduler.dbFailureRetryInterval", "15000");

            //Is the amount of time in milliseconds that the scheduler will wait before re-queries for available triggers when the scheduler is otherwise idle.
            //Values less than 5000 ms are not recommended as it will cause excessive database querying. Values less than 1000 are not legal.
            result.Set("quartz.scheduler.idleWaitTime", "30000");

            return result;
        }

        private NameValueCollection OverrideWithRequiredProperties(NameValueCollection properties)
        {
            NameValueCollection result = new NameValueCollection(properties);

            result.Set("quartz.scheduler.instanceName", "TaskScheduler");
            result.Set("quartz.scheduler.instanceId", "TaskScheduler");

            result = this.AddJobStoreProperties(result);

            return result;
        }

        private NameValueCollection AddJobStoreProperties(NameValueCollection properties)
        {
            NameValueCollection result = new NameValueCollection(properties);

            //JobStore is used to store scheduling information (job, triggers and calendars) 
            //JobStoreTX - within a relational database
            //RAMJobStore- within memory
            result.Set("quartz.jobStore.type", typeof(JobStoreTXCustom).AssemblyQualifiedName);

            //"table prefix" property is a string equal to the prefix given to Quartz's tables that were created in your database. 
            //You can have multiple sets of Quartz's tables within the same database if they use different table prefixes.
            result.Set("quartz.jobStore.tablePrefix", "QRTZ_");

            //The "use properties" flag instructs JDBCJobStore that all values in JobDataMaps will be Strings, 
            //and therefore can be stored as name-value pairs, rather than storing more complex objects in their serialized form in the BLOB column. 
            //This is can be handy, as you avoid the class versioning issues that can arise from serializing your non-String classes into a BLOB.
            result.Set("quartz.jobStore.useProperties", "false");

            //This property must be set to "true" if you are having multiple instances of Quartz use the same set of database tables... 
            result.Set("quartz.jobStore.clustered", "false");

            //The the number of milliseconds the scheduler will 'tolerate' a trigger to pass its next-fire-time by, before being considered "misfired". 
            //The default value (if you don't make an entry of this property in your configuration) is 60000 (60 seconds).
            result.Set("quartz.jobStore.misfireThreshold", "60000");

            result.Set("quartz.jobStore.dataSource", "myDS");

            var dbConfigProvider = this.Container.Resolve<IDbConfigProvider>();

            try
            {
                switch (dbConfigProvider.DbDialect)
                {
                    case DbDialect.PostgreSql:
                        //Driver delegates understand the particular 'dialects' of varies database systems.
                        result.Set("quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz"); 
                        result.Set("quartz.dataSource.myDS.provider", "Npgsql-20");
                        break;

                        //TODO добавить свойства для других типов БД

                    default:
                        break;
                }

                result.Set("quartz.dataSource.myDS.connectionString", dbConfigProvider.ConnectionString);
            }
            finally
            {
                this.Container.Release(dbConfigProvider);
            }

            return result;
        }
    }
}
