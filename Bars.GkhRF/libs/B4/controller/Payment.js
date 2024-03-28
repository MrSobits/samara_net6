Ext.define('B4.controller.Payment', {
/*
* Контроллер раздела оплат
*/
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.permission.Payment',
        'B4.aspects.GkhButtonImportAspect',
        'B4.aspects.ButtonDataExport'
    ],

    controllers: ['B4.controller.PaymentNavigation'],
    models: ['Payment'],
    stores: ['Payment'],
    
    views: [
        'payment.AddWindow',
        'payment.Grid',
        'payment.ImportWindow'
    ],

    aspects: [
        {
            xtype: 'paymentrfperm'
        },
        {
            /*
            *аспект для импорта
            */
            xtype: 'gkhbuttonimportaspect',
            name: 'paymentImportAspect',
            buttonSelector: 'paymentGrid #btnImport',
            codeImport: 'Erc',
            windowImportView: 'payment.ImportWindow',
            windowImportSelector: '#importPaymentWindow',
            maxFileSize: 5242880,
            getUserParams: function () {
                this.params = this.params || {};
                this.params.Resave = this.windowImport.down('#chbResave').checked;
            }
        },
        {
            /*
            аспект взаимодействия таблицы оплат и панели редактирования
            */
            xtype: 'gkhgrideditformaspect',
            name: 'paymentGridWindowAspect',
            gridSelector: 'paymentGrid',
            editFormSelector: '#paymentAddWindow',
            storeName: 'Payment',
            modelName: 'Payment',
            editWindowView: 'payment.AddWindow',
            controllerEditName: 'B4.controller.PaymentNavigation',
            otherActions: function (actions) {
                actions[this.gridSelector + ' #cbShowSum'] = { 'change': { fn: this.onChangeSum, scope: this } };
            },
            onChangeSum: function (checkbox) {
                var grid = this.getGrid(this.gridSelector);
                
                grid.columns[3].setVisible(checkbox.checked);
                grid.columns[4].setVisible(checkbox.checked);
                grid.columns[5].setVisible(checkbox.checked);
                grid.columns[6].setVisible(checkbox.checked);
                grid.columns[7].setVisible(checkbox.checked);
                grid.columns[8].setVisible(checkbox.checked);
                grid.columns[9].setVisible(checkbox.checked);
                grid.columns[10].setVisible(checkbox.checked);
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'PaymentButtonExportAspect',
            gridSelector: 'paymentGrid',
            buttonSelector: 'paymentGrid #btnPaymentExport',
            controllerName: 'Payment',
            actionName: 'Export'
        }
    ],
    
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context:'B4.mixins.Context'
    },

    mainView: 'payment.Grid',
    mainViewSelector: 'paymentGrid',

    refs: [ {
        ref: 'mainView',
        selector: 'paymentGrid'
    }],

    index: function () {
        var view = this.getMainView() || Ext.widget('paymentGrid');

        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('Payment').load();
        this.getAspect('paymentImportAspect').loadImportStore();
    }
});