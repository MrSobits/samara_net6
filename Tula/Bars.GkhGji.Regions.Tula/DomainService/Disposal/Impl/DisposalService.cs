namespace Bars.GkhGji.Regions.Tula.DomainService.Disposal.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

	/// <summary>
	/// Сервис для Приказ
	/// </summary>
	public class DisposalService : GkhGji.DomainService.DisposalService
    {
		/// <summary>
		/// Домен сервис для Приказ
		/// </summary>
		public IDomainService<Disposal> DisposalDomain { get; set; }

		/// <summary>
		/// Домен сервис для Тип обследования приказа
		/// </summary>
		public IDomainService<DisposalTypeSurvey> DisposalTypeSurveyDomain { get; set; }

		/// <summary>
		/// Домен сервис для Тип контрагента типа обследования
		/// </summary>
		public IDomainService<TypeSurveyContragentType> TypeSurveyContragentTypeDomain { get; set; }

		/// <summary>
		/// Домен сервис для Предоставляемые документы приказа
		/// </summary>
		public IDomainService<DisposalProvidedDoc> DisposalProvidedDocDomain { get; set; }

		/// <summary>
		/// Домен сервис для Предоставляемый документ Типа обследования
		/// </summary>
		public IDomainService<TypeSurveyProvidedDocumentGji> TypeSurveyProvidedDocumentGjiDomain { get; set; }

		/// <summary>
		/// Получить информацию о гражданине
		/// </summary>
		/// <param name="baseName">Наименование основания</param>
		/// <param name="planName">Наименование плана</param>
		/// <param name="inspectionId">Идентификатор инспекции</param>
		protected override void GetInfoCitizenStatement(ref string baseName, ref string planName, long inspectionId)
        {
            // распоряжение создано на основе обращения граждан,
            // поле planName = "Обращение № Номер обращения"
            baseName = "Обращение граждан";

            // Получаем из основания наименование плана
            var baseStatement = string.Join(
                ", ",
                Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                    .Where(x => x.Inspection.Id == inspectionId)
                    .Select(x => x.AppealCits.NumberGji));

            if (!string.IsNullOrWhiteSpace(baseStatement))
            {
                planName = string.Format("Обращение № ({0})", baseStatement);
            }
        }

		/// <summary>
		/// Автоматически добавить предоставляемые документы
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public override IDataResult AutoAddProvidedDocuments(BaseParams baseParams)
        {
            try
            {
                var disposalId = baseParams.Params.GetAsId("disposalId");

                // получаю id уже добавленных документов для этого приказа, чтобы исключить их добавление
                var alreadyAddedDocIds =
                    DisposalProvidedDocDomain.GetAll()
                        .Where(x => x.Disposal.Id == disposalId)
                        .Select(x => x.ProvidedDoc.Id)
                        .ToArray();

                //// получаю тип Юр. лица из основания проверки
                //var typeJurPerson = DisposalDomain.Load(disposalId).Inspection.TypeJurPerson;

                //// получаю id типов обследования из справочника с таким же типом Юр. лица
                //var jurTypeSurveyIds =
                //    TypeSurveyContragentTypeDomain.GetAll()
                //        .Where(x => x.TypeJurPerson == typeJurPerson)
                //        .Select(x => x.TypeSurveyGji.Id)
                //        .ToArray();

                // получаю id типов обследования данного приказа
                var dispTypeSurveyIds =
                    DisposalTypeSurveyDomain.GetAll()
                        .Where(x => x.Disposal.Id == disposalId)
                        .Select(x => x.TypeSurvey.Id)
                        .ToArray();

                // получаю id документов из справочника типы обследований согласно условий
                var provDocIds =
                    TypeSurveyProvidedDocumentGjiDomain.GetAll()
                        .Where(x => dispTypeSurveyIds.Contains(x.TypeSurvey.Id))
                        .Where(x => !alreadyAddedDocIds.Contains(x.ProvidedDocGji.Id))
                        .Select(x => x.ProvidedDocGji.Id)
                        .ToArray();

                // добавляю их к приказу
                foreach (var provDocId in provDocIds)
                {
                    var dispProvDoc = new DisposalProvidedDoc
                    {
                        Disposal = new Disposal { Id = disposalId },
                        ProvidedDoc = new ProvidedDocGji { Id = provDocId }
                    };

                    DisposalProvidedDocDomain.Save(dispProvDoc);
                }

                return new BaseDataResult { Success = true };
            }
            catch (Exception e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }
    }
}