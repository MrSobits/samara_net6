namespace Bars.GkhCr.Migrations.Version_2013090600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013090600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2013082600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("CR_CONTROL_DATE",
                    new RefColumn("PROGRAM_ID", ColumnProperty.NotNull, "CR_CONTROL_DATE_PROG", "CR_DICT_PROGRAM", "ID"),
                    new RefColumn("WORK_ID", ColumnProperty.NotNull, "CR_CONTROL_DATE_WORK", "GKH_DICT_WORK", "ID"),
                    new Column("DATE_CTRL", DbType.Date));

            Database.AddEntityTable("CR_CTRL_DATE_STAGE",
                    new RefColumn("CONTROL_DATE_ID", ColumnProperty.NotNull, "CR_CTRL_STAGE_CTRL", "CR_CONTROL_DATE", "ID"),
                    new RefColumn("STAGE_ID", ColumnProperty.NotNull, "CR_CTRL_STAGE_STAGE", "CR_DICT_STAGE_WORK", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("CR_CTRL_DATE_STAGE");
            Database.RemoveTable("CR_CONTROL_DATE");
        }
    }
}