Ext.define('B4.ux.config.PaymentSourceTreeEditor', {
    extend: 'Ext.container.Container',
    requires: [
        'B4.controller.config.PaymentSourceTree'
    ],
    alias: 'widget.paymentsourcetreeeditor',
    items: [],
    initComponent: function () {
        var me = this,
            controller = Ext.create('B4.controller.config.PaymentSourceTree', {
                application: b4app,
                containerSelector: '#' + me.getId()
            });

        controller.init();
        me.items = [controller.getMainView()];
        me.callParent(arguments);
    }
});