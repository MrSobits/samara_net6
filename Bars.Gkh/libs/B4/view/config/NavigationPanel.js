Ext.define('B4.view.config.NavigationPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.confignavigationpanel',

    requires: [
        'Ext.tree.Panel',
        'Ext.data.TreeStore'
    ],

    header: false,

    layout: {
        type: 'fit'
    },

    store: null,

    initComponent: function() {
        var me = this;

        me.store = Ext.create('Ext.data.TreeStore', {
            autoDestroy: true
        });

        Ext.applyIf(me, {
            items: [
                {
                    title: 'Разделы конфигурации',
                    xtype: 'treepanel',
                    store: me.store,
                    rootVisible: false,
                    border: false
                }
            ]
        });

        me.callParent(arguments);
    }
});