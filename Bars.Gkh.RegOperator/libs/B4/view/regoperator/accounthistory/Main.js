Ext.define('B4.view.regoperator.accounthistory.Main', {
    extend: 'Ext.tab.Panel',
    requires: [
        'B4.view.regoperator.accounthistory.SpecialAccountGrid',
        'B4.view.regoperator.accounthistory.RegopAccountGrid'
    ],
    alias: 'widget.accounthistoryMain',

    autoScroll: true,
    title: 'История ведения расчетных счетов',
    closable: true,
    initComponent: function() {
        var me = this;

        Ext.apply(me, {
            items: [
                {
                    xtype: 'accounthistoryRegopAccountGrid'
                },
                {
                    xtype: 'accounthistorySpecialAccountGrid'
                }
            ]
        });

        me.callParent(arguments);
    }
});