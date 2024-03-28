namespace Bars.GkhGji.Migrations._2018.Version_2018020100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2018020100")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2017.Version_2017112900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            if (this.Database.ColumnExists("GJI_APPEAL_CITIZENS", "COMMENT"))
            {
                this.Database.ChangeColumn("GJI_APPEAL_CITIZENS", new Column("COMMENT", DbType.String, 1000));
            }

            if (this.Database.ColumnExists("GJI_APPEAL_CITIZENS", "DESCRIPTION"))
            {
                this.Database.ChangeColumn("GJI_APPEAL_CITIZENS", new Column("DESCRIPTION", DbType.String, 8000));
            }
        }

        public override void Down()
        {
            if (this.Database.ColumnExists("GJI_APPEAL_CITIZENS", "COMMENT"))
            {
                this.Database.ChangeColumn("GJI_APPEAL_CITIZENS", new Column("COMMENT", DbType.String, 255));
            }

            if (this.Database.ColumnExists("GJI_APPEAL_CITIZENS", "DESCRIPTION"))
            {
                this.Database.ChangeColumn("GJI_APPEAL_CITIZENS", new Column("DESCRIPTION", DbType.String, 2000));
            }
        }
    }
}