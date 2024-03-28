Ext.define('B4.ux.config.FixedPeriodCalcPenaltiesEditor', {
    extend: 'Ext.container.Container',
    requires: [
        'B4.controller.dict.FixedPeriodCalcPenalties'
    ],
    alias: 'widget.fixedperiodcalcpenaltieseditor',
    items: [],
    initComponent: function() {
        var me = this,
            controller = Ext.create('B4.controller.dict.FixedPeriodCalcPenalties', {
                application: b4app,
                containerSelector: '#' + me.getId()
        });

        controller.init();
        controller.index();
        me.items = [controller.getMainView()];
        me.callParent(arguments);
    }
});