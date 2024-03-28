Ext.define('B4.store.ListPersonalAccountDebtor', {
    extend: 'B4.base.Store',
    requires: ['B4.model.regop.personal_account.BasePersonalAccount'],
    autoLoad: false,
    model: 'B4.model.regop.personal_account.BasePersonalAccount',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AgentPIRExecute',
        listAction: 'GetListPersonalAccountDebtor'
    }
});