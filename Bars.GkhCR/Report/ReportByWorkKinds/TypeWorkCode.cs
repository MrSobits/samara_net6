namespace Bars.GkhCr.Report.ReportByWorkKinds
{
    using System;
    using System.Collections.Generic;

    public enum TypeWorkCode
    {
        Unknown = 0,

        RepairHeatingSystem = 1,
        RepairHotWaterSystem = 2,
        RepairColdWaterSystem = 3,
        RepairDrainageSystem = 4,
        RepairGasSystem = 5,
        RepairEletricalSystem = 6,

        MountingHeatingDevices = 7,
        MountingHotWaterDevices = 8,
        MountingColdWaterDevices = 9,
        MountingElectricalDevices = 10,
        MountingGasDevices = 11,

        RepairBasements = 12,
        RepairRoof = 13,
        RepairElevator = 14,
        RepairElevatorShaft = 15,
        RepairFacade = 16,
        WinterizingFacade = 17,
        StrengtheningFoundation = 18,
        MountingFirePreventionSystem = 19,
        YardLandscaping = 20,
        RepairPorch = 21,
        RepairOfInjection = 22,
        MountingDismantlingBalcony = 23,

        DesignPsd = 1018,
        ExpertisePsd = 1019,
        TechnicalSupervision = 1020
    }

    public static class TypeWorkCodeTranslator
    {
        static Dictionary<string, TypeWorkCode> TypeWorkCodeDict = new Dictionary<string, TypeWorkCode>();
        static TypeWorkCodeTranslator()
        {
            foreach (TypeWorkCode typeWorkCode in Enum.GetValues(typeof(TypeWorkCode)))
            {
                TypeWorkCodeDict[((int)typeWorkCode).ToString()] = typeWorkCode;
            }
        }

        public static TypeWorkCode GetWorkCode(string workCode)
        {
            if (TypeWorkCodeDict.ContainsKey(workCode))
            {
                return TypeWorkCodeDict[workCode];
            }

            return TypeWorkCode.Unknown;
        }


    }
    
}