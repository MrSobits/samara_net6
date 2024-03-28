namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Такую пустышку навсякий случай нужно чтобы в регионах (Там где уже заменили или отнаследовались от этого класса) непопадало и можно было бы изменять методы как сущности Protocol
    /// </summary>
    public class ProtocolServiceInterceptor : ProtocolServiceInterceptor<Protocol>
    {
        // Внимание !! Код override нужно писать не в этом классе, а в ProtocolServiceInterceptor<T>
    }

    /// <summary>
    /// Короче такой поворот событий делается для того чтобы в Модулях регионов  с помошью 
    /// SubClass расширять сущность Protocol + не переписывать код который регистрируется по сущности
    /// то есть в Protocol добавляеться поля, но интерцептор поскольку Generic просто наследуется  
    /// </summary>
    public class ProtocolServiceInterceptor<T> : DocumentGjiInterceptor<T>
       where T : Protocol
    {
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            // Перед удалением проверяем есть ли дочерние документы
            var annexService = this.Container.Resolve<IDomainService<ProtocolAnnex>>();
            var lawService = this.Container.Resolve<IDomainService<ProtocolArticleLaw>>();
            var definitionService = this.Container.Resolve<IDomainService<ProtocolDefinition>>();
            var domainServiceViolation = Container.Resolve<IDomainService<ProtocolViolation>>();

            try
            {
                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return Failure(result.Message);
                }

                annexService.GetAll().Where(x => x.Protocol.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => annexService.Delete(x));

                lawService.GetAll().Where(x => x.Protocol.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => lawService.Delete(x));

                definitionService.GetAll().Where(x => x.Protocol.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => definitionService.Delete(x));

                domainServiceViolation.GetAll().Where(x => x.Document.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => domainServiceViolation.Delete(x));

                return result;
            }
            finally
            {
                Container.Release(annexService);
                Container.Release(lawService);
                Container.Release(definitionService);
                Container.Release(domainServiceViolation);
            }
        }

        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.Id.ToString());
            }
            catch
            {

            }

            return base.AfterCreateAction(service, entity);
        }

        public override IDataResult AfterUpdateAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
            }
            catch
            {

            }

            return base.AfterUpdateAction(service, entity);
        }

        public override IDataResult AfterDeleteAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
            }
            catch
            {

            }

            return base.AfterDeleteAction(service, entity);
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
                { "JudSector", "Протокол - реквизиты - Судебный участок" },
                { "FiasPlaceAddress", "Протокол - реквизиты - Тип адреса - Выбор из ФИАС" },
                { "TypeAddress", "Протокол - реквизиты - Тип адреса" },
                { "AddressPlace", "Протокол - реквизиты - Адрес места совершения правонарущения" },
                { "Executant", "Тип исполнителя документа" },
                { "Contragent", "Контрагент" },
                { "PhysicalPerson", "Физическое лицо" },
                { "PhysicalPersonPosition", "Физическое лицо - должность" },
                { "PhysicalPersonInfo", "Реквизиты физ.лица" },
                { "PhysicalPersonDocType", "Тип документа физ.лица" },
                { "PhysicalPersonDocumentNumber", "Номер документа физлица" },
                { "PhysicalPersonDocumentSerial", "Серия документа физлица" },
                { "PhysicalPersonIsNotRF", "Не является гражданином РФ" },
                { "DateToCourt", "Дата передачи в суд" },
                { "BirthDay", "Дата рождения" },
                { "Description", "Примечание" },
                { "DateOfProceedings", "Дата рассмотрения дела" },
                { "PersonFollowConversion", "Лицо, выполнившее перепланировку/переустройство" },
                { "FormatPlace", "Место составления" },
                { "FormatDate", " Дата составления протокола" },
                { "NotifNumber", "Номер уведомления о месте и времени составления протокола" },
                { "ProceedingsPlace", "Место рассмотрения дела" },
                { "Remarks", "Замечания к протоколу со стороны нарушителя" },
                { "UIN", "УИН" }
            };
            return result;
        }
    }
}