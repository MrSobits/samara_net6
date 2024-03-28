Ext.define('B4.controller.dict.NetworkAndRiserMaterials', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'networkandrisermaterialsgrid',
            permissionPrefix: 'Gkh.Dictionaries.NetworkAndRiserMaterials'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'networkandrisermaterialsgrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.networkandrisermaterials.Grid'],

    mainView: 'dict.networkandrisermaterials.Grid',
    mainViewSelector: 'networkandrisermaterialsgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'networkandrisermaterialsgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('networkandrisermaterialsgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});