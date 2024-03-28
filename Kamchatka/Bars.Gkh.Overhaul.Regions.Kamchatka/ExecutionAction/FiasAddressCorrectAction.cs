namespace Bars.Gkh.Overhaul.Regions.Kamchatka.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Regions.Kamchatka.DomainService;

    public class FiasAddressCorrectAction : BaseExecutionAction
    {
        public IDomainService<Municipality> MoDomain { get; set; }

        private Dictionary<string, Municipality> _cacheMu { get; set; }

        private Dictionary<string, IDinamicAddress> _cacheDinamicAddress { get; set; }

        public override string Description
        {
            get { return "Корректировка адресов Контрагентов и Жилых домов Относительно МО"; }
        }

        public override string Name
        {
            get { return "Корректировка адресов Контрагентов и Жилых домов Относительно МО"; }
        }

        public override Func<IDataResult> Action
        {
            get { return this.Execute; }
        }

        private BaseDataResult Execute()
        {
            /*
             Вообщем данный метод берет все адреса и еще раз запрашивает для них DinamicAddress
             Дальше сравнивает с тем, чт онасамом деле сохранено в адресе , если адреса разне то заменяет и обновляет и Контрагента и Жилые дома
            */

            var roRepo = this.Container.Resolve<IRepository<RealityObject>>();
            var ctrRepo = this.Container.Resolve<IRepository<Contragent>>();
            var creditOrgRepo = this.Container.Resolve<IDomainService<CreditOrg>>();
            var fiasAddrRepo = this.Container.Resolve<IRepository<FiasAddress>>();
            var fiasCustomRepo = this.Container.Resolve<ICustomFiasRepository>();

            try
            {
                var fiasForUpdate = new List<FiasAddress>();
                var roForUpdate = new Dictionary<long, RealityObject>();
                var ctrForUpdate = new Dictionary<long, Contragent>();
                var creditOrgForUpdate = new Dictionary<long, CreditOrg>();

                this._cacheDinamicAddress = fiasCustomRepo.GetAllDinamicAddress();

                this._cacheMu = this.MoDomain.GetAll()
                    .Where(x => x.FiasId != null && x.FiasId != "")
                    .AsEnumerable()
                    .GroupBy(x => x.FiasId)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                // Получаем сопоставление FiasAdresa с Ro
                var dictRoAdr = roRepo.GetAll()
                    .AsEnumerable()
                    .GroupBy(x => x.FiasAddress.Id)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                var ctrList = ctrRepo.GetAll().ToList();

                // Словарь сопосталвения Юр адреса с контрагентом
                var dictCtrJurAdr = ctrList
                    .Where(x => x.FiasJuridicalAddress != null)
                    .AsEnumerable()
                    .GroupBy(x => x.FiasJuridicalAddress.Id)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                // Словарь сопосталвения факт адреса с контрагентом
                var dictCtrFactAdr = ctrList
                    .Where(x => x.FiasFactAddress != null)
                    .AsEnumerable()
                    .GroupBy(x => x.FiasFactAddress.Id)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                // Словарь сопосталвения почтового адреса с контрагентом
                var dictCtrMailAdr = ctrList
                    .Where(x => x.FiasMailingAddress != null)
                    .AsEnumerable()
                    .GroupBy(x => x.FiasMailingAddress.Id)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                // Сопоставление юр адреса и кредитной организации
                var crOrgJurAddress = creditOrgRepo.GetAll()
                    .Where(x => x.FiasAddress != null)
                    .ToList()
                    .GroupBy(x => x.FiasAddress.Id)
                    .ToDictionary(x => x.Key, x => x.Select(y => y));

                // Сопоставление почт адреса и кредитной организации
                var crOrgPostAddress = creditOrgRepo.GetAll()
                    .Where(x => x.FiasMailingAddress != null)
                    .ToList()
                    .GroupBy(x => x.FiasMailingAddress.Id)
                    .ToDictionary(x => x.Key, x => x.Select(y => y));

                var fiasData = fiasAddrRepo.GetAll()
                    .Where(
                        x => roRepo.GetAll().Any(y => y.FiasAddress.Id == x.Id)
                            || ctrRepo.GetAll().Any(
                                y => y.FiasJuridicalAddress.Id == x.Id
                                    || y.FiasFactAddress.Id == x.Id
                                    || y.FiasMailingAddress.Id == x.Id)
                            || creditOrgRepo.GetAll().Any(
                                y => y.FiasAddress.Id == x.Id
                                    || y.FiasMailingAddress.Id == x.Id))
                    .ToList();

                foreach (var fiasAddress in fiasData)
                {
                    if (string.IsNullOrEmpty(fiasAddress.PlaceGuidId) || string.IsNullOrEmpty(fiasAddress.StreetGuidId))
                    {
                        continue;
                    }

                    var guid = !string.IsNullOrEmpty(fiasAddress.StreetGuidId)
                        ? fiasAddress.StreetGuidId
                        : fiasAddress.PlaceGuidId;

                    IDinamicAddress dynamicAddr = null;

                    if (this._cacheDinamicAddress.ContainsKey(guid))
                    {
                        dynamicAddr = this._cacheDinamicAddress[guid];
                    }
                    else
                    {
                        continue;
                    }

                    string addressName = dynamicAddr.AddressName;

                    // заполняем оставшиеся значения
                    if (!string.IsNullOrEmpty(fiasAddress.House))
                    {
                        addressName += ", д. " + fiasAddress.House;
                    }

                    if (!string.IsNullOrEmpty(fiasAddress.Letter))
                    {
                        addressName += ", лит. " + fiasAddress.Letter;
                    }

                    if (!string.IsNullOrEmpty(fiasAddress.Housing))
                    {
                        addressName += ", корп. " + fiasAddress.Housing;
                    }

                    if (!string.IsNullOrEmpty(fiasAddress.Building))
                    {
                        addressName += ", секц. " + fiasAddress.Building;
                    }

                    if (!string.IsNullOrEmpty(fiasAddress.Flat))
                    {
                        addressName += ", кв. " + fiasAddress.Flat;
                    }

                    string placeAdrName = string.Empty;

                    if (this._cacheDinamicAddress.ContainsKey(fiasAddress.PlaceGuidId))
                    {
                        var placeDynamicAddr = this._cacheDinamicAddress[fiasAddress.PlaceGuidId];

                        if (placeDynamicAddr != null)
                        {
                            placeAdrName = placeDynamicAddr.AddressName;
                        }
                    }

                    if (fiasAddress.AddressName != addressName
                        || (!string.IsNullOrEmpty(placeAdrName) && fiasAddress.PlaceAddressName != placeAdrName))
                    {
                        fiasAddress.AddressName = addressName;
                        fiasAddress.PlaceAddressName = placeAdrName;

                        fiasForUpdate.Add(fiasAddress);
                    }

                    // Поскольку поняли что адрес над оменять значит 

                    #region Объект недвижимости
                    if (dictRoAdr.ContainsKey(fiasAddress.Id))
                    {
                        var ro = dictRoAdr[fiasAddress.Id];
                        var newMo = this.GetMunicipality(fiasAddress);
                        var newAddress = newMo != null
                            ? this.GetAddressForMunicipality(newMo, fiasAddress)
                            : fiasAddress.AddressName;

                        // если что изменилось
                        if ((newMo != null && newMo.Id != ro.Municipality.Id) || newAddress != ro.Address)
                        {
                            ro.Municipality = newMo;

                            ro.Address = newAddress;

                            if (!roForUpdate.ContainsKey(ro.Id))
                            {
                                roForUpdate.Add(ro.Id, ro);
                            }
                        }
                    }
                    #endregion

                    #region Контрагенты
                    if (dictCtrJurAdr.ContainsKey(fiasAddress.Id))
                    {
                        var ctr = dictCtrJurAdr[fiasAddress.Id];

                        var newMo = this.GetMunicipality(fiasAddress);
                        var newAddress = newMo != null
                            ? this.GetAddressForMunicipality(newMo, fiasAddress)
                            : fiasAddress.AddressName;

                        // если что изменилось
                        if ((newMo != null && newMo.Id != ctr.Municipality.Id) || newAddress != ctr.JuridicalAddress)
                        {
                            ctr.Municipality = newMo;

                            ctr.JuridicalAddress = newAddress;

                            if (!ctrForUpdate.ContainsKey(ctr.Id))
                            {
                                ctrForUpdate.Add(ctr.Id, ctr);
                            }
                        }
                    }

                    if (dictCtrFactAdr.ContainsKey(fiasAddress.Id))
                    {
                        var ctr = dictCtrFactAdr[fiasAddress.Id];

                        if (ctr.FactAddress != fiasAddress.AddressName)
                        {
                            ctr.FactAddress = fiasAddress.AddressName;

                            if (!ctrForUpdate.ContainsKey(ctr.Id))
                            {
                                ctrForUpdate.Add(ctr.Id, ctr);
                            }
                        }
                    }

                    if (dictCtrMailAdr.ContainsKey(fiasAddress.Id))
                    {
                        var ctr = dictCtrMailAdr[fiasAddress.Id];

                        if (ctr.MailingAddress != fiasAddress.AddressName)
                        {
                            ctr.MailingAddress = fiasAddress.AddressName;

                            if (!ctrForUpdate.ContainsKey(ctr.Id))
                            {
                                ctrForUpdate.Add(ctr.Id, ctr);
                            }
                        }
                    }
                    #endregion

                    #region Кредитные организации
                    if (crOrgJurAddress.ContainsKey(fiasAddress.Id))
                    {
                        foreach (var crOrg in crOrgJurAddress[fiasAddress.Id])
                        {
                            if (crOrg.FiasAddress.AddressName != fiasAddress.AddressName)
                            {
                                crOrg.FiasAddress = fiasAddress;

                                if (!creditOrgForUpdate.ContainsKey(crOrg.Id))
                                {
                                    creditOrgForUpdate.Add(crOrg.Id, crOrg);
                                }
                            }
                        }
                    }

                    if (crOrgPostAddress.ContainsKey(fiasAddress.Id))
                    {
                        foreach (var crOrg in crOrgPostAddress[fiasAddress.Id])
                        {
                            if (crOrg.FiasMailingAddress.AddressName != fiasAddress.AddressName)
                            {
                                crOrg.FiasMailingAddress = fiasAddress;

                                if (!creditOrgForUpdate.ContainsKey(crOrg.Id))
                                {
                                    creditOrgForUpdate.Add(crOrg.Id, crOrg);
                                }
                            }
                        }
                    }
                    #endregion
                }

                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        foreach (var rec in fiasForUpdate)
                        {
                            fiasAddrRepo.Update(rec);
                        }

                        foreach (var kvp in ctrForUpdate)
                        {
                            ctrRepo.Update(kvp.Value);
                        }

                        foreach (var kvp in roForUpdate)
                        {
                            roRepo.Update(kvp.Value);
                        }

                        foreach (var creditOrg in creditOrgForUpdate)
                        {
                            creditOrgRepo.Update(creditOrg.Value);
                        }

                        tr.Commit();
                    }
                    catch (Exception exc)
                    {
                        tr.Rollback();
                        throw exc;
                    }
                }

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(roRepo);
                this.Container.Release(ctrRepo);
                this.Container.Release(fiasAddrRepo);
                this.Container.Release(fiasCustomRepo);
            }
        }

        private Municipality GetMunicipality(FiasAddress address)
        {
            if (address == null || string.IsNullOrEmpty(address.AddressGuid))
            {
                return null;
            }

            var guidMass = address.AddressGuid.Split('#');

            Municipality result = null;

            foreach (var s in guidMass)
            {
                var t = s.Split('_');

                Guid g;

                if (Guid.TryParse(t[1], out g) && g != Guid.Empty)
                {
                    var guide = g.ToString();
                    if (this._cacheMu.ContainsKey(guide))
                    {
                        var mcp = this._cacheMu[guide];
                        if (mcp != null)
                        {
                            result = mcp;
                        }
                    }
                }
            }

            return result != null && result.ParentMo != null ? result.ParentMo : result;
        }

        private string GetAddressForMunicipality(Municipality mo, FiasAddress address)
        {
            if (address == null)
            {
                return string.Empty;
            }

            if (mo == null)
            {
                return string.Empty;
            }

            var result = address.AddressName ?? string.Empty;

            if (string.IsNullOrEmpty(result) && string.IsNullOrEmpty(mo.FiasId))
            {
                return string.Empty;
            }

            IDinamicAddress dinamicAddress = null;

            if (this._cacheDinamicAddress.ContainsKey(mo.FiasId))
            {
                dinamicAddress = this._cacheDinamicAddress[mo.FiasId];
            }

            if (dinamicAddress == null)
            {
                return string.Empty;
            }

            if (result.StartsWith(dinamicAddress.AddressName))
            {
                if (mo.Level == TypeMunicipality.UrbanArea)
                {
                    var urbanAreaName = dinamicAddress.AddressName.Contains("г.")
                        ? dinamicAddress.AddressName.Split("г.")[0]
                        : dinamicAddress.AddressName;

                    result = result.Replace(urbanAreaName, string.Empty).Trim();
                }
                else
                {
                    result = result.Replace(dinamicAddress.AddressName, string.Empty).Trim();
                }
            }

            if (result.StartsWith(","))
            {
                result = result.Substring(1).Trim();
            }

            return result;
        }
    }
}