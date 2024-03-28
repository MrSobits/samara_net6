Ext.define('B4.model.regoperator.AccountByRo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AccountOwnerName', defaultValue: null },
        { name: 'PersonalAccountNum' }
    ]
});