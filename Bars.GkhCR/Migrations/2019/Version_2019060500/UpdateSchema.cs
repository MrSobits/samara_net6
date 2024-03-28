using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2019.Version_2019060500
{
    [Migration("2019060500")]
    [MigrationDependsOn(typeof(_2019.Version_2019020400.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_PROPOSE",
                new Column("PROGRAM_NUM", DbType.String, 50),
                new Column("DATE_END_BUILDER", DbType.Date),              
                new Column("DATE_START_WORK", DbType.Date),              
                new Column("DESCRIPTION", DbType.String, 500),            
                new RefColumn("OBJECT_CR_ID", ColumnProperty.NotNull, "FK_OVRHL_PROPOSE_CROBJECT", "CR_OBJECT", "ID"),
                new RefColumn("PROGRAM_ID", ColumnProperty.NotNull, "FK_OVRHL_PROPOSE_PROGRAM", "CR_DICT_PROGRAM", "ID"),
                new RefColumn("STATE_ID", "FK_OVRHL_PROPOSE_STATE", "B4_STATE", "ID"));


            //-----Вид работы
            Database.AddEntityTable(
                "OVRHL_PROPOSE_TYPE_WORK",
                new Column("VOLUME", DbType.Decimal),              
                new Column("SUM", DbType.Decimal),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("DATE_START_WORK", DbType.Date),
                new Column("DATE_END_WORK", DbType.Date),              
                new RefColumn("PROPOSAL_ID", ColumnProperty.NotNull, "FK_OVRHL_PROPOSE_WORK_PROPOSE", "OVRHL_PROPOSE", "ID"),
                new RefColumn("WORK_ID", ColumnProperty.NotNull, "FK_OVRHL_PROPOSE_WORK_WORK", "GKH_DICT_WORK", "ID"));
         
            //-----
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_PROPOSE_TYPE_WORK");
            Database.RemoveTable("OVRHL_PROPOSE");
        }
    }
}