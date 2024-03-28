namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2020071400
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020071400")]
    [MigrationDependsOn(typeof(Version_2020071300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            try
            {
                CopyFLDocTypeToPhysicalPersonDocType();
            }
            catch
            {
                throw new Exception("Не удалось заполнить таблицу GJI_DICT_PHYSICAL_PERSON_DOC_TYPE. Сначал выполните миграцию модуля Bars.GkhGji");
            }
            Database.AddColumn("GJI_CH_GIS_GMP", new RefColumn("PHYSICALPERSON_DOCTYPE_ID", ColumnProperty.None, "GJI_CH_GIS_GMP_PHYSICALPERSON_DOCTYPE_ID", "GJI_DICT_PHYSICAL_PERSON_DOC_TYPE", "ID"));
            Database.AddColumn("GJI_CH_RESOLUTION_FIZ", new RefColumn("PHYSICALPERSON_DOCTYPE_ID", ColumnProperty.None, "GJI_CH_RESOLUTION_FIZ_PHYSICALPERSON_DOCTYPE_ID", "GJI_DICT_PHYSICAL_PERSON_DOC_TYPE", "ID"));
            CopyFLDocTypeValues();
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_RESOLUTION_FIZ", "PHYSICALPERSON_DOCTYPE_ID");   
            Database.RemoveColumn("GJI_CH_GIS_GMP", "PHYSICALPERSON_DOCTYPE_ID");
        }

        private void CopyFLDocTypeToPhysicalPersonDocType()
        {
            var sql = @"insert into GJI_DICT_PHYSICAL_PERSON_DOC_TYPE
                        select * from GJI_CH_DICT_FLDOC_TYPE;";

            this.Database.ExecuteNonQuery(sql);
        }

        private void CopyFLDocTypeValues()
        {
            var sql = @"update GJI_CH_RESOLUTION_FIZ set PHYSICALPERSON_DOCTYPE_ID = FLDOCTYPE_ID;
                        update GJI_CH_GIS_GMP set PHYSICALPERSON_DOCTYPE_ID = FLDOCTYPE_ID;";

            this.Database.ExecuteNonQuery(sql);
        }
    }
}