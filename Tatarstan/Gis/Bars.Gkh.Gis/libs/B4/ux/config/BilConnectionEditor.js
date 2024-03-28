Ext.define('B4.ux.config.BilConnectionEditor', {
    extend: 'Ext.container.Container',
    requires: [
        'B4.controller.BilConnection'
    ],
    alias: 'widget.bilconnection',
    items: [],
    initComponent: function() {
        var me = this,
            controller = Ext.create('B4.controller.BilConnection', {
                application: b4app,
                containerSelector: '#' + me.getId()
            });
        controller.init();
        controller.index();
        me.items = [controller.getMainView()];
        me.callParent(arguments);
    }
});