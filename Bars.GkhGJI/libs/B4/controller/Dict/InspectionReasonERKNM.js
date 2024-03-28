Ext.define('B4.controller.dict.InspectionReasonERKNM', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.InspectionReasonERKNM'],
    stores: ['dict.InspectionReasonERKNM'],

    views: ['dict.inspectionreasonerknm.Grid'],

    mainView: 'dict.inspectionreasonerknm.Grid',
    mainViewSelector: 'iinspectionreasonerknmgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'inspectionreasonerknmgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'inspectionreasonerknmGridAspect',
            storeName: 'dict.InspectionReasonERKNM',
            modelName: 'dict.InspectionReasonERKNM',
            gridSelector: 'inspectionreasonerknmgrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('inspectionreasonerknmgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.InspectionReason').load();
    }
});