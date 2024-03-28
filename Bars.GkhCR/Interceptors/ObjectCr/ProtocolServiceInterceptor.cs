namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Localizers;
    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Entities.Dicts.Multipurpose;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Обработка документов сущности "Протокол, акт"
    /// </summary>
    public class ProtocolServiceInterceptor : EmptyDomainInterceptor<ProtocolCr>
    {
        /// <summary>
        /// Действие, выполняемое до добавления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Протокол"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<ProtocolCr> service, ProtocolCr entity)
        {
            var documents =
                service.GetAll()
                    .Where(x => x.ObjectCr.Id == entity.ObjectCr.Id)
                    .Select(x => new DocumentProxy
                    {
                        Id = x.Id,
                        TypeDocumentCr = x.TypeDocumentCr,
                        DateFrom = x.DateFrom

                    })
                    .ToList();
            
            // Документы с заданным типом по данному объекту КР
            var protocolNeedCrList = documents.Where(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolNeedCrKey).ToList();
            var actExpluatatinAfterCrList = documents.Where(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey).ToList();
            var actAuditDataExpenseList = documents.Where(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ActAuditDataExpenseKey).ToList();
            
            var protocolCompleteCrList = documents.Where(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolCompleteCrKey).ToList();
            var protocolChangeCrList = documents.Where(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolChangeCrKey).ToList();
            var dateProtocolCompleteCr = documents.Where(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolCompleteCrKey && x.DateFrom.HasValue).Select(x => x.DateFrom.Value).FirstOrDefault();

            var useDocumentAddingOrder = Container.GetGkhConfig<GkhCrConfig>().General.DocumentAddingOrder == DocumentAddingOrder.Use;

            if (entity.TypeDocumentCr == null)
	        {
				return Failure("Не заполнено поле 'Тип документа'");
	        }

            if (useDocumentAddingOrder)
            {
                // Провереям есть ли у данного объекта уже документ с типом "Протокол о завершении капремонта"
                switch (entity.TypeDocumentCr.Key)
                {
                    case TypeDocumentCrLocalizer.ProtocolCompleteCrKey:
                        if (protocolCompleteCrList.Any())
                        {
                            return this.Failure("У данного объекта капремонта уже существует 'Протокол о завершении капремонта' ");
                        }

                        if (!protocolNeedCrList.Any())
                        {
                            return this.Failure("'Протокол о завершении капремонта' можно добавить если у данного объекта есть документы с типом 'Протокол о необходимости проведения капремонта' ");
                        }

                        break;
                    case TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey:
                        if (!protocolCompleteCrList.Any())
                        {
                            return this.Failure("'Акт ввода в эксплуатацию после капремонта' можно добавить если у данного объекта есть документы с типом 'Протокол о завершении капремонта' ");
                        }

                        break;
                    case TypeDocumentCrLocalizer.ActAuditDataExpenseKey:
                        if (!protocolCompleteCrList.Any())
                        {
                            return this.Failure("'Акт сверки данных о расходах' можно добавить если у данного объекта есть документы с типом 'Протокол о завершении капремонта' ");
                        }

                        break;
                    case TypeDocumentCrLocalizer.ProtocolChangeCrKey:
                        if (!protocolNeedCrList.Any())
                        {
                            return this.Failure("'Протокол о внесении изменений в капремонт' можно добавить если у данного объекта есть документы с типом 'Протокол о необходимости проведения капремонта' ");
                        }

                        break;
                }
            }

            // Проверяем можно ли добавить "Протокол о необх-ти проведения капремонта"
            if (entity.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolNeedCrKey)
            {
                if (protocolNeedCrList.Any(x => (x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolChangeCrKey
                                                || x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolCompleteCrKey
                                                || x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey)))
                {
                    return Failure("'Протокол о необходимости проведения капремонта' можно добавить если у данного объекта нет документов с типами: 'Протокол о внесении изменений в капремонт, Протокол о завершении капремонта, Акт ввода в эксплуатацию после капремонта' ");
                }
            }

            //Проверка дат
            if (useDocumentAddingOrder)
            {
                var res = CheckDates(entity, protocolNeedCrList, protocolCompleteCrList, actExpluatatinAfterCrList, protocolChangeCrList, dateProtocolCompleteCr);
                if (!res.Success)
                {
                    return res;
                }
            }
            
            return Success();
        }

        

        /// <summary>
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Протокол"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<ProtocolCr> service, ProtocolCr entity)
        {
            var documents = service.GetAll()
                .Where(x => x.ObjectCr.Id == entity.ObjectCr.Id)
                .Select(x => new DocumentProxy
                {
                    Id = x.Id,
                    TypeDocumentCr = x.TypeDocumentCr,
                    DateFrom = x.DateFrom

                }).ToList();

            // Документы с заданным типом по данному объекту КР
            var protocolNeedCrList = documents.Where(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolNeedCrKey).ToList();
            var actExpluatatinAfterCrList = documents.Where(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey).ToList();
            var actAuditDataExpenseList = documents.Where(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ActAuditDataExpenseKey).ToList();
            
            var protocolCompleteCrList = documents.Where(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolCompleteCrKey).ToList();
            var protocolChangeCrList = documents.Where(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolChangeCrKey).ToList();
            var dateProtocolCompleteCr = documents.Where(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolCompleteCrKey && x.DateFrom.HasValue).Select(x => x.DateFrom.Value).FirstOrDefault();

			if (entity.TypeDocumentCr == null)
			{
				return Failure("Не заполнено поле 'Тип документа'");
			}

            var useDocumentAddingOrder = Container.GetGkhConfig<GkhCrConfig>().General.DocumentAddingOrder == DocumentAddingOrder.Use;

            switch (entity.TypeDocumentCr.Key)
            {
                case TypeDocumentCrLocalizer.ProtocolCompleteCrKey:
                    if (protocolCompleteCrList.Any(x => x.Id != entity.Id))
                    {
                        return this.Failure("Добавление еще одного протокола с типом \"Протокол о завершении капремонта\" запрещено ");
                    }

                    if (!protocolNeedCrList.Any())
                    {
                        return this.Failure("'Протокол о завершении капремонта' можно добавить если у данного объекта есть документы с типом 'Протокол о необходимости проведения капремонта' ");
                    }

                    break;
                case TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey:
                    if (!protocolCompleteCrList.Any())
                    {
                        return this.Failure("'Акт ввода в эксплуатацию после капремонта' можно добавить если у данного объекта есть документы с типом 'Протокол о завершении капремонта' ");
                    }

                    break;
                case TypeDocumentCrLocalizer.ActAuditDataExpenseKey:
                    if (!protocolCompleteCrList.Any())
                    {
                        return this.Failure("'Акт сверки данных о расходах' можно добавить если у данного объекта есть документы с типом 'Протокол о завершении капремонта' ");
                    }

                    break;
                case TypeDocumentCrLocalizer.ProtocolChangeCrKey:
                    if (!protocolNeedCrList.Any())
                    {
                        return this.Failure("'Протокол о внесении изменений в капремонт' можно добавить если у данного объекта есть документы с типом 'Протокол о необходимости проведения капремонта' ");
                    }

                    break;
                case TypeDocumentCrLocalizer.ProtocolNeedCrKey:
                    if (documents.Any(x => (x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolChangeCrKey
                                                    || x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolCompleteCrKey
													|| x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey)))
                    {
                        return this.Failure("'Протокол о необходимости проведения капремонта' можно добавить если у данного объекта нет документов с типами: 'Протокол о внесении изменений в капремонт, Протокол о завершении капремонта, Акт ввода в эксплуатацию после капремонта' ");
                    }

                    break;
            }

            //Проверка дат
            if (useDocumentAddingOrder)
            {
                var res = CheckDates(entity, protocolNeedCrList, protocolCompleteCrList, actExpluatatinAfterCrList, protocolChangeCrList, dateProtocolCompleteCr);
                if (!res.Success)
                {
                    return res;
                }
            }

            return Success();
        }

        /// <summary>
        /// Действие, выполняемое до удаления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Протокол"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<ProtocolCr> service, ProtocolCr entity)
        {
            var documents = service.GetAll().Where(x => x.ObjectCr.Id == entity.ObjectCr.Id).Select(x => new { x.Id, x.TypeDocumentCr }).ToList();
            var useDocumentAddingOrder = Container.GetGkhConfig<GkhCrConfig>().General.DocumentAddingOrder == DocumentAddingOrder.Use;
            if (useDocumentAddingOrder && entity.TypeDocumentCr != null)
			{
				switch (entity.TypeDocumentCr.Key)
				{
					case TypeDocumentCrLocalizer.ProtocolNeedCrKey:

						if (documents.Any(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolCompleteCrKey))
						{
							return Failure("Нельзя удалить документ \"Протокол о необходимости проведения\", так как существуют зависимые от него документы: \"Протокол о завершении капремонта\"");
						}

						if (documents.Any(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolChangeCrKey))
						{
							return Failure("Нельзя удалить документ \"Протокол о необходимости проведения\", так как существуют зависимые от него документы: \"Протокол о внесение изменений в КР\"");
						}

						break;
					case TypeDocumentCrLocalizer.ProtocolCompleteCrKey:
						if (documents.Any(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey))
						{
							return Failure("Нельзя удалить документ \"Протокол о завершении капремонта\", так как существуют зависимые от него документы: \"Акт ввода в эксплуатацию дома после капремонта\"");
						}

                        if (documents.Any(x => x.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ActAuditDataExpenseKey))
                        {
                            return Failure("Нельзя удалить документ \"Протокол о необходимости проведения\", так как существуют зависимые от него документы: \"Акт сверки данных о расходах\"");
                        }

                        break;
				}
			}

            return Success();
        }

        private IDataResult CheckDates(ProtocolCr entity, 
            List<DocumentProxy> protocolNeedCrList, 
            List<DocumentProxy> protocolCompleteCrList,
            List<DocumentProxy> actExpluatatinAfterCrList, 
            List<DocumentProxy> protocolChangeCrList,
            DateTime dateProtocolCompleteCr)
        {
            // Проверяем дату Протокол о завершении капремонта >= Протокол о необходимости проведения капремонта(если много то самую позднюю берем)
            if (entity.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolCompleteCrKey && protocolNeedCrList.Count > 0)
            {
                if (!protocolNeedCrList.All(x => x.DateFrom.GetValueOrDefault() <= entity.DateFrom.GetValueOrDefault()))
                {
                    return Failure("Дата протокола о завершении капремонта должна быть больше или равна чем любая из дат протокола о необходимости проведения капремонта");
                }
            }

            if (entity.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolNeedCrKey && protocolCompleteCrList.Count > 0)
            {
                if (!protocolCompleteCrList.All(x => x.DateFrom.GetValueOrDefault() > entity.DateFrom.GetValueOrDefault()))
                {
                    return Failure("Дата протокола о завершении капремонта должна быть больше или равна чем любая из дат протокола о необходимости проведения капремонта");
                }
            }

            // Дата "Акт ввода в эксплуатацию после капремонта" >= Дата "Протокол о завершении капремонта".
            if (entity.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey)
            {
                if (entity.DateFrom.GetValueOrDefault() < dateProtocolCompleteCr)
                {
                    return Failure("Дата акта ввода в эксплуатацию после капремонта должна быть больше или равна чем дата протокола о завершении капремонта");
                }
            }

            // Дата "Протокол о внесении изменений в КР" >= Дата "Протокол о необходимости проведения КР".+
            if (entity.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolCompleteCrKey && actExpluatatinAfterCrList.Any())
            {
                if (!actExpluatatinAfterCrList.All(x => x.DateFrom.GetValueOrDefault() >= entity.DateFrom.GetValueOrDefault()))
                {
                    return Failure("Дата акта ввода в эксплуатацию после капремонта должна быть больше или равна чем дата протокола о завершении капремонта");
                }
            }

            // Дата "Протокол о внесении изменений в капремонт" >= Дата "Протокол о необходимости проведения капремонта".
            if (entity.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolChangeCrKey && protocolNeedCrList.Count > 0)
            {
                if (!protocolNeedCrList.All(x => x.DateFrom.GetValueOrDefault() <= entity.DateFrom.GetValueOrDefault()))
                {
                    return Failure("Дата должна быть больше или равна дате протокола о необходимости проведения капремонта");
                }
            }

            if (entity.TypeDocumentCr.Key == TypeDocumentCrLocalizer.ProtocolNeedCrKey && protocolChangeCrList.Count > 0)
            {
                if (!protocolChangeCrList.All(x => x.DateFrom.GetValueOrDefault() > entity.DateFrom.GetValueOrDefault()))
                {
                    return Failure("Дата протокола о внесении изменений в капремонт должна быть больше или равна чем любая из дат протокола о необходимости проведения капремонта");
                }
            }
            return Success();
        }

        /// <summary>
        /// Класс-прокси "Документ"
        /// </summary>
        private class DocumentProxy
        {
            /// <summary>
            /// Идентификатор
            /// </summary>
            public long Id { get; set; }
            /// <summary>
            /// Тип документа - "Наполнитель универсального справочника"
            /// </summary>
            public MultipurposeGlossaryItem TypeDocumentCr { get; set; }
            /// <summary>
            /// Дата от
            /// </summary>
            public DateTime? DateFrom { get; set; }
        }
    }
}
