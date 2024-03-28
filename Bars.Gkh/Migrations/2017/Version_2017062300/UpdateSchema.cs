namespace Bars.Gkh.Migrations._2017.Version_2017062300
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017062300")]
    [MigrationDependsOn(typeof(Version_2017061400.UpdateSchema))]

    public class UpdateSchema : Migration
    {

        public override void Up()
        {
            this.Database.AddEntityTable("GKH_ADDRESS_MATCH",
                new Column("EXTERNAL_ADDRESS", DbType.String, 500, ColumnProperty.NotNull),
                new Column("TYPE_ADDRESS", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("RO_ID", ColumnProperty.NotNull, "ADDRESS_MATCH_RO_ID", "GKH_REALITY_OBJECT", "ID"));

            this.Database.AddIndex("IND_ADDRESS_MATCH_EXTERNAL_ADDRESS", false, "GKH_ADDRESS_MATCH", "EXTERNAL_ADDRESS");
            this.Database.AddIndex("IND_ADDRESS_MATCH_TYPE_ADDRESS", false, "GKH_ADDRESS_MATCH", "TYPE_ADDRESS");

            // будем требовать уникальности связки внешний адрес + тип на уровне бд
            this.Database.AddIndex("IND_ADDRESS_MATCH_ADDR_AND_TYPE", true, "GKH_ADDRESS_MATCH", "EXTERNAL_ADDRESS", "TYPE_ADDRESS");
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_ADDRESS_MATCH");
        }
    }
}
