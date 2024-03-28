namespace Bars.GkhGji.Migrations._2022.Version_2022102700
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022102700")]
    [MigrationDependsOn(typeof(Version_2022102600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL", new Column("BIRTH_DAY", DbType.DateTime, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL", "BIRTH_DAY");
        }
    }
}