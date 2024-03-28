namespace Bars.GkhGji.Migrations._2022.Version_2022061500
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022061500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022060800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
               "GJI_RESOLUTION_DECISION",
                 new RefColumn("RESOLUTION_ID", "GJI_RESOLUTION_DECISION_RESOLUTION", "GJI_RESOLUTION", "ID"),
                 new Column("APPEAL_DATE", DbType.DateTime, ColumnProperty.NotNull),
                 new Column("APPEAL_NUMBER", DbType.String, 50),              
                 new Column("DOCUMENT_NAME", DbType.String, 500),               
                 new Column("ESTABLISHED", DbType.String, 40000),
                 new Column("DECIDED", DbType.String, 40000),            
                 new RefColumn("CONSIDER_INSPECTOR_ID", "GJI_RESOLUTION_DECISION_CONSIDER_INSPECTOR", "GKH_DICT_INSPECTOR", "ID"),
                 new Column("APELLANT", DbType.String, 500),
                 new Column("TYPE_ANSWER", DbType.Int32, 4, ColumnProperty.NotNull, 0),
                 new Column("APELLANT_POSITION", DbType.String, 500));

        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_RESOLUTION_DECISION");
        }
    }
}