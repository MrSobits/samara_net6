namespace Bars.GkhGji.Regions.Tomsk.StateChange
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;

	/// <summary>
	/// Правило проверки возможности формирования номера распоряжения Томска
	/// </summary>
	public class DisposalValidationNumberRule : BaseDocValidationNumberRule
    {
		/// <summary>
		/// Домен сервис для Первичное обращение проверки
		/// </summary>
		public IDomainService<PrimaryBaseStatementAppealCits> PrimaryBaseStatementAppealCitsService { get; set; }

		/// <summary>
		/// Домен сервис для Приказ
		/// </summary>
		public IDomainService<Disposal> DisposalDomain { get; set; }

		/// <summary>
		/// Идентификатор правила
		/// </summary>
        public override string Id { get { return "gji_disposal_validation_number"; } }

		/// <summary>
		/// Наименование
		/// </summary>
        public override string Name { get { return "Проверка возможности формирования номера распоряжения Томска"; } }

		/// <summary>
		/// Идентификатор типа
		/// </summary>
        public override string TypeId { get { return "gji_document_disp"; } }

		/// <summary>
		/// Описание
		/// </summary>
        public override string Description { get { return "Данное правило проверяет формирование номера распоряжения в соответствии с правилами Томска"; } }

        protected override ValidateResult Action(DocumentGji document)
        {
            // Если данное правило подставили под другой документ, то ничего не делаем

            var disposal = document as Disposal;

            if (disposal != null)
            {
                // Если проверка формируется по обращению граждан,то вся нумерация завязана на номере обращения.
                // присваивается номер обращения, из которого формируется распоряжение (Обращение № 1, распоряжение №1),
                // если из одного обращения формируется несколько распоряжений, тогда первое распоряжение = номер обращения, второе = номер обращения/1 и т.дa. (пример, 1, 1/1, 1/2, 1/3)

                if (!disposal.DocumentYear.HasValue)
                {
                    // Год берется из даты документа
                    disposal.DocumentYear = disposal.DocumentDate.Value.Year;
                }

                try
                {

                    switch (disposal.TypeDisposal)
                    {
                        case TypeDisposalGji.Base:
                        case TypeDisposalGji.Licensing:
                            SetNumberMainDisposal(disposal);
                            break;
                        case TypeDisposalGji.DocumentGji:
                            SetNumberPrescrDisposal(disposal);
                            break;
                    }

                    if (string.IsNullOrEmpty(disposal.DocumentNumber))
                    {
                        throw new ValidationException("Не удалось сформировать номер.");
                    }

                    var numberInUsage = DisposalDomain.GetAll()
                    .Where(x => x.Id != disposal.Id)
                    .Where(x => x.DocumentYear == disposal.DocumentYear)
                    .Where(x => x.DocumentNumber == disposal.DocumentNumber);

                    if (numberInUsage.Any())
                    {
                        throw new ValidationException("Указанный номер в этом году уже зарегистрирован в другом приказе.");
                    }
                }
                catch (ValidationException e)
                {
                    return ValidateResult.No(e.Message);
                }
            }

            return ValidateResult.Yes();
        }

        // Проставление номера для главного распоряжения (TypeDisposal == Base)
        private void SetNumberMainDisposal(Disposal disposal)
        {
            if (string.IsNullOrWhiteSpace(disposal.DocumentNumber))
            {
                if (disposal.Inspection.TypeBase != TypeBase.CitizenStatement)
                {
                    // Если проверка не по обращению граждан, то номер забивается вручную иначе выходим отсудыва
                    if (string.IsNullOrEmpty(disposal.DocumentNumber))
                    {
                        throw new ValidationException("Необходимо указать номер приказа");
                    }
                }
                else if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement)
                {
                    var appealNumber = PrimaryBaseStatementAppealCitsService.GetAll()
                    .Where(x => x.BaseStatementAppealCits.Inspection.Id == disposal.Inspection.Id)
                    .Select(x => x.BaseStatementAppealCits.AppealCits.NumberGji)
                    .AsEnumerable()
                    .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

                    if (appealNumber == null)
                    {
                        throw new ValidationException("Обращение, на основе которого создается приказ, не имеет номера.");
                    }

                    // Для получения подномера необходимо сначала найти макс подномер приказа, который создан 
                    // по проверке, главное обращение которого совпадает с нашим

                    // Подзапрос на получение основного обращения проверки приказа
                    var inspectionsPrimaryAppealsQuery = PrimaryBaseStatementAppealCitsService.GetAll()
                        .Where(x => x.BaseStatementAppealCits.Inspection.Id == disposal.Inspection.Id)
                        .Select(x => x.BaseStatementAppealCits.AppealCits.Id);

                    // Подзапрос на получение проверок, для которых основной проверкой является основное обращение проверки приказа
                    var inspectionsByPrimaryAppealQuery = PrimaryBaseStatementAppealCitsService.GetAll()
                        .Where(x => inspectionsPrimaryAppealsQuery.Contains(x.BaseStatementAppealCits.AppealCits.Id))
                        .Select(x => x.BaseStatementAppealCits.Inspection.Id);

                    // Список подномеров приказов (нумерация по типам приказов ведется отдельно), которые созданы
                    // по проверке, главное обращение которого совпадает с нашим
                    var existingDisposalsSubNum = DisposalDomain.GetAll()
                        .Where(x => x.Id != disposal.Id)
                        .Where(x => inspectionsByPrimaryAppealQuery.Contains(x.Inspection.Id))
                        .Where(x => x.TypeDisposal == disposal.TypeDisposal)
                        .Select(x => x.DocumentSubNum)
                        .ToList();

                    disposal.DocumentSubNum = existingDisposalsSubNum.Any() ? (existingDisposalsSubNum.Max() ?? 0) + 1 : (int?)null;

                    disposal.DocumentNum = appealNumber.ToInt();

                    // Строковый номер итоговый формируется после сейчас как "Номер/ДополнительныйНомер"
                    // Если дополнительный номер = 0, то он не входит в стрковый номер и дробь не ставится
                    disposal.DocumentNumber = appealNumber;
                    if (disposal.DocumentSubNum.ToInt() > 0)
                    {
                        disposal.DocumentNumber += "/" + disposal.DocumentSubNum.ToInt().ToString(CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        //проставление номера для распоряжения на проверку предписания (TypeDisposal == DocumentGji)
        private void SetNumberPrescrDisposal(Disposal disposal)
        {
            var mainDisposal = DisposalDomain.GetAll()
                .Where(x => x.Inspection.Id == disposal.Inspection.Id)
                .FirstOrDefault(x => x.TypeDisposal == TypeDisposalGji.Base || x.TypeDisposal == TypeDisposalGji.Licensing);

            if (mainDisposal == null)
            {
                throw new ValidationException("В проверке отсутствует главное распоряжение");
            }

            if (mainDisposal.DocumentNumber.IsEmpty())
            {
                throw new ValidationException("У главного распоряжения проверки отсутствует номер");
            }

            var maxSubNum = DisposalDomain.GetAll()
                .Where(x => x.Inspection.Id == disposal.Inspection.Id)
                .Where(x => x.TypeDisposal == TypeDisposalGji.DocumentGji)
                .Where(x => x.DocumentSubNum != null)
                .Select(x => x.DocumentSubNum)
                .Max() ?? 0;

            disposal.DocumentSubNum = maxSubNum + 1;

            disposal.DocumentNum = mainDisposal.DocumentNum;

            disposal.DocumentNumber = mainDisposal.DocumentNumber + "-" + disposal.DocumentSubNum;
        }
    }
}