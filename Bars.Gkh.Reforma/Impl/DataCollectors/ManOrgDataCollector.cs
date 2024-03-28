namespace Bars.Gkh.Reforma.Impl.DataCollectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Reforma.Interface.DataCollectors;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.Gkh.Reforma.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    using Domain;

    using Interface;

    /// <summary>
    ///     Сервис сбора информации для отправки в Реформу
    /// </summary>
    public partial class ManOrgDataCollector : DataCollectorBase, IManOrgDataCollector, IManOrg988DataCollector
    {
        private readonly ISyncService _syncService;
        private readonly IClassMerger _merger;

        #region Static Fields

        /// <summary>
        ///     Соответствие имени названия поля его имени
        /// </summary>
        protected static readonly Dictionary<string, string> PropertyDescriptions = new Dictionary<string, string>
                                                                                        {
                                                                                            { "name_full", "Полное наименование" }, 
                                                                                            { "name_short", "Краткое наименование" }, 
                                                                                            { "okopf", "ОКОПФ" }, 
                                                                                            { "surname", "Фамилия руководителя" }, 
                                                                                            { "middlename", "Отчество руководителя" }, 
                                                                                            { "firstname", "Имя руководителя" }, 
                                                                                            { "position", "Должность руководителя" }, 
                                                                                            { "ogrn", "ОГРН" }, 
                                                                                            { "date_assignment_ogrn", "Дата регистрации" }, 
                                                                                            {
                                                                                                "name_authority_assigning_ogrn", 
                                                                                                "Наименование органа, принявшего решение о регистрации"
                                                                                            }, 
                                                                                            { "legal_address", "Идентификатор юридического адреса" }, 
                                                                                            { "actual_address", "Идентификатор фактического адреса" }, 
                                                                                            { "post_address", "Идентификатор почтового адреса" }, 
                                                                                            { "phone", "Телефон" }, 
                                                                                            { "email", "Электронный адрес" }, 
                                                                                            { "site", "Интернет сайт" }, 
                                                                                            { "proportion_sf", "Доля участия в уставном капитале Субъекта РФ" }, 
                                                                                            { "proportion_mo", "Доля участия в уставном капитале муниципального образования" }
                                                                                        };

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="syncService">Сервис синхронизации</param>
        /// <param name="merger">Склеиватель инстансов</param>
        public ManOrgDataCollector(IWindsorContainer container, ISyncService syncService, IClassMerger merger)
            : base(container)
        {
            _syncService = syncService;
            _merger = merger;
        }

        #endregion

        #region Public Methods and Operators
        /// <summary>
        ///     Сбор данных по УО для создания новой компании
        /// </summary>
        /// <param name="entity">
        ///     УО
        /// </param>
        /// <param name="period">Период</param>
        /// <returns>
        /// </returns>
        public IDataResult<NewCompanyProfileData> CollectNewCompanyProfileData(ManagingOrganization entity, PeriodDi period)
        {
            var contactService = this.Container.ResolveDomain<ContragentContact>();
            var organizationFormService = this.Container.ResolveDomain<OrganizationForm>();

            try
            {
                var orgForm = entity.Contragent.OrganizationForm;
                if (orgForm == null)
                {
                    return
                        new GenericDataResult<NewCompanyProfileData>(
                            message: "У контрагента не указана организационно-правовая форма") { Success = false };
                }

                if (string.IsNullOrEmpty(orgForm.OkopfCode))
                {
                    return
                        new GenericDataResult<NewCompanyProfileData>(
                            message: string.Format("Для организационно-правовой формы «{0}» не задан код ОКОПФ", orgForm.Name))
                        { Success = false };
                }

                var okopfCodes = organizationFormService.GetAll().Select(x => x.OkopfCode).ToArray();

                if ((entity.TypeManagement == TypeManagementManOrg.TSJ || entity.TypeManagement == TypeManagementManOrg.JSK)
                    && !okopfCodes.Contains(orgForm.OkopfCode))
                {
                    return
                        new GenericDataResult<NewCompanyProfileData>(message: "У контрагента неверно указана организационно-правовая форма")
                        {
                            Success = false
                        };
                }

                var contacts = contactService.GetAll()
                        .Where(x => x.Contragent.Id == entity.Contragent.Id &&
                                (x.Position.Code == "1" || x.Position.Code == "4"))
                        .ToList();

                Func<ContragentContact, bool> activeInperiodFunc;

                if (period != null)
                {
                    //activeInperiodFunc = DateTimeExpressionExtensions.CreatePeriodActiveInExpression<ContragentContact>(
                    //    period.DateStart ?? DateTime.MinValue,
                    //    period.DateEnd ?? DateTime.MaxValue,
                    //    x => x.DateStartWork ?? DateTime.MinValue,
                    //    x => x.DateEndWork ?? DateTime.MaxValue).Compile();
                    // потому что не учитывается частичное пересечение периодов

                    activeInperiodFunc = x =>
                        (((period.DateStart ?? DateTime.MinValue) <= (x.DateEndWork ?? DateTime.MaxValue)) &&
                            ((period.DateEnd ?? DateTime.MaxValue) >= (x.DateStartWork ?? DateTime.MinValue)));
                }
                else
                {
                    activeInperiodFunc = x => (!x.DateStartWork.HasValue || x.DateStartWork <= DateTime.Today) && (!x.DateEndWork.HasValue || x.DateEndWork >= DateTime.Today);
                }

                var contact = contacts.FirstOrDefault(x => activeInperiodFunc.Invoke(x));

                if (contact == null)
                {
                    return
                        new GenericDataResult<NewCompanyProfileData>(
                            message: "Не найдены контактные данные руководителя") { Success = false };
                }

                var result = new NewCompanyProfileData
                                 {
                                     actual_address = entity.Contragent.FiasFactAddress.ToReformaFias(this.Container),
                                     email = entity.Contragent.Email, 
                                     firstname = contact.Name, 
                                     middlename = contact.Patronymic, 
                                     surname = contact.Surname,
                                     legal_address = entity.Contragent.FiasJuridicalAddress.ToReformaFias(this.Container),
                                     name_full = entity.Contragent.Name, 
                                     name_short = entity.Contragent.ShortName, 
                                     ogrn = entity.Contragent.Ogrn, 
                                     okopf = ((Okopf[])Enum.GetValues(typeof(Okopf))).First(x => x.GetAttribute<XmlEnumAttribute>().Name == orgForm.OkopfCode.Trim()), 
                                     phone = entity.Contragent.Phone,
                                     post_address = entity.Contragent.FiasMailingAddress.ToReformaFias(this.Container), 
                                     proportion_mo = (float?)entity.ShareMo, 
                                     proportion_sf = (float?)entity.ShareSf, 
                                     site = entity.Contragent.OfficialWebsite
                                 };

                var emptyFields = this.CheckRequiredFields(result);
                if (emptyFields.Length > 0)
                {
                    return
                        new GenericDataResult<NewCompanyProfileData>(
                            message:
                                string.Format(
                                    "Не заполнены обязательные поля: {0}",
                                    string.Join(", ", emptyFields.Select(x => PropertyDescriptions.Get(x) ?? x))))
                            {
                                Success
                                    =
                                    false
                            };
                }

                return new GenericDataResult<NewCompanyProfileData>(result);
            }
            finally
            {
                this.Container.Release(contactService);
                this.Container.Release(contactService);
            }
        }

        #endregion

        #region Methods

        private bool CheckDisclosureExists(ManagingOrganization org, PeriodDi period)
        {
            return this.GetDisclosure(org, period) != null;
        }

        private DisclosureInfo GetDisclosure(ManagingOrganization org, PeriodDi period)
        {
            var service = this.Container.ResolveDomain<DisclosureInfo>();
            try
            {
                return
                    service.GetAll()
                           .FirstOrDefault(x => x.ManagingOrganization.Id == org.Id && x.PeriodDi.Id == period.Id);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private string GetParticipationInInAssociations(ManagingOrganization org, PeriodDi period)
        {
            var service = this.Container.ResolveDomain<ManagingOrgMembership>();
            try
            {
                var data =
                    service.GetAll()
                        .Where(x => x.ManagingOrganization.Id == org.Id)
                        .Where(
                            x =>
                            (x.DateStart.Value >= period.DateStart.Value && period.DateEnd.Value >= x.DateStart.Value)
                            || (period.DateStart.Value >= x.DateStart.Value && ((x.DateEnd.HasValue && x.DateEnd.Value >= period.DateStart.Value) || !x.DateEnd.HasValue)))
                        .Select(x => x.Name);

                return string.Join(", ", data);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        #endregion

        private string GetWorkTimeData(ManagingOrganization org, TypeMode typeMode = TypeMode.WorkMode)
        {
            var service = this.Container.ResolveDomain<ManagingOrgWorkMode>();
            try
            {
                var data =
                    service.GetAll()
                           .Where(x => x.ManagingOrganization.Id == org.Id && x.TypeMode == typeMode)
                           .Select(x => new { x.TypeDayOfWeek, x.StartDate, x.EndDate, x.AroundClock, x.Pause })
                           .OrderBy(x => x.TypeDayOfWeek)
                           .AsEnumerable()
                           .Select(x => new
                                {
                                   Day = x.TypeDayOfWeek.GetEnumMeta().Display, 
                                   Start = x.StartDate.HasValue ? x.StartDate.Value.ToString("HH:mm") : "-", 
                                   End = x.EndDate.HasValue ? x.EndDate.Value.ToString("HH:mm") : "-", 
                                   x.Pause, 
                                   x.AroundClock
                                })
                           .Select(x => string.Format("{0}: с {1} по {2}. Перерыв: {3}.{4}", x.Day, x.Start, x.End, x.Pause.Or("-"), x.AroundClock ? " Круглосуточно" : string.Empty));

                return string.Join("; ", data);
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}