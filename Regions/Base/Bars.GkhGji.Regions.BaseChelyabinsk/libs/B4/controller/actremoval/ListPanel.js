Ext.define('B4.controller.actremoval.ListPanel', {
    extend: 'B4.base.Controller',
    views: [
        'actremoval.ListPanel'
    ],
    
    requires: [
        'B4.aspects.GjiDocumentList'
    ],

    mainView: 'actremoval.ListPanel',
    mainViewSelector: '#actRemovalListPanel',

    stores: ['ActRemoval'],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },
    
    aspects: [
        {
            /**
             *аспект взаимодействия таблицы списка актов предписаний с таблицей предшествующих или последующих документов 
             */
            xtype: 'gjidocumentlistaspect',
            name: 'actRemovalListPanelAspect',
            panelSelector: '#actRemovalListPanel',
            gridDocumentSelector: '#actRemovalGrid',
            gridRelationSelector: null, //этот грид не нужен на этой форме
            storeDocumentName: 'ActRemoval',
            storeRelationName: null
        }
    ],

    onLaunch: function () {
        this.getAspect('actRemovalListPanelAspect').loadDocumentStore();
    }
});