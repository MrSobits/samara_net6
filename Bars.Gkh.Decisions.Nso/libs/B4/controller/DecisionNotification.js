Ext.define('B4.controller.DecisionNotification', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.StateButton',
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateContextMenu',
        'B4.aspects.ButtonDataExport',
        'B4.controller.realityobj.Navigation',
        'B4.controller.realityobj.DecisionProtocol',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'RealityObjectDecisionProtocol',
        'RealityObject',
        'DecisionNotification'
    ],

    views: [
        'realityobj.decision_protocol.NskDecisionAddConfirmNotif',
        'realityobj.decision_protocol.NskDecisionEdit',
        'DecisionNotificationGrid'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'decisionnotificationgrid'
        },
        {
            ref: 'editWindow',
            selector: 'nskdecisionaddconfirmnotif'
        }
    ],

    aspects: [
        {
            xtype: 'statebuttonaspect',
            name: 'notificationstatebuttonaspect',
            stateButtonSelector: 'nskdecisionaddconfirmnotif button[name=StateButton]',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    var editFormAspect = asp.controller.getAspect('DecisionNotificationGridWindowAspect'),
                        model = asp.controller.getModel('DecisionNotification');

                    asp.setStateData(entityId, newState);

                    model.load(entityId, {
                        success: function(rec) {
                            editFormAspect.setFormData(rec);
                        }
                    });
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'DecisionNotificationGridWindowAspect',
            gridSelector: 'decisionnotificationgrid',
            editFormSelector: 'nskdecisionaddconfirmnotif',
            modelName: 'DecisionNotification',
            editWindowView: 'realityobj.decision_protocol.NskDecisionAddConfirmNotif',
            listeners: {
                aftersetformdata: function(asp, record) {
                    asp.controller.getAspect('notificationstatebuttonaspect').setStateData(record.get('Id'), record.get('State'));
                }
            }
        },
        {
            xtype: 'b4_state_contextmenu',
            stateType: 'gkh_decision_notification',
            menuSelector: 'qqq',
            gridSelector: 'decisionnotificationgrid'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'notificationstatepermissionaspect',
            editFormAspectName: 'DecisionNotificationGridWindowAspect',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                {
                    name: 'Ovrhl.RegistryNotifications.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'nskdecisionaddconfirmnotif'
                }
            ]
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'decisionNotificationButtonExportAspect',
            gridSelector: 'decisionnotificationgrid',
            buttonSelector: 'decisionnotificationgrid #btnExport',
            controllerName: 'DecisionNotification',
            actionName: 'Export',
            usePost: true
        }
    ],

    saveNotification: function(btn) {
        var frm = btn.up('nskdecisionaddconfirmnotif'),
            rec;

        frm.getForm().updateRecord();
        rec = frm.getForm().getRecord();

        frm.submit({
            url: rec.getProxy().getUrl({ action: 'update' }),
            params: {
                records: Ext.encode([rec.getData()])
            },
            success: function() {
                Ext.Msg.alert('Успешно', 'Сохранение прошло успешно!');
            },
            failure: function(form, action) {
                var json = Ext.JSON.decode(action.response.responseText);
                Ext.Msg.alert('Ошибка', json && json.message ? json.message : 'Ошибка при сохранении!');
                me.unmask();
            }
        });
    },

    init: function() {

        var me = this;

        me.control({
            'nskdecisionaddconfirmnotif b4savebutton': {
                click: { fn: me.saveNotification }
            },
            'nskdecisionaddconfirmnotif button[cmd=ToProtocol]': {
                click: { fn: me.toProtocol }
            },
            'nskdecisionaddconfirmnotif button[action=DownloadNotification]': {
                click: { fn: me.onClickDownloadNotification }
            }
        });


        this.callParent(arguments);
    },

    toProtocol: function(btn) {
        var me = this,
            win = btn.up('window'),
            realObjId = win.down('[name=RealObjId]').getValue();

        if (realObjId) {
            me.application.redirectTo(Ext.String.format('realityobjectedit/{0}', realObjId));
        }
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('decisionnotificationgrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },
    
    onClickDownloadNotification: function (btn) {
        var form = btn.up('nskdecisionaddconfirmnotif').getForm(),
            values;
        if (!form) {
            return;
        }

        values = form.getValues();

        var params = {
            Number: values.Number,
            Mu: values.Mu,
            MoSettlement: values.MoSettlement,
            Address: values.Address,
            Manage: values.Manage,
            FormFundType: values.FormFundType,
            OrgName: values.OrgName
        };
        
        var urlParams = [];
        for (var param in params) {
            urlParams.push(encodeURIComponent(param) + "=" + encodeURIComponent(params[param]));
        }

        var newUrl = Ext.urlAppend('/DecisionNotification/DownloadNotification?' + 
            urlParams.join("&"), '_dc=' + (new Date().getTime()));
        window.open(B4.Url.action(newUrl));
    }
});