namespace Bars.GkhGji.Regions.Tomsk
{
    using Bars.B4;
    using Bars.Gkh.TextValues;

    class NavigationProvider : INavigationProvider
    {
        public IMenuItemText MenuItemText
        {
            get;
            set;
        }


        public string Key
        {
            get
            {
                return MainNavigationInfo.MenuName;
            }
        }

        public string Description
        {
            get
            {
                return MainNavigationInfo.MenuDescription;
            }
        }

        public void Init(MenuItem root)
        {
			var dicts = root.Add("Справочники").Add("ГЖИ");

			dicts.Add("Рамки проверки", "frameverificationGrid").AddRequiredPermission("GkhGji.Dict.FrameVerification.View");
			//пока потребвоалось убрать этот реестр
			//root.Add("Жилищная инспекция").Add("Документы").Add("Реестр административных дел", "admincase").AddRequiredPermission("GkhGji.DocumentsGji.AdminCase.View").WithIcon("resolPros");

			dicts.Add("Социальные положения", "socialstatus").AddRequiredPermission("GkhGji.Dict.SocialStatus.View");
	        dicts.Add("Предметы проверки ГЖИ", "surveysubject").AddRequiredPermission("GkhGji.Dict.SurveySubject.View");
	        dicts.Add("Предметы проверки Лицензирование", "surveysubjectlicensing").AddRequiredPermission("GkhGji.Dict.SurveySubject.View");
	        dicts.Add("Приложения к обращению за выдачей лицензии", "annextoappealforlicenseissuance").AddRequiredPermission("GkhGji.Dict.AnnexToAppealForLicenseIssuance.View");
        }
    }
}
