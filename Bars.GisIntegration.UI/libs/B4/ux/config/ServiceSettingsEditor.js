Ext.define('B4.ux.config.ServiceSettingsEditor', {
    extend: 'Ext.container.Container',
    requires: [
        'B4.controller.ServiceSettings'
    ],
    alias: 'widget.servicesettingseditor',
    items: [],
    initComponent: function () {
        var me = this,
            controller = Ext.create('B4.controller.ServiceSettings', {
                application: b4app,
                containerSelector: '#' + me.getId()
            });

        controller.init();
        controller.index();
        me.items = [controller.getMainView()];
        me.callParent(arguments);
    }
});