Ext.define('B4.store.menu.ActionMenu', {
    alias: 'widget.actionMenuStore',
    extend: 'Ext.data.Store',
    menuUrl: '/MenuDi/GetActionMenu',
    autoLoad:false,
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            root: { expanded: true },
            proxy: {
                type: 'ajax',
                reader: { type: 'json' },
                url: B4.Url.action(this.menuUrl)
            },
            fields: [
                { name: 'text' },
                { name: 'type' },
                { name: 'percent' }
            ]
        }, cfg)]);
    }
});