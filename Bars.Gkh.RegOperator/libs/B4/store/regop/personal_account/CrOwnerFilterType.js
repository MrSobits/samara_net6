Ext.define('B4.store.regop.personal_account.CrOwnerFilterType', {
    extend: 'B4.base.Store',
    model: 'B4.model.regop.personal_account.CrOwnerFilterType',
    requires: ['B4.model.regop.personal_account.CrOwnerFilterType'],
    data: [
            { Name: 'Физические лица.', Id: 0 },
            { Name: 'Юридические лица.', Id: 1 },
            { Name: 'Юридические лица с 1 помещением.', Id: 2 }
    ],
    autoLoad: false, // important!!!
    load: function (param) {
        this.callParent([param]);
    }
});