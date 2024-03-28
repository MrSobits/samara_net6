namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021112400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021112400")]
    [MigrationDependsOn(typeof(Version_2021111500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddTable("GJI_TASK_ACTIONISOLATED",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("MUNICIPALITY_ID", DbType.Int64),
                new Column("CONTRAGENT_ID", DbType.Int64),
                new Column("PLAN_ACTION_ID", DbType.Int64),
                new Column("PERSON_NAME", DbType.String),
                new Column("KIND_ACTION", DbType.Int32),
                new Column("TYPE_BASE", DbType.Int32),
                new Column("TYPE_OBJECT", DbType.Int32),
                new Column("DATE_START", DbType.DateTime));
            Database.AddForeignKey("FK_GJI_TASK_ACTION_DOC", "GJI_TASK_ACTIONISOLATED", "ID", "GJI_DOCUMENT", "ID");
            Database.AddForeignKey("FK_GJI_TASK_ACTION_MUNICIPALITY", "GJI_TASK_ACTIONISOLATED", "MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID");
            Database.AddForeignKey("FK_GJI_TASK_ACTION_CONTRAGENT", "GJI_TASK_ACTIONISOLATED", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_GJI_TASK_ACTION_PLAN_ACTION", "GJI_TASK_ACTIONISOLATED", "PLAN_ACTION_ID", "GJI_DICT_PLANACTION", "ID");

            this.Database.AddEntityTable("GJI_TASK_ACTION_ROBJECT",
                new RefColumn("TASK_ID", "GJI_TASK_ACTION_ROBJECT_TASK", "GJI_TASK_ACTIONISOLATED", "ID"),
                new RefColumn("REALITY_OBJECT_ID", "GJI_TASK_ACTION_ROBJECT_ROBJECT", "GKH_REALITY_OBJECT", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GJI_TASK_ACTION_ROBJECT");
            this.Database.RemoveTable("GJI_TASK_ACTIONISOLATED");
        }
    }
}