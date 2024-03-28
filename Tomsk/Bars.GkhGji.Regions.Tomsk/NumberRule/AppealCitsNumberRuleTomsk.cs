namespace Bars.GkhGji.Regions.Tomsk.NumberRule
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class AppealCitsNumberRuleTomsk : IAppealCitsNumberRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<AppealCits> AppealCits { get; set; }

        public IDomainService<DocumentGji> DocumentDomain { get; set; }

        public void SetNumber(IEntity entity)
        {
            var appeal = entity as AppealCits;

            if (appeal == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(appeal.NumberGji))
            {
                appeal.Number = appeal.NumberGji;
                appeal.DocumentNumber = appeal.NumberGji;
                return;
            } 

            var appealNumbers = AppealCits.GetAll()
                .Where(x => x.Year == appeal.Year)
                .Where(x => x.NumberGji != null)
                .Select(x => x.NumberGji)
                .AsEnumerable()
                .Select(x => x.ToInt())
                .ToArray();

            var resolutionProsecutorMaxNumber = DocumentDomain.GetAll()
                .Where(x => x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                .Where(x => x.DocumentYear == appeal.Year || x.DocumentDate.Value.Year == appeal.Year)
                .Where(x => x.DocumentNum != null)
                .Max(x => x.DocumentNum);

            var appealMax = appealNumbers.Any() ? appealNumbers.Max() : 0;
            var resolutionProsecutorMax = resolutionProsecutorMaxNumber ?? 0;

            appeal.NumberGji = ((appealMax > resolutionProsecutorMax ? appealMax : resolutionProsecutorMax) + 1).ToStr();
            appeal.Number = appeal.NumberGji;
            appeal.DocumentNumber = appeal.NumberGji;
        }
    }
}