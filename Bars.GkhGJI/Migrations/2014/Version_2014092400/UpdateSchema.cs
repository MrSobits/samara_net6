namespace Bars.GkhGji.Migration.Version_2014092400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migration.Version_2014091700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {

            // Была обнарушена ошибка после которой в базе оставались висеть ненужные записи нарушений
            // Делаю скрипт который удалит записи нарушений првоерки у которых нет Этапа нарушения в каком либо документе Приказ, Предписание, Протокол, Акт устранения нарушений
            var exist = Database.TableExists("GJI_INSPECTION_VIOLATION");
            if (exist)
            {
                var existWord = Database.TableExists("gji_nnov_insp_viol_word");
                if (existWord)
                {
                    // сначала удаляем формулировку нарушения
                    Database.ExecuteNonQuery(@"delete from gji_nnov_insp_viol_word where INSPECTION_VIOL_ID not in (
                                            select INSPECTION_VIOL_ID from GJI_INSPECTION_VIOL_STAGE
                                        )");
                }
                // затем нарушение проверки
                Database.ExecuteNonQuery(@"delete from GJI_INSPECTION_VIOLATION where ID not in (
                                            select INSPECTION_VIOL_ID from GJI_INSPECTION_VIOL_STAGE
                                        )");
            }
        }

        public override void Down()
        {
            
        }
    }
}