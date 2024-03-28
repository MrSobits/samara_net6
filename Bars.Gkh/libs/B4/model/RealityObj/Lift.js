Ext.define('B4.model.realityobj.Lift', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.LiftAvailabilityDevices'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectLift'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'TypeLiftShaft', defaultValue: null },
        { name: 'TypeLiftMashineRoom', defaultValue: null },
        { name: 'TypeLiftDriveDoors', defaultValue: null },
        { name: 'TypeLift', defaultValue: null },
        { name: 'ModelLift', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'CabinLift', defaultValue: null },
        
        { name: 'AvailabilityDevices', defaultValue: 0 },
        
        { name: 'PorchNum' },
        { name: 'LiftNum' },
        { name: 'FactoryNum' },
        { name: 'RegNum' },
        { name: 'ReplacementPeriod' },
        
        { name: 'YearInstallation' },
        { name: 'YearLastUpgradeRepair' },
        { name: 'YearEstimate' },
        { name: 'YearPlannedReplacement' },
        { name: 'StopCount' },
        
        { name: 'Capacity' },
        { name: 'Cost' },
        { name: 'CostEstimate' },
        { name: 'SpeedRise' },
        
        { name: 'LifeTime' },
        { name: 'ComissioningDate' },
        { name: 'DecommissioningDate' },
        { name: 'PlanDecommissioningDate' },

        { name: 'YearExploitation' },
        { name: 'NumberOfStoreys' },
        { name: 'DepthLiftShaft' },
        { name: 'WidthLiftShaft' },
        { name: 'HeightLiftShaft' },
        { name: 'DepthCabin' },
        { name: 'WidthCabin' },
        { name: 'HeightCabin' },
        { name: 'WidthOpeningCabin' },
        { name: 'OwnerLift' },
        { name: 'State' },
        { name: 'Info' },
        { name: 'CodeErc' },
        { name: 'Municipality' },
        { name: 'Settlement' },
        { name: 'Address' }
    ]
});