namespace Bars.GkhGji.Migrations.Version_2013053000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013053000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013051600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_DOCNUM_VALID_RULE",
                new Column("RULE_ID", DbType.String, 50, ColumnProperty.NotNull),
                new Column("TYPE_DOCUMENT_GJI", DbType.Int32, 4, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DOCNUM_VALID_RULE");
        }
    }
}