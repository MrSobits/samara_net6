Ext.define('B4.controller.presentation.ListPanel', {
    extend: 'B4.base.Controller',
    params: null,
    title: null,
    requires: ['B4.aspects.GjiDocumentList'],

    stores: ['Presentation', 'presentation.Relations'],

    views: ['presentation.ListPanel'],

    mainView: 'presentation.ListPanel',
    mainViewSelector: '#presentationListPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },
    
    aspects: [
        {
            /*
            * аспект взаимодействия таблицы списка Представлений с таблицей предшествующих или последующих документов 
            */
            xtype: 'gjidocumentlistaspect',
            name: 'presentationListPanelAspect',
            panelSelector: '#presentationListPanel',
            gridDocumentSelector: '#presentationGrid',
            gridRelationSelector: '#presentationRelationsGrid',
            storeDocumentName: 'Presentation',
            storeRelationName: 'presentation.Relations'
        }
    ],

    onLaunch: function () {
        this.getAspect('presentationListPanelAspect').loadDocumentStore();
    }
});