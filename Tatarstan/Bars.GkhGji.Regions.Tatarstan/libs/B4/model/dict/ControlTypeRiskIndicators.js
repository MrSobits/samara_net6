Ext.define('B4.model.dict.ControlTypeRiskIndicators', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlTypeRiskIndicators'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ControlType' },
        { name: 'Name' },
        { name: 'ErvkId' }
    ]
});