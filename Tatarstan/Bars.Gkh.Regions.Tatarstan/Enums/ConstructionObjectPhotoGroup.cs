namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Группа фото-архива объекта строительства
    /// </summary>
    public enum ConstructionObjectPhotoGroup
	{
		/// <summary>
		/// Не задано
		/// </summary>
		[Display("Не задано")]
		NotSet = 10,

		/// <summary>
		/// Фото участка застройки
		/// </summary>
		[Display("Фото участка застройки")]
        BuildingSite = 20,

		/// <summary>
		/// Фото хода строительства
		/// </summary>
		[Display("Фото хода строительства")]
		ConstructionProgress = 30,

		/// <summary>
		/// Фото после завершения строительства
		/// </summary>
		[Display("Фото после завершения строительства")]
        AfterCompletingConstruction = 40,

		/// <summary>
		/// Изображение жилого дома после сдачи
		/// </summary>
		[Display("Изображение жилого дома после сдачи")]
        RealityObjectAfterDelivery = 50
    }
}
