namespace Bars.Gkh.Regions.Tatarstan.Navigation
{
	using B4;

	/// <summary>
	/// Провайдер меню для карточки аварийного дома
	/// </summary>
	public class EmergencyObjMenuProvider : INavigationProvider
    {
		/// <summary>
		/// Ключ
		/// </summary>
        public string Key
        {
            get
            {
                return "EmergencyObj";
            }
        }

		/// <summary>
		/// Описание
		/// </summary>
        public string Description
        {
            get
            {
                return "Меню карточки аварийного дома";
            }
        }

		/// <summary>
		/// Проинициализировать меню
		/// </summary>
		/// <param name="root">Корневой элемент меню</param>
        public void Init(MenuItem root)
        {
            root.Add("Сведения о собственнике", "B4.controller.emergencyobj.InterlocutorInformation").AddRequiredPermission("Gkh.EmergencyObject.InterlocutorInformation.View");
            root.Add("Документы", "B4.controller.emergencyobj.Documents").AddRequiredPermission("Gkh.EmergencyObject.Register.Documents.View");
        }
    }
}