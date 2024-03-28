namespace Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014071500
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014071500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014071400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable("GJI_DISPOSAL_SMOL",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("VERIF_PURPOSE", DbType.String, 2000));

            Database.AddForeignKey("FK_GJI_DISPOSAL_SMOL_ID", "GJI_DISPOSAL_SMOL", "ID", "GJI_DISPOSAL", "ID");

            Database.ExecuteNonQuery(@"insert into GJI_DISPOSAL_SMOL (id)
                                     select id from GJI_DISPOSAL");
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_DISPOSAL_SMOL", "FK_GJI_DISPOSAL_SMOL_ID");
            Database.RemoveTable("GJI_DISPOSAL_SMOL");
        }
    }
}