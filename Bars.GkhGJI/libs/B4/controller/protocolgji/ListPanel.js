Ext.define('B4.controller.protocolgji.ListPanel', {
    extend: 'B4.base.Controller',
    params: null,
    title: null,
    requires: ['B4.aspects.GjiDocumentList'],

    views: ['protocolgji.ListPanel'],

    mainView: 'protocolgji.ListPanel',
    mainViewSelector: '#protocolgjiListPanel',

    stores: ['protocolgji.ListForStage', 'protocolgji.Relations'],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },
    
    aspects: [
        {
            /*
            аспект взаимодействия таблицы списка Протоколов с таблицей предшествующих или последующих документов 
            */
            xtype: 'gjidocumentlistaspect',
            name: 'protocolListPanelAspect',
            panelSelector: '#protocolgjiListPanel',
            gridDocumentSelector: '#protocolgjiGrid',
            gridRelationSelector: '#protocolgjiRelationsGrid',
            storeDocumentName: 'protocolgji.ListForStage',
            storeRelationName: 'protocolgji.Relations'
        }
    ],

    onLaunch: function () {
        this.getAspect('protocolListPanelAspect').loadDocumentStore();
    }
});