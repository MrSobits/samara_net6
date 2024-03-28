Ext.define('B4.controller.LicRequestEDS', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateButton',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.StateContextMenu',
        'B4.aspects.ButtonDataExport'
    ],

    models: ['mkdlicrequest.MKDLicRequest',
        'mkdlicrequest.MKDLicRequestFile',
        'mkdlicrequest.MKDLicRequestRealityObject',
        'dict.Inspector',
        'mkdlicrequest.MKDLicRequestQueryAnswer',
        'mkdlicrequest.MKDLicRequestQuery'
    ],
    stores: ['mkdlicrequest.MKDLicRequest',
        'mkdlicrequest.MKDLicRequestFile',
        'mkdlicrequest.MKDLicRequestRealityObject',
        'mkdlicrequest.LicRequestEDSQuery',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'mkdlicrequest.MKDLicRequestQueryAnswer'
    ],
    views: [
        'mkdlicrequest.EditWindow',
        'mkdlicrequest.Grid',
        'mkdlicrequest.FileGrid',
        'mkdlicrequest.FileEditWindow',
        'mkdlicrequest.RealityObjectGrid',
        'mkdlicrequest.LicRequestEDSQueryGrid',
        'mkdlicrequest.MKDLicRequestQueryAnswerGrid',
        'mkdlicrequest.LicRequestQueryEDSEditWindow',
        'mkdlicrequest.MKDLicRequestQueryAnswerEditWindow'
    ],
    mainView: 'mkdlicrequest.LicRequestEDSQueryGrid',
    mainViewSelector: 'licrequestedsquerygrid',
    globalAppeal: null,
    refs: [
        {
            ref: 'mainView',
            selector: 'licrequestedsquerygrid'
        }
    ],
    mkdlicrequestQueryId: null,

    aspects: [
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: 'mkdlicrequestqueryanswergrid',
            controllerName: 'MKDLicRequestQueryAnswer',
            name: 'MKDLicRequestQueryFileSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'licrequestedsquerygridButtonExportAspect',
            gridSelector: 'licrequestedsquerygrid',
            buttonSelector: 'licrequestedsquerygrid #btnExport',
            controllerName: 'MKDLicRequestQuery',
            actionName: 'Export'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'licrequestqueryedsGridWindowAspect',
            gridSelector: 'licrequestedsquerygrid',
            editFormSelector: '#licRequestEDSQueryEditWindow',
            storeName: 'mkdlicrequest.LicRequestEDSQuery',
            modelName: 'mkdlicrequest.MKDLicRequestQuery',
            editWindowView: 'mkdlicrequest.LicRequestQueryEDSEditWindow',
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    debugger;
                    mkdlicrequestQueryId = record.getId();
                    var grid = form.down('mkdlicrequestqueryanswergrid'),
                        store = grid.getStore();
                    store.on('beforeload',
                        function (store, operation) {
                            operation.params.MkdLicRequest = mkdlicrequestQueryId;
                        },
                        me);
                    store.load();

                }
            }
        }, 
        {
            xtype: 'grideditwindowaspect',
            name: 'mkdlicrequestqueryAnswerGridWindowAspect',
            gridSelector: 'mkdlicrequestqueryanswergrid',
            editFormSelector: '#mkdLicRequestQueryAnswerEditWindow',
            storeName: 'mkdlicrequest.MKDLicRequestQueryAnswer',
            modelName: 'mkdlicrequest.MKDLicRequestQueryAnswer',
            editWindowView: 'mkdlicrequest.MKDLicRequestQueryAnswerEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('MKDLicRequestQuery', mkdlicrequestQueryId);
                    }
                }
            }
        },     
        
    ],

    mainView: 'mkdlicrequest.Grid',
    mainViewSelector: 'mkdLicRequestGrid',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    index: function () {

        this.params = {};
        var view = this.getMainView() || Ext.widget('licrequestedsquerygrid');
        this.params.dateStart = view.down('#dfDateStart').getValue();
        this.params.dateEnd = view.down('#dfDateEnd').getValue();
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('mkdlicrequest.LicRequestEDSQuery').load();
    },

    init: function () {
        var me = this,
            actions = {};
        me.params = {};
        this.getStore('mkdlicrequest.LicRequestEDSQuery').on('beforeload', this.onBeforeLoadDoc, this);
        me.callParent(arguments);
    },

    onBeforeLoadDoc: function (store, operation) {
        operation.params.isFiltered = true;
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
        }
    },
});