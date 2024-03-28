Ext.define('B4.controller.motivationconclusion.ListPanel', {
    extend: 'B4.base.Controller',

    params: null,
    title: null,
    requires: ['B4.aspects.GjiDocumentList'],

    views: ['motivationconclusion.ListPanel'],
    stores: [
        'motivationconclusion.ListForStage',
        'motivationconclusion.Relations'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'motivationconclusion.ListPanel',
    mainViewSelector: 'motivationconclusionlistpanel',
    
    aspects: [
        {
            /*
            Аспект взаимодействия списка документов Актов с таблицей Предшествующих  или последующих документов
            */
            xtype: 'gjidocumentlistaspect',
            name: 'motivationConclusionListPanelAspect',
            panelSelector: 'motivationconclusionlistpanel',
            gridDocumentSelector: 'motivationconclusiongrid',
            gridRelationSelector: 'motivationconclusionrelationsgrid',
            storeDocumentName: 'motivationconclusion.ListForStage',
            storeRelationName: 'motivationconclusion.Relations'
        }
    ],
    
    onLaunch: function () {
        this.getAspect('motivationConclusionListPanelAspect').loadDocumentStore();
    }
});