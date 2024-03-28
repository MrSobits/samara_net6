Ext.define('B4.model.realityobj.RealityObjectAntenna', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: [
        'B4.enums.AntennaRange',
        'B4.enums.AntennaReason',
        'B4.enums.YesNoMinus',
        'B4.enums.YesNoNotSet'
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectAntenna'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject' },
        { name: 'Availability', defaultValue: 30 },
        { name: 'Workability', defaultValue: 0 },
        { name: 'Range', defaultValue: 0 },
        { name: 'FileInfo' },
        { name: 'FrequencyFrom', useNull: true },
        { name: 'FrequencyTo', useNull: true },
        { name: 'NumberApartments' },
        { name: 'Reason', defaultValue: 0 }
    ]
});