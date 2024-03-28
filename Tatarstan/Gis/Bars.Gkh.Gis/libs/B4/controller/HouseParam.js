Ext.define('B4.controller.HouseParam', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkuInfoToolbar'],

    mixins: { context: 'B4.mixins.Context', mask: 'B4.mixins.MaskBody' },
    views: ['house.ParamGrid'],
    mainView: 'house.ParamGrid',
    mainViewSelector: 'house_param_grid',

    aspects: [
        {
            xtype: 'gkuInfoToolbar'
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('house_param_grid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
    }
});