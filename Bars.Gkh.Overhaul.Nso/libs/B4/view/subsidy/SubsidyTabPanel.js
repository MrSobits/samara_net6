Ext.define('B4.view.subsidy.SubsidyTabPanel', {
    extend: 'Ext.tab.Panel',

    alias: 'widget.subsidytabpanel',

    closable: true,

    objectId: null,

    requires: [
        'B4.view.subsidy.SubsidyPanel',
        'B4.view.program.FourthStageGrid'
    ],

    title: 'Субсидирование',
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'subsidypanel'
                },
                {
                    xtype: 'programfourthstagegrid'
                }
            ]
        });

        me.callParent(arguments);
    }
});