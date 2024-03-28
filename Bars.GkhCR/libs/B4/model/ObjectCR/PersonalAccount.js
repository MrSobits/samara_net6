Ext.define('B4.model.objectcr.PersonalAccount', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccount'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr' },
        { name: 'FinanceGroup' },
        { name: 'Closed' },
        { name: 'Account' }
    ]
});