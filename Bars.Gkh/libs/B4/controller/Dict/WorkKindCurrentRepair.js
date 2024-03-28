Ext.define('B4.controller.dict.WorkKindCurrentRepair', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.WorkKindCurrentRepair'
    ],

    models: ['dict.WorkKindCurrentRepair'],
    stores: ['dict.WorkKindCurrentRepair'],
    views: [
        'dict.workkindcurrentrepair.EditWindow',
        'dict.workkindcurrentrepair.Grid'
    ],
    
    aspects: [
        {
            xtype: 'workkindcurrentrepairdictperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'workKindCurrentRepairGridAspect',
            gridSelector: 'workKindCurrentRepairGrid',
            editFormSelector: '#workKindCurrentRepairEditWindow',
            storeName: 'dict.WorkKindCurrentRepair',
            modelName: 'dict.WorkKindCurrentRepair',
            editWindowView: 'dict.workkindcurrentrepair.EditWindow'
        }
    ],

    mainView: 'dict.workkindcurrentrepair.Grid',
    mainViewSelector: 'workKindCurrentRepairGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'workKindCurrentRepairGrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('workKindCurrentRepairGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.WorkKindCurrentRepair').load();
    }
});