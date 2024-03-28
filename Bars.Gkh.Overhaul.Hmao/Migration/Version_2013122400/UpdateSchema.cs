namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013122400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013122200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            /*Вообщем сначала была создана таблица и ссылка была создана без FK И IND
             * возможно что придется залезть в pgAdmin и выполнить нижний скрипт если потребуется
             * иначе может быт ьтакое что какойто Дом удалили а Id Его уже был сохранен в таблице OVRHL_LONGTERM_PR_OBJECT
             * досихпор осталасю посольку небыло FK дом удалился без проблем
             */

            Database.ExecuteNonQuery(@"
delete from OVRHL_LONGTERM_PR_OBJECT where id in( 
select t.id from ( 
select t.id, ro.id roId from OVRHL_LONGTERM_PR_OBJECT t 
LEFT JOIN GKH_REALITY_OBJECT ro on ro.id = t.REALITY_OBJ_ID  
) t where t.roId is null 
)");

            //Database.AddIndex("IND_OVRHL_LONGTERMOBJ_RO", false, "OVRHL_LONGTERM_PR_OBJECT", "REALITY_OBJ_ID");
            Database.AddForeignKey("FK_OVRHL_LONGTERMOBJ_RO", "OVRHL_LONGTERM_PR_OBJECT", "REALITY_OBJ_ID", "GKH_REALITY_OBJECT", "ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("OVRHL_LONGTERM_PR_OBJECT", "FK_OVRHL_LONGTERMOBJ_RO");
            Database.RemoveIndex("IND_OVRHL_LONGTERMOBJ_RO", "OVRHL_LONGTERM_PR_OBJECT");
        }
    }
}