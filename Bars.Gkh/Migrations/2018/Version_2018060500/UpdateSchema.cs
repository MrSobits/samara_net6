namespace Bars.Gkh.Migrations._2018.Version_2018060500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018060500")]
    [MigrationDependsOn(typeof(Version_2018052800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
        
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("HAS_CHARGES_185FZ", DbType.Boolean, ColumnProperty.None, false));
        }

        public override void Down()
        {
           
            Database.RemoveColumn("GKH_REALITY_OBJECT", "HAS_CHARGES_185FZ");
        }
    }
}