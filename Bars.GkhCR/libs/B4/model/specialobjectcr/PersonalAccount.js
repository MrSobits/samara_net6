Ext.define('B4.model.specialobjectcr.PersonalAccount', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialPersonalAccount'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr' },
        { name: 'FinanceGroup' },
        { name: 'Closed' },
        { name: 'Account' }
    ]
});