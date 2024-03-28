using Bars.B4;
using Bars.B4.Modules.NH.Extentions;
using Bars.Gkh.Overhaul.Entities;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Castle.Windsor;
using System;

namespace Bars.Gkh.Overhaul.Hmao.DomainService.Version.Impl
{
    public class AddVersionRecord: IAddVersionRecord
    {
        private readonly IWindsorContainer container;

        public AddVersionRecord(IWindsorContainer container)
        {
            this.container = container;
        }

        public IDomainService<VersionRecordStage1> Stage1Domain { get; set; }

        public IDomainService<VersionRecordStage2> Stage2Domain { get; set; }

        public IDomainService<VersionRecord> Stage3Domain { get; set; }

        /// <summary>
        /// Добавить запись в версию
        /// </summary>
        public void Add(ProgramVersion version, RealityObjectStructuralElement rostructel, decimal sum, decimal volume, short year)
        {
            var estateObject = rostructel.StructuralElement.Group?.CommonEstateObject;
            if (estateObject == null)
                throw new ApplicationException($"Не найден ООИ для КЭ {rostructel.StructuralElement.Name}");

            container.InTransaction(() =>
            {
                var stage3 = new VersionRecord
                {
                    ProgramVersion = version,
                    RealityObject = rostructel.RealityObject,
                    Year = year,
                    YearCalculated = year,
                    FixedYear = false,
                    CommonEstateObjects = estateObject.Name,
                    Sum = sum,
                    IndexNumber = 0,
                    Show = true,
                };

                Stage3Domain.Save(stage3);

                var stage2 = new VersionRecordStage2
                {
                    Stage3Version = stage3,
                    CommonEstateObjectWeight = 0,
                    Sum = sum,
                    CommonEstateObject = estateObject
                };

                Stage2Domain.Save(stage2);

                var stage1 = new VersionRecordStage1
                {
                    Stage2Version = stage2,
                    RealityObject = rostructel.RealityObject,
                    StructuralElement = rostructel,
                    Year = year,
                    Sum = sum,
                    SumService = 0,
                    Volume = volume, 
                    VersionRecordState = Enum.VersionRecordState.Actual
                };

                Stage1Domain.Save(stage1);
            });
        }
    }
}
