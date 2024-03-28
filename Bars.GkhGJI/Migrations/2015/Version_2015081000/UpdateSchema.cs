namespace Bars.GkhGji.Migrations._2015.Version_2015081000
{
    using System.Data;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GkhGji.Entities;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015081000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015080500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        private void AfterUp()
        {
            var container = ApplicationContext.Current.Container;

            {
                var domain = container.ResolveDomain<SurveyPurpose>();
                try
                {
                    domain.Save(
                        new SurveyPurpose
                            {
                                Code = "10",
                                Name = "осуществления регионального государственного жилищного надзора"
                            });
                    domain.Save(new SurveyPurpose { Code = "20", Name = "осуществления лицензионного контроля" });
                }
                finally
                {
                    container.Release(domain);
                }
            }

            {
                var domain = container.ResolveDomain<SurveyObjective>();
                try
                {
                    domain.Save(
                        new SurveyObjective
                            {
                                Code = "10",
                                Name =
                                    "предупреждение, выявление и пресечение нарушений обязательных требований"
                            });
                    domain.Save(
                        new SurveyObjective
                            {
                                Code = "20",
                                Name =
                                    "предупреждение, выявление и пресечение нарушений лицензионных требований"
                            });
                }
                finally
                {
                    container.Release(domain);
                }
            }
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_SURVEY_OBJ");
            Database.RemoveTable("GJI_DICT_SURVEY_PURP");
        }

        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_DICT_SURVEY_PURP",
                new Column("CODE", DbType.String, 300),
                new Column("NAME", DbType.String, 500));

            Database.AddEntityTable(
                "GJI_DICT_SURVEY_OBJ",
                new Column("CODE", DbType.String, 300),
                new Column("NAME", DbType.String, 500));

            Database.Commit();

            Database.BeginTransaction();
            this.AfterUp();
        }
    }
}