Ext.define('B4.view.specialobjectcr.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Объекты КР для владельцев спец. счетов',
    alias: 'widget.specialobjectcrpanel',

    layout: {
        type: 'border'
    },

    closeAction: 'destroy',
    requires: [
        'B4.view.specialobjectcr.FilterPanel',
        'B4.ux.grid.Panel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'specialobjectcrfilterpnl',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: 'text-align:center; font: 14px tahoma,arial,helvetica,sans-serif;'
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'specialObjectCrTabPanel',
                    region: 'center',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'specialobjectcrgrid',
                            region: 'center',
                            title: 'Объекты капитального ремонта'
                        },
                        {
                            xtype: 'deletedspecialobjectcrgrid',
                            title: 'Удаленные объекты капитального ремонта'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
