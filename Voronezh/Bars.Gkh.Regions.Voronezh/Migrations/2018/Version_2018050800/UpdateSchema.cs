namespace Bars.Gkh.Regions.Voronezh.Migrations._2018.Version_2018050800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2018050800")]
    [MigrationDependsOn(typeof(Version_2018042400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        SchemaQualifiedObjectName tableDesc = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTDESC", Schema = "IMPORT" };
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"ALTER TABLE import.rosregextractdesc ADD xml xml;");
            this.Database.ExecuteNonQuery(@"create function get_debtor_id(acc_id_p integer)
                                            returns integer
                                            as $$ (select id from regop_debtor where account_id = acc_id_p)$$
                                            LANGUAGE SQL
                                            returns null on null input;");
        }

        public override void Down()
        {
            this.Database.RemoveColumn(tableDesc, "XML");
        }
    }
}