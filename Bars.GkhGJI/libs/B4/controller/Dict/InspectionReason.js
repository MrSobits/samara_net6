Ext.define('B4.controller.dict.InspectionReason', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GkhInlineGrid'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.InspectionReason'],
    stores: ['dict.InspectionReason'],

    views: ['dict.inspectionreason.Grid'],

    mainView: 'dict.inspectionreason.Grid',
    mainViewSelector: 'inspectionreasongrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'inspectionreasongrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'inspectionreasonGridAspect',
            storeName: 'dict.InspectionReason',
            modelName: 'dict.InspectionReason',
            gridSelector: 'inspectionreasongrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('inspectionreasongrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.InspectionReason').load();
    }
});