Ext.define('B4.view.basestatement.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    alias: 'widget.baseStatementPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.basestatement.FilterPanel',
        'B4.view.basestatement.Grid',
        'B4.GjiTextValuesOverride'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            title: B4.GjiTextValuesOverride.getText('Проверки по обращениям граждан'),
            items: [
                {
                    xtype: 'baseStatementFilterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'baseStatementGrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});