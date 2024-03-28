Ext.define('B4.model.PersonalAccountService', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Service' },
        { name: 'Supplier' },
        { name: 'Tariff' }
    ]
});