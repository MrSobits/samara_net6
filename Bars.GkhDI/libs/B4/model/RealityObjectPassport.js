Ext.define('B4.model.RealityObjectPassport', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectPassport'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BuildYear', defaultValue: null },
        { name: 'DateCommissioning', defaultValue: null },
        { name: 'TypeOfProject', defaultValue: null },
        { name: 'TypeHouse', defaultValue: null },
        { name: 'ConditionHouse', defaultValue: null },
        { name: 'TypeOfFormingCr', defaultValue: null },
        { name: 'Floors', defaultValue: null },
        { name: 'MaximumFloors', defaultValue: null },
        { name: 'NumberLifts', defaultValue: null },
        { name: 'NumberEntrances', defaultValue: null },
        { name: 'EnergyEfficiencyClass', defaultValue: null },
        { name: 'NumberApartments', defaultValue: null },
        { name: 'NumberNonResidentialPremises', defaultValue: null },
        { name: 'AllNumberOfPremises', defaultValue: null },

        { name: 'AreaMkd', defaultValue: null },
        { name: 'AreaLiving', defaultValue: null },
        { name: 'AreaNotLivingPremises', defaultValue: null },
        { name: 'AreaOfAllNotLivingPremises', defaultValue: null },

        { name: 'DocumentBasedArea', defaultValue: null },
        { name: 'ParkingArea', defaultValue: null },
        { name: 'CadastreNumber', defaultValue: null },
        { name: 'ChildrenArea', defaultValue: null },
        { name: 'SportArea', defaultValue: null },

        { name: 'OwnerInn', defaultValue: null },
        { name: 'OwnerName', defaultValue: null },
        { name: 'ProtocolDate', defaultValue: null },
        { name: 'ProtocolNumber', defaultValue: null },
        { name: 'Paysize', defaultValue: null }
    ]
});