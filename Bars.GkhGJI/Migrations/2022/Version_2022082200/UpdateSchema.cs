namespace Bars.GkhGji.Migrations._2022.Version_2022082200
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022082200")]
    [MigrationDependsOn(typeof(Version_2022080800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_DECISION_LTEXT",
               new RefColumn("DEC_ID", ColumnProperty.NotNull, "GJI_DECISION_LTEXT_DEC", "GJI_DICISION", "ID"),
               new Column("DESCRIPTION", DbType.Binary));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DECISION_LTEXT");
        }
    }
}