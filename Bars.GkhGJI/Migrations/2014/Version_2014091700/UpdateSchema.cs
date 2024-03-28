namespace Bars.GkhGji.Migration.Version_2014091700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014091700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migration.Version_2014091201.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_INSPECTION_VIOLATION", "ACTION", DbType.String, 2000);
            
            //-----Указание к устранению нарушений входе проверки заносятся в нарушения предписания
            Database.AddTable(
                "GJI_DISPOSAL_VIOLAT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddForeignKey("FK_GJI_DISPOSAL_VIOLAT_ID", "GJI_DISPOSAL_VIOLAT", "ID", "GJI_INSPECTION_VIOL_STAGE", "ID");
            //-----
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_INSPECTION_VIOLATION", "ACTION");
            Database.RemoveConstraint("GJI_DISPOSAL_VIOLAT", "FK_GJI_DISPOSAL_VIOLAT_ID");
            Database.RemoveTable("GJI_DISPOSAL_VIOLAT");
        }
    }
}