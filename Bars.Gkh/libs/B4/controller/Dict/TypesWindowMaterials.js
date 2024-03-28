Ext.define('B4.controller.dict.TypesWindowMaterials', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typeswindowmaterialsgrid',
            permissionPrefix: 'Gkh.Dictionaries.TypesWindowMaterials'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'typeswindowmaterialsgrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.typeswindowmaterials.Grid'],

    mainView: 'dict.typeswindowmaterials.Grid',
    mainViewSelector: 'typeswindowmaterialsgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'typeswindowmaterialsgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('typeswindowmaterialsgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});