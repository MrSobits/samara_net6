namespace Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014060600
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014060500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable("GJI_PROTOCOL_DEF_SMOL",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("DEF_RESULT", DbType.String, 2000),
                new Column("DESCRIPTION_SET", DbType.String, 2000));

            Database.AddForeignKey("FK_GJI_PROTOCOL_DEF_SMOL_ID", "GJI_PROTOCOL_DEF_SMOL", "ID", "GJI_PROTOCOL_DEFINITION", "ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_PROTOCOL_DEF_SMOL", "FK_GJI_PROTOCOL_DEF_SMOL_ID");

            Database.RemoveTable("GJI_PROTOCOL_DEF_SMOL");
        }
    }
}