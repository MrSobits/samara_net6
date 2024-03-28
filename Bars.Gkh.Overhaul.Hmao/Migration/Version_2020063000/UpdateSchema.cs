namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2020063000
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2020063000")]
    [MigrationDependsOn(typeof(Version_2019110700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {           
           Database.AddEntityTable("OVRHL_DICT_CR_PERIOD",                
                new Column("YEAR_START", DbType.Int32, ColumnProperty.NotNull),
                new Column("YEAR_END", DbType.Int32, ColumnProperty.NotNull)
                );
        }
                       
        public override void Down()
        {
            Database.RemoveTable("OVRHL_DICT_CR_PERIOD"); 
        }
    }
}