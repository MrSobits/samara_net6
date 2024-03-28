namespace Bars.GkhGji.Regions.Samara.DomainService
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Samara.Entities;

    using Castle.Windsor;

    public class AppealCitsTesterService : IAppealCitsTesterService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddTesters(BaseParams baseParams)
        {
            var appCitsTesterDomain = Container.Resolve<IDomainService<AppealCitsTester>>();

            try
            {
                var appealCitizensId = baseParams.Params.ContainsKey("appealCitizensId") ? baseParams.Params["appealCitizensId"].ToLong() : 0;
                var objectIds = baseParams.Params.GetAs("objectIds", new long[]{});

                // получаем дома что бы не добавлять их повторно
                var currentTesters =
                    appCitsTesterDomain.GetAll()
                        .Where(x => x.AppealCits.Id == appealCitizensId)
                        .Select(x => x.Tester.Id)
                        .ToList();

                var listToSave = new List<AppealCitsTester>();

                foreach (var id in objectIds)
                {
                    if (currentTesters.Contains(id))
                    {
                        continue;
                    }

                    listToSave.Add(new AppealCitsTester
                        {
                            AppealCits = new AppealCits { Id = appealCitizensId },
                            Tester = new Inspector { Id = id }
                        });
                }

                if (listToSave.Any())
                {
                    using (var tr = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            listToSave.ForEach(appCitsTesterDomain.Save);
                            tr.Commit();
                        }
                        catch (Exception exc)
                        {
                            tr.Rollback();
                            throw exc;
                        }
                    }    
                }
                
                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult {Success = false, Message = e.Message};
            }
        }

    }
}