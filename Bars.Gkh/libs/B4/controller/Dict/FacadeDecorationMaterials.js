Ext.define('B4.controller.dict.FacadeDecorationMaterials', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'facadedecorationmaterialsgrid',
            permissionPrefix: 'Gkh.Dictionaries.FacadeDecorationMaterials'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'facadedecorationmaterialsgrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.facadedecorationmaterials.Grid'],

    mainView: 'dict.facadedecorationmaterials.Grid',
    mainViewSelector: 'facadedecorationmaterialsgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'facadedecorationmaterialsgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('facadedecorationmaterialsgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});