namespace Bars.Gkh.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class AdditionalAppealsDeleteAction : BaseExecutionAction
    {
        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomain { get; set; }

        public IDomainService<InspectionAppealCits> InspectionAppealCitsDomain { get; set; }

        public override string Description
            => "Удаление некорректных обращений (У которых «номер ГЖИ» содеожит сочетание «Доп» и дата обращения >= 01.01.2014)";

        public override string Name => "Удаление некорректных обращений";

        public override Func<IDataResult> Action => this.AdditionalAppealsDelete;

        public BaseDataResult AdditionalAppealsDelete()
        {
            var appealCitsIds = this.AppealCitsDomain.GetAll()
                .Where(x => x.NumberGji.ToLower().Contains("доп"))
                .Where(x => x.DateFrom.HasValue && x.DateFrom >= new DateTime(2014, 1, 1))
                .Where(x => !this.AppealCitsAnswerDomain.GetAll().Any(y => y.AppealCits.Id == x.Id))
                .Where(x => !this.InspectionAppealCitsDomain.GetAll().Any(y => y.AppealCits.Id == x.Id))
                .Select(x => x.Id)
                .ToList();

            foreach (var id in appealCitsIds)
            {
                this.AppealCitsDomain.Delete(id);
            }

            return new BaseDataResult();
        }
    }
}