Ext.define('B4.controller.AppealOrder', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ButtonDataExport'
    ],

    gisERP: null,
    afterset: false,
    appealOrderId: null,
  
    models: [
        'appealcits.AppealOrder',
        'appealcits.AppealOrderFile',
        'appealcits.RealityObject',
        'appealcits.AppealOrderExecutant'
    ],
    stores: [
        'appealcits.AppealOrder',
        'appealcits.AppealOrderFile',
        'appealcits.RealityObject',
        'appealcits.AppealOrderExecutant'

    ],
    views: [
        'appealcits.AppealOrderGrid',
        'appealcits.AppealOrderEditWindow',
        'appealcits.AppealOrderExecutantGrid',
        'appealcits.AppealOrdeRealityObjectGrid',
        'appealcits.AppealOrderFileGrid',
        'appealcits.AppealOrderFileEditWindow'
    ],
    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
             
                { name: 'GkhGji.SOPR.Appeal.Vp_Edit', applyTo: '[name=Confirmed]', selector: '#appealcitsAppealOrderEditWindow' },
             
                //{
                //    name: 'Gkh.Orgs.Contragent.Register.Contact.Delete', applyTo: 'b4deletecolumn', selector: 'contragentContactGrid',
                //    applyBy: function (component, allowed) {
                //        if (allowed) component.show();
                //        else component.hide();
                //    }
                //}
            ]
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
            xtype: 'gkhpermissionaspect',
            permissions: [               
                {
                    name: 'GkhGji.SOPR.Appeal.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'appealordergrid',
                    applyBy: function (component, allowed) {
                        var me = this;
                        debugger;
                        me.controller.params = me.controller.params || {};
                        if (allowed) {
                            component.show();
                        }
                        else {
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appealOrderGridAspect',
            gridSelector: 'appealordergrid',
            editFormSelector: '#appealcitsAppealOrderEditWindow',
            storeName: 'appealcits.AppealOrder',
            modelName: 'appealcits.AppealOrder',
            editWindowView: 'appealcits.AppealOrderEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },   
            otherActions: function (actions) {             
                actions['appealorderexecutantgrid b4updatebutton'] = { 'click': { fn: this.updateExecutorsGrid, scope: this } };  
                actions['appealorderealityobjectgrid b4updatebutton'] = { 'click': { fn: this.updateRealityGrid, scope: this } };  
                actions['appealordergrid #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions[this.gridSelector + ' #cbShowCloseAppeals'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
                actions['appealordergrid #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
            },  
            onChangeDateStart: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateStart = newValue;
                }
            },
            onChangeCheckbox: function (field, newValue) {
                debugger;
                this.controller.params.showCloseAppeals = newValue;
                this.controller.getStore('appealcits.AppealOrder').load();
            },
            onChangeDateEnd: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateEnd = newValue;
                }
            },
            updateExecutorsGrid: function (btn) {
                var me = this,
                    grid = btn.up('appealorderexecutantgrid');
                var window = grid.up('#appealcitsAppealOrderEditWindow');
                var form = window ? window.getForm() : null;
                var rec = form ? form.getRecord() : null;
                if (rec) {
                    var appcit = rec.get('AppealCits').Id;
                    store = grid.getStore();
                    store.on('beforeload',
                        function (store, operation) {
                            operation.params.appealId = appcit;
                        },
                        me);
                    store.load();    
                }
            },
            updateRealityGrid: function (btn) {
                var me = this,
                    grid = btn.up('appealorderealityobjectgrid');
                var window = grid.up('#appealcitsAppealOrderEditWindow');
                var form = window ? window.getForm() : null;
                var rec = form ? form.getRecord() : null;
                if (rec) {
                    var appcit = rec.get('AppealCits').Id;
                    store = grid.getStore();
                    store.on('beforeload',
                        function (store, operation) {
                            operation.params.appealCitizensId = appcit;
                        },
                        me);
                    store.load();
                }
            },

            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    var appcit = record.get('AppealCits').Id;
                    appealOrderId = record.getId();
                    var grid = form.down('appealorderexecutantgrid'),
                        store = grid.getStore();
                    store.on('beforeload',
                        function (store, operation) {
                            operation.params.appealId = appcit;
                        },
                        me);
                    store.load();    
                    var rogrid = form.down('appealorderealityobjectgrid'),
                        rostore = rogrid.getStore();
                    rostore.on('beforeload',
                        function (store, operation) {
                            operation.params.appealCitizensId = appcit;
                        },
                        me);
                    rostore.load(); 
                    var filerid = form.down('appealorderfilegrid'),
                        filestore = filerid.getStore();
                    filestore.on('beforeload',
                        function (store, operation) {
                            operation.params.appealOrderId = appealOrderId;
                        },
                        me);
                    filestore.load(); 

                }
              
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'appealOrderFileGridAspect',
            gridSelector: 'appealorderfilegrid',
            editFormSelector: '#appealOrderFileEditWindow',
            storeName: 'appealcits.AppealOrderFile',
            modelName: 'appealcits.AppealOrderFile',
            editWindowView: 'appealcits.AppealOrderFileEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealOrder', appealOrderId);
                    }
                }
            }
        }      
    ],

    mainView: 'appealcits.AppealOrderGrid',
    mainViewSelector: 'appealordergrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'appealordergrid'
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
        this.getStore('appealcits.AppealOrder').on('beforeload', this.onBeforeLoadDoc, this);
        this.getStore('appealcits.AppealOrderFile').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },

    onLaunch: function () {
        var grid = this.getMainView();
        //this.params.dateFromStart = grid.down('#dfDateFromStart').getValue();
        //this.params.dateFromEnd = grid.down('#dfDateFromEnd').getValue();
        debugger;
        if (this.params && this.params.soprId > 0) {
            var model = this.getModel('appealcits.AppealOrder');
            this.getAspect('appealOrderGridAspect').editRecord(new model({ Id: this.params.soprId }));
            this.params.soprId = 0;
        }
    },

    onBeforeLoad: function (store, operation) {
        operation.params.appealOrderId = this.appealOrderId;
    },

    onBeforeLoadDoc: function (store, operation) {
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
            operation.params.showCloseAppeals = this.params.showCloseAppeals;
        }
    },

    index: function () {
        this.params = {};
        var view = this.getMainView() || Ext.widget('appealordergrid');
        this.params.dateStart = view.down('#dfDateStart').getValue();
        this.params.dateEnd = view.down('#dfDateEnd').getValue();
        this.params.showCloseAppeals = view.down('#cbShowCloseAppeals').getValue();
        afterset = false;
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('appealcits.AppealOrder').load();
    }

});