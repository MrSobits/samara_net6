namespace Bars.Gkh.Migrations._2016.Version_2016080500
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

	[Migration("2016080500")]
    [MigrationDependsOn(typeof(Version_2016062200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
			Database.AddEntityTable("GKH_MUNICIPALITY_FIAS_OKTMO",
				new Column("FIAS_GUID", DbType.String, 36),
				new Column("OKTMO", DbType.Int64),
                new RefColumn("MUNICIPALITY_ID", "GKH_MU_FIAS_OKTMO_MU", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
			Database.RemoveTable("GKH_MUNICIPALITY_FIAS_OKTMO");
        }
    }
}
