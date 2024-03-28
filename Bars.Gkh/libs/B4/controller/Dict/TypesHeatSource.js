Ext.define('B4.controller.dict.TypesHeatSource', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'typesheatsourcegrid',
            permissionPrefix: 'Gkh.Dictionaries.TypesHeatSource'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'typesheatsourcegrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.typesheatsource.Grid'],

    mainView: 'dict.typesheatsource.Grid',
    mainViewSelector: 'typesheatsourcegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'typesheatsourcegrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('typesheatsourcegrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});