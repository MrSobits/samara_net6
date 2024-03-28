Ext.define('B4.controller.warningdoc.ListPanel', {
    extend: 'B4.base.Controller',

    params: null,
    title: null,
    requires: ['B4.aspects.GjiDocumentList'],

    views: ['warningdoc.ListPanel'],
    stores: [
        'warningdoc.ListForStage',
        'warningdoc.Relations'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'warningdoc.ListPanel',
    mainViewSelector: 'warningdoclistpanel',
    
    aspects: [
        {
            /*
            Аспект взаимодействия списка документов Актов с таблицей Предшествующих  или последующих документов
            */
            xtype: 'gjidocumentlistaspect',
            name: 'warningDocListPanelAspect',
            panelSelector: 'warningdoclistpanel',
            gridDocumentSelector: 'warningdocgrid',
            gridRelationSelector: 'warningdocrelationsgrid',
            storeDocumentName: 'warningdoc.ListForStage',
            storeRelationName: 'warningdoc.Relations'
        }
    ],
    
    onLaunch: function () {
        this.getAspect('warningDocListPanelAspect').loadDocumentStore();
    }
});