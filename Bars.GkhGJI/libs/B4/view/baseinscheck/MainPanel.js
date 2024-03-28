Ext.define('B4.view.baseinscheck.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    alias: 'widget.baseInsCheckPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.baseinscheck.FilterPanel',
        'B4.view.baseinscheck.Grid',
        'B4.GjiTextValuesOverride'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            title: B4.GjiTextValuesOverride.getText('Инспекционные проверки'),
            items: [
                {
                    xtype: 'baseInsCheckFilterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: Gkh.bodyStyle
                },
                {
                    xtype: 'baseInsCheckGrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
