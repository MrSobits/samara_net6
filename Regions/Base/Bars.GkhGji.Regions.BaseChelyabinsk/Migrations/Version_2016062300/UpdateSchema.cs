namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2016062300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2016062300")]
    [MigrationDependsOn(typeof(Version_2015111700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
	    public override void Up()
	    {
            if (!this.Database.TableExists("CHEL_GJI_APPEAL_CITIZENS"))
            {
                this.Database.AddJoinedSubclassTable("CHEL_GJI_APPEAL_CITIZENS", "GJI_APPEAL_CITIZENS", "CHEL_APCIT",
                new Column("COMMENT", DbType.String, 2000, ColumnProperty.Null));
            }

            this.Database.ChangeColumn("GJI_APPCIT_EXECUTANT", new Column("EXECUTANT_ID", DbType.Int64, ColumnProperty.Null));
	        if (this.Database.ColumnExists("GJI_APPCIT_EXECUTANT", "RESOLUTION_ID"))
	        {
                this.Database.ChangeColumn("GJI_APPCIT_EXECUTANT", new Column("RESOLUTION_ID", DbType.Int64, ColumnProperty.Null));

                if (!this.Database.ConstraintExists("GJI_APPCIT_EXECUTANT", "FK_APPCIT_EXECUTANT_RESOLUTION"))
                {
                    this.Database.AddForeignKey("FK_APPCIT_EXECUTANT_RESOLUTION", "GJI_APPCIT_EXECUTANT", "RESOLUTION_ID", "B4_FILE_INFO", "ID");
                }
            }
	        else
	        {
	            this.Database.AddRefColumn("GJI_APPCIT_EXECUTANT", new RefColumn("RESOLUTION_ID", "FK_APPCIT_EXECUTANT_RESOLUTION", "B4_FILE_INFO", "ID"));
	        }

	        if (!this.Database.ColumnExists("GJI_APPCIT_EXECUTANT", "CONTROLLER_ID"))
	        {
                this.Database.AddRefColumn("GJI_APPCIT_EXECUTANT", new RefColumn("CONTROLLER_ID", ColumnProperty.Null, "GJI_APPCITEXEC_CTRL", "GKH_DICT_INSPECTOR", "ID"));
            }

            if (this.Database.TableExists("GJI_DISP_CON_MEASURE"))
            {
                this.Database.AddEntityTable("GJI_DISP_CON_MEASURE",
               new RefColumn("CONTROL_MEASURES_ID", ColumnProperty.Null, "DISP_CONTR_MEASURES", "GJI_DICT_CON_ACTIVITY", "ID"),
               new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "DISP_CONMEAS", "GJI_DISPOSAL", "ID"));
            }
        }

	    public override void Down()
        {
            this.Database.ChangeColumn("GJI_APPCIT_EXECUTANT", new RefColumn("EXECUTANT_ID", ColumnProperty.NotNull, "GJI_APPCITEXEC_EXEC", "GKH_DICT_INSPECTOR", "ID"));

            this.Database.RemoveConstraint("GJI_APPCIT_EXECUTANT", "FK_APPCIT_EXECUTANT_RESOLUTION");
            this.Database.RemoveColumn("GJI_APPCIT_EXECUTANT", "RESOLUTION_ID");
            this.Database.RemoveColumn("GJI_APPCIT_EXECUTANT", "CONTROLLER_ID");

            this.Database.RemoveTable("CHEL_GJI_APPEAL_CITIZENS");
            this.Database.RemoveTable("GJI_DISP_CON_MEASURE");
        }
    }
}