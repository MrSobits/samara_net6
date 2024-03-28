namespace Bars.Gkh.Migrations._2020.Version_2020041600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2020041600")]
    
    [MigrationDependsOn(typeof(Version_2020032600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CONTRAGENT", new Column("IS_EDSE", DbType.Boolean, false));

        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTRAGENT", "IS_EDSE");

        }
    }
}