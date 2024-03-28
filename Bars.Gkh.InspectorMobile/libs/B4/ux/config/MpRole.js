Ext.define('B4.ux.config.MpRole', {
    extend: 'Ext.container.Container',
    requires: [
        'B4.controller.MpRole'
    ],
    alias: 'widget.mproleeditor',
    items: [],
    initComponent: function () {
        var me = this,
            controller = Ext.create('B4.controller.MpRole', {
                application: b4app,
                containerSelector: '#' + me.getId()
            });
        controller.init();
        controller.index();
        me.items = [controller.getMainView()];
        me.callParent(arguments);
    }
});