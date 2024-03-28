namespace Bars.GkhGji.Regions.Nso.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Gkh.Domain;
    using Gkh.Entities;
    using Gkh.Utils;
    using GkhGji.Entities;

    public class ActCheckRoNsoService : IActCheckRoNsoService
    {
        private readonly IDomainService<ActCheckRealityObject> _actRoDomain;

        public ActCheckRoNsoService(IDomainService<ActCheckRealityObject> actRoDomain)
        {
            _actRoDomain = actRoDomain;
        }

        public IDataResult GetRobjectCharacteristics(BaseParams baseParams)
        {
            var actRoId = baseParams.Params.GetAsId("actRoId");

            var ro = _actRoDomain.GetAll()
                .Where(x => x.Id == actRoId)
                .Select(x => x.RealityObject)
                .FirstOrDefault();

            return new BaseDataResult(new {chars = GetRobjectChars(ro)});
        }

        private string GetRobjectChars(RealityObject ro)
        {
            if (ro == null) return null;

            var result = new List<string>();

            if (ro.BuildYear.HasValue)
                result.Add(string.Format("Год постройки: {0}", ro.BuildYear.Value));

            if (ro.PhysicalWear.HasValue)
                result.Add(string.Format("% физического износа: {0}", ro.PhysicalWear.Value.RoundDecimal(2)));

            if (ro.Floors.HasValue)
                result.Add(string.Format("Этажность: {0}", ro.Floors.Value));

            if(ro.WallMaterial != null)
                result.Add(string.Format("Материалы стен: {0}", ro.WallMaterial.Name));

            if(ro.RoofingMaterial != null)
                result.Add(string.Format("Тип кровли: {0}", ro.RoofingMaterial.Name));

            if(ro.NumberApartments.HasValue)
                result.Add(string.Format("Количество квартир: {0}", ro.NumberApartments.Value));

            if(ro.AreaLiving.HasValue)
                result.Add(string.Format("Площадь жилых помещений: {0}", ro.AreaLiving.Value.RoundDecimal(2)));

            if (ro.AreaNotLivingFunctional.HasValue)
                result.Add(string.Format("Площадь нежилых помещений: {0}", ro.AreaNotLivingFunctional.Value.RoundDecimal(2)));

            result.Add(string.Format("Наличие лифтов: {0}", ro.NumberLifts.ToInt() > 0 ? "Да" : "Нет"));
                
            if(ro.NumberEntrances.HasValue)
                result.Add(string.Format("Количество подъездов: {0}", ro.NumberEntrances.Value));

            if (ro.HeatingSystem != 0)
                result.Add(string.Format("Степень благоустройства: Система отопления-{0}", ro.HeatingSystem.GetEnumMeta().Display));

            return result.AggregateWithSeparator("; ");
        }
    }
}