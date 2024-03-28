Ext.define('B4.controller.dict.WorkTo', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.dict.WorkTo'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },
    models: ['dict.WorkTo'],
    stores: ['dict.WorkTo'],
    views: ['dict.workto.Grid','dict.workto.EditWindow'],

    mainView: 'dict.workto.Grid',
    mainViewSelector: 'workToGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'workToGrid'
        }
    ],

    aspects: [
        {
            xtype: 'worktoperm'
        },
        {
            /*
            * аспект кнопки экспорта реестра
            */
            xtype: 'b4buttondataexportaspect',
            name: 'workToButtonExportAspect',
            gridSelector: 'workToGrid',
            buttonSelector: 'workToGrid #btnExport',
            controllerName: 'WorkTo',
            actionName: 'Export'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'workToGridWindowAspect',
            gridSelector: 'workToGrid',
            editFormSelector: '#workToEditWindow',
            storeName: 'dict.WorkTo',
            modelName: 'dict.WorkTo',
            editWindowView: 'dict.workto.EditWindow'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('workToGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.WorkTo').load();
    }
});