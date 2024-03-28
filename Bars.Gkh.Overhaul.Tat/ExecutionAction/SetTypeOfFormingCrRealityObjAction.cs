namespace Bars.Gkh.Overhaul.Tat.ExecutionAction
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.RegionalFormingOfCr;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;

    public class SetTypeOfFormingCrRealityObjAction : BaseExecutionAction
    {
        /// <summary>
        /// </summary>
        public ITypeOfFormingCrProvider TypeOfFormingCrProvider { get; set; }

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Установка способа формирования фонда кр";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Установка способа формирования фонда кр";

        /// <summary>
        /// Выполнение
        /// </summary>
        /// <returns></returns>
        private BaseDataResult Execute()
        {
            var realityObjectDomain = this.Container.ResolveRepository<RealityObject>();

            try
            {
                if (this.TypeOfFormingCrProvider != null)
                {
                    this.Container.InTransaction(
                        () =>
                        {
                            var query = realityObjectDomain.GetAll().OrderBy(x => x.Id);
                            var count = query.Count();
                            var skip = 0;
                            while (count > skip)
                            {
                                var querySkip = query.Skip(skip).Take(500);

                                var typeOfFormingCrDictionary = this.TypeOfFormingCrProvider.GetTypeOfFormingCr(querySkip);

                                if (typeOfFormingCrDictionary != null)
                                {
                                    foreach (var realityObject in querySkip)
                                    {
                                        CrFundFormationType type;

                                        if (typeOfFormingCrDictionary.TryGetValue(realityObject.Id, out type))
                                        {
                                            realityObject.AccountFormationVariant = type;
                                        }
                                        else
                                        {
                                            realityObject.AccountFormationVariant = CrFundFormationType.Unknown;
                                        }

                                        realityObjectDomain.Update(realityObject);
                                    }
                                }
                                skip += 500;
                            }
                        });
                }
                return new BaseDataResult();
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                this.Container.Release(realityObjectDomain);
            }
        }
    }
}