namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014102900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014102100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddTable(
                "GJI_NSO_ACTCHECK",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("ACQUAINT_WITH_DISP", DbType.String, 250));

            this.Database.ExecuteNonQuery(@"insert into GJI_NSO_ACTCHECK (id)
                                     select id from GJI_ACTCHECK");
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_NSO_ACTCHECK");
        }
    }
}