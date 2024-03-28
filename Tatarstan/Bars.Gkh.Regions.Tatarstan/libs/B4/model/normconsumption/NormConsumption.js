Ext.define('B4.model.normconsumption.NormConsumption', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NormConsumption'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'Period' },
        { name: 'Type' },
        { name: 'State' }
    ]
});