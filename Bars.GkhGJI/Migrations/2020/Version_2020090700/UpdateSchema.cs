namespace Bars.GkhGji.Migrations._2020.Version_2020090700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020090700")]
    [MigrationDependsOn(typeof(Version_2020072900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "GJI_DICT_MKD_LIC_TYPE_REQUEST",
               new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
               new Column("CODE", DbType.String, 5, ColumnProperty.NotNull),
               new Column("DESCRIPTION", DbType.String, 500, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_MKD_LIC_TYPE_REQUEST");
        }
    }
}