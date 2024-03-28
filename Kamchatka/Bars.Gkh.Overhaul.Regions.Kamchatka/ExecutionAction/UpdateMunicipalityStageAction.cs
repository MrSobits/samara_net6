namespace Bars.Gkh.Overhaul.Regions.Kamchatka.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;

    using Castle.Core.Internal;

    public class UpdateMunicipalityStageAction : BaseExecutionAction
    {
        public override string Name
        {
            get { return "Убрать муниципальные образования второго уровня из справочника 'Муниципальные образования' (КАМЧАТКА)"; }
        }

        public override string Description
        {
            get { return "Убрать муниципальные образования второго уровня из справочника 'Муниципальные образования' (КАМЧАТКА)"; }
        }

        public override Func<IDataResult> Action
        {
            get { return this.Execute; }
        }

        private BaseDataResult Execute()
        {
            var municipalityDomain = this.Container.ResolveDomain<Municipality>();

            try
            {
                municipalityDomain.GetAll().Where(x => x.ParentMo != null && x.IsOld)
                    .AsEnumerable()
                    .ForEach(
                        x =>
                        {
                            x.IsOld = false;
                            municipalityDomain.Update(x);
                        });
            }
            finally
            {
                this.Container.Release(municipalityDomain);
            }

            return new BaseDataResult();
        }
    }
}