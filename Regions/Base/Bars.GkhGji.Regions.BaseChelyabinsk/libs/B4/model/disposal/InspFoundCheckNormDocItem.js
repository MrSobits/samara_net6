Ext.define('B4.model.disposal.InspFoundCheckNormDocItem', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisposalInspFoundCheckNormDocItem'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Number' },
        { name: 'Text' }
    ]
});