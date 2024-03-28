Ext.define('B4.store.regop.personal_account.CrFundFormationDecisionFilterType', {
    extend: 'B4.base.Store',
    model: 'B4.model.regop.personal_account.CrFundFormationDecisionFilterType',
    requires: ['B4.model.regop.personal_account.CrFundFormationDecisionFilterType'],
    data: [
            { Name: 'Специальный счет регионального оператора', Id: 0 },
            { Name: 'Счет регионального оператора', Id: 1 },
            { Name: 'Специальный счет', Id: 2 },
            { Name: 'Не выбран', Id: 3 }
    ],
    autoLoad: false, // important!!!
    load: function (param) {
        this.callParent([param]);
    }
});