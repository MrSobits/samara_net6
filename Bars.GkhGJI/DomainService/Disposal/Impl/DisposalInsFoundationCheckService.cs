namespace Bars.GkhGji.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Entities;

    /// <summary>
	/// Сервис для работы с "НПА проверки"
	/// </summary>
	public class DisposalInsFoundationCheckService : IDisposalInsFoundationCheckService
	{
        /// <summary>
        /// Домен-сервис <see cref="DisposalInspFoundationCheck"/>
        /// </summary>
        public IDomainService<DisposalInspFoundationCheck> FoundCheckDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="DisposalInspFoundCheckNormDocItem"/>
        /// </summary>
        public IDomainService<DisposalInspFoundCheckNormDocItem> NormDocItemDomain { get; set; }

        /// <summary>
        /// Добавить "НПА проверки"
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult AddInspFoundationChecks(BaseParams baseParams)
		{
			var documentId = baseParams.Params.GetAs<long>("documentId");
			var ids = baseParams.Params.GetAs<long[]>("ids") ?? new long[0];
			var result = this.AddInspFoundationChecks(documentId, ids);

			return result;
		}

		/// <summary>
		/// Добавить "НПА проверки"
		/// </summary>
		/// <param name="documentId">Идентификатор документа</param>
		/// <param name="ids">Идентификаторы новых записей</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult AddInspFoundationChecks(long documentId, long[] ids)
		{
			try
			{
				var extingIds = this.FoundCheckDomain.GetAll()
					.Where(x => x.Disposal.Id == documentId)
					.Select(x => x.InspFoundationCheck.Id)
					.ToArray();

				foreach (var id in ids.Distinct())
				{
					if (!extingIds.Contains(id))
					{
						var newObj = new DisposalInspFoundationCheck()
						{
							Disposal = new Disposal { Id = documentId },
							InspFoundationCheck = new NormativeDoc { Id = id }
						};

						this.FoundCheckDomain.Save(newObj);
					}
				}
				return new BaseDataResult();
			}
			catch (ValidationException e)
			{
				return new BaseDataResult { Success = false, Message = e.Message };
			}
		}

        /// <summary>
		/// Добавить Требования НПА проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult AddNormDocItems(BaseParams baseParams)
        {
            var foundCheckId = baseParams.Params.GetAsId("foundCheckId");
            var ids = baseParams.Params.GetAs("ids", new long[0]);

            try
            {
                var existIds = this.NormDocItemDomain.GetAll()
                    .Where(x => x.DisposalInspFoundationCheck.Id == foundCheckId)
                    .Select(x => x.NormativeDocItem.Id)
                    .ToArray();

                var idsForSave = ids.Distinct().Except(existIds);

                foreach (var id in idsForSave)
                {
                        var newRef = new DisposalInspFoundCheckNormDocItem
                        {
                            DisposalInspFoundationCheck = new DisposalInspFoundationCheck { Id = foundCheckId },
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