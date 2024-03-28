Ext.define('B4.view.realityobj.decisionhistory.MainPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.decisionhistorymainpanel',

    requires: [
        'B4.view.realityobj.decisionhistory.TreePanel',
        'B4.view.realityobj.decisionhistory.JobYearsGrid'
    ],

    title: 'Действующие решения',
    closable: true,
    enableColumnHide: true,
    layout: 'fit',

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
                    items: [
                        {
                            xtype: 'decisionhistorytreepanel',
                            flex: 1
                        },
                        {
                            xtype: 'decisionhistoryjobyearsgrid',
                            flex: 1
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});