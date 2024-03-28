namespace Bars.Gkh.Overhaul.SystemDataTransfer
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.SystemDataTransfer.Meta;

    internal class TransferEntityProvider : ITransferEntityProvider
    {
        /// <inheritdoc />
        public void FillContainer(TransferEntityContainer container)
        {
            container.Add<GroupType>("Тип группы ООИ").AddComparer(x => x.Name);
            container.Add<CommonEstateObject>("Объект общего имущества").AddComparer(x => x.Name);
            container.Add<StructuralElementGroup>("Группа конструктивных элементов").AddComparer(x => x.Name, x => x.CommonEstateObject);
            container.Add<StructuralElement>("Конструктивный элемент").AddComparer(x => x.Name, x => x.Group);
            container.Add<NormativeDoc>("Нормативный документ").AddComparer(x => x.Name, x => x.Code);
            container.Add<RealityObjectStructuralElement>("Конструктивный элемент дома").AddComparer(x => x.RealityObject, x => x.StructuralElement);

            container.Add<Job>("Работа").AddComparer(x => x.Work, x => x.Name);
            container.Add<WorkPrice>("Расценки по работе").AddComparer(x => x.Job, x => x.Municipality);

            container.Add<CreditOrg>("Кредитные организации").AddComparer(x => x.Inn, x => x.Kpp).AddComparer(x => x.Parent);
            container.Add<ContragentBankCreditOrg>("Кредитные организации контрагента").AddComparer(x => x.CreditOrg, x => x.Contragent);
            container.Add<ContragentBank>("Банк контрагента").AddComparer(x => x.Contragent, x => x.SettlementAccount);
        }
    }
}