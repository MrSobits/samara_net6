Ext.define('B4.ux.config.PenaltiesWithDeferredEditor', {
    extend: 'Ext.container.Container',
    requires: [
        'B4.controller.dict.PenaltiesWithDeferred'
    ],
    alias: 'widget.penaltieswithdeferrededitor',
    items: [],
    initComponent: function () {
        var me = this,
            controller = Ext.create('B4.controller.dict.PenaltiesWithDeferred', {
                application: b4app,
                containerSelector: '#' + me.getId()
            });

        controller.init();
        controller.index();
        me.items = [controller.getMainView()];
        me.callParent(arguments);
    }
});