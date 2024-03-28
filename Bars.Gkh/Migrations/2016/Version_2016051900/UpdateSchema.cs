namespace Bars.Gkh.Migrations._2016.Version_2016051900
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция конвертации запросов в дизайнера
    /// </summary>
    [Migration("2016051900")]
    [MigrationDependsOn(typeof(Version_2016050500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'Gkh.Orgs.Managing.Register.Contract.ControlTransfer.View', id  from b4_role");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'Gkh.Orgs.Managing.Register.Contract.ControlTransfer.Edit', id  from b4_role;");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'Gkh.Orgs.Managing.Register.Contract.ControlTransfer.Create', id  from b4_role;");
            this.Database.ExecuteNonQuery("INSERT INTO b4_role_permission( permission_id, role_id) Select 'Gkh.Orgs.Managing.Register.Contract.ControlTransfer.Delete', id  from b4_role;");
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'Gkh.Orgs.Managing.Register.Contract.ControlTransfer.View'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'Gkh.Orgs.Managing.Register.Contract.ControlTransfer.Edit'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'Gkh.Orgs.Managing.Register.Contract.ControlTransfer.Create'");
            this.Database.ExecuteNonQuery("delete from b4_role_permission where permission_id = 'Gkh.Orgs.Managing.Register.Contract.ControlTransfer.Delete'");
        }
    }
}
