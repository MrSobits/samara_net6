namespace Bars.Gkh.Regions.Tatarstan.Navigation
{
    using Bars.B4;

    public class ConstructionObjMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "ConstructionObj";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки объекта строительства";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Паспорт объекта", "constructionobjectedit/{0}/edit/").AddRequiredPermission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.View").WithIcon("icon-outline");
            root.Add("Документация", "constructionobjectedit/{0}/documents/").AddRequiredPermission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.View").WithIcon("icon-page-white-edit");
            root.Add("Фото-архив", "constructionobjectedit/{0}/photos/").AddRequiredPermission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.View").WithIcon("icon-photos");
            root.Add("Виды работ", "constructionobjectedit/{0}/typework/").AddRequiredPermission("Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.View").WithIcon("icon-wrench-orange");
            root.Add("Участники строительства", "constructionobjectedit/{0}/participants/").AddRequiredPermission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.View").WithIcon("icon-outline");
            root.Add("Договоры", "constructionobjectedit/{0}/contracts/").AddRequiredPermission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.View").WithIcon("icon-outline");

            var smr = root.Add("Мониторинг СМР", "constructionobjectedit/{0}/smr/").AddRequiredPermission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.View").WithIcon("icon-chart-bar");
            smr.Add("График выполнения работ", "constructionobjectedit/{0}/smr/schedule/").AddRequiredPermission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.View").WithIcon("icon -text-list-numbers");
            smr.Add("Ход выполнения работ", "constructionobjectedit/{0}/smr/progress/").AddRequiredPermission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.View").WithIcon("icon-chart-curve");
            smr.Add("Численность рабочих", "constructionobjectedit/{0}/smr/workers/").AddRequiredPermission("Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Workers.View").WithIcon("icon-text-list-numbers");
        }
    }
}
