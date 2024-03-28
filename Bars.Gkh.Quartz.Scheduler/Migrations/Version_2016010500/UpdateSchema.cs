namespace Bars.Gkh.Quartz.Scheduler.Migrations.Version_2016010500
{
    using System.Collections;
    using System.IO;
    using System.Resources;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция модуля
    /// </summary>
    [Migration("2016010500")]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            var upScript = this.GetScript(true);
            this.Database.ExecuteNonQuery(upScript);
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            var downScript = this.GetScript(false);
            this.Database.ExecuteNonQuery(downScript);
        }

        private string GetScript(bool up)
        {
            var result = string.Empty;

            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            var resourceName = string.Format("{0}.Resources.SchedulerStoreDDL.resources", currentAssembly.GetName().Name);

            var prefix = up ? "Up" : "Down";

            var dbName = "Postgres";

            //TODO добавить выбор dbName в зависимости от текущей БД

            Stream resourceStream = currentAssembly.GetManifestResourceStream(resourceName);

            if (resourceStream != null)
            {
                ResourceReader resourceReader = new ResourceReader(resourceStream);

                foreach (DictionaryEntry resource in resourceReader)
                {
                    if (resource.Key.ToString() == string.Format("{0}_{1}", dbName, prefix))
                    {
                        result = resource.Value.ToString().Replace("\r\n", "\n");
                        break;
                    }
                }
            }

            return result;
        }
    }
}
