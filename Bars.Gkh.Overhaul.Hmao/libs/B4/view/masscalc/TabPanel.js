Ext.define('B4.view.masscalc.TabPanel', {
    extend: 'Ext.panel.Panel',

    alias: 'widget.masscalctabpanel',
    layout: {type: 'vbox', align: 'stretch'},
    title: 'Массовый расчет ДПКР',
    bodyStyle: Gkh.bodyStyle,
    closable: true,
    bodyPadding: '10 10 10 10',
    
    requires: [
        'B4.view.masscalc.CalcPanel',
        'B4.view.masscalc.CopyDefaultCollectionGrid'
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
                            xtype: 'masscalclongprogpanel',
                            flex: 1
                        },
                        {
                            xtype: 'copydefaultcollectiongrid',
                            flex: 1
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});