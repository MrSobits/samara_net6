namespace Bars.GkhCr.Migrations._2015.Version_2015121400
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015121400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations._2015.Version_2015121000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("CR_OBJ_DESIGN_ASSIGNMENT",
                new RefColumn("OBJECT_ID", "CR_OBJ_DESSIGN", "CR_OBJECT", "ID"),
                new RefColumn("TYPE_WORK_ID", "CR_OBJ_DESSIGN_TW", "CR_OBJ_TYPE_WORK", "ID"),
                new RefColumn("FILE_ID", "CR_DESSIGN_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("STATE_ID", "CR_DESSIGN_STATE", "B4_STATE", "ID"),
                new Column("DOCUMENT", DbType.String, 50),
                new Column("DATE", DbType.DateTime, null));

        }

        public override void Down()
        {
            Database.RemoveTable("CR_OBJ_DESIGN_ASSIGNMENT");
        }
    }
}
