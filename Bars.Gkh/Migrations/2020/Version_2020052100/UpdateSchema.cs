namespace Bars.Gkh.Migrations._2020.Version_2020052100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020052100")]
    
    [MigrationDependsOn(typeof(Version_2020051900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_ADDIT_WORK", new Column("QUEUE", DbType.Int16, ColumnProperty.None,null));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_ADDIT_WORK", "QUEUE");
        }
    }
}