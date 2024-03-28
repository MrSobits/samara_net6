namespace Bars.GkhGji.Regions.Tomsk.Report.AdministrativeCase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;

    using Gkh.Entities;
    using Gkh.Report;
    using GkhGji.Entities;
    using GkhGji.Enums;
    using Entities;

    public class AdministrativeCaseStimulReportExtensionDate : GkhBaseStimulReport 
    {
        private long AdminCaseId { get; set; }

        /// <summary>
        /// Домен-сервис "Таблица связи документов (Какой документ из какого был сформирован)"
        /// </summary>
        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }


        /// <summary>
        /// Домен-сервис "Обращение граждан проверки по обращениям граджан"
        /// </summary>
        public IDomainService<InspectionAppealCits> InspectionAppealCits { get; set; }

        public AdministrativeCaseStimulReportExtensionDate()
            : base(new ReportTemplateBinary(Properties.Resources.AdministrativeCaseExtensionDate))
        {
        }

        /// <summary>
        /// Формат документа
        /// </summary>
        public override StiExportFormat ExportFormat => StiExportFormat.Word2007;

        /// <summary>
        /// Id
        /// </summary>
        public override string Id => "AdministrativeCaseExtensionDate";

        /// <summary>
        /// Код формы
        /// </summary>
        public override string CodeForm => "AdministrativeCase";

        /// <summary>
        /// Название
        /// </summary>
        public override string Name => "Определение о продлении срока";

        /// <summary>
        /// Комментарий  
        /// </summary>
        public override string Description => "Определение о продлении срока";

        /// <summary>
        /// Домен-сервис "Словарь Административное дело"
        /// </summary>
        public IDomainService<AdministrativeCaseDescription> DescriptionDomain { get; set; }

        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Получение номера документа 
        /// </summary>
        /// <param name="userParamsValues">Пользовательские параметры</param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.AdminCaseId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "TomskAdministrativeCaseExtensionDate",
                    Name = "AdministrativeCaseExtensionDate",
                    Description = "Определение о продлении срока",
                    Template = Properties.Resources.AdministrativeCaseExtensionDate
                }
            };
        }
        /// <summary>
        /// Формирование документа
        /// </summary>
        /// <param name="reportParams">Параметры отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var adminCaseDocDomain = this.Container.Resolve<IDomainService<AdministrativeCase>>();
            var adminCaseArticleLawDomain = this.Container.Resolve<IDomainService<AdministrativeCaseArticleLaw>>();
            var adminCaseProvidedDocDomain = this.Container.Resolve<IDomainService<AdministrativeCaseProvidedDoc>>();
            var dbConfigProvider = this.Container.Resolve<IDbConfigProvider>();

            try
            {
                var adminCase = adminCaseDocDomain.GetAll().First(x => x.Id == this.AdminCaseId);

                this.ReportParams["ДатаДела"] =
                    adminCase.DocumentDate.HasValue
                        ? adminCase.DocumentDate.Value.ToShortDateString()
                        : string.Empty;

                if (adminCase.Inspector != null)
                {
                    this.ReportParams["ДолжностьИнспектораДела"] = adminCase.Inspector.Position;
                    this.ReportParams["ИнспекторДела"] = adminCase.Inspector.Fio;
                    var fio = adminCase.Inspector.Fio;
                    var fioArray = fio.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var surname = fioArray[0];
                    var name = fioArray[1];
                    var patronymic = fioArray[2];
                    this.ReportParams["ИнициалыИнспекторДела"] = string.Format("{0}.{1} {2}", name[0], patronymic[0], surname);
                }

                this.ReportParams["ОбъектНедвижимости"] =
                    adminCase.RealityObject != null
                        ? adminCase.RealityObject.Address
                        : string.Empty;

                this.ReportParams["Вопрос"] = adminCase.DescriptionQuestion;
                this.ReportParams["Установил"] = this.DescriptionDomain.GetAll().FirstOrDefault(x => x.AdministrativeCase.Id == adminCase.Id).Return(x => Encoding.UTF8.GetString(x.DescriptionSet))
                                      ?? adminCase.DescriptionSet;
                this.ReportParams["НомерДела"] = adminCase.DocumentNumber;

                this.FillContragentCase(adminCase.Contragent);

                var articleLawName = adminCaseArticleLawDomain.GetAll()
                    .Where(x => x.AdministrativeCase.Id == this.AdminCaseId)
                    .Select(x => x.ArticleLaw.Name)
                    .FirstOrDefault();

                this.ReportParams["СтатьяЗакона"] = articleLawName;

                this.ReportParams["ИдентификаторДокументаГЖИ"] = this.AdminCaseId.ToString();
                this.ReportParams["СтрокаПодключениякБД"] = dbConfigProvider.ConnectionString;

                var providedDocs = adminCaseProvidedDocDomain.GetAll()
                    .Where(x => x.AdministrativeCase.Id == adminCase.Id)
                    .Select(x => x.ProvidedDoc.Name)
                    .ToArray();

                var providedDocuments = providedDocs.Select(doc => new ProvidedDocument
                {
                    ПредоставляемыйДокумент = doc
                })
                .ToList();

                this.DataSources.Add(new MetaData
                {
                    SourceName = "ПредоставляемыеДокументы",
                    MetaType = nameof(Object),
                    Data = providedDocuments
                });

                this.ReportParams["Основание"] = adminCase.TypeAdminCaseBase.GetDisplayName();

                this.ReportParams["Документ"] = this.NameDocument(adminCase);

            }
            finally
            {
                this.Container.Release(adminCaseDocDomain);
                this.Container.Release(adminCaseArticleLawDomain);
                this.Container.Release(adminCaseProvidedDocDomain);
                this.Container.Release(dbConfigProvider);
            }
        }

        private void FillContragentCase(Contragent contragent)
        {
            if (contragent != null)
            {
                this.ReportParams["КонтрагентРП"] =
                    !contragent.NameGenitive.IsEmpty()
                        ? contragent.NameGenitive
                        : contragent.Name;

                this.ReportParams["КонтрагентДП"] =
                    !contragent.NameDative.IsEmpty()
                        ? contragent.NameDative
                        : contragent.Name;

                this.ReportParams["КонтрагентВП"] =
                    !contragent.NameAccusative.IsEmpty()
                        ? contragent.NameAccusative
                        : contragent.Name;

                this.ReportParams["КонтрагентТП"] =
                    !contragent.NameAblative.IsEmpty()
                        ? contragent.NameAblative
                        : contragent.Name;

                this.ReportParams["КонтрагентПП"] =
                    !contragent.NamePrepositional.IsEmpty()
                        ? contragent.NamePrepositional
                        : contragent.Name;

                this.ReportParams["Контрагент"] =
                    !contragent.ShortName.IsEmpty()
                        ? contragent.ShortName
                        : contragent.Name;
            }
        }

        protected class ProvidedDocument
        {
            public string ПредоставляемыйДокумент { get; set; }
        }

        /// <summary>
        /// Получение Название документов 
        /// </summary>
        /// <param name="adminCase">Административное дело</param>
        /// <returns>Возвращение название документа</returns>
        private string NameDocument(AdministrativeCase adminCase)
        {
            var parentDoc = this.ChildrenDomain.GetAll().Where(x => x.Children.Id == this.AdminCaseId).Select(x => new { x.Parent.TypeDocumentGji, x.Parent.DocumentDate, x.Parent.DocumentNumber }).FirstOrDefault();

            string result = string.Empty;

            if (parentDoc != null)
            {
                result = string.Format(
                    "№{0} от {1}",
                    parentDoc.DocumentNumber,
                    parentDoc.DocumentDate.HasValue ? parentDoc.DocumentDate.Value.ToShortDateString() : null);
            }
            else
            {
                // Если АД сформировано не из документа то определяем так
                if (adminCase.Inspection != null)
                {
                    switch (adminCase.Inspection.TypeBase)
                    {

                        case TypeBase.CitizenStatement:
                            {
                                // если распоряжение создано на основе обращения граждан
                                this.GetInfoCitizenStatement(ref result, adminCase.Inspection.Id);
                            }

                            break;

                            // Если поднадобится еще доп условия писать тут
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Если было сформировано из Обращение граждан
        /// </summary>
        /// <param name="result">Название</param>
        /// <param name="inspectionId">Id инспектора</param>
        private void GetInfoCitizenStatement(ref string result, long inspectionId)
        {
            // распоряжение создано на основе обращения граждан,
            // поле planName = "№ Номер ГЖИ"

            // Получаем из основания наименование плана
            var baseStatement = this.InspectionAppealCits
                    .GetAll()
                    .Where(x => x.Inspection.Id == inspectionId)
                    .Select(x => new { x.AppealCits.NumberGji, x.AppealCits.DateFrom })
                    .FirstOrDefault();

            if (baseStatement != null)
            {
                result = string.Format("№ {0} от {1}", baseStatement.NumberGji, baseStatement.DateFrom.ToDateTime().ToShortDateString());
            }
        }
    }
}