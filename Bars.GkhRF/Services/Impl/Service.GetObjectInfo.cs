namespace Bars.GkhRf.Services.Impl
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;
    using Bars.GkhRf.Services.DataContracts;
    using Bars.GkhRf.Services.DataContracts.GetObjectInfo;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Enums;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;
    using RealityObject = Bars.Gkh.Entities.RealityObject;

    /// <summary>
    /// Сервис
    /// </summary>
    public partial class Service
    {
        /// <summary>
        /// Получить информацию по объекту
        /// </summary>
        /// <param name="houseId"></param>
        /// <returns></returns>
        public GetObjectInfoResponse GetObjectInfo(string houseId)
        {
            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            NumberFormatInfo numberformat = null;
            if (ci != null)
            {
                ci.NumberFormat.NumberDecimalSeparator = ".";
                numberformat = ci.NumberFormat;
            }

            var idHouse = houseId.ToLong();
            var realityObject = this.Container.Resolve<IDomainService<RealityObject>>().Get(idHouse);
            DataContracts.GetObjectInfo.RealityObject realityObjServ = null;
            if (realityObject != null)
            {
                // Страховая организация
                var belayOrg = this.Container.Resolve<IDomainService<BelayPolicyMkd>>()
                    .GetAll()
                    .Where(x => x.RealityObject.Id == idHouse)
                    .Select(x => x.BelayPolicy.BelayOrganization.Contragent.Name)
                    .FirstOrDefault();

                // Контакты контрагента
                var contragentContact =
                    this.Container.Resolve<IDomainService<ContragentContact>>()
                        .GetAll()
                        .Where(x => (x.Position.Code == "1" || x.Position.Code == "4")
                                    && ((x.DateStartWork.HasValue && x.DateStartWork.Value.Date <= DateTime.Now.Date) || !x.DateStartWork.HasValue)
                                    && ((x.DateEndWork.HasValue && x.DateEndWork.Value.Date >= DateTime.Now.Date) || !x.DateEndWork.HasValue))
                        .Select(x => new { ContragentId = x.Contragent.Id, FIO = x.FullName, x.DateStartWork, x.DateEndWork })
                        .AsEnumerable()
                        .GroupBy(x => x.ContragentId)
                        .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.DateStartWork).FirstOrDefault());

                // Управляющие организации
                var managingOrgs = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                    .GetAll()
                    .Where(x => x.RealityObject.Id == idHouse && x.ManOrgContract.ManagingOrganization != null)
                    .Select(x => new
                    {
                        ManagingOrganizationId = x.ManOrgContract.ManagingOrganization.Id,
                        ContragentId = x.ManOrgContract.ManagingOrganization.Contragent.Id,
                        ContragentName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                        x.ManOrgContract.ManagingOrganization.Contragent.JuridicalAddress,
                        x.ManOrgContract.ManagingOrganization.Contragent.Phone,
                        x.ManOrgContract.TypeContractManOrgRealObj,
                        x.ManOrgContract.StartDate,
                        x.ManOrgContract.EndDate
                    })
                    .AsEnumerable()
                    .Select(x => new ManagingOrgItem
                    {
                        Id = x.ManagingOrganizationId,
                        IdManOrg = x.ManagingOrganizationId,
                        IdCon = x.ContragentId,
                        Name = x.ContragentName,
                        JurAddress = x.JuridicalAddress,
                        FioDirector = contragentContact.ContainsKey(x.ContragentId) ? contragentContact[x.ContragentId].FIO : string.Empty,
                        Phone = x.Phone,
                        StartDate = contragentContact.ContainsKey(x.ContragentId)
                                            && contragentContact[x.ContragentId].DateStartWork.HasValue
                                            && contragentContact[x.ContragentId].DateStartWork.Value != DateTime.MinValue
                                         ? contragentContact[x.ContragentId].DateStartWork.Value.ToShortDateString()
                                         : string.Empty,
                        FinishDate = contragentContact.ContainsKey(x.ContragentId)
                                           && contragentContact[x.ContragentId].DateEndWork.HasValue
                                           && contragentContact[x.ContragentId].DateEndWork.Value != DateTime.MinValue
                                         ? contragentContact[x.ContragentId].DateEndWork.Value.ToShortDateString()
                                         : string.Empty,
                        ContractType = x.TypeContractManOrgRealObj.GetEnumMeta().Display,
                        DateStart = x.StartDate.HasValue && x.StartDate.Value != DateTime.MinValue ? x.StartDate.Value.ToShortDateString() : string.Empty,
                        CurrentDirector = contragentContact.ContainsKey(x.ContragentId)
                                         ? new CurrentDirector
                                         {
                                             FioDirector = contragentContact[x.ContragentId].FIO,
                                             StartDate = contragentContact[x.ContragentId].DateStartWork.HasValue && contragentContact[x.ContragentId].DateStartWork.Value != DateTime.MinValue ? contragentContact[x.ContragentId].DateStartWork.Value.ToShortDateString() : string.Empty,
                                         }
                                         : null
                    })
                    .ToArray();

                // Средства источника финансирования
                var financeSourceResource = this.Container.Resolve<IDomainService<FinanceSourceResource>>()
                    .GetAll()
                    .Select(x => new
                    {
                        ObjectCrid = x.ObjectCr.Id,
                        x.BudgetMu,
                        x.BudgetSubject,
                        x.OwnerResource,
                        x.FundResource
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ObjectCrid)
                    .ToDictionary(x => x.Key, y => y.AsEnumerable());

                // Ход выполнения работ
                var typeWorkCr = this.Container.Resolve<IDomainService<TypeWorkCr>>()
                    .GetAll()
                    .Where(x => x.ObjectCr.RealityObject.Id == idHouse)
                    .Select(x => new
                    {
                        ObjectCrid = x.ObjectCr.Id,
                        x.Id,
                        IdKindWork = x.Work.Id,
                        x.Sum,
                        x.Work.Name,
                        x.DateStartWork,
                        x.PercentOfCompletion
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ObjectCrid)
                    .ToDictionary(x => x.Key);

                // Фотографии работ
                var images = this.Container.Resolve<IDomainService<RealityObjectImage>>()
                    .GetAll()
                    .Where(x => x.RealityObject.Id == idHouse && x.WorkCr != null)
                    .Select(x => new
                    {
                        x.Id,
                        IdKindWork = x.WorkCr.Id,
                        x.ImagesGroup,
                        x.Name,
                        x.ObjectEditDate,
                        x.DateImage,
                        x.File,
                        x.Description
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.IdKindWork)
                    .ToDictionary(x => x.Key, y => y.Select(z => new ImageItem
                    {
                        DateChange = z.ObjectEditDate != DateTime.MinValue ? z.ObjectEditDate.ToShortDateString() : string.Empty,
                        DateImage = z.DateImage.HasValue && z.DateImage.Value != DateTime.MinValue ? z.DateImage.Value.ToShortDateString() : string.Empty,
                        Id = z.Id,
                        Name = z.Name.ToStr(),
                        Group = z.ImagesGroup.GetEnumMeta().Display.ToStr(),
                        NameFile = z.File != null ? z.File.FullName.ToStr() : string.Empty,
                        Notation = z.Description.ToStr(),
                        Value = this.GetFiles(z.File)
                    })
                                                                   .ToArray());

                // Подрядные организации
                var serviceOrg = this.Container.Resolve<IDomainService<BuildContract>>()
                    .GetAll()
                    .Where(x => x.ObjectCr.RealityObject.Id == idHouse && x.Builder != null)
                    .Select(x => new
                    {
                        ObjectCrId = x.ObjectCr.Id,
                        BuildContractId = x.Id,
                        ContragentId = x.Builder.Contragent.Id,
                        ContragentName = x.Builder.Contragent.Name,
                        x.Builder.Contragent.JuridicalAddress,
                        x.Builder.Contragent.Phone
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.ObjectCrId)
                    .ToDictionary(x => x.Key, y => y.Select(z => new ServiceOrgItem
                    {
                        IdDoc = z.BuildContractId,
                        Id = z.ContragentId,
                        Name = z.ContragentName.ToStr(),
                        JurAddress = z.JuridicalAddress.ToStr(),
                        FioDirector = contragentContact.ContainsKey(z.ContragentId) ? contragentContact[z.ContragentId].FIO.ToStr() : string.Empty,
                        Phone = z.Phone
                    })
                                                                    .ToArray());

                // Программы и объекты КР
                var programmCr = this.Container.Resolve<IDomainService<ObjectCr>>()
                    .GetAll()
                    .Where(x => x.RealityObject.Id == idHouse && x.ProgramCr.UsedInExport && x.ProgramCr.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Hidden)
                    .Select(x => new
                    {
                        ObjectCrId = x.Id,
                        ObjectCrDateChange = x.ObjectEditDate,
                        ProgrammId = x.ProgramCr.Id,
                        x.ProgramCr.Period.DateStart.Year,
                        ProgramCrName = x.ProgramCr.Name
                    })
                    .AsEnumerable()
                    .Select(x => new ProgrammCrItem
                    {
                        Id = x.ObjectCrId,
                        DateChange = x.ObjectCrDateChange != DateTime.MinValue ? x.ObjectCrDateChange.ToShortDateString() : string.Empty,
                        IdProg = x.ProgrammId,
                        Year = x.Year.ToStr(),
                        NameProg = x.ProgramCrName,
                        BudgetMo = financeSourceResource.ContainsKey(x.ObjectCrId) ? financeSourceResource[x.ObjectCrId].Where(z => z.BudgetMu.HasValue).Sum(z => z.BudgetMu).Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                        BudgetRegion = financeSourceResource.ContainsKey(x.ObjectCrId) ? financeSourceResource[x.ObjectCrId].Where(z => z.BudgetSubject.HasValue).Sum(z => z.BudgetSubject).Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                        ResourcesOwner = financeSourceResource.ContainsKey(x.ObjectCrId) ? financeSourceResource[x.ObjectCrId].Where(z => z.OwnerResource.HasValue).Sum(z => z.OwnerResource).Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                        Limit = typeWorkCr.ContainsKey(x.ObjectCrId) ? typeWorkCr[x.ObjectCrId].Where(z => z.Sum.HasValue).Sum(z => z.Sum).Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                        ServiceOrg = serviceOrg.ContainsKey(x.ObjectCrId) ? serviceOrg[x.ObjectCrId] : null,
                        Work = typeWorkCr.ContainsKey(x.ObjectCrId)
                                            ? typeWorkCr[x.ObjectCrId].Select(y => new WorkItem
                                            {
                                                IdWork = y.Id,
                                                Name = y.Name,
                                                Percent = y.PercentOfCompletion?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                                                StartDate = y.DateStartWork.HasValue && y.DateStartWork.Value != DateTime.MinValue ? y.DateStartWork.Value.ToShortDateString() : string.Empty,
                                                Image = images.ContainsKey(y.IdKindWork) ? images[y.IdKindWork] : null
                                            })
                                                                       .ToArray()
                                            : null
                    });

                // Движение денежных средств
                var movementMoney = this.Container.Resolve<IDomainService<PaymentItem>>()
                    .GetAll()
                    .Where(x => x.Payment.RealityObject.Id == idHouse)
                    .Select(x => new
                    {
                        RealityObjectId = x.Payment.RealityObject.Id,
                        x.Id,
                        x.TypePayment,
                        x.ChargeDate,
                        x.IncomeBalance,
                        x.OutgoingBalance,
                        x.Recalculation,
                        x.ChargePopulation,
                        x.PaidPopulation,
                        x.TotalArea,
                        ManOrg = x.ManagingOrganization.Contragent.Name
                    })
                    .AsEnumerable()
                    .Select(x => new MovementMoneyItem
                    {
                        Id = x.Id,
                        Code = this.GetCodePaymentRf(x.TypePayment),
                        Date = x.ChargeDate.HasValue && x.ChargeDate.Value != DateTime.MinValue ? x.ChargeDate.Value.ToShortDateString() : string.Empty,
                        ImpBalance = x.IncomeBalance?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                        ExpBalance = x.OutgoingBalance?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                        Recalculation = x.Recalculation?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                        Accrual = x.ChargePopulation?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                        Payed = x.PaidPopulation?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                        GeneralArea = x.TotalArea?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                        ManOrg = x.ManOrg
                    })
                    .ToArray();

                // Денежные средства регфонд
                var transferRfRecObjs =
                    this.Container.Resolve<IDomainService<TransferRfRecObj>>()
                        .GetAll()
                        .Where(x => x.RealityObject.Id == idHouse)
                        .Select(x => new { x.TransferRfRecord.TransferDate, x.TransferRfRecord.DateFrom, x.Sum })
                        .AsEnumerable()
                        .Select(x => new TransfersRegFundItem
                        {
                            DateDecree = x.DateFrom.HasValue && x.DateFrom.Value != DateTime.MinValue ? x.DateFrom.Value.ToShortDateString() : string.Empty,
                            DateRegFund = x.TransferDate.HasValue && x.TransferDate.Value != DateTime.MinValue ? x.TransferDate.Value.ToShortDateString() : string.Empty,
                            SumTransfer = x.Sum?.RoundDecimal(2).ToString(numberformat) ?? string.Empty
                        })
                        .ToArray();

                // Расход кап. ремонта
                var paymentOrders =
                    this.Container.Resolve<IDomainService<BasePaymentOrder>>()
                        .GetAll()
                        .Where(x => x.BankStatement.ObjectCr.RealityObject.Id == idHouse && x.TypePaymentOrder == TypePaymentOrder.In)
                        .Select(x => new { x.DocumentDate, x.Sum })
                        .AsEnumerable()
                        .Select(x => new ExpenditureCrItem
                        {
                            Date = x.DocumentDate.HasValue && x.DocumentDate.Value != DateTime.MinValue ? x.DocumentDate.Value.ToShortDateString() : string.Empty,
                            TransferSum = x.Sum?.RoundDecimal(2).ToString(numberformat) ?? string.Empty
                        })
                        .ToArray();

                var rooms = this.Container.Resolve<IDomainService<Room>>().GetAll()
                    .Where(x => x.RealityObject.Id == idHouse)
                    .Select(x => new
                    {
                        x.Id,
                        x.Type,
                        x.Area
                    })
                    .ToArray();

                realityObjServ = new DataContracts.GetObjectInfo.RealityObject
                {
                    Address = realityObject.Address,
                    DateChange = realityObject.ObjectEditDate != DateTime.MinValue ? realityObject.ObjectEditDate.ToShortDateString() : string.Empty,
                    AddressKladr = realityObject.GkhCode,
                    ExpluatationYear = realityObject.DateCommissioning.HasValue ? realityObject.DateCommissioning.Value.Year.ToStr() : string.Empty,
                    Floor = realityObject.Floors.GetValueOrDefault(),
                    ApartamentCount = realityObject.NumberApartments.GetValueOrDefault(),
                    LivingPeople = realityObject.NumberLiving.GetValueOrDefault(),
                    GeneralArea = realityObject.AreaMkd?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                    Deterioration = realityObject.PhysicalWear?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                    YearCapRepair = realityObject.DateLastOverhaul.HasValue ? realityObject.DateLastOverhaul.Value.Year.ToStr() : string.Empty,
                    SeriaHouse = realityObject.SeriesHome,
                    Fasad = realityObject.HavingBasement.GetEnumMeta().Display,
                    GatesCount = realityObject.NumberEntrances.GetValueOrDefault(),
                    LivingNotLivingArea = realityObject.AreaLivingNotLivingMkd?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                    LivingArea = realityObject.AreaLiving?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                    LivingAreaPeople = realityObject.AreaLivingOwned?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                    LiftCount = realityObject.NumberLifts.GetValueOrDefault(),
                    NotLivingArea = rooms.Length != 0 ? rooms.Where(x => x.Type == RoomType.NonLiving).SafeSum(x => x.Area).RoundDecimal(2).ToString(numberformat) : string.Empty,
                    PublicArea = realityObject.AreaCommonUsage?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                    LivingRoomCount = rooms.Length != 0 ? rooms.Count(x => x.Type == RoomType.Living) : 0,
                    NotLivingRoomCount = rooms.Length != 0 ? rooms.Count(x => x.Type == RoomType.NonLiving) : 0,
                    FasadArea = realityObject.AreaBasement?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                    RoofType = realityObject.TypeRoof.GetEnumMeta().Display,
                    RoofMaterial = realityObject.RoofingMaterial != null ? realityObject.RoofingMaterial.Name : string.Empty,
                    WallMaterial = realityObject.WallMaterial != null ? realityObject.WallMaterial.Name : string.Empty,
                    Date = realityObject.DateTechInspection.HasValue && realityObject.DateTechInspection != DateTime.MinValue ? realityObject.DateTechInspection.Value.ToShortDateString() : string.Empty,
                    Municipality = realityObject.Municipality.Name,
                    Id = realityObject.Id,
                    Insurance = belayOrg,
                    ManagingOrg = managingOrgs,
                    ProgrammCr = programmCr.ToArray(),
                    MovementMoney = movementMoney,
                    ResourceRf = new ResourceRf
                    {
                        ExpenditureCr = paymentOrders,
                        TransfersRegFund = transferRfRecObjs
                    }
                };
            }

            var result = realityObject == null ? Result.DataNotFound : Result.NoErrors;
            return new GetObjectInfoResponse { RealityObject = realityObjServ, Result = result };
        }

        private string GetCodePaymentRf(TypePayment typePayment)
        {
            string code = null;
            switch (typePayment)
            {
                case TypePayment.Cr:
                    code = "267";
                    break;
                case TypePayment.HireRegFund:
                    code = "268";
                    break;
                case TypePayment.Cr185:
                    code = "269";
                    break;
                case TypePayment.BuildingCurrentRepair:
                    code = "2";
                    break;
                case TypePayment.SanitaryEngineeringRepair:
                    code = "21";
                    break;
                case TypePayment.HeatingRepair:
                    code = "22";
                    break;
                case TypePayment.ElectricalRepair:
                    code = "23";
                    break;
                case TypePayment.BuildingRepair:
                    code = "259";
                    break;
            }

            return code;
        }

        /// <summary>
        /// Получаем фаил
        /// </summary>
        /// <param name="fileName">описание файла</param>
        /// <returns>result</returns>
        public string GetFiles(FileInfo fileName)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            try
            {
                var result = fileName != null ? fileManager.GetBase64String(fileName) : string.Empty;

                return result;
            }
            catch (FileNotFoundException)
            {
                return string.Empty;
            }
            finally
            {
                this.Container.Release(fileManager);
            }
        }
    }
}