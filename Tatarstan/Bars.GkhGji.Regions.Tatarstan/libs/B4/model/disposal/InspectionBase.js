Ext.define('B4.model.disposal.InspectionBase', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DecisionInspectionBase'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'InspectionBaseType' },
        { name: 'OtherInspBaseType' },
        { name: 'FoundationDate' },
        { name: 'RiskIndicator' },
        { name: 'Decision' }
    ]
});