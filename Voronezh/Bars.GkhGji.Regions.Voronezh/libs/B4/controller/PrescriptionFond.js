Ext.define('B4.controller.PrescriptionFond', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.Ajax', 'B4.Url'
    ],

    appealCitsPrescriptionFond: null,
    bcId: null,

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'appealcits.PrescriptionFond',
        'appealcits.AppCitPrFondVoilation',
        'appealcits.AppCitPrFondObjectCr',
        'appealcits.ObjectCrList'
    ],

    models: [
        'appealcits.PrescriptionFond'
    ],

    views: [
        'appealcits.PrescriptionFondGrid',
        'appealcits.PrescriptionFondEditWindow',
        'appealcits.PrFondVoilationGrid',
        'appealcits.PrFondVoilationEditWindow',
        'appealcits.PrFondObjectCrGrid',
        'appealcits.PrFondObjectCrEditWindow',
        'appealcits.PrescriptionFondMainPanel',
        'appealcits.PrescriptionFondFilterPanel'
    ],

    mainView: 'appealcits.PrescriptionFondMainPanel',
    mainViewSelector: 'appealcitsPrescriptionFondMainPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'appealcitsPrescriptionFondMainPanel'
        },
        {
            ref: 'prescriptionfondEditWindow',
            selector: 'prescriptionfondeditwindow'
        }
    ],

    aspects: [
        //{
        //    xtype: 'gkhgjidigitalsignaturegridaspect',
        //    gridSelector: '#prescriptionfondgrid',
        //    controllerName: 'AppealCitsPrescriptionFondAnswerSign',
        //    name: 'appealCitsPrescriptionFondAnswerSignatureAspect',
        //    signedFileField: 'SignedAnswerFile'
        //},
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Create', applyTo: 'b4addbutton', selector: '#prescriptionfondgrid' },
                { name: 'GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Registry.Violation.Create', applyTo: 'b4addbutton', selector: '#prFondVoilationGrid' },
                { name: 'GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Registry.ObjectCr.Create', applyTo: 'b4addbutton', selector: '#prFondObjectCrGrid' },
                { name: 'GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Field.All', applyTo: '[name=Contragent]', selector: '#prescriptionfondeditwindow' },
                { name: 'GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Field.All', applyTo: '[name=DocumentName]', selector: '#prescriptionfondeditwindow' },
                { name: 'GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Field.All', applyTo: '[name=DocumentNumber]', selector: '#prescriptionfondeditwindow' },
                { name: 'GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Field.All', applyTo: '[name=DocumentDate]', selector: '#prescriptionfondeditwindow' },
                { name: 'GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Field.All', applyTo: '[name=PerfomanceDate]', selector: '#prescriptionfondeditwindow' },
                { name: 'GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Field.All', applyTo: '[name=PerfomanceFactDate]', selector: '#prescriptionfondeditwindow' },
                { name: 'GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Field.All', applyTo: '[name=File]', selector: '#prescriptionfondeditwindow' },
                { name: 'GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Field.All', applyTo: '[name=Inspector]', selector: '#prescriptionfondeditwindow' },
                { name: 'GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Field.All', applyTo: '[name=Executor]', selector: '#prescriptionfondeditwindow' },
                { name: 'GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Field.All', applyTo: '[name=MassBuildContract]', selector: '#prescriptionfondeditwindow' }
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
            xtype: 'gkhbuttonprintaspect',
            name: 'prescriptionFondPrintAspect',
            buttonSelector: '#prescriptionfondeditwindow #btnPrint',
            codeForm: 'AppealCitsPrescriptionFond',
            getUserParams: function () {
                var param = { Id: appealCitsPrescriptionFond };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        //{
        //    xtype: 'b4buttondataexportaspect',
        //    name: 'disposalGjiButtonExportAspect',
        //    gridSelector: '#prescriptionfondgrid',
        //    buttonSelector: '#prescriptionfondgrid #btnExport',
        //    controllerName: 'AppealCitsPrescriptionFondSign',
        //    actionName: 'Export'
        //},
        {
            xtype: 'grideditwindowaspect',
            name: 'prescriptionfondGridWindowAspect',
            gridSelector: 'prescriptionfondgrid',
            editFormSelector: 'prescriptionfondeditwindow',
            modelName: 'appealcits.PrescriptionFond',
            storeName: 'appealcits.PrescriptionFond',
            editWindowView: 'appealcits.PrescriptionFondEditWindow',
            otherActions: function (actions) {
                actions['#appealcitsPrescriptionFondFilterPanel #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions['#appealcitsPrescriptionFondFilterPanel #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                actions['#appealcitsPrescriptionFondFilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
                actions['prescriptionfondeditwindow #sfBuildContract'] = { 'change': { fn: this.onChangeDoc, scope: this } };
            },
            onChangeDoc: function (field, newValue) {
                debugger;
                if (newValue != null) {
                    bcId = newValue.Id;
                }
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('appealcits.PrescriptionFond');
                str.currentPage = 1;
                str.load();
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
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                aftersetformdata: function (asp, rec, form) {
                    var me = this;
                    appealCitsPrescriptionFond = rec.getId();
                    me.controller.getAspect('prescriptionFondPrintAspect').loadReportStore();

                    var grid = form.down('prFondVoilationGrid'),
                        store = grid.getStore();
                    var grid2 = form.down('prFondObjectCrGrid'),
                        store2 = grid2.getStore();
                    store.filter('AppealCitsPrescriptionFond', rec.getId());
                    store2.filter('AppealCitsPrescriptionFond', rec.getId());
                }
            }
        },

        {
            xtype: 'grideditwindowaspect',
            name: 'prFondVoilationGridWindowAspect',
            gridSelector: '#prFondVoilationGrid',
            editFormSelector: '#prFondVoilationEditWindow',
            storeName: 'appealcits.AppCitPrFondVoilation',
            modelName: 'appealcits.AppCitPrFondVoilation',
            editWindowView: 'appealcits.PrFondVoilationEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCitsPrescriptionFond', appealCitsPrescriptionFond);
                    }
                }
            }
        },

        {
            xtype: 'grideditwindowaspect',
            name: 'prFondObjectCrGridWindowAspect',
            gridSelector: '#prFondObjectCrGrid',
            editFormSelector: '#prFondObjectCrEditWindow',
            storeName: 'appealcits.AppCitPrFondObjectCr',
            modelName: 'appealcits.AppCitPrFondObjectCr',
            editWindowView: 'appealcits.PrFondObjectCrEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AppealCitsPrescriptionFond', appealCitsPrescriptionFond);
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    debugger;
                    var sfObjCr = this.getForm().down('#sfObjCr');
                    sfObjCr.getStore().filter('bcId', bcId);
                }
            }
        }
    ],

    index: function (operation) {
        var me = this,
            view = me.getMainView() || Ext.widget('appealcitsPrescriptionFondMainPanel');
        me.params = {};
        me.params.dateStart = new Date(new Date().getFullYear(), 0, 1);
        me.params.dateEnd = new Date(new Date().getFullYear(), 11, 31);
        debugger;
        me.bindContext(view);
        this.application.deployView(view);
        //me.getAspect('manOrgLicenseNotificationGisEditPanelAspect').setData(id);

        this.getStore('appealcits.PrescriptionFond').load();
        // this.getStore('appealcits.Admonition').filter()
    },

    init: function () {
        var me = this,
            actions = {};
        me.params = {};
        me.params.dateStart = new Date(new Date().getFullYear(), 0, 1);
        me.params.dateEnd = new Date(new Date().getFullYear(), 11, 31);
        this.getStore('appealcits.PrescriptionFond').on('beforeload', this.onBeforeLoadPrescriptionFond, this);
        this.getStore('appealcits.PrescriptionFond').load();
        me.callParent(arguments);
    },

    onBeforeLoadPrescriptionFond: function (store, operation) {
        operation.params.isFiltered = true;
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
        }
    },
});