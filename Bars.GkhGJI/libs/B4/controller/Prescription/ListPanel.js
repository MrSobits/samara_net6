Ext.define('B4.controller.prescription.ListPanel', {
    extend: 'B4.base.Controller',
    params: null,

    requires: [
        'B4.aspects.GjiDocumentList'
    ],

    views: [
        'prescription.ListPanel'
    ],

    mainView: 'prescription.ListPanel',
    mainViewSelector: '#prescriptionListPanel',

    stores: [
        'prescription.ListForStage',
        'prescription.Relations'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },
    
    aspects: [
        {
            /*
            аспект взаимодействия таблицы списка Предписаний с таблицей предшествующих или последующих документов 
            */
            xtype: 'gjidocumentlistaspect',
            name: 'prescriptionListPanelAspect',
            panelSelector: '#prescriptionListPanel',
            gridDocumentSelector: '#prescriptionGrid',
            gridRelationSelector: '#prescriptionRelationsGrid',
            storeDocumentName: 'prescription.ListForStage',
            storeRelationName: 'prescription.Relations'
        }
    ],

    onLaunch: function () {
        var me = this;

            me.getAspect('prescriptionListPanelAspect').loadDocumentStore();
    }
});