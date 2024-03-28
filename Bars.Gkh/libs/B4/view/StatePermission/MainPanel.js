Ext.define('B4.view.StatePermission.MainPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.statepermissionmainpanel',

    requires: [
        'B4.view.StatePermission.Panel',
        'B4.ux.breadcrumbs.Breadcrumbs',
    ],

    title: 'Настройка ограничений по статусам',

    bodyStyle: Gkh.bodyStyle,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'breadcrumbs',
                    data: {
                        text: me.title
                    },
                },
                {
                    xtype: 'statepermissionpanel',
                    closable: false,
                    header: false,
                    border: false,
                    flex: 1,
                }
            ]
        });
        me.callParent(arguments);
    }
});