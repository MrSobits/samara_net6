Ext.define('B4.controller.HouseAccruals', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkuInfoToolbar'],

    mixins: { context: 'B4.mixins.Context', mask: 'B4.mixins.MaskBody' },
    views: ['house.AccrualsGrid'],
    mainView: 'house.AccrualsGrid',
    mainViewSelector: 'house_accruals_grid',

    aspects: [
        {
            xtype: 'gkuInfoToolbar'
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('house_accruals_grid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
    }
});