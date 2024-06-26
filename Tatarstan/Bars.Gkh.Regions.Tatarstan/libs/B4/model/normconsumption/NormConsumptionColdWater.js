﻿Ext.define('B4.model.normconsumption.NormConsumptionColdWater', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NormConsumptionColdWater',
        listAction: 'ListNormConsumptionColdWater'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'NormConsumption' },
        { name: 'Municipality' },
        { name: 'RealityObject' },
        { name: 'Address' },
        { name: 'ObjectId'},
        { name: 'FloorNumber' },
        { name: 'MetersInstalled' },
        { name: 'BuildYear' },
        { name: 'AreaHouse' },
        { name: 'AreaLivingRooms' },
        { name: 'AreaNotLivingRooms' },
        { name: 'AreaOtherRooms' },
        { name: 'IsIpuNotLivingPermises' },
        { name: 'AreaIpuNotLivingPermises' },
        { name: 'VolumeColdWaterNotLivingIsIpu' },
        { name: 'VolumeWaterOpuOnPeriod' },
        { name: 'HeatingPeriod' },
        { name: 'TypeSystemHotWater' },
        { name: 'ResidentsNumber' },
        { name: 'DepreciationIntrahouseUtilities' },
        { name: 'OverhaulDate' },
        { name: 'IsBath1200' },
        { name: 'IsBath1500With1550' },
        { name: 'IsBath1650With1700' },
        { name: 'IsBathNotShower' },
        { name: 'IsShower' },
        { name: 'HvsIsBath1200' },
        { name: 'HvsIsBath1500With1550' },
        { name: 'HvsIsBathNotShower' },
        { name: 'HvsIsShower' },
        { name: 'IsNotBoiler' },
        { name: 'HvsIsNotBoiler' },
        { name: 'IsHvsBathIsNotCentralSewage' },
        { name: 'IsHvsIsNotCentralSewage' },
        { name: 'IsStandpipes' },
        { name: 'IsHostelNoShower' },
        { name: 'IsHostelSharedShower' },
        { name: 'IsHostelShowerAllLivPermises' },
        { name: 'ShowerInHostelInSection' },
        { name: 'Gvs12Floor' }
    ]
});