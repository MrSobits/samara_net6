namespace Bars.GkhCr.SystemDataTransfer
{
    using Bars.Gkh.SystemDataTransfer.Meta;
    using Bars.GkhCr.Entities;

    internal class TransferEntityProvider : ITransferEntityProvider
    {
        /// <inheritdoc />
        public void FillContainer(TransferEntityContainer container)
        {
            container.Add<FinanceSource>("Разрез финансирования по КР").AddComparer(x => x.Code);
            container.Add<StageWorkCr>("Этапы работ капитального ремонта").AddComparer(x => x.Code);
            container.Add<QualificationMember>("Участники квалификационного отбора").AddComparer(x => x.Name, x => x.Role);
            container.Add<FinanceSourceWork>("Работы источника финансирования по КР").AddComparer(x => x.Work, x => x.FinanceSource);

            container.Add<Competition>("Конкурс на проведение работ").AddComparer(x => x.NotifNumber, x => x.NotifDate);
            container.Add<CompetitionLot>("Лот конкурса").AddComparer(x => x.Competition, x => x.LotNumber).AddComparer(x => x.StartingPrice);
            container.Add<CompetitionLotBid>("Заявка лота").AddComparer(x => x.Builder, x => x.Lot).AddComparer(x => x.IncomeDate);
            container.Add<CompetitionProtocol>("Протокол конкурса").AddComparer(x => x.Competition, x => x.SignDate);
            container.Add<CompetitionDocument>("Документ конкурса")
                .AddComparer(x => x.Competition, x => x.DocumentName)
                .AddComparer(x => x.DocumentNumber, x => x.DocumentDate);

            container.Add<ProgramCr>("Программа капитального ремонта").AddComparer(x => x.Period, x => x.Code).AddComparer(x => x.Name);
            container.Add<ControlDate>("Контрольные сроки вида работ").AddComparer(x => x.ProgramCr, x => x.Work).AddComparer(x => x.Date);
            container.Add<ObjectCr>("Объекты капитального ремонта").AddComparer(x => x.ProgramCr, x => x.RealityObject);
            container.Add<ProtocolCr>("Протокол(акт)").AddComparer(x => x.ObjectCr, x => x.TypeDocumentCr).AddComparer(x => x.TypeWork, x => x.DocumentNum);
            container.Add<ProtocolCrTypeWork>("Виды работ протокола(акта) КР").AddComparer(x => x.Protocol, x => x.TypeWork);
            container.Add<DefectList>("Дефектная ведомость").AddComparer(x => x.ObjectCr, x => x.TypeWork).AddComparer(x => x.Work);
            container.Add<TypeWorkCr>("Вид работы КР").AddComparer(x => x.ObjectCr, x => x.Work).AddComparer(x => x.StageWorkCr, x => x.FinanceSource);
            container.Add<FinanceSourceResource>("Средства источника финансирования").AddComparer(x => x.ObjectCr, x => x.FinanceSource).AddComparer(x => x.Year);
            container.Add<DesignAssignment>("Задание на проектирование").AddComparer(x => x.ObjectCr, x => x.Document);
            container.Add<DesignAssignmentTypeWorkCr>("Связь Задание на проектирование и Вид работ").AddComparer(x => x.TypeWorkCr, x => x.DesignAssignment);
            container.Add<CompetitionLotTypeWork>("Связь Лота с Объектом КР").AddComparer(x => x.TypeWork, x => x.Lot);
            container.Add<EstimateCalculation>("Сметный расчет по работе").AddComparer(x => x.ObjectCr, x => x.TypeWorkCr);
            container.Add<Estimate>("Смета").AddComparer(x => x.EstimateCalculation);
            container.Add<ResourceStatement>("Ведомость ресурсов").AddComparer(x => x.EstimateCalculation);
            container.Add<Qualification>("Квалификационный отбор").AddComparer(x => x.Builder, x => x.ObjectCr);
            container.Add<VoiceMember>("Голос участника квалификационного отбора").AddComparer(x => x.QualificationMember, x => x.Qualification);
            container.Add<BuildContract>("Договор подряда КР").AddComparer(x => x.ObjectCr, x => x.TypeWork).AddComparer(x => x.Builder, x => x.Contragent);
            container.Add<BuildContractTypeWork>("Виды работ договора подряда КР").AddComparer(x => x.BuildContract, x => x.TypeWork);
            container.Add<MonitoringSmr>("Мониторинг СМР").AddComparer(x => x.ObjectCr);
            container.Add<DocumentWorkCr>("Документ работы объекта КР").AddComparer(x => x.ObjectCr, x => x.Contragent).AddComparer(x => x.TypeWork);
            container.Add<PerformedWorkAct>("Акт выполненных работ").AddComparer(x => x.TypeWorkCr, x => x.ObjectCr).AddComparer(x => x.DocumentNum);
            container.Add<PerformedWorkActPayment>("Распоряжение к оплате акта").AddComparer(x => x.PerformedWorkAct, x => x.TransferGuid);
            container.Add<TerminationReason>("Причина расторжения договора объекта КР").AddComparer(x => x.Code, x => x.Name);
        }
    }
}