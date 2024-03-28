namespace Bars.GkhGji.Regions.Nso.DomainService.Impl
{
	using System.Collections.Generic;
	using System.Linq;
    using System.Text;
    using B4;
	using B4.Modules.States;
	using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;
    using GkhGji.Entities;
    using GkhGji.Enums;

	/// <summary>
	/// Сервис для работы с Акт проверки предписания
	/// </summary>
	public class NsoActRemovalService : INsoActRemovalService
	{
		private const string ForProcessingStateCode = "2003"; //Код статуса "К обработке"
		private const string ForProcessedStateCode = "2004"; //Код статуса "Обработан"

		private readonly IDomainService<NsoActRemoval> actDomain;
        private readonly IDomainService<DocumentGjiChildren> documentGjiChildrenDomain;
        private readonly IDomainService<ActRemovalWitness> actRemovalWitnessDomain;
        private readonly IDomainService<ActRemovalPeriod> actRemovalPeriodDomain;
        private readonly IDomainService<ActRemovalProvidedDoc> actActRemovalProvidedDocDomain;
        private readonly IDomainService<ActRemovalViolation> actRemovalViolationDomain;
        private readonly IDomainService<State> stateDomain;
        private readonly IWindsorContainer container;

        public NsoActRemovalService(
            IDomainService<NsoActRemoval> actDomain,
            IDomainService<DocumentGjiChildren> documentGjiChildrenDomain,
            IDomainService<ActRemovalWitness> actRemovalWitnessDomain,
            IDomainService<ActRemovalPeriod> actRemovalPeriodDomain,
            IDomainService<ActRemovalProvidedDoc> actActRemovalProvidedDocDomain,
            IDomainService<ActRemovalViolation> actRemovalViolationDomain,
            IDomainService<State> stateDomain,
			IWindsorContainer container)
        {
            this.actDomain = actDomain;
			this.documentGjiChildrenDomain = documentGjiChildrenDomain;
			this.actRemovalWitnessDomain = actRemovalWitnessDomain;
			this.actRemovalPeriodDomain = actRemovalPeriodDomain;
			this.actActRemovalProvidedDocDomain = actActRemovalProvidedDocDomain;
			this.actRemovalViolationDomain = actRemovalViolationDomain;
			this.stateDomain = stateDomain;
			this.container = container;
        }

		/// <summary>
		/// Объединить акты проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult MergeActs(BaseParams baseParams)
	    {
			var documentId = baseParams.Params.GetAsId("documentId");
			var targetAct = this.actDomain.Get(documentId);
		    if (targetAct == null)
		    {
			    return new BaseDataResult(false, "Не найден акт проверки предписания");
		    }

		    var disposal = Utils.Utils.GetParentDocumentByType(documentGjiChildrenDomain, targetAct, TypeDocumentGji.Disposal);
		    if (disposal == null)
		    {
				return new BaseDataResult(false, "Не найден родительский приказ");
			}

			var prescription = Utils.Utils.GetParentDocumentByType(documentGjiChildrenDomain, targetAct, TypeDocumentGji.Prescription);
			if (prescription == null)
			{
				return new BaseDataResult(false, "Не найдено родительское предписание");
			}

			var sourceActs = this.documentGjiChildrenDomain.GetAll()
			    .Where(x => x.Parent.Id == disposal.Id || x.Parent.Id == prescription.Id)
			    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
			    .Where(x => x.Children.State.Code == ForProcessingStateCode)//Статус = "К обработке"
				.Where(x => x.Children.Id != documentId)
				.Select(x => x.Children as ActRemoval)
				.Distinct()
			    .ToList();

		    var sourceActIds = sourceActs
			    .Select(x => x.Id)
			    .ToArray();

			this.container.InTransaction(() =>
			{
				this.MergeWitnesses(targetAct, sourceActIds);
				this.MergePeriods(targetAct, sourceActIds);
				this.MergeViolationDescs(targetAct, sourceActs);
				this.MergeViolations(targetAct, sourceActIds);
				this.MergeProvidedDocs(targetAct, sourceActIds);
				this.ChangeState(targetAct, sourceActs);
			});

			return new BaseDataResult(true, "Объединение актов завершено");
	    }

		private void MergeWitnesses(ActRemoval targetAct, long[] sourceActIds)
		{
			this.actRemovalWitnessDomain.GetAll()
				.Where(x => sourceActIds.Contains(x.ActRemoval.Id))
				.ForEach(x =>
				{
					var witness = new ActRemovalWitness
					{
						ActRemoval = targetAct,
						Fio = x.Fio,
						IsFamiliar = x.IsFamiliar,
						Position = x.Position
					};

					this.actRemovalWitnessDomain.Save(witness);
				});
		}

		private void MergePeriods(ActRemoval targetAct, long[] sourceActIds)
		{
			this.actRemovalPeriodDomain.GetAll()
				.Where(x => sourceActIds.Contains(x.ActRemoval.Id))
				.ForEach(x =>
				{
					var period = new ActRemovalPeriod
					{
						ActRemoval = targetAct,
						DateCheck = x.DateCheck,
						DateStart = x.DateStart,
						DateEnd = x.DateEnd
					};

					this.actRemovalPeriodDomain.Save(period);
				});
		}

		private void MergeViolationDescs(ActRemoval targetAct, IEnumerable<ActRemoval> sourceActs)
		{
			var idx = 0;
			var initStr = targetAct.Description;
			var sb = new StringBuilder(initStr);
			sourceActs.ForEach(x =>
			{
				var value = x.Description;
				if (value.IsNotEmpty())
				{
					if (idx == 0 && initStr.IsEmpty())
					{
						sb.Append(value);
					}
					else
					{
						sb.AppendLine();
						sb.Append(value);
					}
				}
			});

			targetAct.Description = sb.ToString();
		}

		private void MergeProvidedDocs(ActRemoval targetAct, long[] sourceActIds)
		{
			var targetProvidedDocIds = this.actActRemovalProvidedDocDomain.GetAll()
				.Where(x => x.ActRemoval.Id == targetAct.Id)
				.Select(x => x.ProvidedDoc.Id);

			this.actActRemovalProvidedDocDomain.GetAll()
				.Where(x => sourceActIds.Contains(x.ActRemoval.Id))
				.Where(x => !targetProvidedDocIds.Contains(x.ProvidedDoc.Id))
				.GroupBy(x => x.ProvidedDoc.Id, x => x)
				.ToDictionary(x => x.Key, x => x.FirstOrDefault())
				.ForEach(x =>
				{
					var doc = new ActRemovalProvidedDoc
					{
						ActRemoval = targetAct,
						ProvidedDoc = x.Value.ProvidedDoc,
						DateProvided = x.Value.DateProvided
					};

					this.actActRemovalProvidedDocDomain.Save(doc);
				});
		}

		private void MergeViolations(ActRemoval targetAct, long[] sourceActIds)
		{
			this.actRemovalViolationDomain.GetAll()
				.Where(x => sourceActIds.Contains(x.Document.Id))
				.ForEach(x =>
				{
					var violation = new ActRemovalViolation
					{
						Document = targetAct,
						DateFactRemoval = x.DateFactRemoval,
						DatePlanRemoval = x.DatePlanRemoval,
						InspectionViolation = x.InspectionViolation,
						TypeViolationStage = x.TypeViolationStage,
						CircumstancesDescription = x.CircumstancesDescription,
						SumAmountWorkRemoval = x.SumAmountWorkRemoval
					};

					this.actRemovalViolationDomain.Save(violation);
				});
		}

		private void ChangeState(ActRemoval targetAct, ICollection<ActRemoval> listSourceActs)
		{
			var newState = this.stateDomain.GetAll()
				.FirstOrDefault(x => x.Code == ForProcessedStateCode);//Обработан

			if (newState != null)
			{
				listSourceActs.Add(targetAct);
				listSourceActs.ForEach(x =>
				{
					x.State = newState;
					this.actDomain.Update(x);
				});
			}
		}
	}
}