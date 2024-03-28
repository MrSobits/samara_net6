Ext.define('B4.ux.config.ValidityDocPeriod', {
    extend: 'Ext.container.Container',
    requires: [
        'B4.controller.ValidityDocPeriod'
    ],
    alias: 'widget.validitydocperiod',
    items: [],
    initComponent: function () {
        var me = this,
            controller = Ext.create('B4.controller.ValidityDocPeriod', {
                application: b4app,
                containerSelector: '#' + me.getId()
            });
        controller.init();
        controller.index();
        me.items = [controller.getMainView()];
        me.callParent(arguments);
    }
});