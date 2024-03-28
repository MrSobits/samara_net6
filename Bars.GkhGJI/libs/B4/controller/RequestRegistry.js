Ext.define('B4.controller.RequestRegistry', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.ButtonDataExport'
    ],

    requestId: null,
  
    models: [
        'appealcits.Request',
        'appealcits.RequestAnswer'       
    ],
    stores: [
        'appealcits.RequestRegistry',
        'appealcits.RequestAnswerRegistry'

    ],
    views: [
        'appealcits.RequestRegistryGrid',
        'appealcits.RequestRegistryEditWindow',
        'appealcits.RequestAnswerRegistryGrid',
        'appealcits.RequestAnswerEditWindow'
    ],
    aspects: [
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: '#appcitRequestAnswerRegGrid',
            controllerName: 'AppealCitsRequestAnswer',
            name: 'appcitRequestAnswerRegSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'appealcitsrequestregistrygridButtonExportAspect',
            gridSelector: 'appealcitsrequestregistrygrid',
            buttonSelector: 'appealcitsrequestregistrygrid #btnExport',
            controllerName: 'AppealCitsRequest',
            actionName: 'Export'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'appealordergridButtonExportAspect',
            gridSelector: 'appealordergrid',
            buttonSelector: 'appealordergrid #btnExport',
            controllerName: 'CourtPracticeOperations',
            actionName: 'ExportSOPR'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appealRequestRegistryGridAspect',
            gridSelector: 'appealcitsrequestregistrygrid',
            editFormSelector: '#appealCitsRequestRegistryEditWindow',
            storeName: 'appealcits.RequestRegistry',
            modelName: 'appealcits.Request',
            editWindowView: 'appealcits.RequestRegistryEditWindow',
            otherActions: function (actions) {             
                actions['appealcitsrequestregistrygrid #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions['appealcitsrequestregistrygrid #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
            },  
            onChangeDateStart: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateStart = newValue;
                }
            },
            onChangeDateEnd: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateEnd = newValue;
                }
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    requestId = record.getId();
                    var grid = form.down('appcitrequestanswerreggrid'),
                        store = grid.getStore();
                    store.on('beforeload',
                        function (store, operation) {
                            operation.params.appcitRequestId = requestId;
                        },
                        me);
                    store.load();                    

                }
              
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appealrequestreganswerGridAspect',
            gridSelector: 'appcitrequestanswerreggrid',
            editFormSelector: '#appealCitsRequestAnswerEditWindow',
            storeName: 'appealcits.RequestAnswerRegistry',
            modelName: 'appealcits.RequestAnswer',
            editWindowView: 'appealcits.RequestAnswerEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCitsRequest', requestId);
                    }
                }
            }
        }      
    ],

    mainView: 'appealcits.RequestRegistryGrid',
    mainViewSelector: 'appealcitsrequestregistrygrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'appealcitsrequestregistrygrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        var me = this,
            actions = {};
        me.params = {};
        this.getStore('appealcits.RequestRegistry').on('beforeload', this.onBeforeLoadDoc, this);
        this.callParent(arguments);
    },

    //onLaunch: function () {
    //    var grid = this.getMainView();
    //    //this.params.dateFromStart = grid.down('#dfDateFromStart').getValue();
    //    //this.params.dateFromEnd = grid.down('#dfDateFromEnd').getValue();
    //    debugger;
    //    if (this.params && this.params.soprId > 0) {
    //        var model = this.getModel('appealcits.AppealOrder');
    //        this.getAspect('appealOrderGridAspect').editRecord(new model({ Id: this.params.soprId }));
    //        this.params.soprId = 0;
    //    }
    //},

    onBeforeLoadDoc: function (store, operation) {
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
        }
    },

    index: function () {
        this.params = {};
        var view = this.getMainView() || Ext.widget('appealcitsrequestregistrygrid');
        this.params.dateStart = view.down('#dfDateStart').getValue();
        this.params.dateEnd = view.down('#dfDateEnd').getValue();
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('appealcits.RequestRegistry').load();
    }

});