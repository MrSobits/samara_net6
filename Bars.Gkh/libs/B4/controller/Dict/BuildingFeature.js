Ext.define('B4.controller.dict.BuildingFeature', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.Context',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    views: ['dict.BuildingFeatureGrid'],

    models: ['dict.BuildingFeature'],

    stores: ['dict.BuildingFeature'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [{
        ref: 'mainView', selector: 'buildfeaturegrid'
    }],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'buildfeaturegrid',
            permissionPrefix: 'Gkh.Dictionaries.BuildingFeature'
        },
        {
            xtype: 'gkhinlinegridaspect',
            modelName: 'dict.BuildingFeature',
            gridSelector: 'buildfeaturegrid'
        }],

    init: function() {
        var me = this;
        me.control({
            'buildfeaturegrid': {
                render: function(grid) {
                    grid.getStore().load();
                }
            }
        });
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('buildfeaturegrid');

        me.bindContext(view);
        me.application.deployView(view);
    }
});