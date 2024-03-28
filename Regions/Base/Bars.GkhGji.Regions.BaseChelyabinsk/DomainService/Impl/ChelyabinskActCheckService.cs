namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.DocumentGji;

    using Castle.Windsor;

    /// <summary>
	/// Сервис для работы с Акт проверки
	/// </summary>
	public class ChelyabinskActCheckService : IChelyabinskActCheckService
	{
		private const string ForProcessingStateCode = "2001"; //Код статуса "К обработке"
		private const string ForProcessedStateCode = "2002"; //Код статуса "Обработан"

		private readonly IDomainService<ChelyabinskActCheck> actDomain;
        private readonly IDomainService<ActCheckRealityObject> actRoDomain;
        private readonly IDomainService<DocumentGjiChildren> documentGjiChildrenDomain;
        private readonly IDomainService<ActCheckWitness> actCheckWitnessDomain;
        private readonly IDomainService<ActCheckPeriod> actCheckPeriodDomain;
        private readonly IDomainService<ActCheckProvidedDoc> actActCheckProvidedDocDomain;
        private readonly IDomainService<ChelyabinskDocumentLongText> nsoDocumentLongTextDomain;
        private readonly IDomainService<ActCheckRealityObject> actCheckRealityObjectDomain;
        private readonly IDomainService<ActCheckViolation> actCheckViolationDomain;
        private readonly IDomainService<State> stateDomain;
        private readonly IWindsorContainer container;

        public ChelyabinskActCheckService(
            IDomainService<ChelyabinskActCheck> actDomain,
            IDomainService<ActCheckRealityObject> actRoDomain,
            IDomainService<DocumentGjiChildren> documentGjiChildrenDomain,
            IDomainService<ActCheckWitness> actCheckWitnessDomain,
            IDomainService<ActCheckPeriod> actCheckPeriodDomain,
            IDomainService<ActCheckProvidedDoc> actActCheckProvidedDocDomain,
            IDomainService<ChelyabinskDocumentLongText> nsoDocumentLongTextDomain,
            IDomainService<ActCheckRealityObject> actCheckRealityObjectDomain,
            IDomainService<ActCheckViolation> actCheckViolationDomain,
            IDomainService<State> stateDomain,
			IWindsorContainer container)
        {
            this.actDomain = actDomain;
            this.actRoDomain = actRoDomain;
			this.documentGjiChildrenDomain = documentGjiChildrenDomain;
			this.actCheckWitnessDomain = actCheckWitnessDomain;
			this.actCheckPeriodDomain = actCheckPeriodDomain;
			this.actActCheckProvidedDocDomain = actActCheckProvidedDocDomain;
			this.nsoDocumentLongTextDomain = nsoDocumentLongTextDomain;
			this.actCheckRealityObjectDomain = actCheckRealityObjectDomain;
			this.actCheckViolationDomain = actCheckViolationDomain;
			this.stateDomain = stateDomain;
			this.container = container;
        }

		/// <summary>
		/// Проверить, есть ли нарушения
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult IsAnyHasViolation(BaseParams baseParams)
        {
            var actId = baseParams.Params.GetAsId("actId");

            var act = this.actDomain.Get(actId);

            return new BaseDataResult(this.IsAnyHasViolation(act), "");
        }

		/// <summary>
		/// Проверить, есть ли нарушения
		/// </summary>
		/// <param name="act">Акт проверки</param>
		/// <returns>Результат выполнения запроса</returns>
		public bool IsAnyHasViolation(ChelyabinskActCheck act)
        {
            ArgumentChecker.NotNull(act, "act");

            return this.actRoDomain.GetAll()
                .Where(x => x.ActCheck.Id == act.Id)
                .Any(x => x.HaveViolation == YesNoNotSet.Yes);
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
			    return new BaseDataResult(false, "Не найден акт проверки");
		    }

		    var disposal = Utils.Utils.GetParentDocumentByType(this.documentGjiChildrenDomain, targetAct, TypeDocumentGji.Disposal);
		    if (disposal == null)
		    {
				return new BaseDataResult(false, "Не найдено родительский распоряжение");
			}

		    var sourceActs = this.documentGjiChildrenDomain.GetAll()
			    .Where(x => x.Parent.Id == disposal.Id)
			    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
			    .Where(x => x.Children.State.Code == ChelyabinskActCheckService.ForProcessingStateCode)//Статус = "К обработке"
				.Where(x => x.Children.Id != documentId)
				.Select(x => x.Children)
			    .ToList();

		    var sourceActIds = sourceActs
			    .Select(x => x.Id)
			    .ToArray();

			this.container.InTransaction(() =>
			{
				this.MergeWitnesses(targetAct, sourceActIds);
				this.MergePeriods(targetAct, sourceActIds);
				this.MergeViolationDescs(targetAct, sourceActIds);
				this.MergeViolations(targetAct, sourceActIds);
				this.MergeProvidedDocs(targetAct, sourceActIds);
				this.ChangeState(targetAct, sourceActs);
			});

			return new BaseDataResult(true, "Объединение актов завершено");
	    }

		private void MergeWitnesses(ActCheck targetAct, long[] sourceActIds)
		{
			this.actCheckWitnessDomain.GetAll()
				.Where(x => sourceActIds.Contains(x.ActCheck.Id))
				.ForEach(x =>
				{
					var witness = new ActCheckWitness
					{
						ActCheck = targetAct,
						Fio = x.Fio,
						IsFamiliar = x.IsFamiliar,
						Position = x.Position
					};

					this.actCheckWitnessDomain.Save(witness);
				});
		}

		private void MergePeriods(ActCheck targetAct, long[] sourceActIds)
		{
			this.actCheckPeriodDomain.GetAll()
				.Where(x => sourceActIds.Contains(x.ActCheck.Id))
				.ForEach(x =>
				{
					var period = new ActCheckPeriod
					{
						ActCheck = targetAct,
						DateCheck = x.DateCheck,
						DateStart = x.DateStart,
						DateEnd = x.DateEnd
					};

					this.actCheckPeriodDomain.Save(period);
				});
		}

		private void MergeViolationDescs(ActCheck targetAct, long[] sourceActIds)
		{
			var targetLongText = this.nsoDocumentLongTextDomain.GetAll()
				.FirstOrDefault(y => y.DocumentGji.Id == targetAct.Id)
			                     ?? new ChelyabinskDocumentLongText {DocumentGji = targetAct};

			var initStr = targetLongText.ViolationDescription != null
				? Encoding.UTF8.GetString(targetLongText.ViolationDescription)
				: "";

			var idx = 0;
			var sb = new StringBuilder(initStr);
			this.nsoDocumentLongTextDomain.GetAll()
				.Where(y => sourceActIds.Contains(y.DocumentGji.Id))
				.ForEach(x =>
				{
					if (x.ViolationDescription != null)
					{
						var value = Encoding.UTF8.GetString(x.ViolationDescription);
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
					}
					idx++;
				});

			targetLongText.ViolationDescription = Encoding.UTF8.GetBytes(sb.ToString());
			this.nsoDocumentLongTextDomain.SaveOrUpdate(targetLongText);
		}

		private void MergeProvidedDocs(ActCheck targetAct, long[] sourceActIds)
		{
			var targetProvidedDocIds = this.actActCheckProvidedDocDomain.GetAll()
				.Where(x => x.ActCheck.Id == targetAct.Id)
				.Select(x => x.ProvidedDoc.Id);

			this.actActCheckProvidedDocDomain.GetAll()
				.Where(x => sourceActIds.Contains(x.ActCheck.Id))
				.Where(x => !targetProvidedDocIds.Contains(x.ProvidedDoc.Id))
				.GroupBy(x => x.ProvidedDoc.Id, x => x)
				.ToDictionary(x => x.Key, x => x.FirstOrDefault())
				.ForEach(x =>
				{
					var doc = new ActCheckProvidedDoc
					{
						ActCheck = targetAct,
						ProvidedDoc = x.Value.ProvidedDoc,
						DateProvided = x.Value.DateProvided
					};

					this.actActCheckProvidedDocDomain.Save(doc);
				});
		}

		private void MergeViolations(ActCheck targetAct, long[] sourceActIds)
		{
			var targetActRobjects = this.actCheckRealityObjectDomain.GetAll()
				.Where(x => x.ActCheck.Id == targetAct.Id)
				.GroupBy(x => x.RealityObject.Id, x => x)
				.ToDictionary(x => x.Key, x => x.First());

			this.actCheckRealityObjectDomain.GetAll()
				.Where(x => sourceActIds.Contains(x.ActCheck.Id))
				.GroupBy(x => x.RealityObject.Id, x => x)
				.ToDictionary(x => x.Key, x => x.ToArray())
				.ForEach(x =>
				{
					var actRobject = targetActRobjects.Get(x.Key);
					if (actRobject == null)
					{
						var source = x.Value.First();
						actRobject = new ActCheckRealityObject
						{
							ActCheck = targetAct,
							RealityObject = source.RealityObject,
							Description = source.Description,
							NotRevealedViolations = source.NotRevealedViolations,
							HaveViolation = source.HaveViolation,
							PersonsWhoHaveViolated = source.PersonsWhoHaveViolated,
							OfficialsGuiltyActions = source.OfficialsGuiltyActions
						};

						this.actCheckRealityObjectDomain.Save(actRobject);
					}

					var sourceRobjectIds = x.Value.Select(y => y.Id).ToArray();
					this.actCheckViolationDomain.GetAll()
						.Where(y => sourceRobjectIds.Contains(y.ActObject.Id))
						.ForEach(z =>
						{
							var violation = new ActCheckViolation
							{
								Document = targetAct,
								ActObject = actRobject,
								DateFactRemoval = z.DateFactRemoval,
								DatePlanRemoval = z.DatePlanRemoval,
								InspectionViolation = z.InspectionViolation,
								TypeViolationStage = z.TypeViolationStage,
								SumAmountWorkRemoval = z.SumAmountWorkRemoval
							};

							this.actCheckViolationDomain.Save(violation);
						});
				});
		}

		private void ChangeState(DocumentGji targetAct, ICollection<DocumentGji> listSourceActs)
		{
			var newState = this.stateDomain.GetAll()
				.FirstOrDefault(x => x.Code == ChelyabinskActCheckService.ForProcessedStateCode);//Обработан

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