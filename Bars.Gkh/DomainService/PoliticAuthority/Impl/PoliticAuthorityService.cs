namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class PoliticAuthorityService : IPoliticAuthorityService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddMunicipalities(BaseParams baseParams)
        {
            try
            {
                var politicAuthId = baseParams.Params["politicAuthId"].ToInt();
                var politicAuthority = Container.Resolve<IDomainService<PoliticAuthority>>().Get(politicAuthId);
                if (!string.IsNullOrEmpty(baseParams.Params["municipalityIds"] as string))
                {
                    var municipalityIds = baseParams.Params["municipalityIds"].ToStr().Split(',');

                    var servicePoliticAuthorityMunicipality =
                        Container.Resolve<IDomainService<PoliticAuthorityMunicipality>>();

                    var dictMunicipalities =
                        servicePoliticAuthorityMunicipality.GetAll()
                                                          .Where(x => x.PoliticAuthority.Id == politicAuthId)
                                                          .GroupBy(x => x.Municipality.Id)
                                                          .AsEnumerable()
                                                          .ToDictionary(
                                                              x => x.Key, y => y.Select(x => x.Id).FirstOrDefault());

                    // По переданным id инспекторов если их нет в списке существующих, то добавляем
                    foreach (var id in municipalityIds)
                    {
                        var newId = id.ToInt();
                        var municipality = Container.Resolve<IDomainService<Municipality>>().Get(newId);

                        if (dictMunicipalities.ContainsKey(newId))
                        {
                            // Если с таким id уже есть в списке то удалем его из списка и просто пролетаем дальше
                            // без добавления в БД
                            dictMunicipalities.Remove(newId);
                            continue;
                        }

                        var newObj = new PoliticAuthorityMunicipality
                        {
                            PoliticAuthority = politicAuthority,
                            Municipality = municipality
                        };

                        servicePoliticAuthorityMunicipality.Save(newObj);
                    }

                    foreach (var keyValue in dictMunicipalities)
                    {
                        servicePoliticAuthorityMunicipality.Delete(keyValue.Value);
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            try
            {
                var politicAuthId = baseParams.Params["politicAuthId"].ToInt();

                var municipalityNames = string.Empty;
                var municipalityIds = string.Empty;

                var serviceMunicipality = Container.Resolve<IDomainService<PoliticAuthorityMunicipality>>();

                var dataMunicipalities = serviceMunicipality.GetAll()
                  .Where(x => x.PoliticAuthority.Id == politicAuthId)
                  .Select(x => new
                  {
                      MunicipalityId = x.Municipality.Id,
                      x.Municipality.Name
                  })
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

                return new BaseDataResult(new
                {
                    success = true,
                    municipalityNames,
                    municipalityIds
                }) { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }
    }
}