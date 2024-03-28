namespace Bars.Gkh.RegOperator.Controllers.Config
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;

    using DomainService.PersonalAccount.PayDoc.SnapshotBuilders;

    using Npgsql;

    /// <summary>
    /// Констроллер работы с настройками источников для документов на оплату
    /// </summary>
    public class PaymentDocSourceController: BaseController
    {
        private static readonly object lockObject = new object();

        /// <summary>
        /// Получить все настройки в виде дерева
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSourceConfigs()
        {
            var configService = this.Container.Resolve<ISourceConfigService>();
            try
            {
                return new BaseDataResult(configService.GetConfigTree()).ToJsonResult();
            }
            finally
            {
                this.Container.Release(configService);
            }
        }

        /// <summary>
        /// Сохранение источников
        /// </summary>
        /// <param name="baseParams">baseParams</param>
        /// <returns></returns>
        public IDataResult SaveSources(BaseParams baseParams)
        {
            var configDomain = this.Container.ResolveDomain<BuilderConfig>();
            try
            {
                var sources = baseParams.Params.GetAs<List<BuilderConfig>>("sources");
                lock (lockObject)
                {
                    sources.ForEach(configDomain.SaveOrUpdate);
                }                    
            }
            catch (NpgsqlException)
            {
                return new BaseDataResult(false, "При сохранении настройки произошла ошибка. Попробуйте еще раз.");
            }
            finally
            {
                this.Container.Release(configDomain);
            }
            return new BaseDataResult();
        }
    }
}