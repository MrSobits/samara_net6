// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateSchema.cs" company="">
//   
// </copyright>
// <summary>
//   The update schema.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhGji.Regions.Saha.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_SAHA_INSP_VIOL_WORD",
               new Column("WORDING", DbType.String, ColumnProperty.Null, 2000),
               new RefColumn("INSPECTION_VIOL_ID", ColumnProperty.NotNull, "GJI_SAHA_INSP_VIOL_WORD", "GJI_INSPECTION_VIOLATION", "ID"));

            Database.AddEntityTable("GJI_SAHA_DOCGJI_PERSINFO",
                new Column("ADDRESS", DbType.String),
                new Column("JOB", DbType.String),
                new Column("POSITION", DbType.String),
                new Column("BIRTHDAY_AND_PLACE", DbType.String),
                new Column("IDENTITY_DOCUMENT", DbType.String),
                new Column("SALARY", DbType.String),
                new Column("MARITAL_STATUS", DbType.String),
                new RefColumn("DOCUMENT_ID", ColumnProperty.NotNull, "GJI_SAHA_DOCGJI_PERSINFO_DOC", "GJI_DOCUMENT", "ID"));

            //-----Мероприятия по контролю распоряжения
            Database.AddEntityTable(
                "GJI_SAHA_DISP_CON_MEASURE",
                new Column("DISPOSAL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("CONTROL_MEASURES_NAME", DbType.String, 2000));
            Database.AddForeignKey("FK_CONTROL_MEASURES_NAME", "GJI_SAHA_DISP_CON_MEASURE", "DISPOSAL_ID", "GJI_DISPOSAL", "ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_SAHA_DISP_CON_MEASURE", "FK_CONTROL_MEASURES_NAME");
            Database.RemoveTable("GJI_SAHA_DISP_CON_MEASURE");

            Database.RemoveTable("GJI_SAHA_DOCGJI_PERSINFO");

            Database.RemoveTable("GJI_SAHA_INSP_VIOL_WORD");
        }
    }
}