Ext.define('B4.view.objectoutdoorcr.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Объекты программы благоустройства дворов',
    alias: 'widget.objectoutdoorcrpanel',

    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.objectoutdoorcr.FilterPanel',
        'B4.ux.grid.Panel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'objectoutdoorcrfilterpanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: 'text-align:center; font: 14px tahoma,arial,helvetica,sans-serif;'
                },
                {
                    xtype: 'tabpanel',
                    name: 'objectOutdoorCrTabPanel',
                    region: 'center',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'objectoutdoorcrgrid',
                            region: 'center',
                            title: 'Объекты программы благоустройства дворов'
                        },
                        {
                            xtype: 'deletedobjectoutdoorcrgrid',
                            title: 'Удаленные объекты программы благоустройства дворов'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});