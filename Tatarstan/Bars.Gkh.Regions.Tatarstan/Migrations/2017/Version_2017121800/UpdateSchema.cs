namespace Bars.Gkh.Regions.Tatarstan.Migrations._2017.Version_2017121800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2017121800")]
    [MigrationDependsOn(typeof(Version_2017092100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_GAS_EQUIPMENT_ORG",
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "GKH_GAS_EQUIP_CONTRAGENT_ID", "GKH_CONTRAGENT", "ID"),
                new RefColumn("CONTACT_ID", "GKH_GAS_EQUIP_CONTACT_ID", "GKH_CONTRAGENT_CONTACT", "ID"));
            
            this.Database.AddEntityTable("GKH_GAS_EQUIP_ORG_CONTRACT",
                new RefColumn("GAS_EQUIPMENT_ORG_ID", ColumnProperty.NotNull, "GAS_EQUIP_ORG_CONTRACT_GAS_EQUIP_ORG", "GKH_GAS_EQUIPMENT_ORG", "ID"),
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "GAS_EQUIP_CONTRACT_REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID"),
                new Column("START_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("END_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DOCUMENT_NUM", DbType.String, ColumnProperty.Null, 255),
                new FileColumn("FILE_ID", ColumnProperty.Null, "GAS_EQUIP_ORG_FILE"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GKH_GAS_EQUIP_ORG_CONTRACT");
            this.Database.RemoveTable("GKH_GAS_EQUIPMENT_ORG");
        }
    }
}