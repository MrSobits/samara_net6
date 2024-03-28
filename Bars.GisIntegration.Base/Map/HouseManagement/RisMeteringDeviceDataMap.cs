namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    public class RisMeteringDeviceDataMap : BaseRisEntityMap<RisMeteringDeviceData>
    {
        public RisMeteringDeviceDataMap()
            : base("Bars.Gkh.Ris.Entities.HouseManagement.RisMeteringDeviceData", "RIS_METERING_DEVICE_DATA")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.MeteringDeviceType, "MeteringDeviceType").Column("METERING_DEVICE_TYPE");
            this.Property(x => x.MeteringDeviceNumber, "MeteringDeviceNumber").Column("METERING_DEVICE_NUMBER").Length(50);
            this.Property(x => x.MeteringDeviceStamp, "MeteringDeviceStamp").Column("METERING_DEVICE_STAMP").Length(50);
            this.Property(x => x.InstallationDate, "InstallationDate").Column("INSTALLATION_DATE");
            this.Property(x => x.CommissioningDate, "CommissioningDate").Column("COMMISSIONING_DATE");
            this.Property(x => x.ManualModeMetering, "ManualModeMetering").Column("MANUAL_MODE_METERING");
            this.Property(x => x.FirstVerificationDate, "FirstVerificationDate").Column("FIRST_VERIFICATION_DATE");
            this.Property(x => x.VerificationInterval, "VerificationInterval").Column("VERIFICATION_INTERVAL").Length(50);
            this.Property(x => x.VerificationIntervalGuid, "VerificationIntervalGuid").Column("VERIFICATION_INTERVAL_GUID").Length(50);
            this.Property(x => x.DeviceType, "DeviceType").Column("DEVICE_TYPE");                      
            this.Property(x => x.MeteringValueT1, "MeteringValueT1").Column("METERING_VALUE_T1");
            this.Property(x => x.MeteringValueT2, "MeteringValueT2").Column("METERING_VALUE_T2");
            this.Property(x => x.MeteringValueT3, "MeteringValueT3").Column("METERING_VALUE_T3");
            this.Property(x => x.ReadoutDate, "ReadoutDate").Column("READOUT_DATE");
            this.Property(x => x.ReadingsSource, "ReadingsSource").Column("READINGS_SOURCE").Length(50);        
            this.Reference(x => x.House, "House").Column("HOUSE_ID");
            this.Reference(x => x.ResidentialPremises, "ResidentialPremises").Column("RESIDENTIAL_PREMISES_ID");
            this.Reference(x => x.NonResidentialPremises, "NonResidentialPremises").Column("NONRESIDENTIAL_PREMISES_ID");
            this.Property(x => x.MunicipalResourceCode, "MunicipalResourceCode").Column("MUNICIPAL_RESOURCE_CODE").Length(50);
            this.Property(x => x.MunicipalResourceGuid, "MunicipalResourceGuid").Column("MUNICIPAL_RESOURCE_GUID").Length(50);
            this.Property(x => x.ArchivingReasonCode, "ArchivingReasonCode").Column("ARCHIVING_REASON_CODE").Length(50);
            this.Property(x => x.ArchivingReasonGuid, "ArchivingReasonGuid").Column("ARCHIVING_REASON_GUID").Length(50);
            this.Property(x => x.PlannedVerification, "PlannedVerification").Column("PLANNED_VERIFICATION");
            this.Property(x => x.OneRateDeviceValue, "OneRateDeviceValue").Column("ONE_RATE_DEVICE_VALUE");
            this.Property(x => x.Archivation, "Archivation").Column("ARCHIVATION");
            this.Property(x => x.BaseValueT1, "BaseValueT1").Column("BASE_VALUE_T1");
            this.Property(x => x.BaseValueT2, "BaseValueT2").Column("BASE_VALUE_T2");
            this.Property(x => x.BaseValueT3, "BaseValueT3").Column("BASE_VALUE_T3");
            this.Property(x => x.BeginDate, "BeginDate").Column("BEGIN_DATE");
            this.Property(x => x.ReplacingMeteringDeviceGUID, "ReplacingMeteringDeviceGUID").Column("REPLACING_METERING_DEVICE_GUID");
            this.Property(x => x.MeteringDeviceModel, "MeteringDeviceModel").Column("METERING_DEVICE_MODEL");
            this.Property(x => x.FactorySealDate, "FactorySealDate").Column("FACTORY_SEAL_DATE");
            this.Property(x => x.TemperatureSensor, "TemperatureSensor").Column("TEMPERATURE_SENSOR");
            this.Property(x => x.PressureSensor, "PressureSensor").Column("PRESSURE_SENSOR");
            this.Property(x => x.TransformationRatio, "TransformationRatio").Column("TRANSFORMATION_RATIO");
            this.Property(x => x.VerificationDate, "VerificationDate").Column("VERIFICATION_DATE");
            this.Property(x => x.SealDate, "SealDate").Column("SEAL_DATE");
            this.Property(x => x.ReasonVerificationCode, "ReasonVerificationCode").Column("REASON_VERIFICATION_CODE");
            this.Property(x => x.ManualModeInformation, "ManualModeInformation").Column("MANUAL_MODE_INFORMATION");
            this.Property(x => x.TemperatureSensorInformation, "TemperatureSensorInformation").Column("TEMPERATURE_SENSOR_INFORMATION");
            this.Property(x => x.PressureSensorInformation, "PressureSensorInformation").Column("PRESSURE_SENSOR_INFORMATION");
            this.Property(x => x.ReplacingMeteringDeviceVersionGuid, "ReplacingMeteringDeviceVersionGuid").Column("REPLACING_METERING_DEVICE_VERSION_GUID");
        }
    }
}

