Ext.define('B4.controller.dict.ConstructiveElementGroup', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GridEditWindow'
    ],

    models: ['dict.ConstructiveElementGroup'],
    stores: ['dict.ConstructiveElementGroup'],
    views: [
        'dict.constructiveelementgroup.Grid',
        'dict.constructiveelementgroup.EditWindow'
    ],

    mainView: 'dict.constructiveelementgroup.Grid',
    mainViewSelector: 'constructiveElementGroupGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'constructiveElementGroupGrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'constructiveElementGroupGridWindowAspect',
            gridSelector: 'constructiveElementGroupGrid',
            editFormSelector: '#constructiveElementGroupEditWindow',
            storeName: 'dict.ConstructiveElementGroup',
            modelName: 'dict.ConstructiveElementGroup',
            editWindowView: 'dict.constructiveelementgroup.EditWindow'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'constructiveElementGroupButtonExportAspect',
            gridSelector: 'constructiveElementGroupGrid',
            buttonSelector: 'constructiveElementGroupGrid #btnExport',
            controllerName: 'ConstructiveElementGroup',
            actionName: 'Export'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('constructiveElementGroupGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ConstructiveElementGroup').load();
    }
});