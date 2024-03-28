namespace Bars.Gkh.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections;

    using B4;
    using Bars.Gkh.Config.Attributes;
    using DomainService;
    using Entities;
    using Bars.Gkh.Domain;

    /// <summary>
    /// Болванка на случай если от этого класса наследовались
    /// </summary>
    public class OperatorController : OperatorController<Operator>
    {
    }

    /// <summary>
    /// Generic класс чтобы лучше расширять в других регионах по сущности Operator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OperatorController<T> : B4.Alt.DataController<T>
        where T : Operator
    {
        public ActionResult GetActiveOperatorId()
        {
            var OperatorService = Container.Resolve<IOperatorService>();
            try
            {

                var result = OperatorService.GetActiveOperatorId();
                return new JsonGetResult(result.Data);
            }
            finally 
            {
                Container.Release(OperatorService);
            }
        }


        /// <summary>
        /// Сгенерировать новый пароль
        /// </summary>       
        public ActionResult GenerateNewPassword(BaseParams baseParams)
        {
            var OperatorService = Container.Resolve<IOperatorService>();
            var result =  OperatorService.GenerateNewPassword(baseParams).ToJsonResult();
            return result;
        }

        // Временный метод для настройки профайла
        public ActionResult GetProfile()
        {
            var OperatorService = Container.Resolve<IOperatorService>();
            try
            {
                var result = OperatorService.GetProfile();
                return new JsonGetResult(result.Data);
            }
            finally
            {
                Container.Release(OperatorService);
            }
        }

        public ActionResult GetActiveOperator()
        {
            var OperatorService = Container.Resolve<IOperatorService>();
            try
            {
                var result = OperatorService.GetActiveOperator();
                return new JsonGetResult(result.Data);
            }
            finally
            {
                Container.Release(OperatorService);
            }
        }
        
        [ControllerPermission("Administration.Profile.Edit")]
        public ActionResult ChangeProfile(BaseParams baseParams)
        {
            var OperatorService = Container.Resolve<IOperatorService>();
            try
            {
                var result = OperatorService.ChangeProfile(baseParams);
                return new JsonNetResult(result);
            }
            finally
            {
                Container.Release(OperatorService);
            }
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            var OperatorService = Container.Resolve<IOperatorService>();
            try
            {
                var result = OperatorService.GetInfo(baseParams);
                if (result.Success)
                {
                    return new JsonNetResult(result.Data);
                }

                return JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(OperatorService);
            }
        }

        public ActionResult AddInspectors(BaseParams baseParams)
        {
            var OperatorService = Container.Resolve<IOperatorService>();
            try
            {
                var result = OperatorService.AddInspectors(baseParams);
                if (result.Success)
                {
                    return new JsonNetResult(new { success = true });
                }

                return JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(OperatorService);
            }
        }

        public ActionResult AddMunicipalities(BaseParams baseParams)
        {
            var OperatorService = Container.Resolve<IOperatorService>();
            try
            {
                var result = OperatorService.AddMunicipalities(baseParams);
                if (result.Success)
                {
                    return new JsonNetResult(new { success = true });
                }

                return JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(OperatorService);
            }
        }

        public ActionResult AddContragents(BaseParams baseParams)
        {
            var OperatorService = Container.Resolve<IOperatorService>();
            try
            {
                var result = OperatorService.AddContragents(baseParams);
                if (result.Success)
                {
                    return new JsonNetResult(new { success = true });
                }

                return JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(OperatorService);
            }
        }

        public ActionResult ListContragent(BaseParams baseParams)
        {
            var OperatorService = Container.Resolve<IOperatorService>();
            try
            {
                var result = (ListDataResult)OperatorService.ListContragent(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                Container.Release(OperatorService);
            }
        }

        public ActionResult ListMunicipality(BaseParams baseParams)
        {
            var OperatorService = Container.Resolve<IOperatorService>();
            try
            {
                var result = (ListDataResult)OperatorService.ListMunicipality(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                Container.Release(OperatorService);
            }
        }

        public ActionResult ListInspector(BaseParams baseParams)
        {
            var OperatorService = Container.Resolve<IOperatorService>();
            try
            {
                var result = (ListDataResult)OperatorService.ListInspector(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                Container.Release(OperatorService);
            }
        }
    }
}
