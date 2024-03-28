Ext.define('B4.controller.EconFeasibilityCalc', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.ButtonDataExport'
    ],
    stores: [
        'EconFeasibilityCalcResult',
        'EconFeasibilityCalcWork'
    ],
    models: [
        'EconFeasibilityCalcResult',
        'EconFeasibilityCalcWork'
    ],
    views: [
        'econfeasibilitycalcresult.Grid',
        'econfeasibilitycalcresult.EditWindow',
        'econfeasibilitycalcresult.WorkGrid'
    ],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    refs: [
        {
            ref: 'mainView',
            selector: 'econfeasibilitycalcresultgrid'
        }
    ],
    mainView: 'econfeasibilitycalcresult.Grid',
    mainViewSelector: 'econfeasibilitycalcresultgrid',
    
    //init: function () {
    //    var me = this,
    //        actions = {
    //        };
    //    me.control(actions);
    //    me.callParent(arguments);
    //},
    index: function () {
        var view = this.getMainView() || Ext.widget('econfeasibilitycalcresultgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('EconFeasibilityCalcResult').load();
    },
    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'econfeasibilitycalcButtonExportAspect',
            gridSelector: 'econfeasibilitycalcresultgrid',
            buttonSelector: 'econfeasibilitycalcresultgrid #btnExport',
            controllerName: 'OverhaulHmaoScripts',
            actionName: 'Export'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'econfeasibilitycalcGridAspect',
            gridSelector: 'econfeasibilitycalcresultgrid',
            editFormSelector: '#econfeasibilitycalcEditWindow',
            storeName: 'EconFeasibilityCalcResult',
            modelName: 'EconFeasibilityCalcResult',
            editWindowView: 'econfeasibilitycalcresult.EditWindow',
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    calcResult = record.getId();
                    var grid = form.down('econfeasibilityworkgrid'),
                        store = grid.getStore();
                    store.filter('ResultId', record.getId());
                },
            }
        },
        
        
    ]
});