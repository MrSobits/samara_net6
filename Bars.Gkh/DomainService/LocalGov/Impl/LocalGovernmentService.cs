namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class LocalGovernmentService : ILocalGovernmentService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddMunicipalities(BaseParams baseParams)
        {
            var serviceLocalGovernmentMu =
                        Container.Resolve<IDomainService<LocalGovernmentMunicipality>>();

            try
            {
                var localGovId = baseParams.Params["localGovId"].ToInt();

                if (!string.IsNullOrEmpty(baseParams.Params["municipalityIds"] as string))
                {

                    var listToSave = new List<LocalGovernmentMunicipality>();

                    var municipalityIds = baseParams.Params["municipalityIds"].ToStr().Split(',');

                    var dictMunicipalities =
                        serviceLocalGovernmentMu.GetAll()
                                                          .Where(x => x.LocalGovernment.Id == localGovId)
                                                          .GroupBy(x => x.Municipality.Id)
                                                          .AsEnumerable()
                                                          .ToDictionary(
                                                              x => x.Key, y => y.Select(x => x.Id).FirstOrDefault());

                    // По переданным id инспекторов если их нет в списке существующих, то добавляем
                    foreach (var id in municipalityIds)
                    {
                        var newId = id.ToLong();

                        if (dictMunicipalities.ContainsKey(newId))
                        {
                            // Если с таким id уже есть в списке то удалем его из списка и просто пролетаем дальше
                            // без добавления в БД
                            dictMunicipalities.Remove(newId);
                            continue;
                        }

                        listToSave.Add(new LocalGovernmentMunicipality()
                                         {
                                             LocalGovernment =
                                                 new LocalGovernment { Id = localGovId },
                                             Municipality =
                                                 new Municipality { Id = newId }
                                         });

                    }

                    using (var tr = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            //Сохраняем новые
                            listToSave.ForEach(serviceLocalGovernmentMu.Save);

                            //удаляем, те которые больше ненужны
                            foreach (var keyValue in dictMunicipalities)
                            {
                                serviceLocalGovernmentMu.Delete(keyValue.Value);
                            }

                            tr.Commit();
                        }
                        catch (Exception)
                        {
                            tr.Rollback();
                            throw;
                        }
                    }


                    
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(serviceLocalGovernmentMu);
            }
        }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            var serviceMunicipality = Container.Resolve<IDomainService<LocalGovernmentMunicipality>>();

            try
            {
                var localGovId = baseParams.Params["localGovId"].ToInt();

                var municipalityNames = string.Empty;
                var municipalityIds = string.Empty;

                var dataMunicipalities =
                    serviceMunicipality.GetAll()
                                       .Where(x => x.LocalGovernment.Id == localGovId)
                                       .Select(x => new { MunicipalityId = x.Municipality.Id, x.Municipality.Name })
                                       .ToArray();

                foreach (var item in dataMunicipalities)
                {
                    if (!string.IsNullOrEmpty(municipalityNames))
                    {
                        municipalityNames += ", ";
                    }

                    municipalityNames += item.Name;

                    if (!string.IsNullOrEmpty(municipalityIds))
                    {
                        municipalityIds += ", ";
                    }

                    municipalityIds += item.MunicipalityId.ToString();
                }

                return new BaseDataResult(new { success = true, municipalityNames, municipalityIds }) { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(serviceMunicipality);
            }
        }

        public IList GetViewModelList(BaseParams baseParams, out int totalCount, bool usePaging)
        {
            var service = Container.Resolve<IDomainService<LocalGovernment>>();
            try
            {
                var loadParams = GetLoadParam(baseParams);

                var query = service.GetAll()
                .Select(x => new
                {
                    x.Id,
                    ContragentName = x.Contragent.Name,
                    x.Contragent.Inn,
                    Municipality = x.Contragent.Municipality.Name,
                    x.NameDepartamentGkh,
                    x.OfficialSite
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParams, Container);

                if (usePaging)
                {
                    totalCount = query.Count();

                    return query.Order(loadParams).Paging(loadParams).ToList();
                }

                var data = query.Order(loadParams).ToList();

                totalCount = data.Count;

                return data;
            }
            finally 
            {
                Container.Release(service);
            }
        }

        protected LoadParam GetLoadParam(BaseParams baseParams)
        {
            return baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);
        }
    }
}