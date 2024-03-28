namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019111102
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019111102")]
    [MigrationDependsOn(typeof(Version_2019111101.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string ControlOrganizationTableName = "GKH_CONTROL_ORGANIZATION";
        private const string ControlOrganizationControlTypeRelationTableName = "GKH_CONTROL_ORGANIZATION_TYPE_RELATION";
        private const string TatarstanZonalInspectionTableName = "GKH_DICT_TAT_ZONAINSP";

        public override void Up()
        {
            //Таблица КНО
            this.Database.AddEntityTable(UpdateSchema.ControlOrganizationTableName, 
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull,
                "FK_CONTRAGENT_GKH_CONTROL_ORGANIZATION", "GKH_CONTRAGENT", "id"));
            
            //добавляем жил инспекцию в качестве КНО
            this.Database.ExecuteNonQuery(@"insert into GKH_CONTROL_ORGANIZATION (object_version, object_create_date, object_edit_date, contragent_id)
                       values (1, now()::timestamp(0), now()::timestamp(0), (select id from GKH_CONTRAGENT WHERE upper(name) = 'ГОСУДАРСТВЕННАЯ ЖИЛИЩНАЯ ИНСПЕКЦИЯ РЕСПУБЛИКИ ТАТАРСТАН'))");

            //таблица связь КНО и вид контроля.
            this.Database.AddEntityTable(UpdateSchema.ControlOrganizationControlTypeRelationTableName,
                new RefColumn("CONTROL_ORG_ID", ColumnProperty.NotNull, "FK_GKH_CONTROL_ORGANIZATION_TYPE_RELATION_ORG",
                    UpdateSchema.ControlOrganizationTableName, "id"),
                new RefColumn("CONTROL_TYPE_ID", ColumnProperty.NotNull, "FK_GKH_CONTROL_ORGANIZATION_TYPE_RELATION_TYPE",
                    "GJI_DICT_CONTROL_TYPES", "id"));
            
            //таблица зональных инспекций для рт (расширение функционала зональных инспекций)
            this.Database.AddTable(UpdateSchema.TatarstanZonalInspectionTableName,
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new RefColumn("CONTROL_ORG_ID", ColumnProperty.Null,
                    "FK_GKH_DICT_TAT_ZONAINSP_CONTROL_ORG", UpdateSchema.ControlOrganizationTableName, "id"));

            this.Database.ExecuteNonQuery(@"insert into GKH_DICT_TAT_ZONAINSP (id) select id from GKH_DICT_ZONAINSP");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.TatarstanZonalInspectionTableName);
            this.Database.RemoveTable(UpdateSchema.ControlOrganizationControlTypeRelationTableName);
            this.Database.RemoveTable(UpdateSchema.ControlOrganizationTableName);
        }
    }
}
