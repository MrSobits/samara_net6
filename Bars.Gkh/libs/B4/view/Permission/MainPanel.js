Ext.define('B4.view.Permission.MainPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.rolepermissionmainpanel',

    requires: [
        'B4.view.Permission.Panel',
        'B4.ux.breadcrumbs.Breadcrumbs',
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Настройка ограничений',
    closable: true,

    bodyStyle: Gkh.bodyStyle,

    initComponent: function() {
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
                    xtype: 'rolepermissionpanel',
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