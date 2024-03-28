namespace Bars.Gkh.Gis.DomainService.Dict.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Gis.Entities.Dict;
    using Bars.Gkh.Gis.Enum;

    /// <summary>
    /// Конвертер данных
    /// </summary>
    public partial class EiasTariffImportService
    {
        private GisTariffDict GetGisTariffEntity(TariffData tariff)
        {
            var newEntity = new GisTariffDict
            {
                EiasUploadDate = DateTime.ParseExact(tariff.FileName, "MM.yyyy", CultureInfo.InvariantCulture),
                EiasEditDate = tariff.AuditDate,
                Municipality = this.GetMunicipality(tariff),
                Service = this.GetService(tariff),
                Contragent = this.GetContragent(tariff),
                ActivityKind = tariff.ContragentsVdet,
                ContragentName = tariff.BpContragentsName,
                StartDate = tariff.TarifinfoTarifstartdate.Value,
                EndDate = tariff.TarifinfoTarifenddate.Value,
                TariffKind = this.GetTariffKind(tariff),
                ZoneCount = this.GetZoneCount(tariff),
                TariffValue = tariff.ServicesServiceid != 25 ? tariff.TarifinfoTarif1Value : null,
                TariffValue1 = tariff.ServicesServiceid == 25 ? tariff.TarifinfoTarif1Value : null,
                TariffValue2 = tariff.TarifinfoTarif2Value,
                TariffValue3 = tariff.TarifinfoTarif3Value,
                IsNdsInclude = this.GetBoolValue(tariff.TarifinfoNds),
                IsSocialNorm = this.GetBoolValue(tariff.TarifinfoSoc),
                IsMeterExists = this.GetBoolValue(tariff.TarifinfoCe),
                IsElectricStoveExists = this.GetBoolValue(tariff.TarifinfoElectriccooker),
                Floor = tariff.TarifinfoStage,
                ConsumerType = tariff.TarifinfoConsumer,
                SettelmentType = tariff.TarifinfoCity,
                ConsumerByElectricEnergyType = tariff.TarifinfoTypeconsumer,
                RegulatedPeriodAttribute = tariff.TarifinfoDpr,
                BasePeriodAttribute = tariff.TarifinfoDprBp
            };

            var etityId = this.tariffDict.Get(tariff.Id);
            newEntity.Id = etityId;
            newEntity.ExternalId = tariff.Id;

            if (this.IsValid(tariff, newEntity) && this.importedRecords.Add(tariff.Id))
            {
                return newEntity;
            }

            return null;
        }

        private bool IsValid(TariffData tariffData, GisTariffDict tariffEntity)
        {
            if (tariffEntity.Municipality == null)
            {
                this.LogImport.Warn(tariffData.Id.ToStr(),
                    "Не опеределен муниципальный район по указанному MRLIST_MUNICIPALGUID",
                    tariffData.MrlistMunicipalguid);
                return false;
            }

            if (tariffEntity.Service == null)
            {
                this.LogImport.Warn(tariffData.Id.ToStr(),
                    "Не определена услуга по указанному SERVICES_SERVISEID",
                    tariffData.ServicesServiceid.ToStr());
                return false;
            }

            if (tariffEntity.Contragent == null)
            {
                this.LogImport.Warn(tariffData.Id.ToStr(),
                    "Не определен поставщик ресурсов по указанным реквизитам CONTRAGENTS_INN, CONTRAGENTS_KPP",
                    tariffData.ContragentsInn,
                    tariffData.ContragentsKpp);
                return false;
            }

            if (!tariffData.TarifinfoTarifstartdate.HasValue)
            {
                this.LogImport.Warn(tariffData.Id.ToStr(),
                    "Не определена дата начала периода по указанному TARIFINFO_TARIFSTARTDATE",
                    tariffData.TarifinfoTarifstartdate.ToStr());
                return false;
            }

            if (!tariffData.TarifinfoTarifenddate.HasValue)
            {
                this.LogImport.Warn(tariffData.Id.ToStr(),
                    "Не определена дата окончания периода по указанному TARIFINFO_TARIFENDDATE",
                    tariffData.TarifinfoTarifenddate.ToStr());
                return false;
            }

            if (!tariffData.TarifinfoTarifkind.HasValue)
            {
                this.LogImport.Warn(tariffData.Id.ToStr(),
                    "Не определен вид тарифа по указанному TARIFINFO_TARIFKIND",
                    tariffData.TarifinfoTarifkind.ToStr());
                return false;
            }

            if (!tariffData.TarifinfoZones.HasValue)
            {
                this.LogImport.Warn(tariffData.Id.ToStr(),
                    "Не определено количество зон по указанному TARIFINFO_ZONES",
                    tariffData.TarifinfoZones.ToStr());
                return false;
            }

            if (!tariffData.TarifinfoTarif1Value.HasValue)
            {
                this.LogImport.Warn(tariffData.Id.ToStr(),
                    "Не определено значение тарифа по указанному TARIFINFO_TARIF1VALUE",
                    tariffData.TarifinfoTarif1Value.ToStr());
                return false;
            }

            if (tariffData.TarifinfoZones > 1 && !tariffData.TarifinfoTarif2Value.HasValue)
            {
                this.LogImport.Warn(tariffData.Id.ToStr(),
                    "Не определено значение тарифа 2 по указанному TARIFINFO_TARIF2VALUE",
                    tariffData.TarifinfoTarif2Value.ToStr());
                return false;
            }

            if (tariffData.TarifinfoZones > 2 && !tariffData.TarifinfoTarif3Value.HasValue)
            {
                this.LogImport.Warn(tariffData.Id.ToStr(),
                    "Не определено значение тарифа 3 по указанному TARIFINFO_TARIF3VALUE",
                    tariffData.TarifinfoTarif3Value.ToStr());
                return false;
            }

            return true;
        }

        private Municipality GetMunicipality(TariffData tariff)
        {
            var moId = this.municipalityDict.Get(tariff.MrlistMunicipalguid);
            var municipality = moId > 0
                ? new Municipality { Id = moId }
                : null;

            return municipality;
        }

        private ServiceDictionary GetService(TariffData tariff)
        {
            var serviceId = this.serviceDict.Get(tariff.ServicesServiceid ?? 0);
            var service = serviceId > 0
                ? new ServiceDictionary { Id = serviceId }
                : null;

            return service;
        }

        private Contragent GetContragent(TariffData tariff)
        {
            var contragentId = this.contragentDict.Get(tariff.ContragentsKpp ?? string.Empty)?.Get(tariff.ContragentsInn) ?? 0;
            var contragent = contragentId > 0
                ? new Contragent { Id = contragentId }
                : null;

            return contragent;
        }

        private GisTariffKind GetTariffKind(TariffData tariff)
        {
            if (tariff.TarifinfoTarifkind.HasValue)
            {
                return tariff.TarifinfoTarifkind.Value;
            }

            return (GisTariffKind) (-1);
        }

        private int GetZoneCount(TariffData tariff)
        {
            if (tariff.TarifinfoZones.HasValue && tariff.TarifinfoZones > 0)
            {
                return tariff.TarifinfoZones.Value;
            }

            return -1;
        }

        private bool? GetBoolValue(int? value)
        {
            return value == 1
                ? true
                : value == 2
                    ? false
                    : (bool?) null;
        }

        /// <summary>
        /// Таблица сопоставления GUID МО в системе ЕИАС и ЖКХ
        /// </summary>
        private static readonly IReadOnlyDictionary<string, string> MoGuidDict = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>
        {
            { "71684fb9-6d38-19b8-e040-a8c028791e90", "3b67dc8e-79b1-43f4-8f9e-2b4990a1a922" }, // Агрызский муниципальный район
            { "7163a8f6-1712-d663-e040-a8c028790d43", "25108675-9fd9-4325-a1fd-c392c3feedac" }, // Азнакаевский муниципальный район
            { "6ecd5daf-c859-7625-e040-a8c0287916b5", "c4339d8a-d4ef-42e0-be75-6bc551956e5c" }, // Аксубаевский муниципальный район
            { "6f6d202e-fbe4-ec81-e040-a8c0287978e2", "bb9c86eb-30de-4b8f-9ea8-9edc71fc0488" }, // Актанышский муниципальный район
            { "7300d63c-60ed-a113-e040-a8c028790c98", "f7521d33-7cf3-4f6e-bb66-d9e04912b6fc" }, // Алексеевский муниципальный район
            { "7b01b10f-c892-bdf0-e040-007f0100618a", "1053347b-4de8-405d-970e-b42003d49916" }, // Алькеевский муниципальный район
            { "7041d6a3-6588-bcf9-e040-a8c028793082", "b10beeff-7669-45aa-9127-b1cc9db1cc4c" }, // Альметьевский муниципальный район
            { "7384affa-9f4d-d4c6-e040-a8c028791850", "67e7162e-14d0-4418-9923-a948ed6c2936" }, // Апастовский муниципальный район
            { "7041dbee-4311-4b69-e040-a8c02879296b", "8b3bfb8a-d712-4ecd-9192-a7259ea141dc" }, // Арский муниципальный район
            { "752bb1f0-85ac-ca21-e040-a8c0287954b2", "72faa57c-6281-4758-9ba3-516558f19eab" }, // Атнинский муниципальный район
            { "7300d63c-60e7-a113-e040-a8c028790c98", "a2093ccb-a5b8-4956-ad05-c0886fc95cdb" }, // Бавлинский муниципальный район
            { "7300ca13-61a2-d840-e040-a8c028790b1c", "7fca8877-4ce1-443d-b64b-7bf6fb84198a" }, // Балтасинский муниципальный район
            { "7300ca13-61d1-d840-e040-a8c028790b1c", "c4fc378d-08d2-4adc-851f-fe35335e6bc8" }, // Бугульминский муниципальный район
            { "7041d93f-47cc-e4e2-e040-a8c028792969", "2f40b5af-a9f6-4056-8581-48c9d5c44e7e" }, // Буинский муниципальный район
            { "7300da0a-86a3-0c02-e040-a8c028790b22", "6c1c7db4-37b3-4544-9a47-255c02207388" }, // Верхнеуслонский муниципальный район
            { "7300ca13-61a8-d840-e040-a8c028790b1c", "56197a9f-2b30-486d-b31a-21d93c589bb7" }, // Высокогорский муниципальный район
            { "7101f76d-734d-60f0-e040-a8c028794b27", "830b47d1-6fb5-4d9a-b5a0-641258c06fe3" }, // Город Казань (Давай пока все сажать на Казань авиастроительный район. если что потом перекидаем (С) Руслан Саксонов)
            { "7300da0a-86ab-0c02-e040-a8c028790b22", "748d7afa-7407-4876-9f40-764ecdd09bbd" }, // Город Набережные Челны
            { "7300d63c-60ff-a113-e040-a8c028790c98", "a22f07cb-1481-4278-8182-e342b2f0785d" }, // Дрожжановский муниципальный район
            { "6ffaaed4-104e-adbc-e040-a8c028793838", "ac240902-34b0-491c-896b-af76cf6fd4ac" }, // Елабужский муниципальный район
            { "7041d93f-47c4-e4e2-e040-a8c028792969", "3a44bebe-99e7-4e97-b79a-ea36b34e6be1" }, // Заинский муниципальный район
            { "72fda96b-7943-721c-e040-a8c0287931ff", "163ed231-c194-4452-ab1a-7f991b4d2f5c" }, // Зеленодольский муниципальный район
            { "718da3d1-84be-6542-e040-a8c0287933c9", "cc249bd4-51d8-4bc6-bdda-340229f439d2" }, // Кайбицкий муниципальный район
            { "6fe8ddbf-6c16-e135-e040-a8c02879377b", "7e6e53c8-30d0-4d74-8749-2edb12566989" }, // Камско-Устьинский муниципальный район
            { "7300ca13-61c5-d840-e040-a8c028790b1c", "0298be6c-8d8a-4f33-b742-8b8769d86d17" }, // Кукморский муниципальный район
            { "73019bf5-32a4-5266-e040-a8c028791498", "d11bb1b1-27ae-44b6-9d0e-b6fa43f75625" }, // Лаишевский муниципальный район
            { "6f6d1780-b30d-4a01-e040-a8c028791d3d", "75387230-9ca5-4239-be4d-dc4a77a2d6ab" }, // Лениногорский муниципальный район
            { "7300cb5d-6c61-3c92-e040-a8c028790b20", "fdc0d460-4cf0-4dce-9887-043d181c558e" }, // Мамадышский муниципальный район
            { "7300d63c-610a-a113-e040-a8c028790c98", "d5b8fcfb-74a9-4cfa-88e6-e508cb85575b" }, // Менделеевский муниципальный район
            { "733b06e9-5f77-b1cf-e040-a8c028790b48", "72a8ac4a-7c51-4b54-be84-03c9ab3ca362" }, // Мензелинский муниципальный район
            { "73018e76-7931-1fe4-e040-a8c0287914a4", "4d9587db-77da-4f96-8331-bf04599a2878" }, // Муслюмовский муниципальный район
            { "704f2f3c-2342-d4cb-e040-a8c02879093f", "dd36813f-e9e1-482c-8e77-02397d0a3eb3" }, // Нижнекамский муниципальный район
            { "7300cb5d-6c73-3c92-e040-a8c028790b20", "ffe9978a-c14a-41d8-8b21-06233b063267" }, // Новошешминский муниципальный район
            { "7300d779-9b7d-da09-e040-a8c028790b1e", "69dd0cab-c31d-4345-8429-b8540728c484" }, // Нурлатский муниципальный район
            { "73bf6d48-cedc-8699-e040-a8c0287949d4", "64bad346-6dda-4d89-be4e-3e73b0cf1f45" }, // Пестречинский муниципальный район
            { "7041d7ef-6023-3e62-e040-a8c02879296d", "5d20a8a2-cd23-4af0-8f66-0a509ba9e84d" }, // Рыбно-Слободский муниципальный район
            { "7301d501-0e80-6738-e040-a8c02879173b", "d2a1137e-e196-46d3-a8f8-e7cfbb86412b" }, // Сабинский муниципальный район
            { "70516f21-b07b-e072-e040-a8c0287925ec", "f1ca5836-fb9f-4829-9d31-e4fbb3106813" }, // Сармановский муниципальный район
            { "73018e76-7929-1fe4-e040-a8c0287914a4", "81b32a2d-34a6-4ef8-bc97-005c3d5d2b0e" }, // Спасский муниципальный район
            { "776626fa-c25c-5b5e-e040-a8c02879523c", "ff141bdc-ba5a-4d93-ba21-f114931ee193" }, // Тетюшский муниципальный район
            { "6f6d393f-8f06-8eb6-e040-a8c0287978da", "e1594936-3d9f-48d0-9699-6a313885cad1" }, // Тукаевский муниципальный район
            { "78b3eef6-0584-126e-e040-000000007697", "ee0c0b9a-c5fe-4c81-a966-93b8af831441" }, // Тюлячинский муниципальный район
            { "7301d501-0e85-6738-e040-a8c02879173b", "75124fb3-dd85-42b7-8471-ccea65e6f60f" }, // Черемшанский муниципальный район
            { "7300da0a-86bb-0c02-e040-a8c028790b22", "019d7b51-664b-452f-aab7-0f6a73d81dcb" }, // Чистопольский муниципальный район
            { "6fe8e540-470f-2322-e040-a8c028793775", "6efefcee-49b7-403b-b719-46b80cf8d1a8" } // Ютазинский муниципальный район
        });
    }
} 