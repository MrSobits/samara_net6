namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;

    public class CorrectAddressHousingAfterImportOktmoAction : BaseExecutionAction
    {
        public ISessionProvider SessionProvider { get; set; }

        public override string Description => "Исправление корпуса в адресе дома после импотра ОКТМО (к ошибке 40521)";

        public override string Name => "Исправление корпуса в адресе дома после импотра ОКТМО (к ошибке 40521)";

        public override Func<IDataResult> Action => this.AddressCorrrection;

        public BaseDataResult AddressCorrrection()
        {
            var realtyObjRepository = this.Container.Resolve<IRepository<RealityObject>>();

            const string s = "корп.";

            var listRoToUpdate = new List<RealityObject>();

            using (this.Container.Using(realtyObjRepository))
            {
                var roPosibleWrongHousingAddress = realtyObjRepository.GetAll()
                    .Where(x => x.Address.Contains(s))
                    .Where(x => x.FiasAddress.Housing.Length > 0)
                    .ToList();

                var fiasAddressProxyDict = realtyObjRepository.GetAll()
                    .Where(x => x.Address.Contains(s))
                    .Where(x => x.FiasAddress.Housing.Length > 0)
                    .Select(x => new {x.Id, x.FiasAddress.Housing, x.FiasAddress.Building})
                    .ToDictionary(x => x.Id);

                foreach (var ro in roPosibleWrongHousingAddress)
                {
                    if (!fiasAddressProxyDict.ContainsKey(ro.Id))
                    {
                        continue;
                    }

                    var fiasAddressProxy = fiasAddressProxyDict[ro.Id];

                    var addressParts = ro.Address.Split(new[] {s}, StringSplitOptions.None).ToList();

                    if (addressParts.Count == 1)
                    {
                        continue;
                    }

                    addressParts.RemoveAt(addressParts.Count - 1);

                    var newAddress = string.Join(s, addressParts).Trim(' ').Trim(',');

                    if (!string.IsNullOrWhiteSpace(fiasAddressProxy.Housing))
                    {
                        newAddress = string.Format("{0}, {1} {2}", newAddress, s, fiasAddressProxy.Housing);
                    }

                    if (!string.IsNullOrWhiteSpace(fiasAddressProxy.Building))
                    {
                        newAddress = string.Format("{0}, секц. {1}", newAddress, fiasAddressProxy.Building);
                    }

                    if (newAddress != ro.Address)
                    {
                        ro.Address = newAddress;
                        listRoToUpdate.Add(ro);
                    }
                }
            }

            this.SessionProvider.CloseCurrentSession();

            using (var session = this.SessionProvider.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        listRoToUpdate.ForEach(session.Update);

                        transaction.Commit();
                    }
                    catch (Exception exc)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message, exc);
                        }
                    }
                }
            }

            return new BaseDataResult();
        }
    }
}