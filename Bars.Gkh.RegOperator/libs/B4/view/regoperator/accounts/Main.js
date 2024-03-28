Ext.define('B4.view.regoperator.accounts.Main', {
    extend: 'Ext.tab.Panel',
    requires: [
        'B4.view.regoperator.calcaccount.Panel',
        'B4.view.regoperator.accounts.SpecialAccountGrid'
    ],
    alias: 'widget.regopaccmain',

    autoScroll: true,
    title: 'Счета',
    closable: true,
    initComponent: function() {
        var me = this;

        Ext.apply(me, {
            items: [
                {
                    xtype: 'regopcalcaccountpanel'
                },
                {
                    xtype: 'regopspecaccgrid'
                }
            ]
        });

        me.callParent(arguments);
    }
});