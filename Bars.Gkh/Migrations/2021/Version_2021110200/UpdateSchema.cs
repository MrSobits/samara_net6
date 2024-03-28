namespace Bars.Gkh.Migrations._2021.Version_2021110200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021110200")]
    
    [MigrationDependsOn(typeof(Version_2021092000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("IS_NOT_GZHI", DbType.Boolean, ColumnProperty.NotNull, false));         
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "IS_NOT_GZHI");
        }
    }
}