Ext.define('B4.view.baseplanaction.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    alias: 'widget.basePlanActionPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.baseplanaction.FilterPanel',
        'B4.view.baseplanaction.Grid',
        'B4.GjiTextValuesOverride'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            title: B4.GjiTextValuesOverride.getText('Проверки по плану мероприятий'),
            items: [
                {
                    xtype: 'basePlanActionFilterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'basePlanActionGrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
