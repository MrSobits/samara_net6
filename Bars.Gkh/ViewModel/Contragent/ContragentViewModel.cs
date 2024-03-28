namespace Bars.Gkh.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;

    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    public class ContragentViewModel : BaseViewModel<Contragent>
    {

        public IContragentService ContragentService { get; set; }
        
        public override IDataResult List(IDomainService<Contragent> domain, BaseParams baseParams)
        {

            var loadParams = GetLoadParam(baseParams);

            var userManager = Container.Resolve<IGkhUserManager>();
            var contragentList = userManager.GetContragentIds();
            var activeOperator = userManager.GetActiveOperator();
            var contragent1468Id = activeOperator != null && activeOperator.Contragent != null ? activeOperator.Contragent.Id : 0;

            /*
            * параметр, по которому выводится только контрагенты определенные 
            * в Операторе в полях Организации и Контрагент
            */
            var operatorHasContragent = baseParams.Params.ContainsKey("operatorHasContragent") && baseParams.Params["operatorHasContragent"].ToBool();

            /*
            * параметр, по которому выводится только контрагенты определенных организаций
            * используется в документах ГЖИ (Протокол, Предписание, Постановление прокуратуры) в SelectField
            */
            var typeExecutant = baseParams.Params.ContainsKey("typeExecutant")
                ? baseParams.Params["typeExecutant"].ToInt()
                : -1;

            // сначала просто получаем IQueriable<Contragent>
            var data = domain.GetAll();

            data = Filter(data, baseParams);

            // Если передан Тип Исполнителя (а это просто код в справочнике)
            if (typeExecutant != -1)
            {
                // то получаем Контрагентов по типу исполнителя
                data = ContragentService.ListForTypeExecutant(typeExecutant);
                }

            // параметр, по которому выводятся только контрагенты определенных организаций, используется при создании основания для проверок ГЖИ
            var typeJurOrg = baseParams.Params.ContainsKey("typeJurOrg")
                ? baseParams.Params.GetValue("typeJurOrg").ToInt()
                : 0;

            // Если передан ти Юр лица
            if (typeJurOrg > 0)
            {
                // то получаем контрагентов по типу юрилца
                data = ContragentService.ListForTypeJurOrg(typeJurOrg);

                var roId = baseParams.Params.GetAs<long>("roId");
                if (roId > 0 && typeJurOrg == 10)
                {//для управляющей организации фильтруем еще и по домам

                    var contrId = Container.Resolve<IDomainService<ManagingOrgRealityObject>>().GetAll()
                        .Where(x => x.RealityObject.Id == roId)
                        .Select(x => x.ManagingOrganization.Contragent.Id)
                        .FirstOrDefault();

                    if (contrId != 0)
                    {
                        data = data.Where(x => x.Id == contrId);
                    }
                }
            }

            var typeServOrg = baseParams.Params.ContainsKey("typeServOrg")
                ? baseParams.Params["typeServOrg"].ToInt()
                : 0;

            if (typeServOrg > 0)
                        {
                // то получаем контрагентов по типу поставщика
                data = ContragentService.ListForTypeServOrg(typeServOrg);
            }

            var ids = baseParams.Params.ContainsKey("Id")
                ? baseParams.Params["Id"].ToStr()
                : string.Empty;

            var listIds = new List<long>();
            if (!string.IsNullOrEmpty(ids))
            {
                if (ids.Contains(','))
                {
                    listIds.AddRange(ids.Split(',').Select(x => x.ToLong()));
                }
                else
                {
                    listIds.Add(ids.ToLong());
                }
            }

            // для поля "Поставщики" в Услугах Раскрытия нужны все контрагенты без фильтрации по оператору
            var showAll = baseParams.Params.ContainsKey("showAll") && baseParams.Params["showAll"].ToBool();

#warning Переделать получение из контейнера!
            data = showAll
                ? new BaseDomainService<Contragent>
                {
                    Container = Container
                }.GetAll()
                : data;
            
            if (operatorHasContragent)
            {
                data = data.WhereIf(contragentList.Count > 0 || contragent1468Id > 0, x => contragentList.Contains(x.Id) || contragent1468Id == x.Id);
            }

            var dataMain = data
                .CustomFilter(Container, baseParams)
                .WhereIf(listIds.Count > 0, x => listIds.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name,
                    Settlement = x.MoSettlement.Name,
                    MunicipalityId = (long?)x.Municipality.Id,
                    //TODO: Разобраться, откуда такая логика
                    JuridicalAddress = (x.AddressOutsideSubject != null && x.AddressOutsideSubject != "") ? x.AddressOutsideSubject : x.JuridicalAddress, // вариант x.AddressOutsideSubject.Length > 0 в условии не отрабатывает в Самаре на оракле
                    x.Name,
                    ShortName = (x.ShortName == null || x.ShortName.Trim() == "") ? x.Name : x.ShortName,
                    x.Phone,
                    x.Inn,
                    x.Kpp,
                    x.Ogrn, // для раскрытия инф-ии. для получения данных в событии onchange
                    x.ActivityDateStart, // для раскрытия инф-ии. для получения данных в событии onchange
                    x.ContragentState,
                    x.MailingAddress,
                    x.FactAddress,
                    x.DateRegistration,
                    OrganizationForm = x.OrganizationForm.Name,
                    x.AddressOutsideSubject,
                    x.Email,
                    x.EgrulExcDate,
                    //TODO: Объединить с JuridicalAddress
                    TrueJuridicalAddress = x.JuridicalAddress
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.JuridicalAddress)
                .Filter(loadParams, Container);

            int totalCountMain = dataMain.Count();

            return new ListDataResult(dataMain.Order(loadParams).Paging(loadParams).ToList(), totalCountMain);
        }

        /// <summary>
        /// Дополнительная фильтрация контрагентов.
        /// </summary>
        protected virtual IQueryable<Contragent> Filter(IQueryable<Contragent> query, BaseParams baseParams)
        {
            return query;
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<Contragent> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("Id");
            var record = domainService.Get(id);

            return new BaseDataResult(new 
            {
                record.Id,
                record.ActivityDateEnd,
                record.ActivityDateStart,
                record.ActivityDescription,
                record.ActivityGroundsTermination,
                record.AddressOutsideSubject,
                record.ContragentState,
                record.DateRegistration,
                record.DateTermination,
                record.Description,
                record.EgrulExcDate,
                record.EgrulExcNumber,
                record.Email,
                record.FactAddress,
                record.Fax,
                record.IsEDSE,
                record.IsSOPR,
                FiasFactAddress = record.FiasFactAddress?.GetFiasProxy(this.Container),
                FiasJuridicalAddress = record.FiasJuridicalAddress?.GetFiasProxy(this.Container),
                FiasMailingAddress = record.FiasMailingAddress?.GetFiasProxy(this.Container),
                FiasOutsideSubjectAddress = record.FiasOutsideSubjectAddress?.GetFiasProxy(this.Container),
                record.Inn,
                record.IsSite,
                record.JuridicalAddress,
                record.Kpp,
                record.LicenseDateReceipt,
                record.MailingAddress,
                record.MoSettlement,
                record.Municipality,
                record.Name,
                record.NameAblative,
                record.NameAccusative,
                record.NameDative,
                record.NameGenitive,
                record.NamePrepositional,
                record.OfficialWebsite,
                record.Ogrn,
                record.OgrnRegistration,
                record.Okato,
                record.Okpo,
                record.Oktmo,
                record.Okved,
                record.OrganizationForm,
                record.Parent,
                record.Phone,
                record.PhoneDispatchService,
                record.ProviderCode,
                record.RegDateInSocialUse,
                record.ShortName,
                record.SubscriberBox,
                record.TaxRegistrationDate,
                record.TaxRegistrationIssuedBy,
                record.TaxRegistrationNumber,
                record.TaxRegistrationSeries,
                record.TweeterAccount,
                record.FrguRegNumber,
                record.FrguOrgNumber,
                record.FrguServiceNumber,
                record.TypeEntrepreneurship,
                record.YearRegistration,
                record.TimeZoneType,
                record.Okogu,
                record.Okfs,
                record.MainRole,
                record.ReceiveNotifications,
                record.IncludeInSopr
            });
        }
    }
}