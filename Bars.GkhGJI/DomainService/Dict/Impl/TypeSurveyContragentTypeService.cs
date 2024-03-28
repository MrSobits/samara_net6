namespace Bars.GkhGji.DomainService.Dict.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    using Castle.Windsor;

	/// <summary>
	/// Сервис для Типы контрагента
	/// </summary>
	public class TypeSurveyContragentTypeService : ITypeSurveyContragentTypeService
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Добавить типы контрагента
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public IDataResult AddContragentTypes(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDomainService<TypeSurveyContragentType>>();

            try
            {
                var typeSurveyId = baseParams.Params.GetAs("typeSurveyId", 0L);
                var contragentTypes = baseParams.Params.GetAs<string>("contragentTypes");

                if (typeSurveyId == 0 || string.IsNullOrEmpty(contragentTypes))
                {
                    return new BaseDataResult { Success = false, Message = "Не удалось получить тип обследования и/или правовые основания" };
                }

                var existRecords = service.GetAll().Where(x => x.TypeSurveyGji.Id == typeSurveyId).Select(x => x.TypeJurPerson).ToList();

                foreach (var contragentType in contragentTypes.Split(',').Select(x => x.Trim().To<TypeJurPerson>()))
                {
                    if (!existRecords.Contains(contragentType))
                    {
                        var newRec = new TypeSurveyContragentType { TypeJurPerson = contragentType, TypeSurveyGji = new TypeSurveyGji { Id = typeSurveyId } };

                        service.Save(newRec);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}