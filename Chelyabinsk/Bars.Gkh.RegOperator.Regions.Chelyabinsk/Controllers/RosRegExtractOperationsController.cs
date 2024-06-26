﻿namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Domain;
    using Gkh.Domain;
    using B4.DataAccess;
    using Dapper;

    public class RosRegExtractOperationsController : BaseController
    {
        public IRosRegExtractOperationsService Service { get; set; }
        /// <summary>
        /// Получить собственников
        /// </summary>
        public ActionResult GetOwners(BaseParams baseParams)
        {
            return this.Service.GetOwners(baseParams).ToJsonResult();
        }

        public ActionResult UpdateExtractInfo(BaseParams baseParams)
        {
            var StatelessSession = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();
            var connection = StatelessSession.Connection;
            try
            {
                string sql = @"select debtor_cleanup()";
                connection.Execute(sql);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                Container.Release(StatelessSession);
            }
            return JsSuccess();
        }
    }
}
