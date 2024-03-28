namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using System.Collections.Generic;

    using B4;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    using Castle.Windsor;
    using GkhGji.Entities;
    using System.Linq;

    public class CompetentOrgGjiService : ICompetentOrgGjiService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddRevenueSource(BaseParams baseParams)
        {

            var objectIds = baseParams.Params.GetAs<long[]>("objectIds");
            if (objectIds == null)
            {
                return new BaseDataResult { Success = false, Message = "Необходимо выбрать записи  из справочника 'Источники поступлений'"};
            }
            
            var revenueService = Container.Resolve<IDomainService<RevenueSourceGji>>();
            var competenrOrgService = Container.Resolve<IDomainService<CompetentOrgGji>>();
            
            try
            {
                var listToSave = new List<CompetentOrgGji>();

                var data = revenueService.GetAll()
                                  .Where(x => objectIds.Contains(x.Id))
                                  .Where(x => !competenrOrgService.GetAll().Any(y => y.Name == x.Name))
                                  .Select(x => x.Name)
                                  .ToList();

                var codeList = competenrOrgService.GetAll().Where(x => x.Code != null).Select(x => x.Code).AsEnumerable();

                var maxCode = 0;
                
                if (codeList.Any())
                {
                    maxCode = codeList.Select(x =>
                                           {
                                               var i = 0;
                                               int.TryParse(x, out i);
                                               return i;
                                           })
                                       .Max(); 
                }

                foreach (var revSrc in data)
                {
                    var newRec = new CompetentOrgGji()
                                     {
                                         Name = revSrc,
                                         Code = (++maxCode).ToString()
                                     };

                    listToSave.Add(newRec);
                }

                using (var transaction = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        listToSave.ForEach(competenrOrgService.Save);

                        transaction.Commit();
                        return new BaseDataResult();
                    }
                    catch (ValidationException e)
                    {
                        transaction.Rollback();
                        return new BaseDataResult { Success = false, Message = e.Message };
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }
            finally
            {
                Container.Release(revenueService);
                Container.Release(competenrOrgService);
            }
        }

        public IDataResult AddContragents(BaseParams baseParams)
        {

            var objectIds = baseParams.Params.GetAs<long[]>("objectIds");
            if (objectIds == null)
            {
                return new BaseDataResult { Success = false, Message = "Необходимо выбрать записи из рестра Контрагентов" };
            }

            var contragentService = Container.Resolve<IDomainService<Contragent>>();
            var competenrOrgService = Container.Resolve<IDomainService<CompetentOrgGji>>();

            try
            {
                var listToSave = new List<CompetentOrgGji>();

                var data = contragentService.GetAll()
                                  .Where(x => objectIds.Contains(x.Id))
                                  .Where(x => x.ShortName != null && x.ShortName != "")
                                  .Where(x => !competenrOrgService.GetAll().Any(y => y.Name == x.ShortName))
                                  .Select(x => x.ShortName)
                                  .ToList();

                var codeList = competenrOrgService.GetAll().Where(x => x.Code != null).Select(x => x.Code).AsEnumerable();

                var maxCode = 0;

                if (codeList.Any())
                {
                    maxCode = codeList.Select(x =>
                    {
                        var i = 0;
                        int.TryParse(x, out i);
                        return i;
                    })
                    .Max();
                }

                foreach (var contragentShortName in data)
                {
                    var newRec = new CompetentOrgGji()
                    {
                        Name = contragentShortName,
                        Code = (++maxCode).ToString()
                    };

                    listToSave.Add(newRec);
                }

                using (var transaction = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        listToSave.ForEach(competenrOrgService.Save);

                        transaction.Commit();
                        return new BaseDataResult();
                    }
                    catch (ValidationException e)
                    {
                        transaction.Rollback();
                        return new BaseDataResult { Success = false, Message = e.Message };
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                Container.Release(contragentService);
                Container.Release(competenrOrgService);
            }
        }
    }

}
