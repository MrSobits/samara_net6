Ext.define('B4.controller.dict.InspectedPartGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.InspectedPartGji'],
    stores: ['dict.InspectedPartGji'],

    views: ['dict.inspectedpartgji.Grid'],

    mainView: 'dict.inspectedpartgji.Grid',
    mainViewSelector: 'inspectedPartGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'inspectedPartGjiGrid'
        }
    ],

    aspects: [
         {
             /*
             * аспект кнопки экспорта реестра
             */
             xtype: 'b4buttondataexportaspect',
             name: 'inspectedPartGjiButtonExportAspect',
             gridSelector: 'inspectedPartGjiGrid',
             buttonSelector: '#inspectedPartGjiGrid #btnExport',
             controllerName: 'InspectedPartGji',
             actionName: 'Export'
         },
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'inspectedPartGjiGrid',
            permissionPrefix: 'GkhGji.Dict.InspectedPart'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'inspectedPartGjiGridAspect',
            storeName: 'dict.InspectedPartGji',
            modelName: 'dict.InspectedPartGji',
            gridSelector: 'inspectedPartGjiGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('inspectedPartGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.InspectedPartGji').load();
    }
});