namespace Bars.Gkh.Overhaul.Hmao.SystemDataTransfer
{
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities.Version;
    using Bars.Gkh.SystemDataTransfer.Meta;

    internal class TransferEntityProvider : ITransferEntityProvider
    {
        /// <inheritdoc />
        public void FillContainer(TransferEntityContainer container)
        {
            container.Replace<WorkPrice, HmaoWorkPrice>("Расценки по работе").AddComparer(x => x.Job, x => x.Municipality).AddComparer(x => x.RealEstateType);
            container.Add<ShareFinancingCeo>("Доля финанскирования ООИ").AddComparer(x => x.CommonEstateObject);
            container.Add<PublishedProgram>("Опубликованная программа").AddComparer(x => x.ProgramVersion);
            container.Add<PublishedProgramRecord>("Запись Опубликованной программы")
                .AddComparer(x => x.PublishedProgram, x => x.Stage2)
                .AddComparer(x => x.RealityObject);

            container.Add<DpkrCorrectionStage2>("Сущность, содержащая данные, необходимые при учете корректировки ДПКР")
                .AddComparer(x => x.RealityObject, x => x.Stage2);

            container.Add<DpkrGroupedYear>("DpkrGroupedYear").AddComparer(x => x.RealityObject, x => x.Year);

            container.Add<MissingByMargCostDpkrRec>("Отсутствующий по предельной записи")
                .AddComparer(x => x.RealityObject, x => x.Year)
                .AddComparer(x => x.CommonEstateObjects);

            container.Add<RealityObjectStructuralElementInProgramm>("Конструктивный элемент дома в ДПКР")
                .AddComparer(x => x.StructuralElement, x => x.Stage2);

            container.Add<RealityObjectStructuralElementInProgrammStage2>("Конструктивный элемент дома в ДПКР (этап 2)")
                .AddComparer(x => x.RealityObject, x => x.CommonEstateObject).AddComparer(x => x.Stage3);

            container.Add<RealityObjectStructuralElementInProgrammStage3>("Конструктивный элемент дома в ДПКР (этап 3)")
                .AddComparer(x => x.CommonEstateObjects, x => x.RealityObject).AddComparer(x => x.Year);

            container.Add<ShortProgramDifitsit>("Дифицит по МО для года в краткосрочной программе КР")
                .AddComparer(x => x.Municipality, x => x.Version).AddComparer(x => x.Year);

            container.Add<ShortProgramRecord>(
                    "Запись краткосрочной программы в которой бует подробно разбиение из каких Бюджетов будет финансирвоатся выполнение работ по всему ООИ")
                .AddComparer(x => x.RealityObject, x => x.Stage2);

            container.Add<DefaultPlanCollectionInfo>("Плановые показатели собираемости (по умолчанию)")
                .AddComparer(x => x.Year);

            container.Add<SubsidyRecord>("Запись субсидии").AddComparer(x => x.Municiaplity, x => x.SubsidyYear);
            container.Add<SubsidyRecordVersion>("Запись субсидии (Версия программы)")
                .AddComparer(x => x.Version, x => x.SubsidyRecord).AddComparer(x => x.BudgetCr, x => x.SubsidyYear);

            container.Add<VersionRecordStage1>("Версионирование первого этапа").AddComparer(x => x.Stage2Version, x => x.RealityObject)
                .AddComparer(x => x.StructuralElement);

            container.Add<VersionRecordStage2>("Версионирование второго этапа").AddComparer(x => x.Stage3Version, x => x.CommonEstateObject);
            container.Add<ProgramVersion>("Версия программы").AddComparer(x => x.Name, x => x.Municipality);
            container.Add<TypeWorkCrVersionStage1>("Связь записи первого этапа версии и видов работ").AddComparer(x => x.Stage1Version, x => x.TypeWorkCr);
            container.Add<VersionActualizeLog>("Лог актуализации версии")
                .AddComparer(x => x.ProgramVersion, x => x.Municipality)
                .AddComparer(x => x.ProgramCrName);

            container.Add<VersionParam>("Версионируемые параметры").AddComparer(x => x.ProgramVersion, x => x.Municipality).AddComparer(x => x.Code);
            container.Add<VersionRecord>("Запись в версии программы").AddComparer(x => x.ProgramVersion, x => x.RealityObject).AddComparer(x => x.CommonEstateObjects);
            container.Add<CurrentPrioirityParams>("Параметры оценки").AddComparer(x => x.Code, x => x.Municipality);
            container.Add<DpkrParams>("Параметры ДПКР").AddComparer(x => x.Params);
        }
    }
}