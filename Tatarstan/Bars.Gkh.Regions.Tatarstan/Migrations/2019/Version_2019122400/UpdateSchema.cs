namespace Bars.Gkh.Regions.Tatarstan.Migrations._2019.Version_2019122400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019122400")]
    [MigrationDependsOn(typeof(Version_2019070900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RenameTable("CR_DICT_ELEMENT_OUTDOOR", "GKH_DICT_ELEMENT_OUTDOOR");

            this.Database.AddEntityTable("GKH_RO_OUTDOOR_ELEMENT",
                new Column("VOLUME", DbType.Decimal, ColumnProperty.NotNull),
                new Column("CONDITION_ELEMENT", DbType.Int32, ColumnProperty.NotNull, 10),
                new RefColumn("OUTDOOR_ID", "GKH_RO_OUTDOOR_ELEMENT_OUTDOOR", "GKH_REALITY_OBJECT_OUTDOOR", "ID"),
                new RefColumn("ELEMENT_ID", "GKH_RO_OUTDOOR_ELEMENT_ELEMENT", "GKH_DICT_ELEMENT_OUTDOOR", "ID")
            );
        }

        public override void Down()
        {
            this.Database.RenameTable("GKH_DICT_ELEMENT_OUTDOOR", "CR_DICT_ELEMENT_OUTDOOR");
            this.Database.RemoveTable("GKH_RO_OUTDOOR_ELEMENT");
        }
    }
}