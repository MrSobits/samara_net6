Ext.define('B4.ux.config.PaymentPenaltiesEditor', {
    extend: 'Ext.container.Container',
    requires: [
        'B4.controller.dict.PaymentPenalties'
    ],
    alias: 'widget.paymentpenaltieseditor',
    items: [],
    initComponent: function() {
        var me = this,
            controller = Ext.create('B4.controller.dict.PaymentPenalties', {
                application: b4app,
                containerSelector: '#' + me.getId()
        });

        controller.init();
        controller.index();
        me.items = [controller.getMainView()];
        me.callParent(arguments);
    }
});