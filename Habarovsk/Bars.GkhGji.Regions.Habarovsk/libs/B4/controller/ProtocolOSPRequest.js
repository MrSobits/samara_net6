Ext.define('B4.controller.ProtocolOSPRequest', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.StateButton',
        'B4.enums.FuckingOSSState',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu',
        'B4.Ajax',
        'B4.Url'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'ProtocolOSPRequest'
    ],

    models: [
        'ProtocolOSPRequest'
    ],

    views: [
        'protocolosprequest.Grid',
        'protocolosprequest.EditWindow'
    ],

    mainView: 'protocolosprequest.Grid',
    mainViewSelector: 'protocolosprequestgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'protocolosprequestgrid'
        }
    ],

    aspects: [
        {
            /**
            * Вешаем аспект смены статуса 
            */
            xtype: 'statebuttonaspect',
            name: 'protocolosprequestCitsStateButtonAspect',
            stateButtonSelector: '#protocolOSPRequestEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {

                    //После перевода статуса необходимо обновить форму
                    //чтобы права вступили в силу
                    var model = this.controller.getModel('ProtocolOSPRequest');
                    model.load(entityId, {
                        success: function (rec) {                         
                            this.controller.getAspect('protocolOSPRequestGridAspect').setFormData(rec);
                        },
                        scope: this
                    });
                }
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'protocolosprequestButtonExportAspect',
            gridSelector: 'protocolosprequestgrid',
            buttonSelector: 'protocolosprequestgrid #btnExport',
            controllerName: 'ProtocolOSPRequestOperations',
            actionName: 'Export'
        },
        {
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'protocolosprequestGridStateTransferAspect',
            gridSelector: 'protocolosprequestgrid',
            stateType: 'oss_request',
            menuSelector: 'protocolosprequestGridStateMenu'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'protocolOSPRequestGridAspect',
            gridSelector: 'protocolosprequestgrid',
            editFormSelector: 'protocolosprequesteditwindow',
            modelName: 'ProtocolOSPRequest',
            storeName: 'ProtocolOSPRequest',
            editWindowView: 'protocolosprequest.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['protocolosprequesteditwindow #sendCalculateButton'] = { 'click': { fn: this.EGRNRequest, scope: this } };
                actions['protocolosprequesteditwindow #sendEmailButton'] = { 'click': { fn: this.SendEmail, scope: this } };
                actions['protocolosprequesteditwindow #btnCopyButtonFactAddress'] = { 'click': { fn: this.GetProtocolFile, scope: this } };
                actions['protocolosprequesteditwindow #cbFuckingOSSState'] = { 'change': { fn: this.onChangeFuckingOSSState, scope: this } };
                actions['protocolosprequestgrid #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions['protocolosprequestgrid #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };           
                actions[this.gridSelector + ' #cbShowCloseAppeals'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
            },
            onChangeFuckingOSSState: function (field, newValue) {
                var form = field.up('protocolosprequesteditwindow'),
                    taResolutionContent = form.down('#taResolutionContent');
                if (newValue == B4.enums.FuckingOSSState.No) {
                    taResolutionContent.show();
                }
                else{
                    taResolutionContent.hide();
                }

            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    me.controller.getAspect('protocolosprequestCitsStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                }
            },
            onChangeCheckbox: function (field, newValue) {
                var pasams = this.controller.params;
                this.controller.params.showCloseAppeals = newValue;
                this.controller.getStore('ProtocolOSPRequest').load();
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
            GetProtocolFile: function (btn) {
                var me = this,
                    panel = btn.up('protocolosprequesteditwindow'),
                    record = panel.getForm().getRecord();
                var recId = record.getId();
                Ext.Msg.confirm('Получение протокола', 'Система найдет подходящий протокол исходя из параметров запроса', function (result) {
                    if (result == 'yes') {
                        me.mask('Отправка ответа', B4.getBody());
                        B4.Ajax.request({
                            url: B4.Url.action('GetDocInfo', 'ProtocolOSPRequestOperations'),
                            method: 'POST',
                            timeout: 100 * 60 * 60 * 3,
                            params: {
                                docId: recId
                            }
                        }).next(function (responce) {
                            debugger;
                            var data = Ext.decode(responce.responseText);
                            me.unmask();
                            window.open(B4.Url.action('Download', 'FileUpload', { id: data.Id }));
                        }, me)
                            .error(function (result) {
                                if (result.responseData || result.message) {
                                    Ext.Msg.alert('Ошибка!', Ext.isString(result.responseData) ? result.responseData : result.message);
                                }
                                me.unmask();
                            }, me);

                    }
                }, this);
            },
            SendEmail: function (btn) {
                var me = this,
                    panel = btn.up('protocolosprequesteditwindow'),
                    record = panel.getForm().getRecord();
                var recId = record.getId();
                Ext.Msg.confirm('Отправка протокола', 'Подтвердите отправку ответа заявителю', function (result) {
                    if (result == 'yes') {
                        me.mask('Отправка ответа', B4.getBody());
                        B4.Ajax.request({
                            url: B4.Url.action('SendAnswer', 'ProtocolOSPRequestOperations'),
                            method: 'POST',
                            timeout: 100 * 60 * 60 * 3,
                            params: {
                                requestId: recId
                            }
                        }).next(function () {
                            B4.QuickMsg.msg('Внимание!', 'Ответ заявителю направлен', 'success');
                            me.unmask();
                        }, me)
                            .error(function (result) {
                                if (result.responseData || result.message) {
                                    Ext.Msg.alert('Ошибка отправки!', Ext.isString(result.responseData) ? result.responseData : result.message);
                                }
                                me.unmask();
                            }, me);

                    }
                }, this);
            },
            EGRNRequest: function (btn) {
                var me = this,
                    panel = btn.up('protocolosprequesteditwindow'),
                    record = panel.getForm().getRecord();
                var recId = record.getId();
                Ext.Msg.confirm('Запрос в ЕГРН', 'Подтвердите отправку запроса в ЕГРН', function (result) {
                    if (result == 'yes') {
                        me.mask('Отправка запроса', B4.getBody());
                        B4.Ajax.request({
                            url: B4.Url.action('SendEGRNRequest', 'SMEVEGRNExecute'),
                            method: 'POST',
                            timeout: 100 * 60 * 60 * 3,
                            params: {
                                docId: recId
                            }
                        }).next(function () {
                            B4.QuickMsg.msg('СМЭВ', 'Запрос на  размещение проверки в ЕГРН отправлен', 'success');
                            me.unmask();
                        }, me)
                            .error(function (result) {
                                if (result.responseData || result.message) {
                                    Ext.Msg.alert('Ошибка отправки запроса!', Ext.isString(result.responseData) ? result.responseData : result.message);
                                }
                                me.unmask();
                            }, me);

                    }
                }, this);
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('protocolosprequestgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.params.dateStart = view.down('#dfDateStart').getValue();
        this.params.dateEnd = view.down('#dfDateEnd').getValue();
        this.params.showCloseAppeals = view.down('#cbShowCloseAppeals').getValue();
        this.getStore('ProtocolOSPRequest').load();
    },

    onBeforeLoadDoc: function (store, operation) {
        operation.params.isFiltered = true;
        if (this.params) {
            operation.params.showCloseAppeals = this.params.showCloseAppeals;
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
        }
    },

    init: function () {
        var me = this,
            actions = {};
        me.params = {};
        //this.getStore('ProtocolOSPRequest').load();
        this.getStore('ProtocolOSPRequest').on('beforeload', this.onBeforeLoadDoc, this);
        this.callParent(arguments);
    }
});