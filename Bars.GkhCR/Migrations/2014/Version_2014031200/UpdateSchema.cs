using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.GkhCr.Migrations.Version_2014031200
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014021400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("CR_DICT_PROG_CHANGE_JOUR",
                new RefColumn("PROGRAM_ID", ColumnProperty.NotNull, "CR_PROG_CHAN_JOUR_PR", "CR_DICT_PROGRAM", "ID"),
                new Column("TYPE_CHANGE", DbType.Int32, 4, ColumnProperty.NotNull, 0),
                new Column("DATE_CHANGE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("MU_COUNT", DbType.Int32),
                new Column("USER_NAME", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveTable("CR_DICT_PROG_CHANGE_JOUR");
        }
    }
}