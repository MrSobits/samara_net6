namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Castle.Windsor;

    public class AccountOperationProvider : IAccountOperationProvider
    {
        public IWindsorContainer Container { get; set; }

        private static List<AccOperationProxy> _operations;

        public List<AccOperationProxy> GetAllOperations()
        {
            if (_operations != null)
            {
                return _operations;
            }

            return
                _operations = Container.ResolveAll<IPersonalAccountOperation>()
                    .Select(x => new AccOperationProxy
                    {
                        Code = x.Code,
                        Name = x.Name,
                        PermissionKey = x.PermissionKey
                    })
                    .ToList();
        }

        public IDataResult Execute(BaseParams baseParams)
        {
            var code = baseParams.Params.GetAs<string>("operationCode");

            if (code.IsEmpty() || !Container.Kernel.HasComponent(code))
            {
                return new BaseDataResult(false, "Не удалось получить исполнителя операции");
            }

            return Container.Resolve<IPersonalAccountOperation>(code).Execute(baseParams);
        }
    }
}
