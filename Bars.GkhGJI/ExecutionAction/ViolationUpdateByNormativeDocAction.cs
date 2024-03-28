namespace Bars.GkhGji.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.GkhGji.DomainService;

    public class ViolationUpdateByNormativeDocAction : BaseExecutionAction
    {
        /// <summary>
        /// Статический код регистрации.
        /// </summary>
        /// <summary>
        /// IoC контейнер.
        /// </summary>
        /// <summary>
        /// Код для регистрации.
        /// </summary>
        /// <summary>
        /// Описание действия.
        /// </summary>
        public override string Description => "ГЖИ Нарйшения - Действие изменяющее колонку НПД в формате перечисления пунктов НПД через запятую";

        /// <summary>
        /// Название для отображения.
        /// </summary>
        public override string Name => "ГЖИ Нарйшения -  Действие изменяющее колонку НПД в формате перечисления пунктов НПД через запятую";

        /// <summary>
        /// Действие.
        /// </summary>
        public override Func<IDataResult> Action => this.MakeDocs;

        /// <summary>
        /// Метод действия.
        /// </summary>
        /// <returns>
        /// The <see cref="BaseDataResult" />.
        /// </returns>
        public BaseDataResult MakeDocs()
        {
            var violationsDomain = this.Container.Resolve<IViolationNormativeDocItemService>();

            try
            {
                using (var transaction = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        violationsDomain.UpdaeteViolationsByNpd();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(violationsDomain);
            }
        }
    }
}