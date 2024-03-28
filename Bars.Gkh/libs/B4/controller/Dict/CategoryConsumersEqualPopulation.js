Ext.define('B4.controller.dict.CategoryConsumersEqualPopulation', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],
    
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'categoryconsumersequalpopulationgrid',
            permissionPrefix: 'Gkh.Dictionaries.CategoryConsumersEqualPopulation'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'baseHouseEmergencyGridAspect',
            modelName: 'dict.BaseDict',
            gridSelector: 'categoryconsumersequalpopulationgrid'
        }
    ],
    
    models: ['dict.BaseDict'],
    views: ['dict.categoryconsumersequalpopulation.Grid'],

    mainView: 'dict.categoryconsumersequalpopulation.Grid',
    mainViewSelector: 'categoryconsumersequalpopulationgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'categoryconsumersequalpopulationgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('categoryconsumersequalpopulationgrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});