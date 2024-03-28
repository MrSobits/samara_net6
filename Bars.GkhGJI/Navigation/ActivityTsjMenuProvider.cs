namespace Bars.GkhGji.Navigation
{
    using Bars.B4;

    public class ActivityTsjMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "ActivityTsj";
            }
        }

        public string Description
        {
            get
            {
                return "Меню деятельности ТСЖ/ЖСК";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "B4.controller.activitytsj.Edit").WithIcon("icon-shield-rainbow");
            root.Add("Уставы", "B4.controller.activitytsj.Statute").WithIcon("icon-book");
            root.Add("Cписок домов", "B4.controller.activitytsj.RealObj").WithIcon("icon-house");
            root.Add("Протоколы", "B4.controller.activitytsj.Protocol").WithIcon("icon-script");
            root.Add("Документы", "B4.controller.activitytsj.Inspection").WithIcon("icon-paste-plain");
            root.Add("Реестр членов ТСЖ/ЖСК", "B4.controller.activitytsj.MemberTsj");
        }
    }
}