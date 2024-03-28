Ext.define('B4.ux.config.YearCorrection', {
    extend: 'Ext.container.Container',
    requires: [
        'B4.controller.dict.YearCorrection'
    ],
    alias: 'widget.yearcorrectioneditor',
    items: [],
    initComponent: function () {
        var me = this,
            controller = Ext.create('B4.controller.dict.YearCorrection', {
                application: b4app,
                containerSelector: '#' + me.getId()
            });

        controller.init();
        controller.index();
        me.items = [controller.getMainView()];
        me.callParent(arguments);
    }
});