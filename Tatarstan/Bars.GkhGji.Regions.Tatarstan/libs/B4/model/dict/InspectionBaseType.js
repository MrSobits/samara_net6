Ext.define('B4.model.dict.InspectionBaseType', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectionBaseType'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code', useNull: true },
        { name: 'Name' },
        { name: 'InspectionKindId' },
        { name: 'SendErp' },
        { name: 'TorId' },
        { name: 'KindCheck', defaultValue: [] },
        { name: 'ErknmCode' },
        { name: 'HasTextField' },
        { name: 'HasDate' },
        { name: 'HasRiskIndicator' }
    ]
});