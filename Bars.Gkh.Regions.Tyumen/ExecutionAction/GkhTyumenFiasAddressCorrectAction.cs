namespace Bars.Gkh.Regions.Tyumen.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Действие "ЖилФонд - Корректировка адресов Контрагентов и Жилых домов Относительно МО"
    /// </summary>
    public class GkhTyumenFiasAddressCorrectAction : BaseExecutionAction
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        /// <summary>
        /// Домен-сервис МО
        /// </summary>
        public IDomainService<Municipality> MoDomain { get; set; }

        /// <summary>
        /// Вспомогательный словарь МО
        /// </summary>
        private Dictionary<string, Municipality> _cacheMu { get; set; }

        /// <summary>
        /// Вспомогательный словарь адресов ФИАС
        /// </summary>
        private Dictionary<string, IDinamicAddress> _cacheDinamicAddress { get; set; }

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description
            => "Cкрипт необходим когда адрес необходимо полностью перегенерировать относительно Улицы все составляющие и Отрезать по МО";

        /// <summary>
        /// Название действия
        /// </summary>
        public override string Name => "ЖилФонд Тюмень - Корректировка адресов Контрагентов и Жилых домов Относительно МО";

        /// <summary>
        /// Ссылка на метод действия
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        /// Метод реализующий основную логику действия
        /// </summary>
        /// <returns>Результат выполнения действия</returns>
        private BaseDataResult Execute()
        {
            /*
             В общем, данный метод берет все адреса и еще раз запрашивает для них DinamicAddress.
             Дальше, сравнивает с тем, что на самом деле сохранено в адресе. Если адреса разные, то заменяет и обновляет и Контрагента, и Жилые дома.
            */

            var roRepo = this.Container.Resolve<IRepository<RealityObject>>();
            var ctrRepo = this.Container.Resolve<IRepository<Contragent>>();
            var fiasAddrRepo = this.Container.Resolve<IRepository<FiasAddress>>();
            var fiasHouseRepo = this.Container.Resolve<IRepository<FiasHouse>>();
            var fiasCustomRepo = this.Container.Resolve<IGkhCustomFiasRepository>();
            try
            {
                var fiasForUpdate = new List<FiasAddress>();
                var roForUpdate = new Dictionary<long, RealityObject>();
                var ctrForUpdate = new Dictionary<long, Contragent>();

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

                var houseData = fiasHouseRepo.GetAll()
                    .GroupBy(x => x.HouseGuid)
                    .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.EndDate ?? DateTime.MinValue).FirstOrDefault());

                var fiasData = fiasAddrRepo.GetAll()
                    .WhereNotEmptyString(x => x.PlaceGuidId)
                    .Where(
                        x => roRepo.GetAll().Any(y => y.FiasAddress.Id == x.Id)
                            || ctrRepo.GetAll().Any(
                                y => y.FiasJuridicalAddress.Id == x.Id
                                    || y.FiasFactAddress.Id == x.Id
                                    || y.FiasMailingAddress.Id == x.Id))
                    .ToList();

                foreach (var fiasAddress in fiasData)
                {
                    var needUpdate = false;

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

                    var addressName = new StringBuilder(dynamicAddr.AddressName);

                    if (fiasAddress.HouseGuid != null)
                    {
                        var fiasHouse = houseData.Get(fiasAddress.HouseGuid.Value);

                        addressName.Append($", д. {fiasHouse.HouseNum}");

                        if (fiasHouse.BuildNum.IsNotEmpty())
                        {
                            addressName.Append($", корп. {fiasHouse.BuildNum}");
                        }

                        if (fiasAddress.House != fiasHouse.HouseNum 
                            || fiasAddress.Building != fiasHouse.StrucNum 
                            || fiasAddress.Housing != fiasHouse.BuildNum
                            || fiasAddress.Letter != fiasHouse.StructureType.ToInt().ToStr())
                        {
                            fiasAddress.House = fiasHouse.HouseNum;
                            fiasAddress.Building = fiasHouse.StrucNum;
                            fiasAddress.Housing = fiasHouse.BuildNum;
                            fiasAddress.Letter = fiasHouse.StructureType.ToInt().ToStr();
                            needUpdate = true;
                        }

                        if (fiasHouse.StrucNum.IsNotEmpty())
                        {
                            var structName = fiasHouse.StructureType != FiasStructureTypeEnum.NotDefined
                                ? fiasHouse.StructureType.GetDisplayName().ToLower()
                                : FiasStructureTypeEnum.Structure.GetDisplayName().ToLower();

                            addressName.AppendFormat(", {0} {1}", structName, fiasHouse.StrucNum);
                        }
                    }
                    else
                    {
                        FiasStructureTypeEnum strType;

                        // заполняем оставшиеся значения
                        if (!string.IsNullOrEmpty(fiasAddress.House))
                        {
                            addressName.Append($", д. {fiasAddress.House}");

                            if (!Enum.TryParse(fiasAddress.Letter, out strType) && fiasAddress.Building.IsNotEmpty())
                            {
                                fiasAddress.House = fiasAddress.House + fiasAddress.Letter;
                                addressName.Append(fiasAddress.Letter);
                            }
                        }

                        if (!string.IsNullOrEmpty(fiasAddress.Housing))
                        {
                            addressName.Append($", корп. {fiasAddress.Housing}");
                        }

                        if (Enum.TryParse(fiasAddress.Letter, out strType) && fiasAddress.Building.IsNotEmpty())
                        {
                            var structName = strType != FiasStructureTypeEnum.NotDefined
                                ? strType.GetDisplayName().ToLower()
                                : FiasStructureTypeEnum.Structure.GetDisplayName().ToLower();

                            addressName.AppendFormat(", {0} {1}", structName, fiasAddress.Building);
                        }
                        else
                        {
                            if (fiasAddress.Building.IsNotEmpty())
                            {
                                fiasAddress.Letter = FiasStructureTypeEnum.Structure.ToInt().ToStr();

                                addressName.AppendFormat(", {0} {1}", FiasStructureTypeEnum.Structure.GetDisplayName(), fiasAddress.Building);
                            }

                            if (fiasAddress.Letter.IsNotEmpty() && fiasAddress.Building.IsEmpty())
                            {
                                fiasAddress.Building = fiasAddress.Letter;
                                fiasAddress.Letter = FiasStructureTypeEnum.Letter.ToInt().ToStr();

                                addressName.AppendFormat(", {0} {1}", FiasStructureTypeEnum.Letter.GetDisplayName(), fiasAddress.Building);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(fiasAddress.Flat))
                    {
                        addressName.Append($", кв. {fiasAddress.Flat}");
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

                    if (fiasAddress.AddressName != addressName.ToString()
                        || (!string.IsNullOrEmpty(placeAdrName) && fiasAddress.PlaceAddressName != placeAdrName) || needUpdate)
                    {
                        fiasAddress.AddressName = addressName.ToString();
                        fiasAddress.PlaceAddressName = placeAdrName;

                        fiasForUpdate.Add(fiasAddress);
                    }

                    // Поскольку поняли что адрес над оменять значит 
                    if (dictRoAdr.ContainsKey(fiasAddress.Id))
                    {
                        var ro = dictRoAdr[fiasAddress.Id];

                        var settlement = Utils.GetSettlementByRealityObject(this.Container, ro);

                        var newMo = this.GetMunicipality(fiasAddress) ?? (settlement.ReturnSafe(x => x.ParentMo) ?? settlement);

                        var newAddress = newMo != null
                            ? this.GetAddressForMunicipality(newMo, fiasAddress)
                            : fiasAddress.AddressName;

                        // если что изменилось
                        if ((newMo != null && ro.Municipality != null && newMo.Id != ro.Municipality.Id) || newAddress != ro.Address)
                        {
                            ro.Municipality = newMo;

                            ro.Address = newAddress;

                            if (!roForUpdate.ContainsKey(ro.Id))
                            {
                                roForUpdate.Add(ro.Id, ro);
                            }
                        }
                    }

                    if (dictCtrJurAdr.ContainsKey(fiasAddress.Id))
                    {
                        var ctr = dictCtrJurAdr[fiasAddress.Id];

                        var newMo = this.GetMunicipality(fiasAddress);
                        var newAddress = newMo != null
                            ? this.GetAddressForMunicipality(newMo, fiasAddress)
                            : fiasAddress.AddressName;

                        // если что изменилось
                        if ((newMo != null && ctr.Municipality != null && newMo.Id != ctr.Municipality.Id) || newAddress != ctr.JuridicalAddress)
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

                if (t.Length < 2)
                {
                    continue;
                }

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

            var addressParts = dinamicAddress.AddressName.Split(",");

            if (result.StartsWith(addressParts.First()))
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

            if (result.Contains(addressParts[0]))
            {
                result = result.Replace(addressParts[0], "");
            }

            if (result.StartsWith(","))
            {
                result = result.Substring(1).Trim();
            }

            return result;
        }
    }
}