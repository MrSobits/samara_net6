namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019072600
{
    using System.Text;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;

    [Migration("2019072600")]
    [MigrationDependsOn(typeof(Version_2019053000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            if (!this.Database.TableExists("GJI_WORK_WINTER_MARK") || !this.Database.TableExists("GJI_WORK_WINTER_CONDITION"))
            {
                return;
            }

            //добавление показателя готовности ЖКХ "объекты молодежи"
            const string updateRowsQuery = "update GJI_WORK_WINTER_MARK set workmark_row = workmark_row + 1 where workmark_row > 91";
            const string insertQuery = "insert into GJI_WORK_WINTER_MARK (object_version, object_create_date, object_edit_date, " +
                "workmark_name, workmark_row, workmark_measure, workmark_okei) values(0, now(), now(), 'объекты молодежи', 92, 'ед', '---') RETURNING id";
            this.Database.ExecuteNonQuery(updateRowsQuery);
            var workWinterMarkId = this.Database.ExecuteScalar(insertQuery).ToLong();

            //добавление показателя "объекты молодежи" в каждый из периодов
            const string getHeatPeriodIds = "select DISTINCT heatinputperiod_id from GJI_WORK_WINTER_CONDITION";
            var commonQuery = new StringBuilder();
            using (var reader = this.Database.ExecuteQuery(getHeatPeriodIds))
            {
                while (reader.Read())
                {
                    var insertConditionQuery = "insert into GJI_WORK_WINTER_CONDITION (object_version, object_create_date, object_edit_date, heatinputperiod_id, workwintermark_id) " +
                        $"values(0, now()::timestamp(0), now()::timestamp(0), {reader[0].ToLong()}, {workWinterMarkId});";
                    commonQuery.Append(insertConditionQuery);
                }
            }

            this.Database.ExecuteNonQuery(commonQuery.ToString());
        }

        /// <inheritdoc />
        public override void Down()
        {
            if (!this.Database.TableExists("GJI_WORK_WINTER_MARK") || !this.Database.TableExists("GJI_WORK_WINTER_CONDITION"))
            {
                return;
            }

            const string getWinterMarkIdQuery = "select id from GJI_WORK_WINTER_MARK where workmark_row = 92";
            var markId = this.Database.ExecuteScalar(getWinterMarkIdQuery).ToLong();

            var deleteWorkWinterConditionRows = $"delete from GJI_WORK_WINTER_CONDITION where workwintermark_id = {markId}";
            const string deleteWorkMarkRowQuery = "delete from GJI_WORK_WINTER_MARK where workmark_row = 92";
            const string updateWorkMarkRowsQuery = "update GJI_WORK_WINTER_MARK set workmark_row = workmark_row - 1 where workmark_row > 91";

            this.Database.ExecuteNonQuery(deleteWorkWinterConditionRows);
            this.Database.ExecuteNonQuery(deleteWorkMarkRowQuery);
            this.Database.ExecuteNonQuery(updateWorkMarkRowsQuery);
        }
    }
}
