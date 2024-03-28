Ext.define('B4.controller.dict.TypesExternalFacadeInsulation', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typesexternalfacadeinsulationgrid',
            permissionPrefix: 'Gkh.Dictionaries.TypesExternalFacadeInsulation'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'typesexternalfacadeinsulationgrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.typesexternalfacadeinsulation.Grid'],

    mainView: 'dict.typesexternalfacadeinsulation.Grid',
    mainViewSelector: 'typesexternalfacadeinsulationgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'typesexternalfacadeinsulationgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('typesexternalfacadeinsulationgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});