Ext.define('B4.view.claimwork.buildcontract.pretension.TabPanel', {
    extend: 'Ext.tab.Panel',
    closable: true,
    title: 'Претензия',
    alias: 'widget.pretensionbctabpanel',
    requires: [
        'B4.view.claimwork.buildcontract.pretension.EditPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'pretensionbceditpanel',
                    title: 'Общие сведения'
                }
            ]
        });

        me.callParent(arguments);
    }
});
