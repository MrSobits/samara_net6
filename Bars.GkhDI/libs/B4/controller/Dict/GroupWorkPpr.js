Ext.define('B4.controller.dict.GroupWorkPpr', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.GroupWorkPpr'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },
    models: ['dict.GroupWorkPpr'],
    stores: ['dict.GroupWorkPpr'],
    views: ['dict.groupworkppr.Grid', 'dict.groupworkppr.EditWindow'],

    mainView: 'dict.groupworkppr.Grid',
    mainViewSelector: 'groupWorkPprGrid',

    refs: [{
        ref: 'mainView',
        selector: 'groupWorkPprGrid'
    }],

    aspects: [
        {
            xtype: 'groupworkpprperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'groupWorkPprGridAspect',
            gridSelector: 'groupWorkPprGrid',
            editFormSelector: '#groupWorkPprEditWindow',
            storeName: 'dict.GroupWorkPpr',
            modelName: 'dict.GroupWorkPpr',
            editWindowView: 'dict.groupworkppr.EditWindow',
            otherActions: function(actions) {
                actions[this.editFormSelector + ' #cbService'] = { 'storebeforeload': { fn: this.onBeforeLoadService, scope: this } };
            },
            onBeforeLoadService: function (combobox, store, operation) {
                //чтобы получить только услуги с типом ремонт
                operation.params.kindTemplateService = 30;
                operation.params.isTemplateService = true;
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('groupWorkPprGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.GroupWorkPpr').load();
    }
});