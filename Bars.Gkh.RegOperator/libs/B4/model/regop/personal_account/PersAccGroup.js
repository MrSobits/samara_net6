Ext.define('B4.model.regop.personal_account.PersAccGroup', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersAccGroup'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name', defaultValue: null }
    ]
});