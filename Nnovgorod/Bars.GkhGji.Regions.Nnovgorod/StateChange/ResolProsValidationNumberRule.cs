namespace Bars.GkhGji.Regions.Nnovgorod.StateChange
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using B4;
    using B4.Utils;
    using Enums;
    using Gkh.Authentification;
    using Gkh.Entities;
    using GkhGji.Entities;

    public class ResolProsValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id
        {
            get { return "gji_resolpros_validation_number"; }
        }

        public override string Name
        {
            get { return "Проверка возможности формирования номера постановления прокуратуры"; }
        }

        public override string TypeId
        {
            get { return "gji_document_resolpros"; }
        }

        public override string Description
        {
            get
            {
                return
                    "Данное правило проверяет формирование номера постановления прокуратуры в соответствии с правилами";
            }
        }

        protected override void Action(DocumentGji document)
        {
            var zonalInspectionService = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var docGjiService = this.Container.Resolve<IDomainService<DocumentGji>>();
            var userManager = this.Container.Resolve<IGkhUserManager>();

            var gjiNumber = string.Empty;

            try
            {
                document.DocumentYear =
                    document.DocumentDate.HasValue
                        ? document.DocumentDate.Value.Year
                        : 0;
                if (document.DocumentYear == 0)
                {
                    document.DocumentYear = DateTime.Now.Year;
                }

                var maxNum = docGjiService.GetAll()
                    .Where(x =>
                        x.DocumentYear == document.DocumentYear
                        && x.Id != document.Id
                        && (x.TypeDocumentGji == TypeDocumentGji.Disposal
                            || x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor))
                    .Select(x => x.DocumentNum)
                    .Max();

                document.DocumentNum = maxNum.HasValue ? maxNum.Value + 1 : 1;

                var currentOpeartor = userManager.GetActiveOperator();

                if (currentOpeartor == null || currentOpeartor.Inspector == null)
                {
                    throw new DocumentGjiStateChangeException(
                        "Нельзя изменить статус, так как вы не являетесь иснпектором");
                }

                var inspectorId = currentOpeartor.Inspector.Id;

                var zonalInspection =
                    zonalInspectionService.GetAll()
                        .FirstOrDefault(x => x.Inspector.Id == inspectorId)
                        .With(x => x.ZonalInspection);

                if (zonalInspection != null)
                {
                    gjiNumber = zonalInspection.IndexOfGji;
                }

                if (gjiNumber.IsEmpty())
                {
                    throw new DocumentGjiStateChangeException("Не заполнен индекс ГЖИ у отдела жилищной инспекции");
                }

                var findStr = gjiNumber + "-";

                var query = docGjiService.GetAll()
                    .Where(x => x.TypeDocumentGji == document.TypeDocumentGji)
                    .Where(x => x.DocumentNumber.Length > 0 && x.DocumentDate.HasValue)
                    .Where(x =>
                        x.DocumentDate.Value.Year ==
                        document.DocumentYear)
                    .Where(x => x.DocumentNumber.StartsWith(findStr)) // Номер расчитывается в пределах отдела ЗЖИ
                    .Select(x => x.DocumentNumber);

                var maxNumber = 0;

                var docNumbers = query
                    .AsEnumerable()
                    .Select(x => x.Split('-')[2])
                    .ToArray();

                var r = new Regex(@"\d+");

                if (docNumbers.Any())
                {
                    var ordinalNumber =
                        docNumbers.Select(x => r.Match(x).ToInt())
                            .ToList();

                    if (ordinalNumber.Any())
                    {
                        maxNumber =
                            ordinalNumber.Max();
                    }
                }

                maxNumber++;
                document.DocumentNumber = string.Format(
                    "{0}-{1}{2}/{3}",
                    gjiNumber,
                    maxNumber < 100 ? maxNumber.ToString("00") : maxNumber.ToString(CultureInfo.InvariantCulture),
                    document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor ? "ПР" : string.Empty,
                    document.DocumentYear);
            }
            catch (Exception e)
            {
                if (e is DocumentGjiStateChangeException)
                {
                    throw;
                }

                throw new Exception("Не удалось установить номер документа");
            }
        }
    }
}