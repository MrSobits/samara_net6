Ext.define('B4.controller.GisGmp', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
    ],

    afterset: false,
    gisGMP: null,
  
    models: [
        'smev.GisGmp',
        'smev.GisGmpFile',
        'smev.PayReg'
    ],
    stores: [
        'smev.GisGmpFile',
        'smev.GisGmp',
        'smev.PaymentsListByGisGmpId'

    ],
    views: [
        'gisgmp.Grid',
        'gisgmp.EditWindow',
        'gisgmp.FileInfoGrid',
        'gisgmp.GISGMPPaymentsGrid'
    ],
    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'GkhGji.SMEV.GISGMP.ChangeState',
                    applyTo: '#dfRequestState',
                    selector: '#gisgmpEditWindow',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'gisgmpGridAspect',
            gridSelector: 'gisgmpgrid',
            editFormSelector: '#gisgmpEditWindow',
            storeName: 'smev.GisGmp',
            modelName: 'smev.GisGmp',
            editWindowView: 'gisgmp.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                this.controller.afterset = false;
                B4.QuickMsg.msg('Сохранение', 'Данные уроешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#gisgmpEditWindow #dfPayerType'] = { 'change': { fn: this.onChangePayerType, scope: this } };
                actions['#gisgmpEditWindow #dfTypeLicenseRequest'] = { 'change': { fn: this.onChangeTypeLicenseRequest, scope: this } };
                actions['#gisgmpEditWindow #dfIsRF'] = { 'change': { fn: this.onChangeIsRF, scope: this } };            
                actions['#gisgmpEditWindow #dfIdentifierType'] = { 'change': { fn: this.onChangeIdentifierType, scope: this } };
                actions['#gisgmpEditWindow #sendCalculateButton'] = { 'click': { fn: this.sendCalculate, scope: this } };
                actions['#gisgmpEditWindow #getCalculateStatusButton'] = { 'click': { fn: this.getCalculateStatus, scope: this } };
                actions['#gisgmpEditWindow #sendPayButton'] = { 'click': { fn: this.sendPay, scope: this } };
                actions['#gisgmpEditWindow #getPayStatusButton'] = { 'click': { fn: this.getPayStatus, scope: this } };
                actions['#gisgmpEditWindow #dfProtocol'] = { 'change': { fn: this.getPayerInfo, scope: this } };
            },
            getPayerInfo: function (record) {
                var me = this;
                var form = this.getForm();
                var dfProtocol = form.down('#dfProtocol');
                if (dfProtocol.value != null) {
                    var protocolData = dfProtocol.value.Id;
                }
                var dfPayerType = form.down('#dfPayerType');
                var dfKBK = form.down('#dfKBK');
                var dfOKTMO = form.down('#dfOKTMO');
                var dfTotalAmount = form.down('#dfTotalAmount');
                var dfBillFor = form.down('#dfBillFor');
                //физлицо
                var dfPhysicalPersonDocType = form.down('#dfPhysicalPersonDocType');
                var dfDocumentSerial = form.down('#dfDocumentSerial');
                var dfDocumentNumber = form.down('#dfDocumentNumber');
                var sfGISGMPPayerStatus = form.down('#sfGISGMPPayerStatus');
                debugger;
                if (dfProtocol != null) {
                    if (this.controller.afterset) {
                        this.controller.afterset = false;
                        var result = B4.Ajax.request(B4.Url.action('GetPayerInfo', 'GISGMPExecute', {
                            protocolData: protocolData
                        }
                        )).next(function (response) {
                            var data = Ext.decode(response.responseText);
                            debugger;
                            dfPayerType.setValue(data.data.typeVal);
                            dfKBK.setValue(data.data.kbk);
                            dfOKTMO.setValue(data.data.oktmo);
                            dfTotalAmount.setValue(data.data.ammount);
                            dfBillFor.setValue(data.data.reasonVal);
                            sfGISGMPPayerStatus.setValue(data.data.payerstate);
                            if (data.data.typeVal == B4.enums.PayerType.Juridical) {
                                var dfINN = form.down('#dfINN');
                                dfINN.setValue(data.data.innVal);
                                var dfKPP = form.down('#dfKPP');
                                dfKPP.setValue(data.data.kppVal);
                                var dfINN2 = form.down('#dfINN2');
                                dfINN2.setValue('');
                            }
                            else if (data.data.typeVal == B4.enums.PayerType.IP) {
                                var dfINN = form.down('');
                                dfINN.setValue(data.data.innVal);
                                var dfKPP = form.down('');
                                dfKPP.setValue(data.data.kppVal);
                                var dfINN2 = form.down('#dfINN2');
                                dfINN2.setValue(data.data.innVal);
                            }
                            else if (data.data.typeVal == B4.enums.PayerType.Physical) {
                                dfPhysicalPersonDocType.setValue(data.data.doctype);
                                dfDocumentSerial.setValue(data.data.seriesfl);
                                dfDocumentNumber.setValue(data.data.numberfl);
                            }
                            return true;
                        }).error(function () {

                        });
                        //  Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                    }
                    else {
                        //this.afterset = true;
                    }
                }
            },
            onChangeIsRF: function (field, newValue) {
                var form = this.getForm(),
                dfIdentifierType = form.down('#dfIdentifierType'),
                dfKIO = form.down('#dfKIO');

                if (newValue == true)
                {
                    dfIdentifierType.setDisabled(true);
                    dfKIO.setDisabled(true);
                }
                else
                {
                    dfIdentifierType.setDisabled(false);
                }
            },
            onChangeIdentifierType: function (field, newValue) {
                var form = this.getForm(),
                dfKIO = form.down('#dfKIO');

                if (newValue == B4.enums.IdentifierType.INN)
                {
                    dfKIO.setDisabled(true);
                }
                else
                {
                    dfKIO.setDisabled(false);
                }
            },
            onChangePayerType: function (field, newValue) {
                var form = this.getForm(),
                    //dfIdentifierType = form.down('#dfIdentifierType'),
                    //dfFLDocType = form.down('#dfFLDocType'),
                    dfINN = form.down('#dfINN'),
                    dfINN2 = form.down('#dfINN2'),
                    //dfDocumentSerial = form.down('#dfDocumentSerial'),
                    //dfKPP = form.down('#dfKPP'),
                    //dfDocumentNumber = form.down('#dfDocumentNumber'),
                    //dfKIO = form.down('#dfKIO')
                    fsUrParams = form.down('#fsUrParams'),
                    fsFizParams = form.down('#fsFizParams'),
                    fsIpParams = form.down('#fsIpParams')
                    ;
                   
                if (newValue == B4.enums.PayerType.Physical) {
                    fsFizParams.show();
                    fsFizParams.setDisabled(false);
                    fsUrParams.hide();
                    fsUrParams.setDisabled(true);
                    fsIpParams.hide();
                    fsIpParams.setDisabled(true);
                    //dfFLDocType.setDisabled(false);
                    //dfDocumentSerial.setDisabled(false);
                    //dfDocumentNumber.setDisabled(false);
                    //dfFLDocType.allowBlank = false;
                    //dfDocumentSerial.allowBlank = false;
                    //dfDocumentNumber.allowBlank = false;
                    //dfKIO.setDisabled(true);
                    //dfKPP.setDisabled(true);
                    //dfINN.setDisabled(true);
                    //dfIdentifierType.setDisabled(true);
                    //dfIdentifierType.setValue(null);
                    //dfINN.setValue(null);
                    //dfKPP.setValue(null);
                    //dfKIO.setValue(null);
                }
                else if (newValue == B4.enums.PayerType.Juridical) {
                    fsFizParams.hide();
                    fsFizParams.setDisabled(true);
                    fsUrParams.show();
                    fsUrParams.setDisabled(false);
                    fsIpParams.hide();
                    fsIpParams.setDisabled(true);
                    //dfFLDocType.setDisabled(true);
                    //dfDocumentSerial.setDisabled(true);
                    //dfDocumentNumber.setDisabled(true);
                    //dfIdentifierType.allowBlank = false;
                    //dfDocumentSerial.allowBlank = true;
                    //dfDocumentNumber.allowBlank = true;
                    //dfKIO.setDisabled(false);
                    //dfKPP.setDisabled(false);
                    //dfINN.setDisabled(false);
                    //dfIdentifierType.setDisabled(false);
                    //dfFLDocType.setValue(null);
                    //dfDocumentSerial.setValue(null);
                    //dfDocumentNumber.setValue(null);
                    //dfINN.regex = /^(\d{10})$/;
                }
                else  {
                    fsFizParams.hide();
                    fsFizParams.setDisabled(true);
                    fsUrParams.hide();
                    fsUrParams.setDisabled(true);
                    fsIpParams.show();
                    fsIpParams.setDisabled(false);

                    dfINN2.setValue(dfINN.getValue());

                    //dfFLDocType.setDisabled(true);
                    //dfDocumentSerial.setDisabled(true);
                    //dfDocumentNumber.setDisabled(true);
                    //dfINN.allowBlank = false;
                    //dfKIO.setDisabled(true);
                    //dfKPP.setDisabled(true);
                    //dfINN.setDisabled(false);
                    //dfIdentifierType.setDisabled(true);
                    //dfDocumentSerial.allowBlank = true;
                    //dfDocumentNumber.allowBlank = true;
                    //dfFLDocType.allowBlank = true;
                    //dfKIO.allowBlank = true;
                    //dfKPP.allowBlank = true;
                    //dfIdentifierType.setValue(null);
                    //dfFLDocType.setValue(null);
                    //dfDocumentSerial.setValue(null);
                    //dfKPP.setValue(null);
                    //dfDocumentNumber.setValue(null);
                    //dfKIO.setValue(null);
                    //dfINN.regex = /^(\d{12})$/;
                }
            },

            onChangeTypeLicenseRequest: function (field, newValue) {
                var form = this.getForm(),
                    dfBillFor = form.down('#dfBillFor'),
                    dfProtocol = form.down('#dfProtocol'),
                    dfLicenseReissuance = form.down('#dfLicenseReissuance'),   
                    dfManOrgLicenseRequest = form.down('#dfManOrgLicenseRequest');

                if (newValue == B4.enums.TypeLicenseRequest.First) {
                    dfManOrgLicenseRequest.show();
                    dfManOrgLicenseRequest.setDisabled(false);

                    dfProtocol.hide();
                    dfProtocol.setDisabled(true);

                    dfLicenseReissuance.hide();
                    dfLicenseReissuance.setDisabled(true);

                    dfBillFor.setValue("Пошлина за выдачу лицензии");
                
                   
                }
                else if (newValue == B4.enums.TypeLicenseRequest.Reissuance || newValue == B4.enums.TypeLicenseRequest.Copy) {
                    dfLicenseReissuance.show();
                    dfLicenseReissuance.setDisabled(false);

                    dfProtocol.hide();
                    dfProtocol.setDisabled(true);

                    dfManOrgLicenseRequest.hide();
                    dfManOrgLicenseRequest.setDisabled(true);
                    if (newValue == B4.enums.TypeLicenseRequest.Reissuance)
                    {
                        dfBillFor.setValue("Пошлина за переоформление лицензии");
                    }
                    if (newValue == B4.enums.TypeLicenseRequest.Copy)
                    {
                        dfBillFor.setValue("Пошлина за выдачу дубликата лицензии");
                    }
                }
                else {
                    dfProtocol.show();
                    dfProtocol.setDisabled(false);

                    dfManOrgLicenseRequest.hide();
                    dfManOrgLicenseRequest.setDisabled(true);

                    dfLicenseReissuance.hide();
                    dfLicenseReissuance.setDisabled(true);

                    dfBillFor.setValue("Административный штраф");
                }
            },

            sendCalculate: function (record) {
                var me = this;
                var taskId = gisGMP;
                var form = this.getForm();
                var dfAnswer = form.down('#dfAnswer');
               
               if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
               else {
                   me.mask('Обмен данными с ГИС ГМП', me.getForm());
                   var result = B4.Ajax.request(B4.Url.action('SendCalcRequest', 'GISGMPExecute', {
                        taskId: taskId
                    }
                   )).next(function (response)
                   {           
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var grid = form.down('gisgmpfileinfogrid'),
                        store = grid.getStore();
                        store.filter('GisGmp', taskId);

                        me.unmask();

                        return true;
                   })
                   .error(function (resp){
                       Ext.Msg.alert('Ошибка', resp.message);
                       me.unmask();                           
                   });

                }
            },
            getCalculateStatus: function (record) {
                var me = this;
                var taskId = gisGMP;
                var form = this.getForm();
                //var dfAnswerGet = form.down('#dfAnswerGet');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Обмен данными с ГИС ГМП', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('CheckAnswer', 'GISGMPExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {                     

                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        //var grid = form.down('gisgmpfileinfogrid'),
                        //store = grid.getStore();
                        //store.filter('GisGmp', taskId);
                        //dfAnswerGet.setValue(data.data);

                        me.unmask();

                        return true;
                    }).error(function (resp) {
                        Ext.Msg.alert('Ошибка', resp.message);
                        me.unmask();   
                    });
                }
            },
            sendPay: function (record) {
                var me = this;
                var taskId = gisGMP;
                var form = this.getForm();
                var dfAnswer = form.down('#dfAnswer');
               
               if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
               else {
                   me.mask('Обмен данными с ГИС ГМП', me.getForm());
                   var result = B4.Ajax.request(B4.Url.action('SendPayRequest', 'GISGMPExecute', {
                        taskId: taskId
                    }
                   )).next(function (response)
                   {                     
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var grid = form.down('gisgmpfileinfogrid'),
                        store = grid.getStore();
                        store.filter('GisGmp', taskId);

                        me.unmask();

                        return true;
                   })
                   .error(function (resp){
                       Ext.Msg.alert('Ошибка', resp.message);
                       me.unmask();                           
                   });

                }
            },
            getPayStatus: function (record) {
                var me = this;
                var taskId = gisGMP;
                var form = this.getForm();
                //var dfAnswerGet = form.down('#dfAnswerGet');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Обмен данными с ГИС ГМП', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('CheckAnswer', 'GISGMPExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {                     

                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        //var grid = form.down('gisgmpfileinfogrid'),
                        //store = grid.getStore();
                        //store.filter('GisGmp', taskId);
                        //dfAnswerGet.setValue(data.data);

                        me.unmask();

                        return true;
                    }).error(function (resp) {
                        Ext.Msg.alert('Ошибка', resp.message);
                        me.unmask();   
                    });
                }
            },
            check: function (record) {
                var me = this;
                var taskId = gisGMP;
                var form = this.getForm();
                //var dfAnswerGet = form.down('#dfAnswer');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Запрос статуса', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('GetPaketStatus', 'GISGMPExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        //var data = Ext.decode(response.responseText);
                        //var grid = form.down('gisgmpfileinfogrid'),
                        //store = grid.getStore();
                        //store.filter('GisGmp', taskId);

                        //dfAnswerGet.setValue(data.data.answer);
                        return true;
                        }).error(function (resp) {
                            Ext.Msg.alert('Ошибка', resp.message);
                            me.unmask(); 
                    });

                }
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    debugger;
                    this.controller.afterset = true;
                    gisGMP = record.getId();
                    var grid = form.down('gisgmpfileinfogrid'),
                    store = grid.getStore();
                    store.filter('GisGmp', record.getId());

                    var paygrid = form.down('gisgmppaymentsgrid'),
                        paystore = paygrid.getStore();
                    paystore.filter('GisGmpId', gisGMP);
                   
                },
                beforesetformdata: function (me, record) {
                    this.controller.afterset = false;
                }
              
             
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        }
    ],

    mainView: 'gisgmp.Grid',
    mainViewSelector: 'gisgmpgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'gisgmpgrid'
        },
        {
            ref: 'gisgmpFileInfoGrid',
            selector: 'gisgmpfileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.afterset = false;
        this.control({
           
            'gisgmppaymentsgrid actioncolumn[action="reconsile"]': { click: { fn: this.runexport, scope: this } },
            'gisgmppaymentsgrid #btnReconcileAll': { click: { fn: this.reconcileAll, scope: this } },

        });

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('gisgmpgrid');
        this.bindContext(view);
        this.afterset = false;
        this.application.deployView(view);
        this.getStore('smev.GisGmp').load();
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        debugger;
        if (rec.get('Reconcile') == 10) {
            Ext.Msg.alert('Внимание', 'Оплата сквитирована, повторный запуск невозможен');
        }
        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());
        //Ext.Msg.alert('Внимание!', 'Запуск экспорта.');

            var result = B4.Ajax.request(B4.Url.action('SendReconcileRequest', 'GISGMPExecute', {
                taskId: rec.getId()
            }
            ))
          .next(function (response) {
            me.unmask();
            var data = Ext.decode(response.responseText);
            Ext.Msg.alert('Сообщение', data.data);
           
            return true;
        }).error(function () {
            me.unmask();
            
        });
    },

    reconcileAll: function () {
        var me = this;
        //var grid = btn.up('gisgmppaymentsgrid');
        //var editW = grid.up('#gisgmpEditWindow');
        //var gisGmp = Ext.get(editW);
        //if (rec.get('Reconcile') == 10) {
        //    Ext.Msg.alert('Внимание', 'Оплата сквитирована, повторный запуск невозможен');
        //}
        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());
        //Ext.Msg.alert('Внимание!', 'Запуск экспорта.');
        
        var result = B4.Ajax.request(B4.Url.action('SendReconcileAllRequest', 'GISGMPExecute', {
            taskId: gisGMP
        }
        ))
            .next(function (response) {
                me.unmask();
                Ext.Msg.alert('Сообщение', response.message);
                return true;
            }).error(function (response) {
                debugger;
                me.unmask();
                Ext.Msg.alert('Сообщение', response.message);
            });
    },
});