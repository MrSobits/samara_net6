Ext.define('B4.model.integrationerp.TatarstanDisposal', {
    extend: 'B4.model.baseintegration.Document',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TatarstanDisposal'
    },
    fields: [
        { name: 'ErpGuid' },
        { name: 'ErpRegistrationDate' }
    ]
});