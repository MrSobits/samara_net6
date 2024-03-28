namespace Bars.GkhGji.StateChange
{
    using System;
    using System.Globalization;
    using System.Linq;
    using B4;
    using B4.Modules.States;
    using B4.Utils;

    using Bars.B4.DataAccess;

    using Gkh.Entities;
    using Entities;
    using Enums;

    using Castle.Windsor;

    /// <summary>
    /// Формирование номера в соответствии с правилами РТ
    /// </summary>
    public abstract class BaseDocValidationNumberTatRule : IRuleChangeStatus
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<Municipality> MunicipalityDomain { get; set; }
        public IDomainService<ViewDisposal> ViewDisposalDomain { get; set; }

        public IRepository<Disposal> DisposalRepository { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public abstract string Id { get; }

        /// <summary>
        /// Название
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Тип док-та
        /// </summary>
        public abstract string TypeId { get; }

        /// <summary>
        /// Описание
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Валидация
        /// </summary>
        /// <param name="statefulEntity">Сущность со статусами</param>
        /// <param name="oldState">Предыдущий статус</param>
        /// <param name="newState">Новый статус</param>
        /// <returns>Результат валидации</returns>
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is DocumentGji document)
            {
                if (document.DocumentDate == null)
                {
                    return ValidateResult.No("Невозможно сформировать номер, поскольку дата документа не указана");
                }

#warning перенести проверку в правило для каждого документа отдельно
                if (document.TypeDocumentGji != TypeDocumentGji.ActRemoval)
                {
                    /* проставление номера возможно только если у всех родительских документов есть строковый номер */
                    var parentDocWithoutNumber = this.DocumentGjiChildrenDomain.GetAll()
                        .FirstOrDefault(x => x.Children.Id == document.Id &&
                            (x.Parent.DocumentNumber == null || x.Parent.DocumentNumber == string.Empty))
                        ?.Parent;

                    if (parentDocWithoutNumber.IsNotNull())
                    {
                        return ValidateResult.No(string.Format(
                            "Номер не может быть присвоен, потому что у предыдущего документа {0} от {1} нет номера",
                            parentDocWithoutNumber.TypeDocumentGji.GetEnumMeta().Display,
                            parentDocWithoutNumber.DocumentDate.ToDateTime().ToShortDateString()));
                    }
                }

                /*проверка что у родительского документа по Stage был строковый номер*/
                if (document.Stage.Parent != null)
                {
                    var parentDocWithoutNumber = this.DocumentGjiDomain.GetAll()
                        .FirstOrDefault(x => x.Stage.Id == document.Stage.Parent.Id &&
                            (x.DocumentNumber == null || x.DocumentNumber == string.Empty));

                    if (parentDocWithoutNumber.IsNotNull())
                    {
                        return ValidateResult.No(string.Format(
                            "Номер не может быть присвоен, потому что у предыдущего документа {0} от {1} нет номера",
                            parentDocWithoutNumber.TypeDocumentGji.GetEnumMeta().Display,
                            parentDocWithoutNumber.DocumentDate.ToDateTime().ToShortDateString()));
                    }
                }

                /* Если регистрируется акт проверки с типом - Акт проверки документа ГЖИ
                 * то дополнительно необходимо чтобы были зарегистрированы дочерние акты проверки предписаний 
                 */
                if (document is ActCheck actCheck && actCheck.TypeActCheck == TypeActCheckGji.ActCheckDocumentGji)
                {
                    var actRemovalWithoutNumberExists = this.DocumentGjiChildrenDomain.GetAll()
                        .Any(x => x.Parent.Id == document.Id &&
                            x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval &&
                            (x.Children.DocumentNumber == null || x.Children.DocumentNumber == string.Empty));

                    if (actRemovalWithoutNumberExists)
                    {
                        return ValidateResult.No("Необходимо присвоить номера актам проверки предписаний");
                    }
                }

                //если номер уже присвоен, то ничего не делаем, чтобы не проставлялся другой номер
                if (string.IsNullOrEmpty(document.DocumentNumber))
                {
                    // Если дошли досюда то производим действие
                    Action(document);
                }

            }

            return ValidateResult.Yes();
        }

        /// <summary>
        /// Формируем номер
        /// </summary>
        /// <param name="document">Базовый документ ГЖИ</param>
        protected virtual void Action(DocumentGji document)
        {
            // Формируем номер
            // Номер формируется для:
            // распоряжений, распоряжений вне инспекторской деятельности, постановлений прокуратуры
            // путем получения максимального номера за текущий год + 1
            // для других документов номер берется относительно Главных документов
            // либо документов Распоряжения, либо документов Постановления прокуратуры
            // строковый номер формируется следующим образом:
            // если распоряжение вне инспекторской деятельности то просто Номер
            // для распоряжений и постановлений прокуратуры ЛитераРайона-Номер,
            // для остальных документов ЛитераРайона-Номер/ДополнительныйНомер
            // Дополнительный номер формируется внутри Этапа, то есть внутри документов этапа проверки берется
            // максимальный дополнительный номер + 1
            // Если дополнительный номер = 0, то он не входит в строковый номер и дробь не ставится
            // для остальных документов копируем из главного распоряжения проверки или постановления прокуратуры
            // литера района берется из поля код в справочнике муниципальных образований

            // Год берется из даты документа
            document.DocumentYear = document.DocumentDate.HasValue ? document.DocumentDate.Value.Year : (int?)null;

            if (document.TypeDocumentGji == TypeDocumentGji.Disposal ||
                document.TypeDocumentGji == TypeDocumentGji.Decision ||
                document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
            {
                if (document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                {
                    document.DocumentDate = DateTime.Now.Date;
                }

                // т.к. для постановлений сформированных из протокола МВД должна быть сквозная нумерация через все распоряжения и постановления прокуратуры с начала года
                var resolutionsByProtocolMvdQuery = this.DocumentGjiChildrenDomain.GetAll()
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ProtocolMvd)
                    .Select(x => x.Children.Id);

                var maxNum = this.DocumentGjiDomain.GetAll()
                    .Where(x => x.DocumentYear == document.DocumentYear)
                    .Where(x => x.Id != document.Id)
                    .Where(x =>
                        x.TypeDocumentGji == TypeDocumentGji.Disposal ||
                        x.TypeDocumentGji == TypeDocumentGji.Decision ||
                        x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor ||
                        x.TypeDocumentGji == TypeDocumentGji.Resolution && resolutionsByProtocolMvdQuery.Contains(x.Id))
                    .Select(x => x.DocumentNum)
                    .Max();

                document.DocumentNum = maxNum.HasValue ? maxNum.Value + 1 : 1;
            }
            else
            {
                if (document.Inspection.TypeBase != TypeBase.ProsecutorsResolution)
                {
                    //получаем родительский стейдж
                    if (document.Stage.Parent != null)
                    {
                        var disp = this.DocumentGjiDomain.GetAll().FirstOrDefault(x => x.Stage.Id == document.Stage.Parent.Id);
                        document.DocumentNum = disp?.DocumentNum;
                    }
                }
                else
                {
                    var resolPros = Utils.Utils.GetParentDocumentByType(this.DocumentGjiChildrenDomain, document, TypeDocumentGji.ResolutionProsecutor);
                    document.DocumentNum = resolPros?.DocumentNum;
                }

                // теперь формируем дополнительный номер среди документов Этапа в котором находится данный документ
                // Это необходимо для того чтобы была нумерация внутри группы документов этапа
                if (!this.DocumentGjiDomain.GetAll().Any(x => x.Stage.Id == document.Stage.Id && x.Id != document.Id))
                {
                    // Если в этапе нет ни одного документа кроме текущего
                    document.DocumentSubNum = 0;
                }
                else
                {
                    document.DocumentSubNum = this.DocumentGjiDomain.GetAll()
                        .Where(x => x.Stage.Id == document.Stage.Id && x.Id != document.Id)
                        .Select(x => x.DocumentSubNum)
                        .Max();

                    document.DocumentSubNum = document.DocumentSubNum.HasValue ? document.DocumentSubNum.Value + 1 : 0;
                }
            }

            var muCode = "";

            if (document.Inspection != null)
            {
                if (document.Inspection.TypeBase == TypeBase.ProsecutorsResolution)
                {
                    var resolPros = document as ResolPros;
                    if (resolPros != null)
                    {
                        muCode = resolPros.ReturnSafe(x => x.Municipality.Code);
                    }
                }
                else
                {
                    var mainDispId = this.DisposalRepository.GetAll()
                            .Where(x => x.Inspection.Id == document.Inspection.Id
                                        && x.TypeDisposal == TypeDisposalGji.Base)
                            .Select(x => x.Id)
                            .FirstOrDefault();

                    if (mainDispId > 0)
                    {
                        var mainDisp = this.ViewDisposalDomain.Load(mainDispId);

                        if (mainDisp != null)
                        {
                            var mu = this.MunicipalityDomain.Load(mainDisp.MunicipalityId);

                            if (mu != null)
                            {
                                muCode = mu.Code;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(muCode))
                {
                    document.DocumentNumber = muCode + "-" + document.DocumentNum.ToLong().ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    document.DocumentNumber = document.DocumentNum.ToLong().ToString(CultureInfo.InvariantCulture);
                }

                if (document.DocumentSubNum.ToLong() > 0)
                {
                    document.DocumentNumber += "/" + document.DocumentSubNum.ToLong().ToString(CultureInfo.InvariantCulture);
                }
            }
        }
    }
}