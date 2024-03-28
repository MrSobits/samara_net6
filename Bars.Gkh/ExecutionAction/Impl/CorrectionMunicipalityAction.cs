namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class CorrectionMunicipalityAction : BaseExecutionAction
    {
        private Dictionary<long, long?> MunicipalityDictionary { get; set; }

        public override string Description
            => "Изменение муниципального образования у тех домов, где в качестве муниципального образования указаны МО нижних уровней (КАМЧАТКА)";

        public override string Name => "Исправление муниципального образования домов (КАМЧАТКА)";

        public override Func<IDataResult> Action => this.CorrectionMunicipality;

        public BaseDataResult CorrectionMunicipality()
        {
            var municipalityRepository = this.Container.Resolve<IRepository<Municipality>>();
            var realtyObjRepository = this.Container.Resolve<IRepository<RealityObject>>();

            using (this.Container.Using(municipalityRepository, realtyObjRepository))
            {
                this.MunicipalityDictionary =
                    municipalityRepository.GetAll()
                        .Select(x => new {x.Id, ParentId = (long?) x.ParentMo.Id})
                        .AsEnumerable()
                        .ToDictionary(x => x.Id, x => x.ParentId);

                var incorrectRo =
                    realtyObjRepository.GetAll()
                        .Where(x => x.Municipality != null)
                        .Where(x => x.Municipality.ParentMo != null)
                        .Where(x => x.Municipality.Level != TypeMunicipality.MunicipalArea)
                        .ToList();

                foreach (var ro in incorrectRo)
                {
                    if (ro == null)
                    {
                        continue;
                    }

                    var municipalAreaId = this.GetMunicipalUnion(ro.Municipality.Id);

                    ro.Municipality = new Municipality {Id = municipalAreaId};

                    realtyObjRepository.Update(ro);
                }
            }

            return new BaseDataResult();
        }

        private long GetMunicipalUnion(long childMunicipalityId)
        {
            if (this.MunicipalityDictionary.ContainsKey(childMunicipalityId))
            {
                var parentMuId = this.MunicipalityDictionary[childMunicipalityId];

                if (!parentMuId.HasValue || !this.MunicipalityDictionary.ContainsKey(parentMuId.Value))
                {
                    return childMunicipalityId;
                }

                if (this.MunicipalityDictionary[parentMuId.Value] != null)
                {
                    parentMuId = this.GetMunicipalUnion(parentMuId.Value);
                }

                if (this.MunicipalityDictionary[parentMuId.Value] == null)
                {
                    return parentMuId.Value;
                }
            }

            return childMunicipalityId;
        }
    }
}