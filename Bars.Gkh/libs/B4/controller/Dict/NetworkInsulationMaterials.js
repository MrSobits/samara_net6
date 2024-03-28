Ext.define('B4.controller.dict.NetworkInsulationMaterials', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'networkinsulationmaterialsgrid',
            permissionPrefix: 'Gkh.Dictionaries.NetworkInsulationMaterials'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'networkinsulationmaterialsgrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.networkinsulationmaterials.Grid'],

    mainView: 'dict.networkinsulationmaterials.Grid',
    mainViewSelector: 'networkinsulationmaterialsgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'networkinsulationmaterialsgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('networkinsulationmaterialsgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});