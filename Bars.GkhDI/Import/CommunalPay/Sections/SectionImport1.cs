namespace Bars.GkhDi.Import.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    public class SectionImport1 : ISectionImport
    {
        public string Name => "Импорт из комплат секция #1";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var inn = importParams.Inn;
            var sectionsData = importParams.SectionData;
            var logImport = importParams.LogImport;

            var contragentService = this.Container.Resolve<IDomainService<Contragent>>();
            var managingOrgService = this.Container.Resolve<IDomainService<ManagingOrganization>>();
            var contragentContactService = this.Container.Resolve<IDomainService<ContragentContact>>();
            var managingOrgWorkModeService = this.Container.Resolve<IDomainService<ManagingOrgWorkMode>>();

            using (this.Container.Using(contragentService, managingOrgService, contragentContactService, managingOrgWorkModeService))
            {
                var contragent = contragentService.GetAll().FirstOrDefault(x => x.Inn == inn);
                if (contragent == null)
                {
                    logImport.Error(Name, string.Format("Не найден контрагент по данному ИНН = {0}", inn));
                    return;
                }

                var managingOrg = managingOrgService.GetAll().FirstOrDefault(x => x.Contragent.Id == contragent.Id);
                if (managingOrg == null)
                {
                    logImport.Error(Name, string.Format("Не найдена управляющая организация для контрагента с ИНН = {0}", inn));
                    return;
                }

                var section1Record = sectionsData.Section1.FirstOrDefault();
                if (section1Record == null)
                {
                    logImport.Error(Name, string.Format("В выгрузке не найдена запись для организации с ИНН = {0}", inn));
                    return;
                }

                var opf = this.Container.Resolve<IDomainService<OrganizationForm>>().GetAll().FirstOrDefault(x => x.Code == section1Record.Opf);
                if (opf == null)
                {
                    logImport.Warn(this.Name, string.Format("Не найдена организационно-правовая форма с кодом {0}", section1Record.Opf));
                }

                switch (section1Record.ControlType)
                {
                    case "УК":
                        managingOrg.TypeManagement = TypeManagementManOrg.UK;
                        break;
                    case "ТСЖ":
                        managingOrg.TypeManagement = TypeManagementManOrg.TSJ;
                        break;
                }

                var positions = this.Container.Resolve<IDomainService<Position>>().GetAll();
                var position = positions.FirstOrDefault(x => x.Code == section1Record.ManagerPost);
                if (position == null)
                {
                    logImport.Warn(this.Name, string.Format("Не найдена должность с кодом {0}", section1Record.ManagerPost));
                }

                var positionDirector = positions.FirstOrDefault(x => x.Code == "1");

                if (positionDirector == null)
                {
                    logImport.Warn(this.Name, string.Format("Не найдена должность с кодом {0} (директор)", section1Record.ManagerPost));
                }

                contragent.Inn = section1Record.Inn;
                contragent.Kpp = section1Record.Kpp;
                if (opf != null)
                {
                    contragent.OrganizationForm = opf;
                }

                contragent.Email = section1Record.Email;
                contragent.Ogrn = section1Record.Ogrn;
                contragent.Phone = section1Record.Phone;
                contragent.OgrnRegistration = section1Record.OgrnAgency;
                contragent.OfficialWebsite = section1Record.OfficialSite;

                var contragentContact =
                    contragentContactService.GetAll()
                        .Where(x => x.Contragent.Id == contragent.Id)
                        .FirstOrDefault(x =>
                            x.FullName == string.Format("{0} {1} {2}",
                                section1Record.ManagerFamily,
                                section1Record.ManagerName,
                                section1Record.ManagerPatronomyc))
                    ?? new ContragentContact
                    {
                        Id = 0,
                        Contragent = contragent
                    };

                contragentContact.Position = position;
                contragentContact.Surname = section1Record.ManagerFamily;
                contragentContact.Name = section1Record.ManagerName;
                contragentContact.Patronymic = section1Record.ManagerPatronomyc;
                contragentContact.Phone = section1Record.ManagerPhone;
                contragentContact.Email = section1Record.ManagerEmail;
                contragentContact.DateStartWork = section1Record.ManagerDate;

                var contragentContactDirector =
                    contragentContactService.GetAll()
                        .Where(x => x.Position.Id == positionDirector.Id)
                        .Where(x => x.Contragent.Id == contragent.Id)
                        .FirstOrDefault(x =>
                            x.FullName == string.Format("{0} {1} {2}", section1Record.RukFamily, section1Record.RukName, section1Record.RukPatronomyc))
                    ??
                    new ContragentContact
                    {
                        Id = 0,
                        Contragent = contragent
                    };

                contragentContactDirector.Position = positionDirector;
                contragentContactDirector.Surname = section1Record.RukFamily;
                contragentContactDirector.Name = section1Record.RukName;
                contragentContactDirector.Patronymic = section1Record.RukPatronomyc;

                var managingOrgWorkModeList = managingOrgWorkModeService.GetAll().Where(x => x.ManagingOrganization.Id == managingOrg.Id).ToList();

                var listWorkMode = this.GetWorkMode(section1Record.WorkTimeBlock, managingOrgWorkModeList, managingOrg, TypeMode.WorkMode);
                listWorkMode.AddRange(this.GetWorkMode(section1Record.DispWorkTimeBlock, managingOrgWorkModeList, managingOrg, TypeMode.DispatcherWork));
                listWorkMode.AddRange(
                    this.GetWorkMode(section1Record.PriemWorkTimeBlock, managingOrgWorkModeList, managingOrg, TypeMode.ReceptionCitizens));

                foreach (var workMode in listWorkMode)
                {
                    if (workMode.Id > 0)
                    {
                        managingOrgWorkModeService.Update(workMode);
                        logImport.CountChangedRows++;
                    }
                    else
                    {
                        managingOrgWorkModeService.Save(workMode);
                        logImport.CountAddedRows++;
                    }
                }

                if (contragentContact.Id > 0)
                {
                    contragentContactService.Update(contragentContact);
                    logImport.CountChangedRows++;
                }
                else
                {
                    contragentContactService.Save(contragentContact);
                    logImport.CountAddedRows++;
                }

                if (contragentContactDirector.Id > 0)
                {
                    contragentContactService.Update(contragentContactDirector);
                    logImport.CountChangedRows++;
                }
                else
                {
                    contragentContactService.Save(contragentContactDirector);
                    logImport.CountAddedRows++;
                }
            }
        }

        private List<ManagingOrgWorkMode> GetWorkMode(string block, IEnumerable<ManagingOrgWorkMode> workModeList, ManagingOrganization managingOrganization, TypeMode typeMode)
        {
            var result = new List<ManagingOrgWorkMode>();

            var getValueFromArray =
                new Func<string[], int, string>(
                    (array, elementNum) => array.Length >= elementNum + 1
                        ? array[elementNum] != string.Empty ? array[elementNum] : null
                        : null);

            var recWorkModeList = block.Split('|');
            foreach (var recWorkMode in recWorkModeList.Where(x => !string.IsNullOrEmpty(x)))
            {
                var workModeParts = recWorkMode.Split(',');
                if (string.IsNullOrEmpty(workModeParts[0]))
                {
                    continue;
                }

                var dayWeek = TypeDayOfWeek.Monday;
                switch (workModeParts[0])
                {
                    case "Понедельник":
                        dayWeek = TypeDayOfWeek.Monday;
                        break;
                    case "Вторник":
                        dayWeek = TypeDayOfWeek.Tuesday;
                        break;
                    case "Среда":
                        dayWeek = TypeDayOfWeek.Wednesday;
                        break;
                    case "Четверг":
                        dayWeek = TypeDayOfWeek.Thursday;
                        break;
                    case "Пятница":
                        dayWeek = TypeDayOfWeek.Friday;
                        break;
                    case "Суббота":
                        dayWeek = TypeDayOfWeek.Saturday;
                        break;
                    case "Воскресенье":
                        dayWeek = TypeDayOfWeek.Sunday;
                        break;
                }

                var item = workModeList.FirstOrDefault(x => x.TypeDayOfWeek == dayWeek && x.TypeMode == typeMode)
                    ?? new ManagingOrgWorkMode
                    {
                        Id = 0,
                        ManagingOrganization = managingOrganization,
                        TypeDayOfWeek = dayWeek,
                        TypeMode = typeMode
                    };

                item.AroundClock = getValueFromArray(workModeParts, 1) == "Да";
                item.StartDate = getValueFromArray(workModeParts, 2) != null ? workModeParts[2].ToDateTime().To<DateTime?>() : null;
                item.EndDate = getValueFromArray(workModeParts, 3) != null ? workModeParts[3].ToDateTime().To<DateTime?>() : null;
                item.Pause = getValueFromArray(workModeParts, 4) != null && getValueFromArray(workModeParts, 5) != null ? string.Format("{0}-{1}", workModeParts[4], workModeParts[5]) : null;

                result.Add(item);
            }

            return result;
        }
    }
}
