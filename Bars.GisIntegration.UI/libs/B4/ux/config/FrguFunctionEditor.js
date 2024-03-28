Ext.define('B4.ux.config.FrguFunctionEditor', {
    extend: 'Ext.container.Container',
    requires: [
        'B4.controller.frgufunction.FrguFunction'
    ],
    alias: 'widget.frgufunctioneditor',
    items: [],
    initComponent: function () {
        var me = this,
            controller = Ext.create('B4.controller.frgufunction.FrguFunction', {
                application: b4app,
                containerSelector: '#' + me.getId()
            });

        controller.init();
        controller.index();
        me.items = [controller.getMainView()];
        me.callParent(arguments);
    }
});