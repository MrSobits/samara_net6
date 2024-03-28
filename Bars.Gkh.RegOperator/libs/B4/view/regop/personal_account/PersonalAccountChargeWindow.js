Ext.define('B4.view.regop.personal_account.PersonalAccountChargeWindow', {
    extend: 'Ext.window.Window',

    requires: ['B4.view.regop.personal_account.PersonalAccountChargeGrid'],

    alias: 'widget.pachargewin',

    modal: true,

    width: 600,

    closeAction: 'hide',
    title: 'Начисления',

    items: {
        xtype: 'pachargegrid',
        header: false
    },
    listeners: {
        'show': function (win) {
            win.down('pachargegrid').getStore().load({
                params: {
                    accountId: win.accountId
                }
            });
        }
    }
});