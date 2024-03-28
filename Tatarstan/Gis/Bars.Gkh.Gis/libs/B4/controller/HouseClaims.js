Ext.define('B4.controller.HouseClaims', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkuInfoToolbar'],

    mixins: { context: 'B4.mixins.Context', mask: 'B4.mixins.MaskBody' },
    views: ['house.ClaimsGrid'],
    mainView: 'house.ClaimsGrid',
    mainViewSelector: 'house_claims_grid',

    aspects: [
        {
            xtype: 'gkuInfoToolbar'
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('house_claims_grid'),
            gridStore = view.getStore();

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        gridStore.on({
            'beforeload': function (store, operation) {
                operation.params.realityObjectId = id;
            }
        });
    }
});