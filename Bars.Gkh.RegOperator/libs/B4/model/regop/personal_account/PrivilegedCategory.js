Ext.define('B4.model.regop.personal_account.PrivilegedCategory', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountPrivilegedCategory'
    },

    fields: [
        { name: 'Id' },
        { name: 'DateFrom' },
        { name: 'DateTo' },
        { name: 'Name' },
        { name: 'Percent' },
        { name: 'PrivilegedCategory' },
        { name: 'PersonalAccount' }
    ]
});