namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2014102100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2014100500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_SHORT_PROG_OBJ", new Column("YEAR", DbType.Int32, ColumnProperty.NotNull, 0));
            Database.ExecuteNonQuery("UPDATE OVRHL_SHORT_PROG_OBJ SET YEAR = 2014");
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_SHORT_PROG_OBJ", "YEAR");
        }
    }
}