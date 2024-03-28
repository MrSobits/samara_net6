Ext.define('B4.controller.GISERP', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
    ],

    gisERP: null,
    afterset: false,

    models: [
        'smev.GISERP',
        'smev.GISERPResultViolations',
        'smev.GISERPFile'
    ],
    stores: [
        'smev.GISERPFile',
        'smev.GISERPResultViolations',
        'smev.GISERP'

    ],
    views: [
        'giserp.Grid',
        'giserp.EditWindow',
        'giserp.FileInfoGrid',
        'giserp.GISERPResultViolationsGrid',
        'giserp.ViolationEditWindow'
    ],
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'giserpGridAspect',
            gridSelector: 'giserpgrid',
            editFormSelector: '#giserpEditWindow',
            storeName: 'smev.GISERP',
            modelName: 'smev.GISERP',
            editWindowView: 'giserp.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                //actions['#gisgmpEditWindow #dfPayerType'] = { 'change': { fn: this.onChangePayerType, scope: this } };
                //actions['#gisgmpEditWindow #dfTypeLicenseRequest'] = { 'change': { fn: this.onChangeTypeLicenseRequest, scope: this } };
                //actions['#gisgmpEditWindow #dfIsRF'] = { 'change': { fn: this.onChangeIsRF, scope: this } };            
                //actions['#gisgmpEditWindow #dfIdentifierType'] = { 'change': { fn: this.onChangeIdentifierType, scope: this } };
                actions['#giserpEditWindow #sendCalculateButton'] = { 'click': { fn: this.sendCalculate, scope: this } };
                actions['#giserpEditWindow #dfProtocol'] = { 'change': { fn: this.getInspectionInfo, scope: this } };
                actions['#giserpEditWindow #cbGisErpRequestType'] = { 'change': { fn: this.getCorrectionInfo, scope: this } };
                actions['#giserpEditWindow #getCalculateStatusButton'] = { 'click': { fn: this.getXmlStatus, scope: this } };
                //actions['#gisgmpEditWindow #sendPayButton'] = { 'click': { fn: this.sendPay, scope: this } };
                //actions['#gisgmpEditWindow #getPayStatusButton'] = { 'click': { fn: this.getPayStatus, scope: this } };
            },
            getXmlStatus: function (record) {
                var me = this;
                var taskId = gisERP;
                var form = this.getForm();
                var dfAnswer = form.down('#dfAnswer');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Обмен данными со СМЭВ', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('SendXML', 'GISERPExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var grid = form.down('giserpfileinfogrid'),
                            store = grid.getStore();
                        store.filter('GISERP', taskId);

                        me.unmask();

                        return true;
                    })
                        .error(function (resp) {
                            Ext.Msg.alert('Ошибка', resp.message);
                            me.unmask();
                        });

                }
            },
            getInspectionInfo: function (record) {
                var me = this;
                var form = this.getForm();
                var dfProtocol = form.down('#dfProtocol');
                if (dfProtocol.value != null) {
                    var protocolData = dfProtocol.value.Id;
                }
                var sfProsecutorOffice = form.down('#sfProsecutorOffice');
                var tfInspectionName = form.down('#tfInspectionName');
                var tfCarryoutEvents = form.down('#tfCarryoutEvents');
                var cbERPInspectionType = form.down('#cbERPInspectionType');
                var cbERPNoticeType = form.down('#cbERPNoticeType');
                var cbERPReasonType = form.down('#cbERPReasonType');
                var cbERPRiskType = form.down('#cbERPRiskType');
                var cbERPAddressType = form.down('#cbERPAddressType');
                var tfSubjectAddress = form.down('#tfSubjectAddress');
                var tfCTADDRESS = form.down('#tfCTADDRESS');
                var cbKindKND = form.down('#cbKindKND');
                var tfOKATO = form.down('#tfOKATO');
                var tfGoals = form.down('#tfGoals');

                var cbHasViolations = form.down('#cbHasViolations');
                var tfREPRESENTATIVE_POSITION = form.down('#tfREPRESENTATIVE_POSITION');
                var tfREPRESENTATIVE_FULL_NAME = form.down('#tfREPRESENTATIVE_FULL_NAME');
                var tfDURATION_HOURS = form.down('#tfDURATION_HOURS');
                var tfSTART_DATE = form.down('#tfSTART_DATE');
                var tfACT_DATE_CREATE = form.down('#tfACT_DATE_CREATE');

                if (dfProtocol.value != "") {
                    if (afterset) {
                        afterset = false;
                        var result = B4.Ajax.request(B4.Url.action('GetInspectionInfo', 'GISERPExecute', {
                            protocolData: protocolData
                        }
                        )).next(function (response) {
                            var data = Ext.decode(response.responseText);
                            if (data.data.prosOffice != null) {
                                sfProsecutorOffice.setValue(data.data.prosOffice);
                            }
                            tfInspectionName.setValue(data.data.inspectionName);
                            tfCarryoutEvents.setValue(data.data.carryoutEvents);
                            cbERPInspectionType.setValue(data.data.inspType);
                            cbERPNoticeType.setValue(data.data.noticeType);
                            cbERPReasonType.setValue(data.data.reasonType);
                            cbERPRiskType.setValue(data.data.riskType);
                            cbERPAddressType.setValue(data.data.adrtype);
                            tfSubjectAddress.setValue(data.data.subjectAddress);
                            tfCTADDRESS.setValue(data.data.ctAddress);
                            tfOKATO.setValue(data.data.oktmo);
                            tfGoals.setValue(data.data.goals);
                            cbKindKND.setValue(data.data.kindKnd);
                            cbHasViolations.setValue(data.data.HasViolations);
                            tfREPRESENTATIVE_POSITION.setValue(data.data.REPRESENTATIVE_POSITION);
                            tfREPRESENTATIVE_FULL_NAME.setValue(data.data.REPRESENTATIVE_FULL_NAME);
                            tfDURATION_HOURS.setValue(data.data.DURATION_HOURS);
                            tfSTART_DATE.setValue(data.data.START_DATE);
                            tfACT_DATE_CREATE.setValue(data.data.ACT_DATE_CREATE);
                            return true;
                        }).error(function () {

                        });
                        //  Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                    }
                    else {
                        afterset = true;
                    }
                }


            },
            getCorrectionInfo: function (record) {
                var me = this;
                var form = this.getForm();
                var dfProtocol = form.down('#dfProtocol');
                if (dfProtocol.value != null) {
                    var protocolData = dfProtocol.value.Id;
                }
                
                var cbGisErpRequestType = form.down('#cbGisErpRequestType');
                var sfProsecutorOffice = form.down('#sfProsecutorOffice');
                var tfInspectionName = form.down('#tfInspectionName');
                var tfCarryoutEvents = form.down('#tfCarryoutEvents');
                var cbERPInspectionType = form.down('#cbERPInspectionType');
                var cbERPNoticeType = form.down('#cbERPNoticeType');
                var cbERPReasonType = form.down('#cbERPReasonType');
                var cbERPRiskType = form.down('#cbERPRiskType');
                var cbERPAddressType = form.down('#cbERPAddressType');
                var tfSubjectAddress = form.down('#tfSubjectAddress');
                var cbKindKND = form.down('#cbKindKND');
                var tfOKATO = form.down('#tfOKATO');
                var tfGoals = form.down('#tfGoals');

                var cbHasViolations = form.down('#cbHasViolations');
                var tfREPRESENTATIVE_POSITION = form.down('#tfREPRESENTATIVE_POSITION');
                var tfREPRESENTATIVE_FULL_NAME = form.down('#tfREPRESENTATIVE_FULL_NAME');
                var tfDURATION_HOURS = form.down('#tfDURATION_HOURS');
                var tfSTART_DATE = form.down('#tfSTART_DATE');
                var tfACT_DATE_CREATE = form.down('#tfACT_DATE_CREATE');
                var enumValue = cbGisErpRequestType.value;
                debugger;
                if (dfProtocol.value != "" && cbGisErpRequestType.value == 2) {
                    if (afterset) {
                        afterset = false;
                        var result = B4.Ajax.request(B4.Url.action('GetInspectionInfo', 'GISERPExecute', {
                            protocolData: protocolData
                        }
                        )).next(function (response) {
                            debugger;
                            var data = Ext.decode(response.responseText);
                            if (data.data.prosOffice != null) {
                                sfProsecutorOffice.setValue(data.data.prosOffice);
                            }
                            tfInspectionName.setValue(data.data.inspectionName);
                            tfCarryoutEvents.setValue(data.data.carryoutEvents);
                            cbERPInspectionType.setValue(data.data.inspType);
                            cbERPNoticeType.setValue(data.data.noticeType);
                            cbERPReasonType.setValue(data.data.reasonType);
                            cbERPRiskType.setValue(data.data.riskType);
                            cbERPAddressType.setValue(data.data.adrtype);
                            tfSubjectAddress.setValue(data.data.subjectAddress);
                            tfOKATO.setValue(data.data.oktmo);
                            tfGoals.setValue(data.data.goals);
                            cbKindKND.setValue(data.data.kindKnd);
                            cbHasViolations.setValue(data.data.HasViolations);
                            tfREPRESENTATIVE_POSITION.setValue(data.data.REPRESENTATIVE_POSITION);
                            tfREPRESENTATIVE_FULL_NAME.setValue(data.data.REPRESENTATIVE_FULL_NAME);
                            tfDURATION_HOURS.setValue(data.data.DURATION_HOURS);
                            tfSTART_DATE.setValue(data.data.START_DATE);
                            tfACT_DATE_CREATE.setValue(data.data.ACT_DATE_CREATE);
                            return true;
                        }).error(function () {

                        });
                        //  Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                    }
                    else {
                        afterset = true;
                    }
                }


            },
            sendCalculate: function (record) {
                var me = this;
                var taskId = gisERP;
                var form = this.getForm();
                var dfAnswer = form.down('#dfAnswer');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Обмен данными со СМЭВ', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('SendInitiateRequest', 'GISERPExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var grid = form.down('giserpfileinfogrid'),
                            store = grid.getStore();
                        store.filter('GISERP', taskId);

                        me.unmask();

                        return true;
                    })
                        .error(function (resp) {
                            Ext.Msg.alert('Ошибка', resp.message);
                            me.unmask();
                        });

                }
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var disp = form.down('#dfProtocol');
                    if (disp.value != "") {
                        afterset = false;
                        disp.setDisabled(true);
                    }
                    else {
                        afterset = true;
                    }
                    gisERP = record.getId();
                    var grid = form.down('giserpfileinfogrid'),
                        store = grid.getStore();
                    store.filter('GISERP', record.getId());
                    var violgrid = form.down('giserpresultviolationsgrid'),
                        violstore = violgrid.getStore();
                    violstore.filter('GISERP', record.getId());
                }


                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'giserpViolationsGridAspect',
            gridSelector: 'giserpresultviolationsgrid',
            editFormSelector: '#giserpViolationEditWindow',
            storeName: 'smev.GISERPResultViolations',
            modelName: 'smev.GISERPResultViolations',
            editWindowView: 'giserp.ViolationEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('GISERP', gisERP);
                    }
                }
            }
        },
    ],

    mainView: 'giserp.Grid',
    mainViewSelector: 'giserpgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'giserpgrid'
        },
        {
            ref: 'giserpFileInfoGrid',
            selector: 'giserpfileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('giserpgrid');
        afterset = false;
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.GISERP').load();
    }

});