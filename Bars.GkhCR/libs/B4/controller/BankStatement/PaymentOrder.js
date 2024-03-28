/*
 * Контроллер раздела платежных поручений
 * перекрыт в модуле для Тулы
 */
Ext.define('B4.controller.bankstatement.PaymentOrder', {
    extend: 'B4.base.Controller',
    requires:
    [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.PaymentOrder',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhButtonImportAspect'
    ],
    mixins: {
        context: 'B4.mixins.Context'
    },
    models: ['bankstatement.PaymentOrder'],
    stores: [
        'bankstatement.PaymentOrder',
        'dict.Period'
    ],
    views:
     [
         'bankstatement.PaymentOrderEditWindow',
         'bankstatement.PaymentOrderPanel',
         'objectcr.ImportWindow'
     ],

    mainView: 'bankstatement.PaymentOrderPanel',
    mainViewSelector: 'paymentOrderPanel',

    refs: [
        {
            ref: 'PaymentOrderFilterPanel',
            selector: '#paymentOrderFilterPanel'
        },
        {
            ref: 'mainView',
            selector: 'paymentOrderPanel'
        }
    ],

    aspects: [
        {
            xtype: 'paymentorderperm'
        },
        {
            /*
            * Аспект взаимодействия грида и формы редактировани платежных поручений
            */
            xtype: 'grideditwindowaspect',
            name: 'paymentOrderGridWindowAspect',
            gridSelector: 'paymentordergrid',
            editFormSelector: '#paymentOrderEditWindow',
            storeName: 'bankstatement.PaymentOrder',
            modelName: 'bankstatement.PaymentOrder',
            editWindowView: 'bankstatement.PaymentOrderEditWindow',
            otherActions: function (actions) {
                actions[this.editFormSelector + ' #chkbxRepeatSend'] = { 'change': { fn: this.onChangeRepeatSend, scope: this } };
                actions['#paymentOrderFilterPanel #sfPeriod'] = { 'change': { fn: this.onChangePeriod, scope: this } };
            },
            onChangePeriod: function (checkbox) {
                this.controller.getStore('bankstatement.PaymentOrder').load();
            },
            onChangeRepeatSend: function (checkbox) {
                var editForm = this.getForm();
                if (checkbox.checked) {
                    editForm.down('#dcmfRedirectFunds').enable();
                } else {
                    editForm.down('#dcmfRedirectFunds').disable();
                }
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    if (record.get('TypePaymentOrder') == 10) {
                        asp.getForm().title = "Платежное поручение (приход)";
                    }
                    if (record.get('TypePaymentOrder') == 20) {
                        asp.getForm().title = "Платежное поручение (расход)";
                    }
                }
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'managingOrganizationButtonExportAspect',
            gridSelector: 'paymentordergrid',
            buttonSelector: 'paymentordergrid #btnExport',
            controllerName: 'BasePaymentOrder',
            actionName: 'Export'
        },
        {
            /*
            *аспект для импорта
            */
            xtype: 'gkhbuttonimportaspect',
            name: 'paymentOrderImportAspect',
            buttonSelector: 'paymentordergrid #btnImport',
            codeImport: 'PaymentOrder',
            windowImportView: 'objectcr.ImportWindow',
            windowImportSelector: '#importObjectCrWindow',
            refreshData: function (importId) {
                if (importId == 'PaymentOrderBankStatementImport') {
                    this.controller.getStore('bankstatement.PaymentOrder').load();
                }
            }
        }
    ],
    
    init: function () {
        this.getStore('bankstatement.PaymentOrder').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('paymentOrderPanel');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('bankstatement.PaymentOrder').load();
        this.getAspect('paymentOrderImportAspect').loadImportStore();
    },
    
    onBeforeLoad: function (store, operation) {
        var filterPanel = this.getPaymentOrderFilterPanel();
        if (filterPanel) {
            operation.params.periodId = filterPanel.down('#sfPeriod').getValue();
        }
    }
});