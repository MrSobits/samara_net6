namespace Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014071600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014071600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014071500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable("GJI_ACTCHECK_SMOL",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("HAVE_VIOLATION", DbType.Int16, ColumnProperty.NotNull, 30));

            Database.AddForeignKey("FK_GJI_ACTCHECK_SMOL_ID", "GJI_ACTCHECK_SMOL", "ID", "GJI_ACTCHECK", "ID");

            Database.ExecuteNonQuery(@"insert into GJI_ACTCHECK_SMOL (id)
                                     select id from GJI_ACTCHECK");
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_ACTCHECK_SMOL", "FK_GJI_ACTCHECK_SMOL_ID");
            Database.RemoveTable("GJI_ACTCHECK_SMOL");
        }
    }
}