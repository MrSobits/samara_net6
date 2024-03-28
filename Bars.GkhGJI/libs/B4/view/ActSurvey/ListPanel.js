Ext.define('B4.view.actsurvey.ListPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: null,
    title: 'Акты обследований',
    itemId: 'actSurveyListPanel',
    layout: {
        type: 'border'
    },
    
    requires: [
        'B4.view.actsurvey.RelationsGrid',
        'B4.view.actsurvey.Grid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                layout: 'fit',
                xtype: 'panel',
                border: false
            },
            items: [
                {
                    region: 'west',
                    flex: .5,
                    padding: '0 5 0 0',
                    split: true,
                    items: [
                        {
                            xtype: 'actSurveyGrid'
                        }
                    ]
                },
                {
                    region: 'center',
                    items: [
                        {
                            xtype: 'actSurveyRelationGrid'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
