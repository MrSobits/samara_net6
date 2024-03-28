namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Entities;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
  

    using Castle.Windsor;

    public class LicenseReissuancePersonService : ILicenseReissuancePersonService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Person> PersonDomain { get; set; }


        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IDomainService<LicenseReissuancePerson> LicenseReissuancePersonDomain { get; set; }

        public IDomainService<LicenseReissuance> LicenseReissuanceDomain { get; set; }

        public IDomainService<LicenseReissuanceProvDoc> LicenseReissuanceDocDomain { get; set; }

        public IDataResult AddPersons(BaseParams baseParams)
        {
            var requestId = baseParams.Params.GetAs("requestId", 0L);
            var personIds = baseParams.Params.GetAs("personIds", new long[0]);

            var request = LicenseReissuanceDomain.GetAll().FirstOrDefault(x => x.Id == requestId);

            if (request == null)
            {
                return new BaseDataResult(false, "Не удалось определить заявку на переоформление по Id " + request.ToStr());
            }

            var personToSave = new List<LicenseReissuancePerson>();

            var currentIds = LicenseReissuancePersonDomain.GetAll()
                .Where(x => x.LicenseReissuance.Id == requestId)
                .Select(x => x.Person.Id)
                .Distinct()
                .ToList();

            foreach (var id in personIds)
            {
                if (currentIds.Contains(id))
                {
                    continue;
                }

                personToSave.Add(new LicenseReissuancePerson
                {
                    Person = new Person {Id = id},
                    LicenseReissuance = request
                });
            }

            if (personToSave.Any())
            {
                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        personToSave.ForEach(x => LicenseReissuancePersonDomain.Save(x));

                        tr.Commit();
                    }
                    catch (Exception exc)
                    {
                        tr.Rollback();
                        return new BaseDataResult(false, exc.Message);
                    }
                }
            }

            return new BaseDataResult();
        }

        public IDataResult AddProvDocs(BaseParams baseParams)
        {
            var requestId = baseParams.Params.GetAs("requestId", 0L);
            var provdocIds = baseParams.Params.GetAs("provdocIds", new long[0]);

            var request = LicenseReissuanceDomain.GetAll().FirstOrDefault(x => x.Id == requestId);

            if (request == null)
            {
                return new BaseDataResult(false, "Не удалось определить заявку на лицензию по Id " + request.ToStr());
            }

            var provDocToSave = new List<LicenseReissuanceProvDoc>();

            var currentIds = LicenseReissuanceDocDomain.GetAll()
                .Where(x => x.LicenseReissuance.Id == requestId)
                .Select(x => x.LicProvidedDoc.Id)
                .Distinct()
                .ToList();

            foreach (var id in provdocIds)
            {
                if (currentIds.Contains(id))
                {
                    continue;
                }

                provDocToSave.Add(new LicenseReissuanceProvDoc
                {
                    LicProvidedDoc = new LicenseProvidedDoc { Id = id },
                    LicenseReissuance = request
                });
            }

            if (provDocToSave.Any())
            {
                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        provDocToSave.ForEach(x => LicenseReissuanceDocDomain.Save(x));

                        tr.Commit();
                    }
                    catch (Exception exc)
                    {
                        tr.Rollback();
                        return new BaseDataResult(false, exc.Message);
                    }
                }
            }

            return new BaseDataResult();
        }

    
    }
}