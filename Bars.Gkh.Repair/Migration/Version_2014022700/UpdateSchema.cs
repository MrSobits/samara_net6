// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateSchema.cs" company="">
//   
// </copyright>
// <summary>
//   The update schema.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.Gkh.Repair.Migrations.Version_2014022700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Repair.Migrations.Version_2014022601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("RP_CONTROL_DATE",
                    new RefColumn("RP_PROGRAM_ID", ColumnProperty.NotNull, "RP_CONTROL_DATE_PROG", "RP_DICT_PROGRAM", "ID"),
                    new RefColumn("RP_WORK_ID", ColumnProperty.NotNull, "RP_CONTROL_DATE_WORK", "GKH_DICT_WORK_CUR_REPAIR", "ID"),
                    new Column("RP_DATE_CTRL", DbType.Date));
        }

        public override void Down()
        {
            Database.RemoveTable("RP_CONTROL_DATE");
        }
    }
}