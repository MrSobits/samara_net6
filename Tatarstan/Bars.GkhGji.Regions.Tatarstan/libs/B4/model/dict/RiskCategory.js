Ext.define('B4.model.dict.RiskCategory', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TatRiskCategory'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'Name' },
        { name: 'ErvkGuid' }
    ]
});