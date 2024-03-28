namespace Bars.Gkh1468.Migrations.Version_2017050400
{
    using System.Data;
    using System.Linq;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017050400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2015100100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!this.Database.TableExists("GKH_PLACEMENT_TYPE"))
            {
                this.Database.AddEntityTable(
                 "GKH_PLACEMENT_TYPE",
                 new Column("ID", DbType.Int32),
                 new Column("NAME", DbType.String),
                 new Column("SHORT_NAME", DbType.String));

                this.Database.ExecuteNonQuery(
                    "INSERT INTO GKH_PLACEMENT_TYPE(ID, OBJECT_VERSION, OBJECT_CREATE_DATE, OBJECT_EDIT_DATE, NAME, SHORT_NAME) VALUES" +
                        "(1, 0, current_date, current_date, 'офис', 'оф.')," +
                        "(2, 0, current_date, current_date, 'квартира', 'кв.')," +
                        "(3, 0, current_date, current_date, 'помещение', 'пом.')," +
                        "(4, 0, current_date, current_date, 'комната', 'ком.')");

                this.Database.ExecuteNonQuery(
                    "insert into b4_role_permission(permission_id, role_id) " +
                        "select 'Gkh.Orgs.Contragent.Field.AddressCoords_View', id " +
                            "from b4_role " +
                        "union " +
                        "select 'Gkh.Orgs.Contragent.Field.AddressCoords_Edit', id " +
                            "from b4_role ");
            }
        }

        public override void Down()
        {
                this.Database.RemoveTable("GKH_PLACEMENT_TYPE");

            this.Database.ExecuteNonQuery(
                "delete from b4_role_permission where permission_id in ('Gkh.Orgs.Contragent.Field.AddressCoords_View', 'Gkh.Orgs.Contragent.Field.AddressCoords_Edit')");
        }
    }
}