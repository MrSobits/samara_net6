// ------------------------------------------------------------------------------------------------
// <copyright file="FillGeneralDataRatingCR.cs" company="BarsGroup">
//   BarsGroup
// </copyright>
// <summary>
//   Отчет "Заполнение общей информации об УК для Рейтинга УК"
// </summary>
// ------------------------------------------------------------------------------------------------

namespace Bars.GkhDi.Report
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Отчет "Заполнение общей информации об УК для Рейтинга УК"
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class FillGeneralDataRatingCr : BasePrintForm
    {
        #region параметры

        /// <summary>
        /// The municipality ids.
        /// </summary>
        private List<long> municipalityIds = new List<long>();

        #endregion
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FillGeneralDataRatingCr"/> class.
        /// </summary>
        public FillGeneralDataRatingCr() : base(new ReportTemplateBinary(Properties.Resources.FillGeneralDataRatingCR))
        {
        }
        
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Заполнение общей информации об УК для Рейтинга УК";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Desciption
        {
            get
            {
                return "Заполнение общей информации об УК для Рейтинга УК";
            }
        }

        /// <summary>
        /// Gets the group name.
        /// </summary>
        public override string GroupName
        {
            get
            {
                return "Раскрытие информации";
            }
        }

        /// <summary>
        /// Gets the parameters controller.
        /// </summary>
        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.FillGeneralDataRatingCR";
            }
        }

        /// <summary>
        /// Gets the required permission.
        /// </summary>
        public override string RequiredPermission
        {
            get
            {
                return "Reports.Di.FillGeneralDataRatingCR";
            }
        }

        /// <summary>
        /// The set user parameters.
        /// </summary>
        /// <param name="baseParams">
        /// The base parameters.
        /// </param>
        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        /// <summary>
        /// The prepare report.
        /// </summary>
        /// <param name="reportParams">
        /// The report parameters.
        /// </param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var managingOrganization = this.Container.Resolve<IDomainService<ManagingOrganization>>().GetAll()
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Contragent.Municipality.Id));

            var contragentId = managingOrganization.Select(x => x.Contragent.Id).Distinct();

            var contragentContact = this.Container.Resolve<IDomainService<ContragentContact>>().GetAll()
                                        .Where(x => contragentId.Contains(x.Contragent.Id) && x.Position.Code == "1")
                                        .Select(x => new
                                                {
                                                    id = x.Contragent.Id,
                                                    surname = string.IsNullOrEmpty(x.Surname) ? 0 : 1,
                                                    name = string.IsNullOrEmpty(x.Name) ? 0 : 1,
                                                    patronymic = string.IsNullOrEmpty(x.Patronymic) ? 0 : 1
                                                })
                                        .AsEnumerable()
                                        .GroupBy(x => x.id)
                                        .ToDictionary(x => x.Key, x => x.Select(y => new { y.surname, y.name, y.patronymic }).FirstOrDefault());

            var organizations = managingOrganization
                    .Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet && x.TypeManagement != TypeManagementManOrg.Other)
                    .Select(x => new
                                     {
                                         id = x.Contragent.Id,
                                         muName = x.Contragent.Municipality.Name ?? string.Empty,
                                         uoName = x.Contragent.ShortName,
                                         inn = string.IsNullOrEmpty(x.Contragent.Inn) ? 0 : 1,
                                         shortName = string.IsNullOrEmpty(x.Contragent.ShortName) ? 0 : 1,
                                         factAddress = string.IsNullOrEmpty(x.Contragent.FactAddress) ? 0 : 1,
                                         organizationForm = x.Contragent.OrganizationForm == null ? 0 : 1,
                                         ogrn = string.IsNullOrEmpty(x.Contragent.Ogrn) ? 0 : 1,
                                         phone = string.IsNullOrEmpty(x.Contragent.Phone) ? 0 : 1,
                                         mail = string.IsNullOrEmpty(x.Contragent.Email) ? 0 : 1
                                     })
                    .AsEnumerable()
                    .OrderBy(x => x.muName)
                    .GroupBy(x => x.muName).ToDictionary(x => x.Key, x => x.OrderBy(y => y.uoName).ToList());

            var counter = 1;
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");
            var sectionMo = section.ДобавитьСекцию("СекцияМО");

            foreach (var org in organizations)
            {
                double averageMo = 0;
                section.ДобавитьСтроку(); 
                section["МО"] = org.Key;

                foreach (var obj in org.Value)
                {
                    sectionMo.ДобавитьСтроку();
                    sectionMo["Номер"] = counter;

                    sectionMo["НаименованиеМО"] = obj.muName;
                    sectionMo["НаименованиеУО"] = obj.uoName;
                    sectionMo["ИНН"] = obj.inn;
                    sectionMo["КраткоеНаименование"] = obj.shortName;
                    sectionMo["ФактическийАдрес"] = obj.factAddress;
                    sectionMo["ОрганизационнаяФорма"] = obj.organizationForm;

                    int surname = 0, name = 0, patronymic = 0, position = 0;
                    if (contragentContact.ContainsKey(obj.id))
                    {
                        surname = contragentContact[obj.id].surname;
                        name = contragentContact[obj.id].name;
                        patronymic = contragentContact[obj.id].patronymic;
                        position = 1;
                    }
                    
                    sectionMo["ФамилияРуководителя"] = surname;
                    sectionMo["ИмяРуководителя"] = name;
                    sectionMo["ОтчествоРуководителя"] = patronymic;
                    sectionMo["ДолжностьРуководителя"] = position;

                    sectionMo["ОГРН"] = obj.ogrn;
                    sectionMo["Телефон"] = obj.phone;
                    sectionMo["ЭлектронныйАдрес"] = obj.mail;

                    double summ = obj.inn + obj.shortName + obj.factAddress + surname + name + patronymic + position + obj.organizationForm + obj.ogrn + obj.phone + obj.mail;
                    double average = (summ / 11) * 100;
                    
                    sectionMo["ПроцентРаскрытойИнформации"] = string.Format("{0:0.##}%", average);
                    averageMo += average;
                    ++counter;
                }

                averageMo = averageMo / org.Value.Count;
                section["СреднийПроцентПоМО"] = string.Format("{0:0.##}%", averageMo);
            }
        }       
    }
}
