namespace Bars.Gkh.Migrations._2021.Version_2021082000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021082000")]
    
    [MigrationDependsOn(typeof(Version_2021072700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_RO_LIFT", new Column("COMISS_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GKH_RO_LIFT", new Column("DECOMISS_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GKH_RO_LIFT", new Column("PLAN_DECOMISS_DATE", DbType.DateTime, ColumnProperty.None));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_RO_LIFT", "PLAN_DECOMISS_DATE");
            Database.RemoveColumn("GKH_RO_LIFT", "DECOMISS_DATE");
            Database.RemoveColumn("GKH_RO_LIFT", "COMISS_DATE");
        }
    }
}