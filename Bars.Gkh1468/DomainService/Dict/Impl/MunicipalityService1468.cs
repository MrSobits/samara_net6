namespace Bars.Gkh1468.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.Enums;

    using Castle.Windsor;

    public class MunicipalityService1468 : IMunicipalityService1468
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetMunicipalityMap()
        {
            var service = Container.Resolve<IDomainService<Municipality>>();
            var okiService = Container.Resolve<IOkiPassportService>();
            var housePaspService = Container.Resolve<IHousePassportService>();

            var today = DateTime.Today;

            var mkdPercents = housePaspService.GetPassportsPercentage(TypeRealObj.Mkd, today.Year, today.Month);
            var phPercents = housePaspService.GetPassportsPercentage(TypeRealObj.RealityObject, today.Year, today.Month);
            var pokiPercents = okiService.GetPassportsPercentage(today.Year, today.Month);

            var data =
                service.GetAll()
                       .Where(x => x.PolygonPointsArray != null)
                       .Select(x => new { id = x.Id, raion = x.Name, coords = x.PolygonPointsArray })
                       .ToList()
                       .Select(
                               x =>
                                   new
                                   {
                                       x.id,
                                       pmkd = mkdPercents.ContainsKey(x.id) ? mkdPercents[x.id] : 0M, // по МКД
                                       ph = phPercents.ContainsKey(x.id) ? phPercents[x.id] : 0M, // по ЖД
                                       poki = pokiPercents.ContainsKey(x.id) ? pokiPercents[x.id] : 0M, // по ОКИ
                                       x.raion,
                                       x.coords
                                   }).ToArray();

            return new ListDataResult(data, data.Count());
        }

        public IDataResult AddMoFromFias(BaseParams baseParams)
        {

            var fiasIds = baseParams.Params.GetAs("fiasIds", string.Empty);

            var ids = !string.IsNullOrEmpty(fiasIds) ? fiasIds.Split(',').Select(id => id.ToInt()).ToArray() : new int[0];

            if (!String.IsNullOrEmpty(fiasIds))
            {
                var moService = Container.Resolve<IDomainService<Municipality>>();
                var fiasService = Container.Resolve<IDomainService<Fias>>();

                foreach (var fiasId in ids)
                {
                    var fiasObj = fiasService.Load(fiasId);

                    var moExists = moService.GetAll().Any(x => x.FiasId == fiasObj.AOGuid || x.Okato == fiasObj.OKATO);

                    if (fiasObj != null && !moExists)
                    {
                        var mo = new Municipality
                        {
                            FiasId = fiasObj.AOGuid,
                            Okato = fiasObj.OKATO,
                            Name = fiasObj.OffName + " " + fiasObj.ShortName,
                        };

                        moService.Save(mo);
                    }
                }
            }

            return new BaseDataResult();
        }
    }
}