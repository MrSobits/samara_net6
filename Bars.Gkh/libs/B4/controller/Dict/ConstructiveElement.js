Ext.define('B4.controller.dict.ConstructiveElement', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.ConstructiveElement'
    ],

    models: ['dict.ConstructiveElement'],
    stores: ['dict.ConstructiveElement'],
    views: [
        'dict.constructiveelement.Grid',
        'dict.constructiveelement.EditWindow'
    ],

    mainView: 'dict.constructiveelement.Grid',
    mainViewSelector: 'constructiveElementGrid',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'constructiveelementdictperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'constructiveElementGridWindowAspect',
            gridSelector: 'constructiveElementGrid',
            editFormSelector: '#constructiveElementEditWindow',
            storeName: 'dict.ConstructiveElement',
            modelName: 'dict.ConstructiveElement',
            editWindowView: 'dict.constructiveelement.EditWindow'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'constructiveElementButtonExportAspect',
            gridSelector: 'constructiveElementGrid',
            buttonSelector: '#constructiveElementGrid #btnExport',
            controllerName: 'ConstructiveElement',
            actionName: 'Export'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('constructiveElementGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ConstructiveElement').load();
    }
});