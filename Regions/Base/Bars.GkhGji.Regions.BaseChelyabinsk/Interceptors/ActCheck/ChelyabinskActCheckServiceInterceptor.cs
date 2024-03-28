
namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.ActCheck
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.DocumentGji;

    public class ChelyabinskActCheckServiceInterceptor : ActCheckServiceInterceptor<ChelyabinskActCheck>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ChelyabinskActCheck> service, ChelyabinskActCheck entity)
        {
            var provDocService = this.Container.Resolve<IDomainService<ActCheckProvidedDoc>>();
            var documentLongTextService = this.Container.Resolve<IDomainService<ChelyabinskDocumentLongText>>();

            try
            {
                var refFuncs = new List<Func<long, string>>
                {
                    id => provDocService.GetAll().Any(x => x.ActCheck.Id == id) ? "Предоставленные документы" : null
                };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return this.Failure(message);
                }

                // Удаляем записи в таблице gji_actcheck_ltext
                var ids = documentLongTextService.GetAll()
                    .Where(x => x.DocumentGji.Id == entity.Id)
                    .Select(x => x.Id).ToList();

                foreach (var id in ids)
                {
                    documentLongTextService.Delete(id);
                }

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                this.Container.Release(provDocService);
                this.Container.Release(documentLongTextService);
            }
        }

        public override IDataResult AfterCreateAction(IDomainService<ChelyabinskActCheck> service, ChelyabinskActCheck entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ChelyabinskActCheck> service, ChelyabinskActCheck entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ChelyabinskActCheck> service, ChelyabinskActCheck entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "Inspection", "Проверка ГЖИ" },
                { "Stage", "Этап проверки" },
                { "TypeDocumentGji", "Тип документа ГЖИ" },
                { "DocumentDate", "Дата документа" },
                { "DocumentNumber", "Номер документа" },
                { "DocumentSubNum", "Дополнительный номер документа (порядковый номер если документов одного типа несколько)" },
                { "State", "Статус" },
                { "Area", "Проверяемая площадь" },
                { "ToProsecutor", "Передано в прокуратуру" },
                { "DateToProsecutor", "Дата передачи" },
                { "TypeActCheck", "Тип акта" },
                { "TypeCheckAct", "Тип акта 2" },
                { "Flat", "Квартира" },
                { "Unavaliable", "Невозможно провести проверку" },
                { "UnavaliableComment", "Причина невозможности проведения проверки" },
                { "ReferralResolutionToRospotrebnadzor", "Требуется направление в Роспотребнадзор" },
                { "DocumentPlace", "Место составления" },
                { "DocumentPlaceFias", "Место составления (выбор из ФИАС)" },
                { "DocumentTime", "Время составления акта" },
                { "AcquaintState", "Статус ознакомления с результатами проверки" },
                { "RefusedToAcquaintPerson", "ФИО должностного лица, отказавшегося от ознакомления с актом проверки" },
                { "AcquaintedPerson", "ФИО должностного лица, ознакомившегося с актом проверки" },
                { "AcquaintedDate", "Дата ознакомления" },
                { "SignatoryInspector", "Инспектор подписавший акт проверки" }
            };
            return result;
        }
    }
    
}