namespace Bars.GkhGji.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    using Castle.Windsor;

    /// <summary>
	/// Сервис для работы с "Предметы проверки"
	/// </summary>
	public class DisposalVerificationSubjectService : IDisposalVerificationSubjectService
	{
		/// <summary>
		/// Контейнер
		/// </summary>
		public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Домен-сервис для "Приказ"
		/// </summary>
		public IDomainService<Disposal> DisposalDomain { get; set; }

		/// <summary>
		/// Домен сервис для "Предмет проверки Приказа"
		/// </summary>
		public IDomainService<DisposalVerificationSubject> DisposalSurveySubjectDomain { get; set; }

		/// <summary>
		/// Домен сервис для "Предмет проверки"
		/// </summary>
		public IDomainService<SurveySubject> SurveySubjectDomain { get; set; }

		/// <summary>
		/// Домен сервис для "Цель проверки"
		/// </summary>
		public IDomainService<TypeSurveyGoalInspGji> TypeSurveyPurposeDomain { get; set; }

		/// <summary>
		/// Домен сервис для "Задачи проверки"
		/// </summary>
		public IDomainService<TypeSurveyTaskInspGji> TypeSurveyTaskInspGjiDomain { get; set; }

		/// <summary>
		/// Домен сервис для "Правовое основание проведения проверки"
		/// </summary>
		public IDomainService<TypeSurveyInspFoundationGji> TypeSurveyInspFoundationGjiDomain { get; set; }

		/// <summary>
		/// Домен сервис для "НПА проверки"
		/// </summary>
		public IDomainService<TypeSurveyInspFoundationCheckGji> TypeSurveyInspFoundationCheckGjiDomain { get; set; }

		/// <summary>
		/// Домен сервис для "Административные регламенты"
		/// </summary>
		public IDomainService<TypeSurveyAdminRegulationGji> TypeSurveyAdminRegulationGjiDomain { get; set; }

		/// <summary>
		/// Домен сервис для "Предоставляемый документ Типа обследования"
		/// </summary>
		public IDomainService<TypeSurveyProvidedDocumentGji> TypeSurveyProvidedDocumentGjiDomain { get; set; }

		/// <summary>
		/// Домен сервис для "Тип обследования ГЖИ"
		/// </summary>
		public IDomainService<TypeSurveyGji> TypeSurveyDomain { get; set; }

		/// <summary>
		/// Домен сервис для "Вид проверки"
		/// </summary>
		public IDomainService<TypeSurveyKindInspGji> TypeSurveyKindDomain { get; set; }

		/// <summary>
		/// Сервис для работы с "Тип обследования приказа"
		/// </summary>
		public IDisposalTypeSurveyService DisposalTypeSurveyService { get; set; }

		/// <summary>
		/// Сервис для работы с "Цели проверки приказа"
		/// </summary>
		public IDisposalSurveyPurposeService DisposalSurveyPurposeService { get; set; }

		/// <summary>
		/// Сервис для работы с "Задачи проверки приказа"
		/// </summary>
		public IDisposalSurveyObjectiveService DisposalSurveyObjectiveService { get; set; }

		/// <summary>
		/// Сервис для работы с "НПА требования приказа"
		/// </summary>
		public IDisposalInsFoundationService DisposalInsFoundationService { get; set; }

		/// <summary>
		/// Сервис для работы с "НПА проверки приказа"
		/// </summary>
		public IDisposalInsFoundationCheckService DisposalInsFoundationCheckService { get; set; }

		/// <summary>
		/// Сервис для работы с "Административные регламенты приказа"
		/// </summary>
		public IDisposalAdminRegulationService DisposalAdminRegulationService { get; set; }

		/// <summary>
		/// Сервис для работы с "Предоставленные документы приказа"
		/// </summary>
		public IDisposalProvidedDocService DisposalProvidedDocService { get; set; }

		/// <summary>
		/// Добавить предметы проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult AddSurveySubjects(BaseParams baseParams)
		{
			var documentId = baseParams.Params.GetAs<long>("documentId");
			var ids = baseParams.Params.GetAs<long[]>("ids") ?? new long[0];
			var result = this.AddSurveySubjects(documentId, ids);

			return result;
		}

		/// <summary>
		/// Добавить предметы проверки
		/// </summary>
		/// <param name="documentId">Идентификатор документа</param>
		/// <param name="ids">Идентификаторы новых записей</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult AddSurveySubjects(long documentId, long[] ids)
		{
			try
			{
				var disposal = this.DisposalDomain.Get(documentId);
				if (disposal == null)
				{
					return new BaseDataResult(false, "Не найден документ распоряжения");
				}

				this.Container.InTransaction(() =>
				{
					var extingIds = this.DisposalSurveySubjectDomain.GetAll()
						.Where(x => x.Disposal.Id == documentId)
						.Select(x => x.SurveySubject.Id)
						.ToArray();

					var uniqueIds = new List<long>();

					foreach (var id in ids)
					{
						if (!extingIds.Contains(id))
						{
							var newObj = new DisposalVerificationSubject
							{
								Disposal = new Disposal { Id = documentId },
								SurveySubject = new SurveySubject() { Id = id }
							};

							this.DisposalSurveySubjectDomain.Save(newObj);
							uniqueIds.Add(id);
						}
					}

					var codes = this.SurveySubjectDomain.GetAll()
						.Where(x => uniqueIds.Contains(x.Id))
						.Select(x => x.Code)
						.ToArray();

					if (codes.IsNotEmpty())
					{
						var typeSurveyIds = this.TypeSurveyDomain.GetAll()
							.Where(x => codes.Contains(x.Code))
							.Select(x => x.Id)
							.ToArray();

						typeSurveyIds = this.TypeSurveyKindDomain.GetAll()
							.Where(x => typeSurveyIds.Contains(x.TypeSurvey.Id))
							.Where(x => x.KindCheck.Id == disposal.KindCheck.Id)
							.Select(x => x.TypeSurvey.Id)
							.Distinct()
							.ToArray();

						var purposeIds = this.TypeSurveyPurposeDomain.GetAll()
							.Where(x => typeSurveyIds.Contains(x.TypeSurvey.Id))
							.Select(x => x.SurveyPurpose.Id)
							.ToArray();

						var objectiveIds = this.TypeSurveyTaskInspGjiDomain.GetAll()
							.Where(x => typeSurveyIds.Contains(x.TypeSurvey.Id))
							.Select(x => x.SurveyObjective.Id)
							.ToArray();

						var insFoundIds = this.TypeSurveyInspFoundationGjiDomain.GetAll()
							.Where(x => typeSurveyIds.Contains(x.TypeSurvey.Id))
							.Select(x => x.NormativeDoc.Id)
							.ToArray();

						var insFoundCheckIds = this.TypeSurveyInspFoundationCheckGjiDomain.GetAll()
							.Where(x => typeSurveyIds.Contains(x.TypeSurvey.Id))
							.Select(x => x.NormativeDoc.Id)
							.ToArray();

						var adminRegIds = this.TypeSurveyAdminRegulationGjiDomain.GetAll()
							.Where(x => typeSurveyIds.Contains(x.TypeSurvey.Id))
							.Select(x => x.NormativeDoc.Id)
							.ToArray();

						var providedDocIds = this.TypeSurveyProvidedDocumentGjiDomain.GetAll()
							.Where(x => typeSurveyIds.Contains(x.TypeSurvey.Id))
							.Select(x => x.ProvidedDocGji.Id)
							.ToArray();

						this.DisposalTypeSurveyService.AddTypeSurveys(documentId, typeSurveyIds);
						this.DisposalSurveyPurposeService.AddSurveyPurposes(documentId, purposeIds);
						this.DisposalSurveyObjectiveService.AddSurveyObjectives(documentId, objectiveIds);
						this.DisposalInsFoundationService.AddInspFoundations(documentId, insFoundIds);
						this.DisposalInsFoundationCheckService.AddInspFoundationChecks(documentId, insFoundCheckIds);
						this.DisposalAdminRegulationService.AddAdminRegulations(documentId, adminRegIds);
						this.DisposalProvidedDocService.AddProvidedDocs(documentId, providedDocIds);
					}
				});

				return new BaseDataResult();
			}
			catch (ValidationException e)
			{
				return new BaseDataResult { Success = false, Message = e.Message };
			}
			catch (Exception)
			{
				return new BaseDataResult { Success = false, Message = "Произошла ошибка при добавлении предметов проверки" };
			}
		}

		public IDomainService<DecisionVerificationSubjectNormDocItem> NormDocItemDomain { get; set; }

		/// <summary>
		/// Добавить Требования НПА проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult AddNormDocItems(BaseParams baseParams)
		{
			var subjId = baseParams.Params.GetAsId("subjId");
			var ids = baseParams.Params.GetAs("ids", new long[0]);

			try
			{
				var existIds = this.NormDocItemDomain.GetAll()
					.Where(x => x.DecisionVerificationSubject.Id == subjId)
					.Select(x => x.NormativeDocItem.Id)
					.ToArray();

				var idsForSave = ids.Distinct().Except(existIds);

				foreach (var id in idsForSave)
				{
					var newRef = new DecisionVerificationSubjectNormDocItem
					{
						DecisionVerificationSubject = new DecisionVerificationSubject { Id = subjId },
						NormativeDocItem = new NormativeDocItem { Id = id }
					};

					this.NormDocItemDomain.Save(newRef);
				}

				return new BaseDataResult();
			}
			catch (Exception exception)
			{
				return BaseDataResult.Error(exception.Message);
			}
		}
	}
}