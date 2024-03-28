namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022111400
{
    using System;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;

    [Migration("2022111400")]
    [MigrationDependsOn(typeof(Version_2022111000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName stateTable = new SchemaQualifiedObjectName { Name = "B4_STATE" };

        /// <inheritdoc />
        public override void Up()
        {
            var statuses = new []
            {
                new State{Code = "1", Name = "Новое", StartState = true, OrderValue = 1},
                new State{Code = "2", Name = "В работе", OrderValue = 2},
                new State{Code = "3", Name = "Обработано", FinalState = true, OrderValue = 3},
                new State{Code = "4", Name = "Не обработано", FinalState = true, OrderValue = 3},
            };
            var columns = new[] { "object_version", "object_create_date", "object_edit_date", "code", "name", "type_id", "start_state", "final_state", "order_value" };

            statuses.ForEach(status =>
            {
                var statusExists =
                    this.Database.ExecuteScalar(
                        $"SELECT EXISTS (SELECT FROM {stateTable.Name} WHERE type_id = 'gji_rapid_response_system_appeal_details' AND code = '{status.Code}')").ToBool();

                if (statusExists)
                    return;
                
                var values = new[]
                {
                    "0", $"'{DateTime.Now}'", $"'{DateTime.Now}'", $"'{status.Code}'", $"{status.Name}", "'gji_rapid_response_system_appeal_details'",
                    $"{status.StartState}", $"{status.FinalState}", $"{status.OrderValue}"
                };

                this.Database.Insert(this.stateTable, columns, values);
            });
        }
    }
}