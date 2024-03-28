namespace Bars.GkhGji.Regions.Tomsk.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Properties;
    using Bars.GkhGji.Report;

    public class Form1ControlServiceData : Bars.GkhGji.Report.Form1ContolServiceData
    {
        public override byte[] GetTemplate()
        {
            return Resources.TomskForm1_Kontrol;
        }

        /// <summary>
        /// Акты проверки предписаний
        /// </summary>
        protected override Dictionary<long, List<ActDataProxy>> GetDisposalsActsRemoval(Form1ContolData data, IList<long> disposalIds)
        {
            //В томске нет Актов проверки предписания , после того когда создается разспоряжение на проверку предписания просто создается новый Акт проверки
            return null;
        }

        /// <summary>
        /// Получение данных об актах распоряжений
        /// </summary>
        protected override Dictionary<long, List<ActDataProxy>> GetDisposalActData(Form1ContolData data, IList<long> municipalities, IList<long> disposalIdsNotByPrescription, IList<long> filteredDisposalsByPrescription)
        {
            var disposalIds = disposalIdsNotByPrescription;

            foreach (var id in filteredDisposalsByPrescription)
            {
                disposalIds.Add(id);
            }

            // Акты для распоряжений не по предписаниям
            var res1 = GetDisposalActs(municipalities, disposalIds);

            var disposalActsDict = res1
                .GroupBy(d => d.Key)
                .ToDictionary(d => d.Key, d => d.First().Value);

            var actIdList = disposalActsDict.SelectMany(x => x.Value.Select(y => y.actid).ToList()).ToArray();

            var disposalsByActs = disposalActsDict.Keys.ToList();

            var dictDisposalStages = DisposalDomain.GetAll()
                              .Where(x => disposalsByActs.Contains(x.Id))
                              .Select(x => new { StageId = x.Stage.Id, x.Id })
                              .AsEnumerable()
                              .GroupBy(x => x.StageId)
                              .ToDictionary(x => x.Key, y => y.Select(z => z.Id).First());

            var dispStages = dictDisposalStages.Keys.ToList();

            var actPrescriptionList = new List<ParentChildProxy>();

            if (dispStages.Any())
            {
                actPrescriptionList.AddRange(
                    serviceDocument.GetAll()
                                   .Where(x => dispStages.Contains(x.Stage.Parent.Id))
                                   .Where(x => x.TypeDocumentGji == TypeDocumentGji.Prescription)
                                   .Where(x => x.State.FinalState)
                                   .Select(x => new { parentStageId = x.Stage.Parent.Id, x.Id })
                                   .AsEnumerable()
                                   .Select(
                                       x =>
                                       new ParentChildProxy
                                           {
                                               parent = dictDisposalStages[x.parentStageId],
                                               child = x.Id
                                           })
                                   .ToList());
            }

            var actsOfPrescriptionDict = actPrescriptionList.ToDictionary(x => x.child, x => x.parent);

            var prescriptionsWithProtocolList = new List<long>();
            var actPrescriptionIds = actsOfPrescriptionDict.Keys.ToArray();
            if (actPrescriptionIds.Any())
            {
                // тут нужно получить созданные из предписаний протокола 
                prescriptionsWithProtocolList.AddRange(serviceDocumentGjiChildren.GetAll()
                    .Where(x => actPrescriptionIds.Contains(x.Parent.Id))
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .Where(x => x.Children.State.FinalState)
                    .Select(x => x.Parent.Id)
                    .Distinct());
            }

            var actsWithProtocolList = prescriptionsWithProtocolList.Select(x => actsOfPrescriptionDict[x]).ToList();

            if (actIdList.Any())
            {
                // тут получем созданные из актов протокола
                actsWithProtocolList.AddRange(serviceDocumentGjiChildren.GetAll()
                    .Where(x => actIdList.Contains(x.Parent.Id))
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .Where(x => x.Children.State.FinalState)
                    .Select(x => x.Parent.Id)
                    .Distinct());
            }

            actsWithProtocolList = actsWithProtocolList.Distinct().ToList();

            disposalActsDict.ForEach(x => x.Value.ForEach(y => y.hasDependentProtocol = actsWithProtocolList.Contains(y.actid)));

            // Теперь для того чтобы посчитать значение в строке 23 необходимо
            // для актов на проверку предписаняи получить коилчество нарушений
            // учесть что нужно брать относительно вида проверки указанного в самом первом приказе
            

            var voilationCountByDispPrescription = disposalActsDict.Where(x => filteredDisposalsByPrescription.Contains(x.Key)).SelectMany(kvp => kvp.Value).ToList();

            var voilationCountByInspection = voilationCountByDispPrescription.GroupBy(x => x.inspectionId).ToDictionary(x => x.Key, x => x.Sum(y => y.violationsCount));

            var inspectionIds = voilationCountByDispPrescription.Select(x => x.inspectionId).Distinct().ToList();

            var disposalsList = new List<DisposalProxy>();

            if (inspectionIds.Any())
            {
                disposalsList.AddRange(DisposalDomain.GetAll()

                    .Where(x => inspectionIds.Contains(x.Inspection.Id))
                    .Where(x => x.TypeDisposal == TypeDisposalGji.Base)
                    .Select(x => new DisposalProxy
                    {
                        id = x.Id,
                        kindCheckCode = ((TypeCheck?)x.KindCheck.Code) ?? 0,
                        inspectionId = x.Inspection.Id
                    })
                    .ToList());
            }

            var disposalDict = disposalsList.ToDictionary(x => x.id);

            var inspectionFirstDisposalDict = disposalsList
                .GroupBy(x => x.inspectionId)
                .ToDictionary(x => x.Key, x =>
                {
                    var disposalId = x.Select(y => y.id).First();
                    var disposalKindCheck = disposalDict[disposalId].kindCheckCode;
                    var violationsCount = voilationCountByInspection.ContainsKey(x.Key)
                                              ? voilationCountByInspection[x.Key]
                                              : 0;

                    return new { disposalKindCheck, violationsCount };
                });

            data.cell23_1 = inspectionFirstDisposalDict
                .Where(x => plannedList.Contains(x.Value.disposalKindCheck))
                .Select(x => x.Value.violationsCount)
                .Sum();

            data.cell23_2 = inspectionFirstDisposalDict
                .Where(x => unplannedList.Contains(x.Value.disposalKindCheck))
                .Select(x => x.Value.violationsCount)
                .Sum();

            return disposalActsDict;
        }

    }
}
