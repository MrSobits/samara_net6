namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
	using Bars.GkhGji.Regions.Tomsk.Entities.AppealCits;

    /// <summary>
    /// Используется для дополнительных процедур перед/после изменения объекта
    /// </summary>
    public class AppealCitsServiceInterceptor : GkhGji.Interceptors.AppealCitsServiceInterceptor<TomskAppealCits>
    {
        /// <summary>
        /// Провайдер для работы со статусами
        /// </summary>
        public IStateProvider StateProvider { get; set; }

        /// <summary>
        /// Сервис простановки номера документа
        /// </summary>
        public IAppealCitsNumberRule AppealCitsNumperRule { get; set; }

        /// <summary>
        /// Домен-сервис обращений
        /// </summary>
        public IDomainService<TomskAppealCits> AppealCitsDomain { get; set; }

        /// <summary>
        /// Домен-сервис документов ГЖИ
        /// </summary>
        public IDomainService<DocumentGji> DocumentDomain { get; set; }

        /// <summary>
        /// Домен-сервис ответов по обращению
        /// </summary>
        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomain { get; set; }

        /// <summary>
        /// Домен-сервис Исполнитель по обращению
        /// </summary>
		public IDomainService<AppealCitsExecutant> AppealCitsExecutantDomain { get; set; }

        /// <summary>
        /// Метод перед созданием экземпляра объекта. Проставляет начальный статус и дату от, год
        /// </summary>
        /// <param name="service">Домен-сервис обращений</param>
        /// <param name="entity">Обращение</param>
        /// <returns></returns>
        public override IDataResult BeforeCreateAction(IDomainService<TomskAppealCits> service, TomskAppealCits entity)
        {
            if (!entity.DateFrom.HasValue)
            {
                entity.DateFrom = DateTime.Now.Date;
            }

            if (entity.Year < 2)
            {
                entity.Year = entity.DateFrom.Value.Year;
            }

            // Перед сохранением присваиваем начальный статус 
            try
            {
                AppealCitsNumperRule.SetNumber(entity);
                StateProvider.SetDefaultState(entity);
            }
            finally
            {
                Container.Release(StateProvider);
            }

            return ChechForNumberUsage(entity);
        }

        /// <summary>
        /// Метод перед обновлением сущности. Проставляет значения Даты ответа обращения, дату от, год. Перед сохранением проверяет на используемый номер документа
        /// </summary>
        /// <param name="service">Домен-сервис обращения</param>
        /// <param name="entity">Обращение</param>
        /// <returns></returns>
		public override IDataResult BeforeUpdateAction(IDomainService<TomskAppealCits> service, TomskAppealCits entity)
        {
            // Последнею дату из Ответов обращения сохранить в Срок исполнения поручителя (Дата ответа)
            // Сформированным ответом считается ответ, у которого в форме редактирования обращения на вкладке "Ответы" заполненны столбцы "Документ", "Дата документа", "Номер документа"
            var answersLastDate = AppealCitsAnswerDomain.GetAll()
                .Where(x => x.AppealCits.Id == entity.Id)
                .Where(x => x.DocumentNumber != "" && x.DocumentNumber != null)
                .Where(x => x.DocumentName != "" && x.DocumentName != null)
                .Where(x => x.DocumentDate.HasValue)
                .Select(x => x.DocumentDate)
                .OrderByDescending(x => x)
                .FirstOrDefault();

            if (answersLastDate != null)
            {
                entity.SuretyDate = answersLastDate;
            }
            
            if (!entity.DateFrom.HasValue)
            {
                entity.DateFrom = DateTime.Now.Date;
            }

            if (entity.Year < 2)
            {
                entity.Year = entity.DateFrom.Value.Year;
            }

            AppealCitsNumperRule.SetNumber(entity);

            return ChechForNumberUsage(entity);
        }

		private IDataResult ChechForNumberUsage(TomskAppealCits entity)
        {
            var numberInUsageAtAppeal = AppealCitsDomain.GetAll()
                   .Where(x => x.Id != entity.Id)
                   .Any(x => x.Year == entity.Year && x.NumberGji == entity.NumberGji);

            var numberInUsageAtResolutionProsecutor = DocumentDomain.GetAll()
                .Where(x => x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                .Where(x => x.DocumentYear == entity.Year || x.DocumentDate.Value.Year == entity.Year)
                .Any(x => x.DocumentNumber == entity.NumberGji);

            if (numberInUsageAtAppeal || numberInUsageAtResolutionProsecutor)
            {
                return Failure("Указанный номер в этом году уже используется в другом документе.");
            }

            return this.Success();
        }

        /// <summary>
        /// Метод перед удалением экземпляра объекта. Удаляет связанные объекты
        /// </summary>
        /// <param name="service">Домен-сервис обращений</param>
        /// <param name="entity">Обращение</param>
        /// <returns></returns>
	    public override IDataResult BeforeDeleteAction(IDomainService<TomskAppealCits> service, TomskAppealCits entity)
	    {
		    var executantIds = AppealCitsExecutantDomain.GetAll()
			    .Where(x => x.AppealCits.Id == entity.Id)
			    .Select(x => x.Id)
			    .AsEnumerable();

		    foreach (var id in executantIds)
		    {
			    AppealCitsExecutantDomain.Delete(id);
		    }

		    return base.BeforeDeleteAction(service, entity);
	    }
    }
}