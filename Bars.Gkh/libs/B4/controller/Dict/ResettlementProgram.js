Ext.define('B4.controller.dict.ResettlementProgram', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.ResettlementProgram'
    ],

    aspects: [
        {
            xtype: 'resettlementprogramdictperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'resettlementProgramGridAspect',
            gridSelector: 'resettlementProgramGrid',
            editFormSelector: 'resettlementProgramEditWindow',
            storeName: 'dict.ResettlementProgram',
            modelName: 'dict.ResettlementProgram',
            editWindowView: 'dict.resettlementprogram.EditWindow'
        }
    ],

    models: ['dict.ResettlementProgram'],
    stores: ['dict.ResettlementProgram'],
    views: ['dict.resettlementprogram.EditWindow',
            'dict.resettlementprogram.Grid'],

    mainView: 'dict.resettlementprogram.Grid',
    mainViewSelector: 'resettlementProgramGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'resettlementProgramGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('resettlementProgramGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ResettlementProgram').load();
    }
});