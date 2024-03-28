Ext.define('B4.view.claimwork.pretension.TabPanel', {
    extend: 'Ext.tab.Panel',
    closable: true,
    title: 'Претензия',
    alias: 'widget.pretensiontabpanel',
    requires: [
        'B4.view.claimwork.pretension.EditPanel',
        'B4.view.claimwork.pretension.DebtPaymentGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'pretensioneditpanel',
                    title: 'Общие сведения'
                },
                {
                    xtype: 'pretensiondebtpaymentgrid',
                    title: 'Оплата задолженности'
                }
            ]
        });

        me.callParent(arguments);
    }
});
