Ext.define('B4.model.dict.FederalStandardFeeCr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FederalStandardFeeCr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Value' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});