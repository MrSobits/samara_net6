namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;

	/// <summary>
	/// Интерцептор для Приказ
	/// </summary>
    // Тут намеренное наследование! Необходимо для гарантирования, что в DocumentGjiChildren запись была уже создана родительским Interceptor
    public class TomskDisposalServiceInterceptor : EmptyDomainInterceptor<Disposal>
    {
		/// <summary>
		/// Домен сервис для Таблица связи документов (Какой документ из какого был сформирован)
		/// </summary>
		public IDomainService<DocumentGjiChildren> DocumentGjiChildrenService { get; set; }

		/// <summary>
		/// Домен сервис для Инспекторы документа ГЖИ
		/// </summary>
		public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomainService { get; set; }

		/// <summary>
		/// Домен сервис для Первичное обращение проверки
		/// </summary>
		public IDomainService<PrimaryBaseStatementAppealCits> PrimaryBaseStatementAppealCitsDomainService { get; set; }

		/// <summary>
		/// Выполнить действие перед удалением
		/// </summary>
		/// <param name="service">Домен сервис</param>
		/// <param name="entity">Сущность</param>
		/// <returns>Результат выполнения действия</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<Disposal> service, Disposal entity)
        {
            var refFuncs = new List<Func<long, string>>
            {
                id => Container.Resolve<IDomainService<Requirement>>().GetAll().Any(x => x.Document.Id == id) 
                        ? "Требования"
                        : null
            };

            var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

            if (refs.Length > 0)
            {
                return Failure(string.Format("Существуют связанные записи в следующих таблицах: {0}", refs.AggregateWithSeparator("; ")));
            }

            var provDocDateService = Container.Resolve<IDomainService<DisposalProvidedDocNum>>();

            provDocDateService.GetAll()
                .Where(x => x.Disposal.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => provDocDateService.Delete(x));

			var dispVerifSubjLicensingService = Container.Resolve<IDomainService<DisposalVerificationSubjectLicensing>>();

			dispVerifSubjLicensingService.GetAll()
				.Where(x => x.Disposal.Id == entity.Id)
				.Select(x => x.Id)
				.ForEach(x => dispVerifSubjLicensingService.Delete(x));

			return Success();
        }

		/// <summary>
		/// Выполнить действие после создания
		/// </summary>
		/// <param name="service">Домен сервис</param>
		/// <param name="entity">Сущность</param>
		/// <returns>Результат выполнения действия</returns>
		public override IDataResult AfterCreateAction(IDomainService<Disposal> service, Disposal entity)
        {
            var primaryAppealCitsData = PrimaryBaseStatementAppealCitsDomainService.GetAll()
                .Where(x => x.BaseStatementAppealCits.Inspection.Id == entity.Inspection.Id)
                .Select(x => new
            {
                                     x.BaseStatementAppealCits.AppealCits.Executant,
                                     x.BaseStatementAppealCits.AppealCits.Surety,
                                     x.ObjectCreateDate
                                 })
                .OrderByDescending(x => x.ObjectCreateDate)
                .FirstOrDefault();

            if (primaryAppealCitsData != null)
                {
                if (entity.TypeDisposal == TypeDisposalGji.DocumentGji)
                        {
                    if (primaryAppealCitsData.Surety != null)
                                {
                        entity.IssuedDisposal = primaryAppealCitsData.Surety;
                    }

                    if (primaryAppealCitsData.Executant != null)
                    {
                        entity.ResponsibleExecution = primaryAppealCitsData.Executant;
                    }
                }
            }

            return Success();
        }
    }
}