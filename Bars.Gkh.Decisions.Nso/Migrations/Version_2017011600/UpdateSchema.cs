namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2017011600
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция 2017011600
    /// </summary>
    [Migration("2017011600")]
    [MigrationDependsOn(typeof(Version_2016122100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            string[] permissionToCreate =
            {
                "Gkh.RealityObject.Register.DecisionProtocolsViewCreate.ProtocolTypes.Owners",
                "Gkh.RealityObject.Register.DecisionProtocolsViewCreate.ProtocolTypes.Government"
            };

           var existPermissionsReader = this.Database.ExecuteQuery($"select role_id, permission_id from b4_role_permission where permission_id in " +
                $"({permissionToCreate.Select(x => $"'{x}'").AggregateWithSeparator(",")})");

            IDictionary<long, bool[]> perms = new Dictionary<long, bool[]>();

            // достаём имеющиеся права доступа
            using (existPermissionsReader)
            {
                while (existPermissionsReader.Read())
                {
                    var roleId = existPermissionsReader.GetInt64(0);
                    var permId = existPermissionsReader.GetString(1);

                    var array = perms.Get(roleId);
                    if (array.IsNull())
                    {
                        array = new bool[2];
                        perms[roleId] = array;
                    }

                    array[Array.IndexOf(permissionToCreate, permId)] = true;
                }
            }

            var viewPermissionReader =
               this.Database.ExecuteQuery(
                   "select distinct role_id from b4_role_permission where permission_id = \'Gkh.RealityObject.Register.DecisionProtocolsViewCreate.View\'");

            var operations = new List<string>();
            using (viewPermissionReader)
            {
                while (viewPermissionReader.Read())
                {
                    var roleId = viewPermissionReader.GetInt64(0);

                    foreach (var permission in permissionToCreate)
                    {
                        // если права доступа ещё не созданы, то создаём
                        if (perms.Get(roleId)?[Array.IndexOf(permissionToCreate, permission)] != true)
                        {
                            operations.Add($"INSERT INTO b4_role_permission (permission_id, role_id) VALUES ('{permission}', {roleId})");
                        }
                    }
                }
            }

            foreach (var operation in operations)
            {
                this.Database.ExecuteNonQuery(operation);
            }
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
        }
    }
}
