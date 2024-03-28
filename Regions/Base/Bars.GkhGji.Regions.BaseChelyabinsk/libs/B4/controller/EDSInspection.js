Ext.define('B4.controller.EDSInspection', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
    ],

    afterset: false,
    edsId: null,
    inspectionId: null,

    models: [
        'eds.EDSInspection',
        'eds.EDSDocument',
        //'eds.EDSPetition',
        'DocumentGji',
        'eds.UKDocument',
        'eds.EDSNotice',
        'eds.EDSMotivRequst'
    ],
    stores: [
        'eds.EDSInspection',
        'eds.EDSDocument',
        'eds.UKDocument',
        'eds.ListDocumentsForPetition',
        //'eds.EDSPetition',
        'eds.EDSNotice',
        'eds.EDSMotivRequst'

    ],
    views: [
        'eds.Grid',
        'eds.EditWindow',
        'eds.DocumentGrid',
        'eds.NoticeGrid',
        'eds.MotivRequstGrid',
        'eds.UKDocumentGrid',
        'eds.UKDocumentEditWindow'
        //'eds.PetitionEditWindow',
        //'eds.PetitionGrid'
    ],
    aspects: [
        //{
        //    xtype: 'gkhgjidigitalsignaturegridaspect',
        //    gridSelector: '#edsPetitionGrid',
        //    controllerName: 'EDSScript',
        //    name: 'edsPetitionSignatureAspect',
        //    signedFileField: 'SignedFile'
        //},
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: 'ukdocumentgrid',
            controllerName: 'UKDocument',
            name: 'ukdocumentSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'edsGridAspect',
            gridSelector: 'esdgrid',
            editFormSelector: '#edsEditWindow',
            storeName: 'eds.EDSInspection',
            modelName: 'eds.EDSInspection',
            editWindowView: 'eds.EditWindow',
            otherActions: function (actions) {
                actions['esddocumentgrid b4updatebutton'] = { 'click': { fn: this.updateDocumentGrid, scope: this } };
                actions['esdnoticegrid b4updatebutton'] = { 'click': { fn: this.updateNoticeGrid, scope: this } };
                actions['esdmotivrequstgrid b4updatebutton'] = { 'click': { fn: this.updateMotiveGrid, scope: this } };
                //actions['esdpetitiongrid b4updatebutton'] = { 'click': { fn: this.updatePetitionGrid, scope: this } };
            },
            //updatePetitionGrid: function (btn) {
            //    var me = this,
            //        grid = btn.up('esdpetitiongrid');
            //    var window = grid.up('#edsEditWindow');
            //    var form = window ? window.getForm() : null;
            //    var rec = form ? form.getRecord() : null;
            //    if (rec) {
            //        debugger;
            //        var inspection = rec.getId();
            //        store = grid.getStore();
            //        store.on('beforeload',
            //            function (store, operation) {
            //                operation.params.edsId = inspection;
            //            },
            //            me);
            //        store.load();
            //    }
            //},
            updateDocumentGrid: function (btn) {
                var me = this,
                    grid = btn.up('esddocumentgrid');
                var window = grid.up('#edsEditWindow');
                var form = window ? window.getForm() : null;
                var rec = form ? form.getRecord() : null;
                if (rec) {
                    var inspection = rec.get('InspectionGji').Id;
                    store = grid.getStore();
                    store.on('beforeload',
                        function (store, operation) {
                            operation.params.inspId = inspection;
                        },
                        me);
                    store.load();
                }
            },
            updateNoticeGrid: function (btn) {
                var me = this,
                    grid = btn.up('esdnoticegrid');
                var window = grid.up('#edsEditWindow');
                var form = window ? window.getForm() : null;
                var rec = form ? form.getRecord() : null;
                if (rec) {
                    var inspection = rec.get('InspectionGji').Id;
                    store = grid.getStore();
                    store.on('beforeload',
                        function (store, operation) {
                            operation.params.inspId = inspection;
                        },
                        me);
                    store.load();
                }
            },
            updateMotiveGrid: function (btn) {
                var me = this,
                    grid = btn.up('esdmotivrequstgrid');
                var window = grid.up('#edsEditWindow');
                var form = window ? window.getForm() : null;
                var rec = form ? form.getRecord() : null;
                if (rec) {
                    var inspection = rec.get('InspectionGji').Id;
                    store = grid.getStore();
                    store.on('beforeload',
                        function (store, operation) {
                            operation.params.inspId = inspection;
                        },
                        me);
                    store.load();
                }
            },

            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    var inspection = record.get('InspectionGji').Id;
                    inspectionId = inspection;
                    edsId = record.getId();
                    if (edsId != 0) {
                        asp.controller.mask('Отметка о прочтении', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('TrySetOpenEDS', 'GjiScript', {
                            docId: edsId
                        })).next(function (response) {
                            asp.controller.unmask();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    var grid = form.down('esddocumentgrid'),
                        store = grid.getStore();
                    store.on('beforeload',
                        function (store, operation) {
                            operation.params.inspId = inspection;
                        },
                        me);
                    store.load();
                    var rogrid = form.down('esdnoticegrid'),
                        rostore = rogrid.getStore();
                    rostore.on('beforeload',
                        function (store, operation) {
                            operation.params.inspId = inspection;
                        },
                        me);
                    rostore.load();
                    var motivgrid = form.down('esdmotivrequstgrid'),
                        motivstore = motivgrid.getStore();
                    motivstore.on('beforeload',
                        function (store, operation) {
                            operation.params.inspId = inspection;
                        },
                        me);
                    motivstore.load();
                    var ukdocgrid = form.down('ukdocumentgrid'),
                        ukdocstore = ukdocgrid.getStore();
                    ukdocstore.on('beforeload',
                        function (store, operation) {
                            operation.params.edsId = edsId;
                        },
                        me);
                    ukdocstore.load();
                    //var petgrid = form.down('esdpetitiongrid'),
                    //    petstore = petgrid.getStore();
                    //petstore.on('beforeload',
                    //    function (store, operation) {
                    //        operation.params.edsId = edsId;
                    //    },
                    //    me);
                    //petstore.load();

                }

            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'ukdocumantGridAspect',
            gridSelector: 'ukdocumentgrid',
            editFormSelector: '#ukDocumentEditWindow',
            storeName: 'eds.UKDocument',
            modelName: 'eds.UKDocument',
            editWindowView: 'eds.UKDocumentEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        debugger;
                        record.set('EDSInspection', edsId);
                    }
                }
            }
        }
    ],

    mainView: 'eds.Grid',
    mainViewSelector: 'esdgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'esdgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {

        //this.getStore('appealcits.AppealOrderFile').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },

    onBeforeLoad: function (store, operation) {
        operation.params.edsId = this.edsId;
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('esdgrid');
        afterset = false;
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('eds.EDSInspection').load();
    }

});