namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016033000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016033000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2016031500.UpdateSchema))]

    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
	        this.Database.AddEntityTable(
		        "GKH_CONSTRUCT_OBJ_PARTICIPANT",
		        new Column("PARTICIPANT_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
		        new Column("CUSTOMER_TYPE", DbType.Int32, 4),
		        new Column("DESCRIPTION", DbType.String, 300),
		        new RefColumn("OBJECT_ID", ColumnProperty.NotNull, "GKH_CONSTRUCT_OBJ_PARTICIPANT_OBJ", "GKH_CONSTRUCTION_OBJECT", "ID"),
		        new RefColumn("CONTRAGENT_ID", "GKH_CONSTRUCT_OBJ_PARTICIPANT_C", "GKH_CONTRAGENT", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_CONSTRUCT_OBJ_PARTICIPANT");
        }
    }
}
