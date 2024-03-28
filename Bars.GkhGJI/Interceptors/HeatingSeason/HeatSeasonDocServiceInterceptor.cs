using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    // Пустышка на тот случай если от этого класа наследовались в регионах
    public class HeatSeasonDocServiceInterceptor : HeatSeasonDocServiceInterceptor<HeatSeasonDoc>
    {
        // Внимание методы добавлят ьи переопределять толко у Generic 
    }

    // Generic Класс дял лучшей расширяемости в региноах, чтобы избежать дублирваония методов при расширении сущностей через subclass
    public class HeatSeasonDocServiceInterceptor<T> : EmptyDomainInterceptor<T>
        where T : HeatSeasonDoc
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            var result = CheckDocuments(service, entity);

            if (!result.Success)
                return result;

            var stateProvider = Container.Resolve<IStateProvider>();

            try
            {
                // Перед сохранением присваиваем начальный статус

                stateProvider.SetDefaultState(entity);

                return Success();
            }
            finally 
            {
                Container.Release(stateProvider);
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            return CheckDocuments(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var domainDocument = Container.Resolve<IDomainService<HeatSeasonDoc>>();

            try
            {
                // Удалить документ можно только если несуществует документа с типом Паспорт готовности
                if (entity.TypeDocument != HeatSeasonDocType.Passport)
                {
                    if (domainDocument.GetAll().Count(x => x.HeatingSeason.Id == entity.HeatingSeason.Id && x.Id != entity.Id && x.TypeDocument == HeatSeasonDocType.Passport) > 0)
                    {
                        return Failure("Поскольку паспорт готовности уже существует нельзя удалить документ");
                    }
                }

                return Success();
            }
            finally
            {
                Container.Release(domainDocument);
            }
        }

        /// <summary>
        /// Данные типы документов необходимы для того тчобы можно было сформирвоать документ пАспорт готовности
        /// </summary>
        public virtual List<HeatSeasonDocType> GetListTypesForCentralizedPasport()
        {
            return new List<HeatSeasonDocType>
                    {
                        HeatSeasonDocType.ActFlushingHeatingSystem,
                        HeatSeasonDocType.ActPressingHeatingSystem,
                        HeatSeasonDocType.ActCheckVentilation
                    };
        }

        public virtual IDataResult CheckDocuments(IDomainService<T> service, T entity)
        {
            // 1проверка для каждого отопительного сезона должен быть только один документ с каждым типом
            if (service.GetAll().Any(x => x.HeatingSeason.Id == entity.HeatingSeason.Id && x.Id != entity.Id && x.TypeDocument == entity.TypeDocument))
            {
                return Failure("Документ с таким типом уже существует");
            }

            // существующие типы документов
            var existDocsType =
                service.GetAll()
                    .Where(x => x.HeatingSeason.Id == entity.HeatingSeason.Id)
                    .Select(x => x.TypeDocument)
                    .ToList();

            // обязательные документы для централизованного типа отопления
            var mandatoryDocsTypeCentralized = GetListTypesForCentralizedPasport();

            // Если создают документ с типом Паспорт то он может быть создан только если существуют все документы
            if (entity.TypeDocument == HeatSeasonDocType.Passport)
            {
                var heatingSystem = entity.HeatingSeason.RealityObject.HeatingSystem;
                if (heatingSystem == HeatingSystem.Centralized && mandatoryDocsTypeCentralized.Any(x => !existDocsType.Contains(x)))
                {
                    var message = string.Empty;
                    message = mandatoryDocsTypeCentralized.Aggregate(message, (current, str) => current + string.Format("{0}; ", str.GetEnumMeta().Display));
                    return Failure(string.Format("Паспорт готовности можно сформировать только если существуют следующие типы документов: {0}", message));
                }

                if (heatingSystem == HeatingSystem.Individual
                    && (!existDocsType.Contains(HeatSeasonDocType.ActCheckVentilation)
                        && !existDocsType.Contains(HeatSeasonDocType.ActCheckChimney)))
                {
                    return Failure(
                            string.Format(
                                "Паспорт готовности можно сформировать только если существуют следующие типы документов: {0} либо {1}",
                                HeatSeasonDocType.ActCheckChimney.GetEnumMeta().Display,
                                HeatSeasonDocType.ActCheckVentilation.GetEnumMeta().Display));
                }
            }

            return Success();
        } 
    }
}