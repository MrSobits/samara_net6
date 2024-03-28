namespace Bars.Gkh.Migrations._2021.Version_2021092000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021092000")]
    
    [MigrationDependsOn(typeof(Version_2021082000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("CLW_JUR_INSTITUTION", new Column("HEADER_TEXT", DbType.String, 5000, ColumnProperty.None));         
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("CLW_JUR_INSTITUTION", "HEADER_TEXT");
        }
    }
}