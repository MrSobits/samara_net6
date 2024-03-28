Ext.define('B4.model.regop.personal_account.CrOwnerFilterType', {
    extend: 'B4.base.Model',
    fields: [{ name: 'Name' }],

    proxy: {
        type: 'memory'
    }
});