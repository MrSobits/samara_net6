namespace Bars.GkhCr.Migrations.Version_2013060400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013060400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2013042500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "CR_DICT_QMEM_ROLE",
                new RefColumn("QUAL_MEMBER_ID", ColumnProperty.NotNull, "CR_DICT_QMEM_ROLE_QMEM", "CR_DICT_QUAL_MEMBER", "ID"),
                new RefColumn("ROLE_ID", ColumnProperty.NotNull, "CR_DICT_QMEM_ROLE_ROLE", "B4_ROLE", "ID"),
                new Column("EXTERNAL_ID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveTable("CR_DICT_QMEM_ROLE");
        }
    }
}