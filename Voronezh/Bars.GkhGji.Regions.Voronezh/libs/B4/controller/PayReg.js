Ext.define('B4.controller.PayReg', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonPrintAspect'
    ],

    payREG: null,

    models: [
        'smev.PayRegRequests',
        'smev.PayRegFile',
        'smev.PayReg',
        'smev.GisGmp',
    ],
    stores: [
        'smev.PayRegFile',
        'smev.PayRegRequests',
        'smev.PayReg',
        'smev.GisGmp',
    ],
    views: [
        'payreg.Grid',
        'payreg.RequestsEditWindow',
        'payreg.RequestsWindow',
        'payreg.EditWindow',
        'payreg.RequestsGrid',
    ],
    aspects: [
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'PayRegPrintAspect',
            buttonSelector: '#payregEditWindow #btnPrint',
            codeForm: 'PayRegReport',
            getUserParams: function () {
                var param = { Id: this.controller.payREG };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'payregGridAspect',
            gridSelector: 'payreggrid',
            editFormSelector: '#payregEditWindow',
            storeName: 'smev.PayReg',
            modelName: 'smev.PayReg',
            editWindowView: 'payreg.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#payregEditWindow #btnFindGisGmp'] = { 'click': { fn: this.findgisgmp, scope: this } };
            },
            findgisgmp: function (btn) {
                var me = this;
                var form = this.getForm();
                var winParam = btn.up('#payregEditWindow');
                param1 = winParam.down('#dfSupplierBillID').getValue();
                param2 = winParam.down('#dfPurpose').getValue();
                me.mask('Поиск начисления', me.getForm());
                var result = B4.Ajax.request(B4.Url.action('FindGisGmp', 'PAYREGExecute', {
                    id: payREG, UIN: param1, purpose: param2
                }))
                    .next(function (response) {
                        var data = Ext.decode(response.responseText);
                        var grid = Ext.ComponentQuery.query('payreggrid')[0];
                        store = grid.getStore();
                        store.load();
                        model = me.getModel();
                        model.load(payREG, {
                            success: function (rec) {
                                me.setFormData(rec);
                            }
                        });
                        me.unmask();
                        Ext.Msg.alert('Сообщение', data.data);
                        return true;
                    }).error(function () {
                        me.unmask();
                    });
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    me.controller.getAspect('PayRegPrintAspect').loadReportStore();
                    payREG = record.data.Id;
                    asp.controller.payREG = record.data.Id;
                    // запрещаем выбор начисления для привязки, если платёж уже сквитирован
                    if (record.data.Reconcile == 10)
                    {
                        var dfCalculation = form.down('#dfCalculation');
                        dfCalculation.setDisabled(true);
                    }
                    // запрещаем автопривязку начисления, если начисление уже привязано
                    if (record.data.GisGmp != null)
                    {
                        var btnFindGisGmp = form.down('#btnFindGisGmp');
                        btnFindGisGmp.setDisabled(true);
                    }
                }
            }
        },
    ],

    mainView: 'payreg.Grid',
    mainViewSelector: 'payreggrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'payreggrid'
        }
        //{
        //    ref: 'payregFileInfoGrid',
        //    selector: 'payregfileinfogrid'
        //}
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
            
            'payreggrid #btnGetPayments': { click: { fn: this.openrequestseditwindow, scope: this } },
            'payreggrid #btnGetPaymentsHistory': { click: { fn: this.openrequestswindow, scope: this } },
            '#payregRequestsEditWindow #btnSendPaymentsRequest': { click: { fn: this.runexport, scope: this } },
            '#payregRequestsWindow #btnClose': { click: { fn: this.closerequestswindow, scope: this } }
            //'#payregEditWindow #btnSave': { click: { fn: this.match, scope: this } },
           
        });

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('payreggrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.PayReg').load();
    },


    openrequestseditwindow: function () {
        gisinfo = Ext.create('B4.view.payreg.RequestsEditWindow');
        gisinfo.show();
    },

    closerequestswindow: function (btn) {
        var reqWin = btn.up('#payregRequestsWindow');
        reqWin.close();
    },

    openrequestswindow: function () {
        reqinfo = Ext.create('B4.view.payreg.RequestsWindow');
        reqinfo.show();
        this.getStore('smev.PayRegRequests').load();
    },

    runexport: function (btn) {
        var me = this;
        var winParam = btn.up('#payregRequestsEditWindow');
        param1 = winParam.down('#dfGetPaymentsStartDate').getValue();
        param2 = winParam.down('#dfGetPaymentsEndDate').getValue();
        var curDate = new Date();
        if ((param1 != null) && (param2 != null)) {
            if (param2 <= curDate) {
                if (param1 < param2) {
                    me.mask('Обмен информацией со СМЭВ', this.getMainComponent());
                    var result = B4.Ajax.request(B4.Url.action('SendPaymentRequest', 'PAYREGExecute', {
                        paymentsStartDate: param1, paymentsEndDate: param2
                    }))
                        .next(function (response) {
                            me.unmask();
                            var data = Ext.decode(response.responseText);
                            Ext.Msg.alert('Сообщение', data.data);

                            return true;
                        }).error(function () {
                            me.unmask();

                        });
                    winParam.close();
                }
                else {
                    me.unmask();
                    Ext.Msg.alert('Внимание!', 'Конечная дата должна быть больше, чем начальная');
                }
            }
            else {
                me.unmask();
                Ext.Msg.alert('Внимание!', 'Конечная дата не может быть больше текущей');
            }
        } else {
            me.unmask();
            Ext.Msg.alert('Внимание!', 'Заполните все поля!');

        };
    },

    //match: function (btn) {
    //    var me = this;
    //    me.mask('Сопоставление', this.getMainComponent());
    //    debugger;
    //    var winEdit = btn.up('#payregEditWindow');
    //    var param1 = winEdit.down('#dfId').getValue();
    //    var param2 = winEdit.down('#dfCalculation').getValue();
        
    //    var result = B4.Ajax.request(B4.Url.action('Match', 'PAYREGExecute', {
    //        id: param1, gisGmpId: param2
    //    }));

    //    //Ext.Msg.alert(result.data);
    //    winEdit.close();
    //    me.unmask();
    //    Ext.Msg.alert('Сообщение', 'Успешно');
    //},
});