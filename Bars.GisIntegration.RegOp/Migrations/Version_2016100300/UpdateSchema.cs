namespace Bars.GisIntegration.RegOp.Migrations.Version_2016100300
{
    using System.Collections.Generic;
    using System.Linq;

    using B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция модуля
    /// </summary>
    [Migration("2016100300")]
    public partial class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //Добавление РИСовых сущностей
            //foreach (var table in this.tables)
            //{
            //    if (!this.Database.TableExists(table.Name))
            //    {
            //        this.Database.ExecuteNonQuery(table.CreationScript);
            //    }
            //    foreach (var index in table.IndexList)
            //    {
            //        if (!this.Database.IndexExists(index.Name, table.Name))
            //        {
            //            this.Database.ExecuteNonQuery(index.CreationScript);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //var reverseTables = this.tables.ToList();
            //reverseTables.Reverse();

            //foreach (var table in reverseTables)
            //{
            //    if (this.Database.TableExists(table.Name))
            //    {
            //        this.Database.RemoveTable(table.Name);
            //    }
            //}
        }

        //private class TableProxy
        //{
        //    public string Name { get; set; }
        //    public string CreationScript { get; set; }
        //    public List<IndexProxy> IndexList { get; set; }

        //    public TableProxy(string name, string creationScript, List<IndexProxy> indexList = null)
        //    {
        //        this.Name = name;
        //        this.CreationScript = creationScript;
        //        this.IndexList = indexList ?? new List<IndexProxy>();
        //    }
        //}

        //private class IndexProxy
        //{
        //    public string Name { get; set; }
        //    public string CreationScript { get; set; }

        //    public IndexProxy(string name, string creationScript)
        //    {
        //        this.Name = name;
        //        this.CreationScript = creationScript;
        //    }
        //}
    }
}
