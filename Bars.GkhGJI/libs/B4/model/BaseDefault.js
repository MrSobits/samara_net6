Ext.define('B4.model.BaseDefault', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseDefault'
    },
    fields: [
        { name: 'ContragentName' },
        { name: 'MunicipalityNames' },
        { name: 'PhysicalPerson' },
        { name: 'InspectionNumber' },
        { name: 'State', defaultValue: null },
        { name: 'MoSettlement' },
        { name: 'PlaceName' }
    ]
});