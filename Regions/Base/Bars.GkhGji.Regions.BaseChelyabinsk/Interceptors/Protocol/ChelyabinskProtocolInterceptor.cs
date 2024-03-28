namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.Protocol
{
    using System.Linq;
    using System;
    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.DomainService;
    using System.Collections.Generic;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    public class ChelyabinskProtocolInterceptor : ProtocolServiceInterceptor<ChelyabinskProtocol>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ChelyabinskProtocol> service, ChelyabinskProtocol entity)
        {
            if (entity.Contragent != null)
                entity.Contragent = entity.Inspection.Contragent;

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ChelyabinskProtocol> service, ChelyabinskProtocol entity)
        {
            var longTextService = this.Container.Resolve<IDomainService<ProtocolLongText>>();

            try
            {
                var longIds = longTextService.GetAll().Where(x => x.Protocol.Id == entity.Id).ToList();

                foreach (var id in longIds)
                {
                    longTextService.Delete(id);
                }

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                this.Container.Release(longTextService);
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ChelyabinskProtocol> service, ChelyabinskProtocol entity)
        {
            if (entity.JudSector != null) 
            {
                return base.BeforeUpdateAction(service, entity);
            }
            if (entity.PlaceOffense == PlaceOffense.AddressUr)
            {
                if (entity.Contragent != null)
                {
                    entity.FiasPlaceAddress = entity.Contragent.FiasJuridicalAddress;
                }
            }
            else if (entity.PlaceOffense == PlaceOffense.PlaceFact)
            {
                var data = Container.Resolve<IDomainService<ProtocolViolation>>().GetAll()
                     .Where(x => x.Document.Id == entity.Id)
                     .Select(x => new
                     {
                         x.InspectionViolation.RealityObject
                     }).FirstOrDefault();
                if (data != null)
                {
                    entity.FiasPlaceAddress = data.RealityObject.FiasAddress;
                }

            }
            if (entity.FiasPlaceAddress != null) 
            {
                var violationHouse = entity.FiasPlaceAddress.House;
                if (violationHouse.Contains("/"))
                {
                    violationHouse = violationHouse.Split('/')[0];
                }                
 
                char[] violationHouseChar = violationHouse.ToCharArray();
                for (int i = 0; i < violationHouse.Length; i++)
                {
                    if (!char.IsDigit(violationHouseChar[i]))
                    {
                        violationHouse = violationHouse.Replace(violationHouseChar[i].ToString(), string.Empty);
                    }
                }
                //автоподстановка суд.участка по адресу места совершения правонарушения
                var violationHouseInt = int.Parse(violationHouse);
                Utils.SaveFiasAddress(this.Container, entity.FiasPlaceAddress);
                int checkedMath = 0;
                long? jurId = null;
                Container.Resolve<IDomainService<JurInstitutionRealObj>>().GetAll()
                .Where(x => x.RealityObject.FiasAddress.StreetGuidId == entity.FiasPlaceAddress.StreetGuidId)
                .OrderBy(x => x.RealityObject.FiasAddress.House)
                .Select(x => new
                {
                    x.RealityObject.FiasAddress.House,
                    x.JurInstitution.Id
                })
                .ToList().ForEach(x =>
                {
                    var house = x.House;
                    if (house.Contains("/"))
                    {
                        house = house.Split('/')[0];
                    }

                    char[] houseChar = house.ToCharArray();
                    for (int i = 0; i < houseChar.Length; i++)
                    {
                        if (!char.IsDigit(houseChar[i]))
                        {
                            house = house.Replace(houseChar[i].ToString(),string.Empty);

                        }
                    }
                    var houseCharInt = int.Parse(house);
                    var module = Math.Abs(houseCharInt - violationHouseInt);
                    if (houseCharInt != violationHouseInt)
                    {
                        if (checkedMath == 0)
                        {
                            checkedMath = module;
                        }
                        else
                        {
                            if (checkedMath > module)
                            {
                                checkedMath = module;
                                jurId = x.Id;
                            }
                        }
                    }
                    else
                    {
                        jurId = x.Id;
                    }
                });
                if (jurId.HasValue)
                    entity.JudSector = new JurInstitution { Id = jurId.Value };
            }
               
            return base.BeforeUpdateAction(service, entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<ChelyabinskProtocol> service, ChelyabinskProtocol entity)
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

        public override IDataResult AfterUpdateAction(IDomainService<ChelyabinskProtocol> service, ChelyabinskProtocol entity)
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

        public override IDataResult AfterDeleteAction(IDomainService<ChelyabinskProtocol> service, ChelyabinskProtocol entity)
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
                { "UIN", "УИН" },
                { "PersonRegistrationAddress", "Адрес регистрации (место жительства, телефон)" },
                { "PersonFactAddress", "Фактический адрес" },
                { "PersonJob", "Место работы" },
                { "PersonPosition", "Должность" },
                { "PersonBirthDatePlace", "Дата, место рождения" },
                { "PersonDoc", "Документ, удостоверяющий личность" },
                { "PersonSalary", "Заработная плата" },
                { "PersonRelationship", "Семейное положение, кол-во иждивенцев" },
                { "TypePresence", "Протокол - Реквизиты - В присуствии/отсутствии" },
                { "Representative", "Представитель" },
                { "ReasonTypeRequisites", "Вид и реквизиты основания" },
                { "DateOfViolation", "Нарушения - Дата правонарушения" },
                { "ResolveViolationClaim", "Нарушения - Наименование требования" },
                { "NormativeDoc", "Правовое основание" },
            };
            return result;
        }
    }
}
