Ext.define('B4.view.contragent.BankPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.contragentbankpanel',
    title: 'Расчетные счета',

    closable: true,
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    autoScroll: true,

    requires: [
        'B4.view.contragent.BankGrid',
        'B4.ux.grid.EntityChangeLogGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    flex: 1,
                    items: [
                        {
                            xtype: 'contragentBankGrid',
                            closable: false,
                            autoScroll: true
                        },
                        {
                            xtype: 'entitychangeloggrid',
                            autoScroll: true
                        }
                    ]
                }]
        });

        me.callParent(arguments);
    }
});