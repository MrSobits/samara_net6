Ext.define('B4.view.regop.personal_account.action.CloseAccount', {
    extend: 'B4.view.regop.personal_account.action.BaseAccountWindow',


    alias: 'widget.closeaccountwin',

    modal: true,

    width: 350,

    title: 'Закрытие',

    initComponent: function() {
        var me = this;
        Ext.apply(me, {
            items: [ {
                xtype: 'form',
                border: false,
                bodyStyle: Gkh.bodyStyle,
                defaults: {
                    labelWidth: 100,
                    anchor: '100%'
                },
                items: [
                    {
                        xtype: 'datefield',
                        fieldLabel: 'Дата закрытия',
                        name: 'closeDate'
                    }
                ]
            }]
        });
        me.callParent(arguments);
    }
    
});