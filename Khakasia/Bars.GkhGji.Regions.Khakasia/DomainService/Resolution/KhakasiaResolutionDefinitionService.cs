namespace Bars.GkhGji.Regions.Khakasia.DomainService
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Enums;

	/// <summary>
	/// Сервис для Определение постановления
	/// </summary>
    public class KhakasiaResolutionDefinitionService : Bars.GkhGji.DomainService.ResolutionDefinitionService
    {
		/// <summary>
		/// Получить список типов определений
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public override IDataResult ListTypeDefinition(BaseParams baseParams)
        {
            /*
             Поскольку в базовый енум добавляется куча типов котоыре не вовсех регионах нужны
             то тогда в этом серверном методе возвращаем типы котоыре нужны только для этого региона
            */

            var list = new List<TypeResolutionDefinitionProxy>();
            foreach (var type in this.DefinitionTypes())
            {
                var display = type.GetEnumMeta().Display;

                if (type == TypeDefinitionResolution.Deferment)
                {
                    // нужно чтобы существующий тип назывался иначе в Хакасии 
                    display = "Об отсрочке исполнения постановления";
                }

                list.Add(new TypeResolutionDefinitionProxy
                {
                    Id = (int)type,
                    Display = display,
                    Name = type.ToString()
                });
            }

            var total = list.Count;

            return new ListDataResult(list, total);
        }

		/// <summary>
		/// Получить список типов определений
		/// </summary>
		/// <returns></returns>
        public override TypeDefinitionResolution[] DefinitionTypes()
        {
            // Вообщем по умолчанию регистрируются только такие типы 
            // в слуаче если в регионе нобходимы другие, то тогда заменяем реализацию
            return new []
                {
                    TypeDefinitionResolution.Deferment,
                    TypeDefinitionResolution.Installment,
                    TypeDefinitionResolution.CorrectionError,
                    TypeDefinitionResolution.AppointmentPlaceTime
                };
        }
    }
}