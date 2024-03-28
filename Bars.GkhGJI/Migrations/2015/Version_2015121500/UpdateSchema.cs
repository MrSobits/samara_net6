namespace Bars.GkhGji.Migrations.Version_2015121500
{
    using System;
    using System.Data;
    using B4.Application;
    using B4.DataAccess;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Entities;
    using Entities.Dict;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2015121500
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015121500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2015111300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable("GJI_DICT_SURVEY_SUBJ_LICENSING",
                new Column("NAME", DbType.String),
                new Column("CODE", DbType.String));

            Database.ExecuteNonQuery("insert into GJI_DICT_SURVEY_SUBJ_LICENSING(object_version, object_create_date, object_edit_date,CODE, NAME) values " +
                                     "(0, now(), now(), '10', 'Cоблюдение лицензионных требований при осуществлении предпринимательской деятельности по управлению многоквартирными домами'), " +
                                     "(0, now(), now(), '20', 'Cоблюдение соискателем лицензии лицензионных требований')");

        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_SURVEY_SUBJ_LICENSING");
        }
    }
}