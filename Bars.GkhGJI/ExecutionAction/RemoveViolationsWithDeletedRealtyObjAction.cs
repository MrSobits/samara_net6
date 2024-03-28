namespace Bars.GkhGji.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.GkhGji.Entities;

    using Microsoft.Extensions.Logging;

    public class RemoveViolationsWithDeletedRealtyObjAction : BaseExecutionAction
    {
        public ILogger Logger { get; set; }

        public override string Description => "Удалить нарушения с несуществующими домами - выполнить перед миграцией 2015052500";

        public override string Name => "Удалить нарушения с несуществующими домами";

        public override Func<IDataResult> Action => this.RemoveInvalidViolations;

        /// <summary>
        /// Метод действия - Удалить нарушения с несуществующими домами
        /// </summary>
        /// <returns>
        /// The <see cref="BaseDataResult" />.
        /// </returns>
        private BaseDataResult RemoveInvalidViolations()
        {
            var inspectionViolationService = this.Container.Resolve<IDomainService<InspectionGjiViol>>();
            var inspectionGjiViolStageService = this.Container.Resolve<IDomainService<InspectionGjiViolStage>>();
            var realityObjectService = this.Container.Resolve<IDomainService<RealityObject>>();

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    //выбрать идентификаторы нарушений с несуществующими домами
                    var voilationIdsToDelete = inspectionViolationService.GetAll()
                        .Where(
                            voilation => voilation.RealityObject != null &&
                                !realityObjectService.GetAll()
                                    .Any(realtyObject => realtyObject.Id == voilation.RealityObject.Id))
                        .Select(voilation => voilation.Id).ToArray();

                    //выбрать идентификаторы связанных записей - этапов нарушения проверки ГЖИ
                    var stagesIdsToDelete = inspectionGjiViolStageService.GetAll()
                        .Where(
                            voilationStage => voilationIdsToDelete.Contains(voilationStage.InspectionViolation.Id))
                        .Select(voilation => voilation.Id).ToArray();

                    //удалить связанные записи - этапы нарушения проверки ГЖИ
                    stagesIdsToDelete.ForEach(id => inspectionGjiViolStageService.Delete(id));

                    //удалить нарушения с несуществующими домами
                    voilationIdsToDelete.ForEach(id => inspectionViolationService.Delete(id));

                    transaction.Commit();
                }
                catch (Exception exp)
                {
                    transaction.Rollback();
                    this.Logger.LogError(exp, "Ошибка удаления нарушений с несуществующими домами");
                    return new BaseDataResult(false, "Не удалось удалить нарушения с несуществующими домами");
                }
            }

            return new BaseDataResult();
        }
    }
}