namespace Bars.GkhGji.Regions.Tatarstan.Controller
{
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Controllers;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    public class WarningDocController : WarningDocController<WarningDoc>
    {
        /// <summary>
        /// Массовое добавление оснований для предостережения
        /// </summary>
        /// <param name="baseParams">
        /// warningIds - идентификаторы оснований для предостержения
        /// documentId - идентификатор предостережения
        /// </param>
        public ActionResult AddWarningsBasis(BaseParams baseParams)
        {
            var warningIds = baseParams.Params.GetAs<long[]>("warningIds");
            var documentId = baseParams.Params.GetAs<long>("documentId");

            if (warningIds.IsEmpty())
            {
                return this.JsFailure("Не переданы идентификаторы оснований для предостережения");
            }
            if (documentId == 0)
            {
                return this.JsFailure("Не передан идентификатор предостережения");
            }

            var toSave = new List<WarningDocBasis>();
            var doc = new WarningDoc { Id = documentId };
            foreach (var warningId in warningIds)
            {
                toSave.Add(new WarningDocBasis
                {
                    WarningDoc = doc,
                    WarningBasis = new WarningBasis { Id = warningId }
                });
            }

            TransactionHelper.InsertInManyTransactions(this.Container, toSave, useStatelessSession: true);

            return this.JsSuccess();
        }

        public ActionResult ListView(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IWarningDocService>();
            try
            {
                var result = (ListDataResult)service.ListView(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult ListForStage(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IWarningDocService>();

            try
            {
                var result = (ListDataResult)service.ListForStage(baseParams);
                return result.Success
                    ? new JsonListResult((IList)result.Data, result.TotalCount)
                    : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}