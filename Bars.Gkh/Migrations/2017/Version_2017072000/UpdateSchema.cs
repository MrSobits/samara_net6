namespace Bars.Gkh.Migrations._2017.Version_2017072000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2017072000")]
    [MigrationDependsOn(typeof(Version_2017071300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_OBJ_TECHNICAL_MONITORING", 
                new Column("NAME", DbType.String),
                new Column("DOCUMENT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("USED_IN_EXPORT", DbType.Int32, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String),
                new RefColumn("RO_ID", ColumnProperty.NotNull, "GKH_OBJ_TECH_MON_RO_ID", "GKH_REALITY_OBJECT", "ID"),
                new FileColumn("FILE_ID", ColumnProperty.NotNull, "GKH_OBJ_TECH_MON_FILE_ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_OBJ_TECHNICAL_MONITORING");
        }
    }
}