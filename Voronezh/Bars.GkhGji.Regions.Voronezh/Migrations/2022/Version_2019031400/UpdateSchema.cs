namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2019031400
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2019031400")]
    [MigrationDependsOn(typeof(Version_2018092600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        /// 
        public override void Up()
        {
            Database.AddColumn("GJI_EMAIL_LIST", new Column("GJI_NUMBER", DbType.String, 500));
            Database.AddColumn("GJI_EMAIL_LIST", new Column("ANSWER_DATE", DbType.DateTime, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_EMAIL_LIST", "ANSWER_DATE");
            Database.RemoveColumn("GJI_EMAIL_LIST", "GJI_NUMBER");
        }
    }
}