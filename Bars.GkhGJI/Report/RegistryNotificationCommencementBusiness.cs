using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.GkhGji.Report
{
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    class RegistryNotificationCommencementBusiness : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private List<long> municipalityListId = new List<long>();

        public RegistryNotificationCommencementBusiness()
            : base(new ReportTemplateBinary(Bars.GkhGji.Properties.Resources.RegistryNotificationCommencementBusiness))
        {
        }

        public override string Name
        {
            get
            {
                return "Реестр уведомлений о начале предпринимательской деятельности"; 
            }
        }

        public override string Desciption
        {
            get
            {
                return "Реестр уведомлений о начале предпринимательской деятельности"; 
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отчет ГЖИ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.RegistryNotificationCommencementBusiness";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.RegistryNotificationCommencementBusiness";
            }
        }

        private void ParseIds(List<long> list, string Ids)
        {
            list.Clear();

            if (!string.IsNullOrEmpty(Ids))
            {
                var ids = Ids.Split(',');
                foreach (var id in ids)
                {
                    long Id;

                    if (long.TryParse(id, out Id))
                    {
                        if (!list.Contains(Id))
                        {
                            list.Add(Id);
                        }
                    }
                }
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.ParseIds(municipalityListId, baseParams.Params["municipalityIds"].ToString());
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var notEmptyMunicipal = this.municipalityListId.Count > 0 && this.municipalityListId.First() != 0;
            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();
            var serviceContragent = Container.Resolve<IDomainService<Contragent>>();
            var serviceBusiness = Container.Resolve<IDomainService<BusinessActivity>>().GetAll()
                .WhereIf(notEmptyMunicipal, x => municipalityListId.Contains(x.Contragent.Municipality.Id));
            var serviceServiceJuridicalGji = Container.Resolve<IDomainService<ServiceJuridicalGji>>();
            
            var dictMunName = serviceMunicipality.GetAll()
                         .WhereIf(notEmptyMunicipal, x => municipalityListId.Contains(x.Id))
                         .Select(x => new { x.Id, x.Name })
                         .OrderBy(x => x.Name)
                         .ToDictionary(x => x.Id, v => v.Name);

            var businessIdsQuery = serviceBusiness.Select(x => x.Id);

            var serviceJuridicalGjiDict = serviceServiceJuridicalGji.GetAll()
                .Where(x => businessIdsQuery.Contains(x.BusinessActivityNotif.Id))
                .Select(x => new
                {
                    businessActivityId = x.BusinessActivityNotif.Id,
                    serviceName = x.KindWorkNotif.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.businessActivityId)
                .ToDictionary(x => x.Key, x =>
                    {
                        var i = 0;
                        return x.Select(y => ++i + ". " + y.serviceName).Aggregate((curr, next) => curr + "\n" +  next);
                    });

            var contragent = serviceContragent.GetAll()
                        .Where(x => x.Municipality != null)
                        .WhereIf(notEmptyMunicipal, x => municipalityListId.Contains(x.Municipality.Id))
                        .GroupBy(x => new
                        {
                            MunName = x.Municipality.Name,
                            ContragentName = x.Name,
                            OrgForm = x.OrganizationForm.Name,
                            Mail = x.MailingAddress,
                            OgrnValue = x.Ogrn,
                            INN = x.Inn,
                            ContragentId = x.Id
                        })
                        .Select(x => new
                        {
                            x.Key,
                            muId = x.Min(y => y.Municipality.Id)
                        })
                        .ToList();

           var businessActivityDict = serviceBusiness
                .Select(x => new
                {
                    contragentId = x.Contragent.Id,
                    RegisterNumber = x.RegNum,
                    RegisterDate = x.DateRegistration ?? DateTime.MinValue,
                    x.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.contragentId)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.RegisterDate).ThenByDescending(y => y.Id).First());
            
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            int num = 1;

            foreach (var contr in contragent.OrderBy(x => x.Key.MunName))
            {
                if (!businessActivityDict.ContainsKey(contr.Key.ContragentId))
                {
                    continue;
                }
                section.ДобавитьСтроку();

                section["Num"] = num++;
                section["MO"] = dictMunName[contr.muId];
                section["JurNameOrgName"] = contr.Key.ContragentName + "; " + contr.Key.OrgForm;
                section["Mail"] = contr.Key.Mail;
                section["OGRN"] = contr.Key.OgrnValue;
                section["INN"] = contr.Key.INN;

                var currentbusinessActivity = businessActivityDict[contr.Key.ContragentId];

                section["Date"] = currentbusinessActivity.RegisterNumber != null
                    ? "№" + currentbusinessActivity.RegisterNumber + " от " + currentbusinessActivity.RegisterDate.ToDateTime().ToShortDateString()
                    : string.Empty;
                if (serviceJuridicalGjiDict.ContainsKey(currentbusinessActivity.Id))
                {
                    section["ViewDeytViewWork"] = serviceJuridicalGjiDict[currentbusinessActivity.Id];
                }
            }

        }
    }
}
