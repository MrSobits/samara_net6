namespace Bars.GkhGji.Regions.Stavropol.Migrations.Version_2014102000
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Stavropol.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable("GJI_RESOL_PROS_STAVROPOL",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OFFICIAL", DbType.String, 100));

            Database.AddForeignKey("FK_GJI_RP_STPL_ID", "GJI_RESOL_PROS_STAVROPOL", "ID", "GJI_RESOLPROS", "ID");

            Database.ExecuteNonQuery(@"insert into GJI_RESOL_PROS_STAVROPOL (id)
                                     select id from GJI_RESOLPROS");
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_RESOL_PROS_STAVROPOL");
        }
    }
}