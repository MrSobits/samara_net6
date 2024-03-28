namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class SetMunicipalityTypeAction : BaseExecutionAction
    {
        public override string Description
            =>
                "Проставление типа муниципальных образований проимходит в зависимости от наименования (КАМЧАТКА). Удаление МО - Городской округ пос.Палана (не активный)"
            ;

        public override string Name => "Проставление типа муниципальных образований (КАМЧАТКА)";

        public override Func<IDataResult> Action => this.SetMunicipalityType;

        public BaseDataResult SetMunicipalityType()
        {
            var muRepository = this.Container.Resolve<IRepository<Municipality>>();

            foreach (var mu in muRepository.GetAll().ToList())
            {
                var muLevel = this.GetLevelByName(mu.Name.ToLower());

                if (muLevel == 0)
                {
                    continue;
                }

                mu.Level = muLevel;
                muRepository.Update(mu);
            }

            return new BaseDataResult();
        }

        private TypeMunicipality GetLevelByName(string name)
        {
            foreach (TypeMunicipality level in Enum.GetValues(typeof(TypeMunicipality)))
            {
                if (name.Contains(level.GetEnumMeta().Display.ToLower()))
                {
                    return level;
                }
            }

            return name.Contains("село") ? TypeMunicipality.Settlement : 0;
        }
    }
}