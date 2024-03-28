namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021042100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021042100")]
    [MigrationDependsOn(typeof(Version_2021040800.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_LICENSE_ACTION", new Column("POST_ADDRESS", DbType.String, 500));
 			Database.AddColumn("GJI_LICENSE_ACTION", new Column("TYPE_ANSWER", DbType.String, 1000));
            Database.AddColumn("GJI_CH_SMEV_STAYING_PLACE", new Column("ANSWER", DbType.String));
            Database.AddColumn("GJI_CH_SMEV_STAYING_PLACE", new Column("TASK_ID", DbType.String));
            Database.AddColumn("GJI_CH_SMEV_LIVING_PLACE", new Column("ANSWER", DbType.String));
            Database.AddColumn("GJI_CH_SMEV_LIVING_PLACE", new Column("TASK_ID", DbType.String));
            Database.AddColumn("GJI_CH_SMEV_VALID_PASSPORT", new Column("ANSWER", DbType.String));
            Database.AddColumn("GJI_CH_SMEV_VALID_PASSPORT", new Column("TASK_ID", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_STAYING_PLACE", "ANSWER");
            Database.RemoveColumn("GJI_CH_SMEV_STAYING_PLACE", "TASK_ID");
            Database.RemoveColumn("GJI_CH_SMEV_LIVING_PLACE", "ANSWER");
            Database.RemoveColumn("GJI_CH_SMEV_LIVING_PLACE", "TASK_ID");
            Database.RemoveColumn("GJI_CH_SMEV_VALID_PASSPORT", "ANSWER");
            Database.RemoveColumn("GJI_CH_SMEV_VALID_PASSPORT", "TASK_ID");
            Database.RemoveColumn("GJI_LICENSE_ACTION", "TYPE_ANSWER");
			Database.RemoveColumn("GJI_LICENSE_ACTION", "POST_ADDRESS");
        }


    }
}
