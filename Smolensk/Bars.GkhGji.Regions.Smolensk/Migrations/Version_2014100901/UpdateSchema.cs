namespace Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014100901
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014100900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable("GJI_ACTREMOVAL_SMOL",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("DATE_NOTIFICATION", DbType.DateTime),
                new Column("NUMBER_NOTIFICATION", DbType.String, 100));

            Database.AddForeignKey("FK_GJI_ACTRMVL_SMOL_ID", "GJI_ACTREMOVAL_SMOL", "ID", "Gji_ACTREMOVAL", "ID");

            Database.ExecuteNonQuery(@"insert into GJI_ACTREMOVAL_SMOL (id)
                                     select id from Gji_ACTREMOVAL");
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_ACTSURVEY_SMOL");
        }
    }
}