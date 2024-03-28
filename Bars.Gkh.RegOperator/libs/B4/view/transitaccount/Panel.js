Ext.define('B4.view.transitaccount.Panel', {
    extend: 'Ext.tab.Panel',

    closable: true,
    layout: 'anchor',
    bodyPadding: 5,
    alias: 'widget.transitaccountpanel',
    title: 'Контроль транзитного счета',
    autoScroll: true,
    frame: true,
    requires: [
        'B4.view.transitaccount.DebetGrid',
        'B4.view.transitaccount.CreditGrid',
        'B4.ux.button.Update'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'transitaccountdebetgrid'
                },
                {
                    xtype: 'transitaccountcreditgrid'
                }
            ]
        });

        me.callParent(arguments);
    }
});