Ext.define('B4.ux.config.ContextSettingsEditor', {
    extend: 'Ext.container.Container',
    requires: [
        'B4.controller.ContextSettings'
    ],
    alias: 'widget.contextsettingseditor',
    items: [],
    initComponent: function () {
        var me = this,
            controller = Ext.create('B4.controller.ContextSettings', {
                application: b4app,
                containerSelector: '#' + me.getId()
            });

        controller.init();
        controller.index();
        me.items = [controller.getMainView()];
        me.callParent(arguments);
    }
});