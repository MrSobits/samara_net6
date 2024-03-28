namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019111101
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019111101")]
    [MigrationDependsOn(typeof(Version_2019111100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string ControlTypeTableName = "GJI_DICT_CONTROL_TYPES";
        private const string TatarstanDisposalTableName = "GJI_TAT_DISPOSAL";
        private const string ControlTypeColumnName = "CONTROL_TYPE_ID";

        public override void Up()
        {
            //справочник "Виды контроля"
            this.Database.AddEntityTable(UpdateSchema.ControlTypeTableName,
                new Column("EXTERNAL_ID", DbType.String, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                new Column("LEVEL", DbType.Int32, ColumnProperty.Null));
            this.FillControlTypeTable();

            //добавление поля "Вид контроля" в tatarstanDisposal
            this.Database.AddRefColumn(UpdateSchema.TatarstanDisposalTableName, 
                new RefColumn(UpdateSchema.ControlTypeColumnName, ColumnProperty.Null,
                    "FK_CONTROL_TYPE_GJI_TAT_DISPOSAL", UpdateSchema.ControlTypeTableName, "id"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(UpdateSchema.TatarstanDisposalTableName, UpdateSchema.ControlTypeColumnName);
            this.Database.RemoveTable(UpdateSchema.ControlTypeTableName);
        }

        private void FillControlTypeTable()
        {
            var query = $@"insert into {UpdateSchema.ControlTypeTableName} (object_version, object_create_date, object_edit_date, external_id, name, level) values
                (1, now()::timestamp(0), now()::timestamp(0), '4b0e0048-cf12-11e9-86e8-be11044acab4', 'Лицензионный контроль в отношении юридических лиц или индивидуальных предпринимателей, осуществляющих деятельность по управлению многоквартирными домами на основании лицензии', null),
	            (1, now()::timestamp(0), now()::timestamp(0), '4b0e00ca-cf12-11e9-86e8-be11044acab4', 'Государственный жилищный надзор', null); ";
            this.Database.ExecuteNonQuery(query);
        }
    }
}
