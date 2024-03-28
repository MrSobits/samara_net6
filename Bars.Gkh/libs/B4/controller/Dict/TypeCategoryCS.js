Ext.define('B4.controller.dict.TypeCategoryCS', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.Context',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    views: ['dict.TypeCategoryCSGrid'],

    models: ['cscalculation.TypeCategoryCS'],

    stores: ['cscalculation.TypeCategoryCS'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [{
        ref: 'mainView', selector: 'typecategorycsgrid'
    }],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            modelName: 'cscalculation.TypeCategoryCS',
            gridSelector: 'typecategorycsgrid'
        }],

    init: function() {
        var me = this;
        me.control({
            'typecategorycsgrid': {
                render: function(grid) {
                    grid.getStore().load();
                }
            }
        });
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('typecategorycsgrid');

        me.bindContext(view);
        me.application.deployView(view);
    }
});