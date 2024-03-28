using Bars.Gkh.Entities;
using System;
using System.Collections.Generic;

namespace Bars.Gkh.Regions.Tyumen.Import.RealityObjectExaminationImport
{
    public sealed class Record
    {
        public bool isValidRecord { get; set; }
        public long RoId { get; set; }
        public string Address { get; set; }
        public string BuildYear { get; set; }
        public string PassportType { get; set; }
        public string LastRepairYear { get; set; }
        public string TechDate { get; set; }
        public string AreaMkd { get; set; }
        public string AreaMkdNotLiv { get; set; }
        public string FloorHeight { get; set; }
        public string FloorCount { get; set; }
        public string EntranceCount { get; set; }
        public string LiftCount { get; set; }
        public string RoofMaterial { get; set; }
        public string WallMaterial { get; set; }
        public string RoofType { get; set; }
        public string TotalBuildingVolume { get; set; }
        
        public Dictionary<int, StructElement> structElemDict;
       
    }

    public sealed class StructElement
    {
        public string Year { get; set; }
        public string Wearout { get; set; }
        public string Volume { get; set; }
    }

}