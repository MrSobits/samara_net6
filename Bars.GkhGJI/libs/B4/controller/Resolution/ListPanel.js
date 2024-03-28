Ext.define('B4.controller.resolution.ListPanel', {
    extend: 'B4.base.Controller',
 views: [ 'resolution.ListPanel' ], 

    params: null,
    title: null,
    requires: ['B4.aspects.GjiDocumentList'],

    mainView: 'resolution.ListPanel',
    mainViewSelector: '#resolutionListPanel',

    stores: ['Resolution', 'resolution.Relations'],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },
    
    aspects: [
        {
            /*
            аспект взаимодействия таблицы списка Постановлений с таблицей предшествующих или последующих документов 
            */
            xtype: 'gjidocumentlistaspect',
            name: 'resolutionListPanelAspect',
            panelSelector: '#resolutionListPanel',
            gridDocumentSelector: '#resolutionGrid',
            gridRelationSelector: '#resolutionRelationsGrid',
            storeDocumentName: 'Resolution',
            storeRelationName: 'resolution.Relations'
        }
    ],

    onLaunch: function () {
        this.getAspect('resolutionListPanelAspect').loadDocumentStore();
    }
});