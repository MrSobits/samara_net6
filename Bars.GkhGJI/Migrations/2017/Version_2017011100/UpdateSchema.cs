namespace Bars.GkhGji.Migrations._2017.Version_2017011100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    /// <summary>
    /// Миграция 2017011100
    /// </summary>
    [Migration("2017011100")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2016.Version_2016122100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GJI_DICT_FEATUREVIOL", new Column("GKH_REFORM_CODE", DbType.String, ColumnProperty.None, 300));
            this.Database.ExecuteNonQuery("UPDATE GJI_DICT_FEATUREVIOL SET GKH_REFORM_CODE = CODE;");

            //ViewManager.Drop(this.Database, "GkhGji");
            //ViewManager.Create(this.Database, "GkhGji");
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_DICT_FEATUREVIOL", "GKH_REFORM_CODE");
        }
    }
}