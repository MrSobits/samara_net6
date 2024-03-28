Ext.define('B4.controller.dict.FoundationMaterials', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'foundationmaterialsgrid',
            permissionPrefix: 'Gkh.Dictionaries.FoundationMaterials'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'foundationmaterialsgrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.foundationmaterials.Grid'],

    mainView: 'dict.foundationmaterials.Grid',
    mainViewSelector: 'foundationmaterialsgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'foundationmaterialsgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('foundationmaterialsgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});