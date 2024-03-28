Ext.define('B4.model.regop.personal_account.BanRecalc', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountBanRecalc'
    },

    fields: [
        { name: 'Id' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'Reason' },
        { name: 'File' },
        { name: 'Type' }
    ]
});