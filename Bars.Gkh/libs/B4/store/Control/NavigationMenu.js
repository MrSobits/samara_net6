Ext.define('B4.store.Control.NavigationMenu', {
    alias: 'widget.navigationMenuStore',
    extend: 'Ext.data.TreeStore',
    menuUrl: '/Menu/GetMenu',
    autoLoad: false,

    constructor: function () {
        var me = this;

        Ext.applyIf(me, {
            root: {
                expanded: true,
                loaded: !me.autoLoad
            },
            proxy: {
                type: 'ajax',
                reader: { type: 'json' },
                url: B4.Url.action(me.menuUrl)
            },
            fields: [
                { name: 'text' },
                { name: 'moduleScript' },
                { name: 'expanded' },
                { name: 'children' },
                { name: 'selected' },
                { name: 'iconCls' },
                { name: 'ScriptHref' },
                { name: 'leaf' },
                { name: 'options' }
            ]
        });

        me.callParent(arguments);
    }
});