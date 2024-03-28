Ext.define('B4.controller.dict.WorkPpr', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.dict.WorkPpr'
    ],

    models: ['dict.WorkPpr'],
    stores: ['dict.WorkPpr'],
    views: ['dict.workppr.Grid','dict.workppr.EditWindow'],

    mixins: {
        context: 'B4.mixins.Context'
    },
    mainView: 'dict.workppr.Grid',
    mainViewSelector: 'workPprGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'workPprGrid'
        }
    ],

    aspects: [
        {
            xtype: 'workpprperm'
        },
        {
            /*
            * аспект кнопки экспорта реестра
            */
            xtype: 'b4buttondataexportaspect',
            name: 'workPprButtonExportAspect',
            gridSelector: 'workPprGrid',
            buttonSelector: 'workPprGrid #btnExport',
            controllerName: 'WorkPpr',
            actionName: 'Export'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'workPprGridWindowAspect',
            gridSelector: 'workPprGrid',
            editFormSelector: '#workPprEditWindow',
            storeName: 'dict.WorkPpr',
            modelName: 'dict.WorkPpr',
            editWindowView: 'dict.workppr.EditWindow'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('workPprGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.WorkPpr').load();
    }
});