Ext.define('B4.view.reminder.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    layout: {
        type: 'border'
    },

    alias: 'widget.reminderPanel',

    requires: [
        'B4.view.reminder.InspectorGrid',
        'B4.view.reminder.HeadGrid',
        'B4.view.reminder.FilterPanel',
        'B4.ux.breadcrumbs.Breadcrumbs'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs'
                },
                {
                    xtype: 'panel',
                    region: 'west',
                    itemId: 'reminderWestPanel',
                    split: true,
                    collapsible: true,
                    border: false,
                    width: 250,
                    layout: 'fit',
                    header: false,
                    items: [
                        {
                            xtype: 'reminderfilterpnl',
                            bodyStyle: 'backrgound-color:transparent;',
                            padding: '5 5 5 5'
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    region: 'center',
                    itemId: 'reminderCenterPanel',
                    layout: 'fit',
                    border: false,
                    items: []
                }
            ]
        });

        me.callParent(arguments);
    }
});