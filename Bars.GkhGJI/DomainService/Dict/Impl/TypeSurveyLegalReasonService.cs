namespace Bars.GkhGji.DomainService.Dict.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    using Castle.Windsor;

	/// <summary>
	/// Сервис для Правовые основания
	/// </summary>
	public class TypeSurveyLegalReasonService : ITypeSurveyLegalReasonService
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Добавить правовые основания
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public IDataResult AddLegalReasons(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDomainService<TypeSurveyLegalReason>>();

            try
            {
                var typeSurveyId = baseParams.Params.GetAs("typeSurveyId", 0L);
                var objectIds = baseParams.Params.GetAs<string>("objectIds");

                if (typeSurveyId == 0 || string.IsNullOrEmpty(objectIds))
                {
                    return new BaseDataResult { Success = false, Message = "Не удалось получить тип обследования и/или правовые основания" };
                }

                var existRecords = service.GetAll().Where(x => x.TypeSurveyGji.Id == typeSurveyId).Select(x => x.Id).ToList();

                foreach (var id in objectIds.Split(',').Select(x => x.Trim().ToLong()))
                {
                    if (!existRecords.Contains(id))
                    {
                        var newRec = new TypeSurveyLegalReason { LegalReason = new LegalReason { Id = id }, TypeSurveyGji = new TypeSurveyGji { Id = typeSurveyId } };

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