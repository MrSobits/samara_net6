namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using B4.DataAccess;
    using System;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService;
    using System.Collections.Generic;

    public class Protocol197ServiceInterceptor : DocumentGjiInterceptor<Protocol197>
	{
		public override IDataResult BeforeCreateAction(IDomainService<Protocol197> service, Protocol197 entity)
        {
			var domainServiceInspection = this.Container.Resolve<IDomainService<BaseDefault>>();
            var domainStage = this.Container.Resolve<IDomainService<InspectionGjiStage>>();

            try
            {
				var newInspection = new BaseDefault()
                {
                    TypeBase = TypeBase.Protocol197
                };

                domainServiceInspection.Save(newInspection);

                var newStage = new InspectionGjiStage
                {
                    Inspection = newInspection,
                    TypeStage = TypeStage.Protocol197,
                    Position = 0
                };

                domainStage.Save(newStage);

				entity.TypeDocumentGji = TypeDocumentGji.Protocol197;
                entity.Inspection = newInspection;
                entity.Stage = newStage;

                if (entity.DocumentNum != null)
                {
                   
                    string UIN = "39645f";
                    string s1 = Convert.ToInt32(UIN, 16).ToString().PadLeft(8, '0');
                    string s2 = (entity.DocumentDate?.ToString("yyyyMMdd") ?? "00000000");
                    string s3 = "";
                    if (entity.DocumentNumber.Contains("-"))
                    {
                        if (entity.DocumentNumber.Split('-').Count() > 2)
                        {
                            s3 = (entity.DocumentNumber.Split('-')[1] + entity.DocumentNumber.Split('-')[2]).PadRight(8, '0');
                        }
                        else if (entity.DocumentNumber.Split('-').Count() == 2)
                        {
                            s3 = entity.DocumentNumber.Split('-')[1].PadRight(8, '0');
                        }
                        else
                        {
                            s3 = entity.DocumentNumber.Replace("-", "").PadRight(8, '0');
                        }
                    }
                    else
                    {
                        s3 = entity.DocumentNumber.PadRight(8, '0');
                    }
                    s3 = s3.Replace('/', '1');
                    s3 = s3.Replace('\\', '0');
                    s3 = s3.Replace('№', '4');
                    char[] charsS3 = s3.ToCharArray();
                    for (int i = 0; i < s3.Length; i++)
                    {
                        if (!char.IsDigit(charsS3[i]))
                        {
                            s3 = s3.Replace(charsS3[i], '0');
                        }
                    }
                    entity.UIN = (s1 + s2 + s3).Substring(0, 24);
                    entity.UIN += CheckSum(entity.UIN);
                   
                }

                return base.BeforeCreateAction(service, entity);
            }
            finally 
            {
                this.Container.Release(domainServiceInspection);
                this.Container.Release(domainStage);
            }
        }

        private Int32 CheckSum(String number)
        {
            Int32 result = CheckSum(number, 1);

            return result != 10 ? result : CheckSum(number, 3) % 10;
        }

        private Int32 CheckSum(String number, Int32 ves)
        {
            int sum = 0;
            for (int i = 0; i < number.Length; i++)
            {
                int t = (int)Char.GetNumericValue(number[i]);
                int rrr = ((ves % 10) == 0 ? 10 : ves % 10);

                sum += t * rrr;
                ves++;
            }

            return sum % 11;
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Protocol197> service, Protocol197 entity)
        {
           
            if (entity.JudSector == null)
            {
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
                    Utils.SaveFiasAddress(Container, entity.FiasPlaceAddress);
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
                        char[] houseChar = house.ToCharArray();
                        for (int i = 0; i < houseChar.Length; i++)
                        {
                            if (!char.IsDigit(houseChar[i]))
                            {
                                house = house.Replace(houseChar[i].ToString(), string.Empty);
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
            }

            if (entity.DocumentNumber != null)
            {
                string UIN = "39645f";
                string s1 = Convert.ToInt32(UIN, 16).ToString().PadLeft(8, '0');
                string s2 = (entity.DocumentDate?.ToString("yyyyMMdd") ?? "00000000");
                string s3 = "";
                if (entity.DocumentNumber.Contains("-"))
                {
                    if (entity.DocumentNumber.Split('-').Count() > 2)
                    {
                        s3 = (entity.DocumentNumber.Split('-')[1] + entity.DocumentNumber.Split('-')[2]).PadRight(8, '0');
                    }
                    else if (entity.DocumentNumber.Split('-').Count() == 2)
                    {
                        s3 = entity.DocumentNumber.Split('-')[1].PadRight(8, '0');
                    }
                    else
                    {
                        s3 = entity.DocumentNumber.Replace("-", "").PadRight(8, '0');
                    }
                }
                else
                {
                    s3 = entity.DocumentNumber.PadRight(8, '0');
                }
                entity.UIN = (s1 + s2 + s3).Substring(0, 24);
                entity.UIN += CheckSum(entity.UIN);
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<Protocol197> service, Protocol197 entity)
		{
			var annexService = this.Container.Resolve<IDomainService<Protocol197Annex>>();
			var lawService = this.Container.Resolve<IDomainService<Protocol197ArticleLaw>>();
			var violationService = this.Container.Resolve<IDomainService<Protocol197Violation>>();
			var activitiyDirectionService = this.Container.Resolve<IDomainService<Protocol197ActivityDirection>>();
			var surveySubjectReqService = this.Container.Resolve<IDomainService<Protocol197SurveySubjectRequirement>>();
			var longTextService = this.Container.Resolve<IDomainService<Protocol197LongText>>();

			try
			{
				var result = base.BeforeDeleteAction(service, entity);

				if (!result.Success)
				{
					return this.Failure(result.Message);
				}

				annexService.GetAll().Where(x => x.Protocol197.Id == entity.Id)
					.Select(x => x.Id).ForEach(x => annexService.Delete(x));

				lawService.GetAll().Where(x => x.Protocol197.Id == entity.Id)
					.Select(x => x.Id).ForEach(x => lawService.Delete(x));

				violationService.GetAll().Where(x => x.Document.Id == entity.Id)
					.Select(x => x.Id).ForEach(x => violationService.Delete(x));

				activitiyDirectionService.GetAll().Where(x => x.Protocol197.Id == entity.Id)
					.Select(x => x.Id).ForEach(x => activitiyDirectionService.Delete(x));

				surveySubjectReqService.GetAll().Where(x => x.Protocol197.Id == entity.Id)
					.Select(x => x.Id).ForEach(x => surveySubjectReqService.Delete(x));

				longTextService.GetAll().Where(x => x.Protocol197.Id == entity.Id)
					.Select(x => x.Id).ForEach(x => longTextService.Delete(x));

				return result;
			}
			finally
			{
				this.Container.Release(annexService);
				this.Container.Release(lawService);
				this.Container.Release(violationService);
				this.Container.Release(activitiyDirectionService);
				this.Container.Release(surveySubjectReqService);
			}
		}

        public override IDataResult AfterCreateAction(IDomainService<Protocol197> service, Protocol197 entity)
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

        public override IDataResult AfterUpdateAction(IDomainService<Protocol197> service, Protocol197 entity)
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

        public override IDataResult AfterDeleteAction(IDomainService<Protocol197> service, Protocol197 entity)
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
                { "PlaceOffense", "Протокол - реквизиты - Место совершения правонарушения" },
                { "AddressPlace", "Протокол - реквизиты - Адрес места совершения правонарущения" },
                { "Executant", "Тип исполнителя документа" },
                { "Contragent", "Контрагент" },
                { "PhysicalPerson", "Физическое лицо" },
                { "PhysicalPersonInfo", "Реквизиты физ.лица" },
                { "PhysicalPersonDocType", "Тип документа физ.лица" },
                { "PhysicalPersonDocumentNumber", "Номер документа физлица" },
                { "PhysicalPersonDocumentSerial", "Серия документа физлица" },
                { "PhysicalPersonIsNotRF", "Не является гражданином РФ" },
                { "DateToCourt", "Дата передачи в суд" },
                { "Description", "Примечание" },
                { "DateOfProceedings", "Дата рассмотрения дела" },
                { "PersonFollowConversion", "Лицо, выполнившее перепланировку/переустройство" },
                { "FormatDate", "Дата составления протокола" },
                { "FormatPlace", "Место составления" },
                { "NotifNumber", "Номер уведомления о месте и времени составления протокола" },
                { "ProceedingsPlace", "Место рассмотрения дела" },
                { "Remarks", "Замечания к протоколу со стороны нарушителя" },
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
                { "UIN", "УИН" },
                { "ControlType", "Вид контроля" }
            };
            return result;
        }
    }
}