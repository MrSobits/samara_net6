using System.Collections.Generic;
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities;
using Bars.Gkh.Import;
using Bars.Gkh.Integration.DataFetcher;
using Castle.Windsor;
using Newtonsoft.Json.Linq;

namespace Bars.Gkh.Integration.Embir.Import
{
    using System.Reflection;
    using Bars.Gkh.Overhaul.Import.CommonRealtyObjectImport;
    using Gkh.Import.Impl;

    public class ImportOrgForm : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "ImportEmbir"; }
        }

        public override string Name
        {
            get { return "Импорт справочника 'Тип юридического лица' с ЕМБИР"; }
        }

        public override string PossibleFileExtensions
        {
            get { return string.Empty; }
        }

        public override string PermissionName
        {
            get { return "Import.Embir.View"; }
        }

        public IWindsorContainer Container { get; set; }

        public ILogImportManager LogManager { get; set; }

        public ILogImport LogImport { get; set; }

        public IDomainService<OrganizationForm> OrganizationFormDomain { get; set; }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            return true;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var webClientFetcher = new WebClientFetcher();
            var importHelper = new ImportIntegrationHelper(Container);

            var httpQueryBuilder = importHelper.GetHttpQueryBuilder();
            httpQueryBuilder.AddParameter("type", "DictLegalType");

            dynamic[] listOrgForm = Enumerable.ToArray(webClientFetcher.GetData(httpQueryBuilder));

            var orgForms = OrganizationFormDomain.GetAll()
                .GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, y => y.First());

            var listOrgFormToSave = new List<OrganizationForm>();

            foreach (var dynOrgForm in listOrgForm)
            {
                var orgForm = (JObject)(dynOrgForm);

                var orgFormCode = orgForm["Code"].ToStr();
                var orgFormName = orgForm["Name"].ToStr();
                if (!orgForms.ContainsKey(orgFormCode))
                {
                    listOrgFormToSave.Add(new OrganizationForm
                    {
                        Name = orgFormName,
                        Code = orgFormCode
                    });

                    LogImport.Info("Информация", "Добавлена запись в справочник 'Организационно-правовые формы'. Наименование: {0}".FormatUsing(orgFormName));

                    LogImport.CountAddedRows++;
                }
                else
                {
                    var existOrgForm = orgForms.Get(orgFormCode);
                    if (string.IsNullOrEmpty(existOrgForm.Name))
                    {
                        existOrgForm.Name = orgFormName;
                        listOrgFormToSave.Add(existOrgForm);
                        LogImport.Info("Информация", "Добавлена запись в справочник 'Организационно-правовые формы'. Наименование: {0}".FormatUsing(orgFormName));

                        LogImport.CountAddedRows++;
                    }
                    else
                    {
                        LogImport.Info("Информация",
                               "В справочнике 'Организационно-правовые формы' уже есть запись с таким кодом. Код: {0}".FormatUsing(orgFormCode));
                    }
                }
            }

            TransactionHelper.InsertInManyTransactions(Container, listOrgFormToSave, 10000, true, true);

            LogImport.SetFileName(Name);
            LogImport.ImportKey = this.Key;
            LogManager.AddLog(LogImport);
            LogManager.FileNameWithoutExtention = Name;
            LogManager.Save();

            return new ImportResult();
        }
    }
}