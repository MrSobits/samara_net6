Ext.define('B4.controller.actsurvey.ListPanel', {
    extend: 'B4.base.Controller',
    params: null,
    title: null,
    requires: [
        'B4.aspects.GjiDocumentList'
    ],

    views: ['actsurvey.ListPanel'],

    mainView: 'actsurvey.ListPanel',
    mainViewSelector: '#actSurveyListPanel',

    stores: [
        'ActSurvey',
        'actsurvey.Relations'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            /* 
             * Аспект взаимодействия списка документов Актов обследования с таблицей Предшествующих  или последующих документов 
             */
            xtype: 'gjidocumentlistaspect',
            name: 'actSurveyListPanelAspect',
            panelSelector: '#actSurveyListPanel',
            gridDocumentSelector: '#actSurveyGrid',
            gridRelationSelector: '#actSurveyRelationsGrid',
            storeDocumentName: 'ActSurvey',
            storeRelationName: 'actsurvey.Relations'
        }
    ],

    onLaunch: function () {
        this.getAspect('actSurveyListPanelAspect').loadDocumentStore();
    }
});