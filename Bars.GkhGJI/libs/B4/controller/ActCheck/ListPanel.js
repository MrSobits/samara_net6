Ext.define('B4.controller.actcheck.ListPanel', {
    extend: 'B4.base.Controller',
    views: [ 'actcheck.ListPanel' ], 

    params: null,
    title: null,
    requires: ['B4.aspects.GjiDocumentList'],

    mainView: 'actcheck.ListPanel',
    mainViewSelector: '#actCheckListPanel',

    stores: ['actcheck.ListForStage', 'actcheck.Relations'],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },
    
    aspects: [
        {
            /*
            Аспект взаимодействия списка документов Актов с таблицей Предшествующих  или последующих документов
            */
            xtype: 'gjidocumentlistaspect',
            name: 'actCheckListPanelAspect',
            panelSelector: '#actCheckListPanel',
            gridDocumentSelector: '#actCheckGrid',
            gridRelationSelector: '#actCheckRelationsGrid',
            storeDocumentName: 'actcheck.ListForStage',
            storeRelationName: 'actcheck.Relations'
        }
    ],
    
    onLaunch: function () {
        this.getAspect('actCheckListPanelAspect').loadDocumentStore();
    }
});