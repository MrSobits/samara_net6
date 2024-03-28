Ext.define('B4.controller.resolutionrospotrebnadzor.ListPanel', {
    extend: 'B4.base.Controller',
    views: ['resolutionrospotrebnadzor.ListPanel'],

    params: null,
    title: null,
    requires: ['B4.aspects.GjiDocumentList'],

    mainView: 'resolutionrospotrebnadzor.ListPanel',
    mainViewSelector: '#resolutionRospotrebnadzorListPanel',

    stores: [
        'ResolutionRospotrebnadzor',
        'resolutionrospotrebnadzor.Relations'
    ],

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
            name: 'resolutionRospotrebnadzorListPanelAspect',
            panelSelector: '#resolutionRospotrebnadzorListPanel',
            gridDocumentSelector: '#resolutionRospotrebnadzorGrid',
            gridRelationSelector: '#resolutionRospotrebnadzorRelationsGrid',
            storeDocumentName: 'ResolutionRospotrebnadzor',
            storeRelationName: 'resolutionrospotrebnadzor.Relations'
        }
    ],

    onLaunch: function () {
        var me = this;

        me.getAspect('resolutionRospotrebnadzorListPanelAspect').loadDocumentStore();
    }
});