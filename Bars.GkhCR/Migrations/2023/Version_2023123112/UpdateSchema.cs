using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2023.Version_2023123112
{
    [Migration("2023123112")]
    [MigrationDependsOn(typeof(_2023.Version_2023123111.UpdateSchema))]
    // Является Version_2023092800 из ядра
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            Database.ExecuteNonQuery("COMMENT ON TABLE public.CR_DICT_PROGRAM IS 'Программа капитального ремонта';");

            Database.AddColumn("CR_DICT_PROGRAM", new Column("USE_FOR_REFORMA_GIS_GKH_REPORTS", DbType.Boolean, ColumnProperty.None, false));
        }
    }
}