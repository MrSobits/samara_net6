using Bars.Gkh.Utils;

namespace Bars.Gkh.Report
{
    using Bars.B4;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using B4.Modules.Reports;
    using System.Linq;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Config;
    using Slepov.Russian.Morpher;
    using Bars.B4.Utils;

    class PersonRequestToOGJN : GkhBaseStimulReport
    {

        #region Fields
        private long personId;

        protected Person Person { get; set; }
        #endregion

        public PersonRequestToOGJN()
            : base(new ReportTemplateBinary(Properties.Resources.PersonRequestToOGJN))
        {
        }

        public IDomainService<Person> PersonDomain { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            personId = userParamsValues.GetValue<object>("PersonId").ToLong();

            Person = PersonDomain.GetAll().FirstOrDefault(x => x.Id == personId);

            if (Person == null)
            {
                throw new Exception(string.Format("Не удалось определить должностное лицо по Id {0}", personId));
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                    {
                        Code = "PersonRequestToOGJN_1",
                        Name = "Запрос ЛК в ОГЖН",
                        Description =
                            "Запрос ЛК в ОГЖН",
                        Template = Properties.Resources.PersonRequestToOGJN
                    }
            };
        }

        public override string CodeForm
        {
            get { return "Person"; }
        }

        public override string Name
        {
            get { return "Запрос ЛК в ОГЖН"; }
        }

        public override string Description
        {
            get { return "Должностное лицо - Запрос ЛК в ОГЖН"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string Id
        {
            get { return "PersonRequestToOGJN"; }
        }

        public override string Extention
        {
            get { return "mrt"; }
        }

        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
            set { }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["Фамилия"] = Person.Surname;
            this.ReportParams["Имя"] = Person.Name;
            this.ReportParams["Отчество"] = Person.Patronymic;
            this.ReportParams["ДатаРождения"] = Person.Birthdate.ToDateString();
            this.ReportParams["МестоРождения"] = Person.AddressBirth;
        }
    }
}
