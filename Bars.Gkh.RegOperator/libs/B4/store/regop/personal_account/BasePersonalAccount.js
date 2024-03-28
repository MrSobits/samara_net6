Ext.define('B4.store.regop.personal_account.BasePersonalAccount', {
    extend: 'B4.base.Store',
    model: 'B4.model.regop.personal_account.BasePersonalAccount',
    requires: ['B4.model.regop.personal_account.BasePersonalAccount'],
    autoLoad: false, // important!!!
    proxy: {
        type: 'b4proxy',
        controllerName: 'BasePersonalAccount',
        timeout: 999999
    },
    load: function (param) {
        if (this.summary !== undefined) {
            param = param || {};
            param.params = param.params || {};
            param.params.summary = this.summary;
        }

        this.callParent([param]);
    }
});