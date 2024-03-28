namespace Bars.Gkh.Migrations._2021.Version_2021042800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021042800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021041500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_PERSON_CERTIFICATE", new Column("ISSUED_BY", DbType.String, ColumnProperty.None, 500));       
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "ISSUED_BY");
        }
    }
}