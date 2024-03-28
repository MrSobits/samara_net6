namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014072100
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014072100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014071800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable("GJI_TOMSK_DICT_VIOLATION", new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique), new Column("RULE_OF_LAW", DbType.String, 2000));

            Database.AddForeignKey("FK_GJI_TOMSK_VIOLAT_ID", "GJI_TOMSK_DICT_VIOLATION", "ID", "GJI_DICT_VIOLATION", "ID");

            Database.ExecuteNonQuery(@"insert into GJI_TOMSK_DICT_VIOLATION (id)
                                     select id from GJI_DICT_VIOLATION");

            Database.AddEntityTable(
                "GJI_TOMSK_DICT_VIOL_DESCR",
                new RefColumn("VIOLATION_ID", ColumnProperty.NotNull, "TOMSK_VIOL_DICT_DESCR", "GJI_TOMSK_DICT_VIOLATION", "ID"),
                new Column("RULE_OF_LAW", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_TOMSK_DICT_VIOL_DESCR");

            Database.RemoveConstraint("GJI_TOMSK_DICT_VIOLATION", "FK_GJI_TOMSK_VIOLAT_ID");
            Database.RemoveTable("GJI_TOMSK_DICT_VIOLATION");
        }
    }
}