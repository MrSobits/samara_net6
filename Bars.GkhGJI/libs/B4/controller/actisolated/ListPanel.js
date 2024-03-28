Ext.define('B4.controller.actisolated.ListPanel', {
    extend: 'B4.base.Controller',

    params: null,
    title: null,
    requires: ['B4.aspects.GjiDocumentList'],

    views: ['actisolated.ListPanel'],
    stores: [
        'actisolated.ListForStage',
        'actisolated.Relations'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'actisolated.ListPanel',
    mainViewSelector: 'actisolatedlistpanel',
    
    aspects: [
        {
            /*
            Аспект взаимодействия списка документов Актов с таблицей Предшествующих  или последующих документов
            */
            xtype: 'gjidocumentlistaspect',
            name: 'actIsolatedListPanelAspect',
            panelSelector: 'actisolatedlistpanel',
            gridDocumentSelector: 'actisolatedgrid',
            gridRelationSelector: 'actisolatedrelationsgrid',
            storeDocumentName: 'actisolated.ListForStage',
            storeRelationName: 'actisolated.Relations'
        }
    ],
    
    onLaunch: function () {
        this.getAspect('actIsolatedListPanelAspect').loadDocumentStore();
    }
});