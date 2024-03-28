Ext.define('B4.controller.realityobj.DecisionProtocol', {
    extend: 'B4.controller.MenuItemController',
    params: {},

    requires: [
        'B4.aspects.StatusButton',
        'B4.mixins.Context',
        'B4.aspects.StateContextButton',
        'B4.aspects.GridEditWindow',
        'B4.aspects.EntityHistoryFieldWindow',
        'B4.aspects.StateContextMenu',
        'B4.form.ComboBox',
        'B4.view.realityobj.decision_protocol.ProtocolPanel',
        'B4.view.realityobj.decision_protocol.ProtocolEdit',
        'B4.view.realityobj.decision_protocol.DecisionEdit',
        'B4.view.realityobj.decision_protocol.DecisionGrid',
        'B4.view.realityobj.decision_protocol.NskDecisionEdit',
        'B4.view.realityobj.decision_protocol.NskDecisionAddConfirm',
        'B4.enums.CrFundFormationDecisionType',
        'B4.enums.EntityHistoryType',
        'B4.enums.AccountOwnerDecisionType',
        'B4.Ajax',
        'B4.Url',
        'B4.QuickMsg',
        'B4.model.DecisionNotification',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.FieldRequirementAspect',
        'B4.enums.CoreDecisionType'
    ],

    views: [
        'realityobj.decision_protocol.ProtocolPanel',
        'realityobj.decision_protocol.ProtocolEdit',
        'realityobj.decision_protocol.DecisionEdit',
        'realityobj.decision_protocol.DecisionGrid'
    ],

    models: [
        'RealityObjectDecisionProtocol',
        'GenericDecision',
        'DecisionNotification',
        'CoreDecisionInfo',
        'GovDecision'
    ],

    stores: [
        'realityobj.decision_protocol.Protocol',
        'realityobj.decision_protocol.Decision',
        'realityobj.decision_protocol.CoreDecisionProtocol',
        'realityobj.GovDecision'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    parentCtrlCls: 'B4.controller.realityobj.Navi',

    avaliableTypes: null,

    mainViewSelector: 'protocolpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'protocolpanel'
        },
        {
            ref: 'mainGrid',
            selector: 'protocolgrid'
        },
        {
            ref: 'decisionGrid',
            selector: 'protodecisiongrid'
        },
        {
            ref: 'showNonActiveCheckbox',
            selector: 'protodecisiongrid checkbox[name=showNonActive]'
        },
        {
            ref: 'protocolEdit',
            selector: 'protocoledit'
        },
        {
            ref: 'protocolIdField',
            selector: 'nskdecisionedit hiddenfield[fname=Protocol]'
        },
        {
            ref: 'decisionButton',
            selector: 'button[action=nskdecision]'
        },
        {
            ref: 'protocolDateField',
            selector: 'datefield[name=ProtocolDate]'
        },
        {
            ref: 'protocolStateBtn',
            selector: '#protocolStateBtn'
        },
        {
            ref: 'protocolDecisionForm',
            selector: 'protocoldecision'
        },
        {
            ref: 'nskDecisionEdit',
            selector: 'nskdecisionedit'
        },
        {
            ref: 'createNotifBtn',
            selector: '#formconfirmbtn'
        },
        {
            ref: 'downloadContrBtn',
            selector: '#downloadContract'
        },
        {
            ref: 'win',
            selector: 'nskdecisionedit'
        },
        {
            ref: 'minFundField',
            selector: 'nskdecisionedit textfield[name=MinFund]'
        },
        {
            ref: 'fundFormatField',
            selector: 'nskdecisionedit checkbox[name=FundFormationByRegop]'
        }
    ],

    aspects: [
        {
            xtype: 'statusbuttonaspect',
            name: 'protocolOwnerStateBtnAspect',
            stateButtonSelector: 'nskdecisionedit button[owner=0]',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    var //editWindowAspect = asp.controller.getAspect('protocolDecisionAspect'),
                        permissionAspect = asp.controller.getAspect('protocolStatePermissionAspect'),
                        model = asp.controller.getModel('RealityObjectDecisionProtocol');

                    if (!newState.FinalState) {
                        B4.QuickMsg.msg('Внимание!', 'Внесенные правки могут повлиять на изменение программы ДПКР/КПКР/начислений', 'warning', 5000);
                    }

                    asp.setStateData(entityId, newState);
                    asp.controller.getProtocolDecisionForm().state = newState;

                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            //editWindowAspect.setFormData(rec);
                            permissionAspect.setPermissionsByRecord(rec);
                        },
                        scope: asp
                    }) : permissionAspect.setPermissionsByRecord(new model({ Id: 0 }));
                }
            },
            changeState: function (newStateId) {
                var asp = this,
                    id = asp.entityId,
                    grid = asp.controller.getMainGrid(),
                    decisProtocolWin = asp.controller.getNskDecisionEdit();

                var doChangeState = function () {
                    asp.controller.mask('Изменение статуса', decisProtocolWin);
                    B4.Ajax.request({
                        method: 'GET',
                        url: B4.Url.action('/StateTransfer/StateChange'),
                        params: {
                            newStateId: newStateId,
                            entityId: id,
                            description: asp.emptyDescription
                        },
                        timeout: 9999999
                    }).next(function (response) {
                        //апдейтим грид
                        grid.getStore().load();
                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText);
                        asp.controller.unmask();

                        if (obj.success) {
                            B4.QuickMsg.msg('Смена статуса', 'Статус переведен успешно', 'success');

                            asp.fireEvent('transfersuccess', asp, asp.entityId, obj.newState);
                        } else {
                            Ext.Msg.alert('Ошибка!', obj.message);
                        }
                    }).error(function (result) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                    });
                };

                var protocolFile = Ext.ComponentQuery.query('protocoldecision b4filefield');

                if (protocolFile.length > 0) {
                    if (!protocolFile[0].value) {
                        Ext.Msg.confirm('Отсутствует файл протокола!', 'Файл протокола не добавлен. Сменить статус?', function (result) {
                            if (result == 'yes') {
                                doChangeState();
                            }
                        }, asp);
                    } else {
                        doChangeState();
                    }
                }
            }
        },
        {
            xtype: 'statusbuttonaspect',
            name: 'protocolGovStateBtnAspect',
            stateButtonSelector: 'nskdecisionedit button[owner=1]',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    var //editWindowAspect = asp.controller.getAspect('protocolDecisionAspect'),
                        permissionAspect = asp.controller.getAspect('editGovProtocolDecisionStatePerm'),
                        model = asp.controller.getModel('GovDecision');

                    if (!newState.FinalState) {
                        B4.QuickMsg.msg('Внимание!', 'Внесенные правки могут повлиять на изменение программы ДПКР/КПКР/начислений', 'warning', 5000);
                    }

                    asp.setStateData(entityId, newState);
                    asp.controller.getProtocolDecisionForm().state = newState;

                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            //editWindowAspect.setFormData(rec);
                            permissionAspect.setPermissionsByRecord(rec);
                        },
                        scope: this
                    }) : permissionAspect.setPermissionsByRecord(new model({ Id: 0 }));
                }
            },
            changeState: function (newStateId) {
                var asp = this,
                    id = asp.entityId,
                    grid = asp.controller.getMainGrid(),
                    decisProtocolWin = asp.controller.getNskDecisionEdit();

                var doChangeState = function () {
                    asp.controller.mask('Изменение статуса', decisProtocolWin);

                    B4.Ajax.request({
                        method: 'GET',
                        url: B4.Url.action('/StateTransfer/StateChange'),
                        params: {
                            newStateId: newStateId,
                            entityId: id,
                            description: asp.emptyDescription
                        }
                    }).next(function (response) {
                        grid.getStore().load();
                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText);
                        asp.controller.unmask();

                        if (obj.success) {
                            B4.QuickMsg.msg('Смена статуса', 'Статус переведен успешно', 'success');

                            asp.fireEvent('transfersuccess', asp, asp.entityId, obj.newState); 
                        } else {
                            Ext.Msg.alert('Ошибка!', obj.message);
                        }
                    }).error(function (result) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                    });
                };

                var protocolFile = Ext.ComponentQuery.query('nskdecisionedit #decisProtocol');

                if (protocolFile.length > 0) {
                    if (!protocolFile[0].value) {

                        Ext.Msg.confirm('Отсутствует файл протокола!', 'Файл протокола не добавлен. Сменить статус?', function (result) {
                            if (result == 'yes') {
                                doChangeState();
                            }
                        }, asp);
                    } else {
                        doChangeState();
                    }
                }
            }
        },
        {
            xtype: 'statusbuttonaspect',
            name: 'protocolOtherStateBtnAspect',
            stateButtonSelector: 'nskdecisionedit button[owner=2]',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    var permissionAspect = asp.controller.getAspect('protocolStatePermissionAspect'),
                        model = asp.controller.getModel('RealityObjectDecisionProtocol');

                    if (!newState.FinalState) {
                        B4.QuickMsg.msg('Внимание!', 'Внесенные правки могут повлиять на изменение программы ДПКР/КПКР/начислений', 'warning', 5000);
                    }

                    asp.setStateData(entityId, newState);
                    asp.controller.getProtocolDecisionForm().state = newState;

                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            permissionAspect.setPermissionsByRecord(rec);
                        },
                        scope: this
                    }) : permissionAspect.setPermissionsByRecord(new model({ Id: 0 }));
                }
            },
            changeState: function (newStateId) {
                var asp = this,
                    id = asp.entityId,
                    grid = asp.controller.getMainGrid(),
                    decisProtocolWin = asp.controller.getNskDecisionEdit();

                var doChangeState = function () {
                    asp.controller.mask('Изменение статуса', decisProtocolWin);

                    B4.Ajax.request({
                        method: 'GET',
                        url: B4.Url.action('/StateTransfer/StateChange'),
                        params: {
                            newStateId: newStateId,
                            entityId: id,
                            description: asp.emptyDescription
                        }
                    }).next(function (response) {
                        grid.getStore().load();
                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText);
                        asp.controller.unmask();

                        if (obj.success) {
                            B4.QuickMsg.msg('Смена статуса', 'Статус переведен успешно', 'success');

                            asp.fireEvent('transfersuccess', asp, asp.entityId, obj.newState);
                        } else {
                            Ext.Msg.alert('Ошибка!', obj.message);
                        }
                    }).error(function (result) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                    });
                };

                var protocolFile = Ext.ComponentQuery.query('nskdecisionedit #decisProtocol');

                if (protocolFile.length > 0) {
                    if (!protocolFile[0].value) {

                        Ext.Msg.confirm('Отсутствует файл протокола!', 'Файл протокола не добавлен. Сменить статус?', function (result) {
                            if (result == 'yes') {
                                doChangeState();
                            }
                        }, asp);
                    } else {
                        doChangeState();
                    }
                }
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'protocolStatePermissionAspect',
            //editFormAspectName: 'protocolDecisionAspect',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                {
                    name: 'Gkh.RealityObject.Register.DecisionProtocolsEditDelete.CreateNotification',
                    applyTo: '#formconfirmbtn',
                    selector: 'nskdecisionedit'
                },
                {
                    name: 'Gkh.RealityObject.Register.DecisionProtocolsEditDelete.DownloadContract',
                    applyTo: '#downloadContract',
                    selector: 'nskdecisionedit',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'Gkh.RealityObject.Register.DecisionProtocolsEditDelete.Delete' }],
            name: 'deleteDecisionProtocolStatePerm',
            loadPermissions: function (rec) {
                var me = this,
                    id = rec.get('Id');
                return B4.Ajax.request({
                    url: B4.Url.action('/Permission/GetObjectSpecificPermissions'),
                    params: {
                        permissions: Ext.encode(me.collectPermissions()),
                        ids: Ext.encode([id])
                    }
                });
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'Gkh.RealityObject.Register.GovProtocolDecisionEditDelete.Delete' }],
            name: 'deleteGovProtocolDecisionStatePerm',
            loadPermissions: function (rec) {
                var me = this,
                    id = rec.get('Id');
                return B4.Ajax.request({
                    url: B4.Url.action('/Permission/GetObjectSpecificPermissions'),
                    params: {
                        permissions: Ext.encode(me.collectPermissions()),
                        ids: Ext.encode([id])
                    }
                });
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'Gkh.RealityObject.Register.CrFundDecisionProtocol.Delete' }],
            name: 'deleteCrFundDecisionProtocolStatePerm',
            loadPermissions: function (rec) {
                var me = this,
                    id = rec.get('Id');
                return B4.Ajax.request({
                    url: B4.Url.action('/Permission/GetObjectSpecificPermissions'),
                    params: {
                        permissions: Ext.encode(me.collectPermissions()),
                        ids: Ext.encode([id])
                    }
                });
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'Gkh.RealityObject.Register.ManagementOrganizationDecisionProtocol.Delete' }],
            name: 'deleteManagementOrganizationDecisionProtocolStatePerm',
            loadPermissions: function (rec) {
                var me = this,
                    id = rec.get('Id');
                return B4.Ajax.request({
                    url: B4.Url.action('/Permission/GetObjectSpecificPermissions'),
                    params: {
                        permissions: Ext.encode(me.collectPermissions()),
                        ids: Ext.encode([id])
                    }
                });
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'Gkh.RealityObject.Register.MkdManagementTypeDecisionProtocol.Delete' }],
            name: 'deleteMkdManagementTypeDecisionProtocolStatePerm',
            loadPermissions: function (rec) {
                var me = this,
                    id = rec.get('Id');
                return B4.Ajax.request({
                    url: B4.Url.action('/Permission/GetObjectSpecificPermissions'),
                    params: {
                        permissions: Ext.encode(me.collectPermissions()),
                        ids: Ext.encode([id])
                    }
                });
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'Gkh.RealityObject.Register.OoiManagementDecisionProtocol.Delete' }],
            name: 'deleteOoiManagementDecisionProtocolStatePerm',
            loadPermissions: function (rec) {
                var me = this,
                    id = rec.get('Id');
                return B4.Ajax.request({
                    url: B4.Url.action('/Permission/GetObjectSpecificPermissions'),
                    params: {
                        permissions: Ext.encode(me.collectPermissions()),
                        ids: Ext.encode([id])
                    }
                });
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'Gkh.RealityObject.Register.TariffApprovalDecisionProtocol.Delete' }],
            name: 'deleteTariffApprovalDecisionProtocolStatePerm',
            loadPermissions: function (rec) {
                var me = this,
                    id = rec.get('Id');
                return B4.Ajax.request({
                    url: B4.Url.action('/Permission/GetObjectSpecificPermissions'),
                    params: {
                        permissions: Ext.encode(me.collectPermissions()),
                        ids: Ext.encode([id])
                    }
                });
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'editDecisionProtocolStatePerm',
            permissions: [
                {
                    name: 'Gkh.RealityObject.Register.DecisionProtocolsEditDelete.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'nskdecisionedit',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.enable();
                        } else {
                            component.disable();
                        }
                    }
                },
            ],
            loadPermissions: function (rec) {
                var me = this,
                    id = rec.getId();
                return B4.Ajax.request({
                    url: B4.Url.action('/Permission/GetObjectSpecificPermissions'),
                    params: {
                        permissions: Ext.encode(me.collectPermissions()),
                        ids: Ext.encode([id])
                    }
                });
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'editGovProtocolDecisionStatePerm',
            permissions: [
                {
                    name: 'Gkh.RealityObject.Register.GovProtocolDecisionEditDelete.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'nskdecisionedit',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.enable();
                        } else {
                            component.disable();
                        }
                    }
                }
            ],
            loadPermissions: function (rec) {
                var me = this,
                    id = rec.getId();
                return B4.Ajax.request({
                    url: B4.Url.action('/Permission/GetObjectSpecificPermissions'),
                    params: {
                        permissions: Ext.encode(me.collectPermissions()),
                        ids: Ext.encode([id])
                    }
                });
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'editCrFundDecisionProtocolStatePerm',
            permissions: [
                {
                    name: 'Gkh.RealityObject.Register.CrFundDecisionProtocol.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'nskdecisionedit',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.enable();
                        } else {
                            component.disable();
                        }
                    }
                }
            ],
            loadPermissions: function (rec) {
                var me = this,
                    id = rec.getId();
                return B4.Ajax.request({
                    url: B4.Url.action('/Permission/GetObjectSpecificPermissions'),
                    params: {
                        permissions: Ext.encode(me.collectPermissions()),
                        ids: Ext.encode([id])
                    }
                });
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'editManagementOrganizationDecisionProtocolStatePerm',
            permissions: [
                {
                    name: 'Gkh.RealityObject.Register.ManagementOrganizationDecisionProtocol.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'nskdecisionedit',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.enable();
                        } else {
                            component.disable();
                        }
                    }
                }
            ],
            loadPermissions: function (rec) {
                var me = this,
                    id = rec.getId();
                return B4.Ajax.request({
                    url: B4.Url.action('/Permission/GetObjectSpecificPermissions'),
                    params: {
                        permissions: Ext.encode(me.collectPermissions()),
                        ids: Ext.encode([id])
                    }
                });
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'editMkdManagementTypeDecisionProtocolStatePerm',
            permissions: [
                {
                    name: 'Gkh.RealityObject.Register.MkdManagementTypeDecisionProtocol.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'nskdecisionedit',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.enable();
                        } else {
                            component.disable();
                        }
                    }
                }
            ],
            loadPermissions: function (rec) {
                var me = this,
                    id = rec.getId();
                return B4.Ajax.request({
                    url: B4.Url.action('/Permission/GetObjectSpecificPermissions'),
                    params: {
                        permissions: Ext.encode(me.collectPermissions()),
                        ids: Ext.encode([id])
                    }
                });
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'editOoiManagementDecisionProtocolStatePerm',
            permissions: [
                {
                    name: 'Gkh.RealityObject.Register.OoiManagementDecisionProtocol.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'nskdecisionedit',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.enable();
                        } else {
                            component.disable();
                        }
                    }
                }
            ],
            loadPermissions: function (rec) {
                var me = this,
                    id = rec.getId();
                return B4.Ajax.request({
                    url: B4.Url.action('/Permission/GetObjectSpecificPermissions'),
                    params: {
                        permissions: Ext.encode(me.collectPermissions()),
                        ids: Ext.encode([id])
                    }
                });
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'editTariffApprovalDecisionProtocolStatePerm',
            permissions: [
                {
                    name: 'Gkh.RealityObject.Register.TariffApprovalDecisionProtocol.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'nskdecisionedit',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.enable();
                        } else {
                            component.disable();
                        }
                    }
                }
            ],
            loadPermissions: function (rec) {
                var me = this,
                    id = rec.getId();
                return B4.Ajax.request({
                    url: B4.Url.action('/Permission/GetObjectSpecificPermissions'),
                    params: {
                        permissions: Ext.encode(me.collectPermissions()),
                        ids: Ext.encode([id])
                    }
                });
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.RealityObject.Register.DecisionProtocolsViewCreate.Create', applyTo: 'b4addbutton', selector: 'protocolgrid' }
            ]
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'protocolStateTransferAspect',
            gridSelector: 'protocolgrid',
            menuSelector: 'protocolgridStateMenu',
            stateType: 'gkh_real_obj_dec_protocol',
            cellClickAction: function (grid, e, action, record, store) {
                switch (action.toLowerCase()) {
                    case 'statechange':
                        switch (record.get('ProtocolType')) {
                            case B4.enums.CoreDecisionType.Owners:
                                this.stateType = 'gkh_real_obj_dec_protocol';
                                break;
                            case B4.enums.CoreDecisionType.Government:
                                this.stateType = 'gkh_real_obj_gov_dec';
                                break;
                            case B4.enums.CoreDecisionType.CrFund:
                                this.stateType = 'gkh_real_obj_crfund_dec';
                                break;
                            case B4.enums.CoreDecisionType.MkdManagementType:
                                this.stateType = 'gkh_real_obj_mkdorg_dec';
                                break;
                            case B4.enums.CoreDecisionType.ManagementOrganization:
                                this.stateType = 'gkh_real_obj_mkdmanage_type_dec';
                                break;
                            case B4.enums.CoreDecisionType.TariffApproval:
                                this.stateType = 'gkh_real_obj_tariff_approval_dec';
                                break;
                            case B4.enums.CoreDecisionType.OoiManagement:
                                this.stateType = 'gkh_real_obj_ooi_manage_dec';
                                break;
                        }

                        e.stopEvent();
                        var menu = this.getContextMenu();
                        this.currentRecord = record;
                        menu.updateData(record, e.xy, this.stateType, this.stateProperty);

                        break;
                }
            },
            onSaveBtnClick: function (btn, e) {
                var me = this,
                    menu = me.getContextMenu(),
                    desc = menu.getFocusEl().menu.down('textareafield').getValue(),
                    stateId = menu.getFocusEl().did,
                    entityId = menu.currentRecord.get(me.entityIdProperty);

                btn.setDisabled(true);
                me.changeState(stateId, desc, entityId, btn);

                if (menu.currentRecord.get('State').FinalState) {
                    B4.QuickMsg.msg('Внимание!', 'Внесенные правки могут повлиять на изменение программы ДПКР/КПКР/начислений', 'warning', 5000);
                };
            },
            changeState: function () {
                var args = Array.prototype.slice.call(arguments);
                args.push(9999999);
                this.self.prototype.changeState.apply(this, args);
            }
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                {
                    name: 'Gkh.RealityObject.Register.DecisionProtocolsField.AccountNum',
                    applyTo: '[name=AccountNum]',
                    selector: 'nskdecisionaddconfirm'
                },
                {
                    name: 'Gkh.RealityObject.Protocols.GovDecision.Fields.MinFundPaymentSize_Rqrd',
                    applyTo: '[name=MinFundPaymentSize]',
                    selector: 'nskdecisionaddconfirm'
                }
            ]
        },
        {
            xtype: 'entityhistoryfieldwindowaspect',
            getParentId: function() {
                var me = this.controller,
                    view = me.getMainView();

                return me.getContextValue(view, 'realityObjectId');
            }
        }
    ],

    onRowAction: function (grid, action, record) {
        switch (action.toLowerCase()) {
            case 'edit':
                this.editRecord(record);
                break;
            case 'delete':
                this.deleteRecord(record);
                break;
        }
    },

    deleteRecord: function (record) {
        var me = this,
            delAction = function () {
                me.mask('Удаление..', B4.getBody().getActiveTab().getEl());
                B4.Ajax.request(B4.Url.action('Delete',
                        'RealityObjectBothProtocol',
                        {
                            protocolType: record.get('ProtocolType'),
                            id: record.get('Id')
                        })
                ).next(function () {
                    me.unmask();
                    me.getMainGrid().getStore().load();
                }).error(function (e) {
                    me.unmask();
                    B4.QuickMsg.msg('Ошибка', e.message || e.Message, 'error');
                });
            },
            protocolType = record.get('ProtocolType'),
            permissionAspect = me.getDeleletePermissionAspect(protocolType);

        if (record.get('State').FinalState) {
            B4.QuickMsg.msg('Внимание!', 'Внесенные правки могут повлиять на изменение программы ДПКР/КПКР/начислений', 'warning', 5000);
        };

        permissionAspect.loadPermissions(record)
            .next(function (response) {
                var grants = Ext.decode(response.responseText);

                if (grants && grants[0]) {
                    grants = grants[0];
                }

                // проверяем пермишшен колонки удаления
                if (grants[0] == 0) {
                    Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                    return;
                }
                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?',
                    function (result) {
                        if (result == 'yes') {
                            delAction();
                        }
                    });
            });
    },

    editRecord: function(record) {
debugger;
        var me = this,
            id = record ? record.get('Id') : 0,
            available = me.getAvailableProtocolTypes() || [B4.enums.CoreDecisionType.Owners],
            protocolType = record ? record.get('ProtocolType') : available[0],
            win = me.getProtocolWindow(),
            npaPanel = win.down('protocolnpapanel'),
            monthlyFeeView = win.down('monthlyfeedecision grid');

        win.show();

        if (monthlyFeeView) {
            monthlyFeeView.disable();
        };

        win.down('[name=ProtocolType]').setValue(protocolType);
        me.getEditPermissionAspect(protocolType)
            .setPermissionsByRecord({
                getId: function() {
                    return id;
                }
            });

        if (id) {
            me.loadProtocol(id, protocolType, win);
        }
        if (!id) {
            var gridFee = win.down('[entity=MonthlyFeeAmountDecision]');
            gridFee.disable();
        }
    },

    getProtocolWindow: function () {
        var me = this,
            win = me.getNskDecisionEdit();

        if (!win) {
            win = Ext.widget('nskdecisionedit', {
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy',
                ctxKey: me.getCurrentContextKey()
            });

            win.down('[name=ProtocolType]').on('refresh', function (field) {
                var str = field.getStore(),
                    items = str.data.items,
                    picker = field.picker,
                    callback = function (result) {
                        Ext.Array.forEach(items,
                            function (rec) {
                                if (rec && result.indexOf(rec.get('Value')) < 0) {
                                    var node = picker.getNodeByRecord(rec);
                                    node.style.display = 'none';
                                    picker.updateLayout();
                                }
                            });
                    },
                    allowedTypes = me.getAvailableProtocolTypes(callback);

                if (allowedTypes) {
                    callback(allowedTypes);
                }

            }, me);
        }

        return win;
    },

    getAvailableProtocolTypes: function (callback) {
        var me = this;

        if (me.avaliableTypes) {
            return me.avaliableTypes;
        }

        B4.Ajax.request({
            url: B4.Url.action('GetAvaliableTypes', 'RealityObjectBothProtocol'),
            method: 'GET'
        }).next(function (response) {
            me.avaliableTypes = Ext.JSON.decode(response.responseText);

            if (callback) {
                callback(me.avaliableTypes);
            }
        });

        return null;
    },

    loadProtocol: function (protocolId, protocolType, form) {
        var me = this,
            roId = me.params.realityObjectId,
            protocolField = me.getNskDecisionEdit().down('[name=ProtocolType]');

        me.mask('Загрузка...', form);

        form.protocolId = protocolId;

        B4.Ajax.request({
            url: B4.Url.action('Get', 'RealityObjectBothProtocol'),
            params: {
                protocolT: protocolType,
                protocolId: protocolId,
                roId: roId
            },
            method: 'GET'
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText);

            protocolField.readOnly = protocolId > 0;
            form.loadValues(json.data);

            if (protocolType === B4.enums.CoreDecisionType.Owners) {
                me.getAspect('protocolOwnerStateBtnAspect').setStateData(json.data.Protocol.Id, json.data.Protocol.State);
                form.down('hiddenfield[fname=Protocol]').setValue(json.data.Protocol.Id);
                form.down('hiddenfield[name=RealityObject]').setValue(me.params.realityObjectId);
            } else if (protocolType === B4.enums.CoreDecisionType.Government) {
                me.getAspect('protocolGovStateBtnAspect').setStateData(json.data.Id, json.data.State);
                form.down('form[ownerType=1]').getForm().setValues(json.data);
                form.down('protocolnpapanel').getForm().setValues(json.data);
            } else {
                me.getAspect('protocolOtherStateBtnAspect').setStateData(json.data.Id, json.data.State);
                form.down('form[ownerType=2]').getForm().setValues(json.data);
            }

            protocolField.setValue(protocolType);
            me.getAspect('protocolStatePermissionAspect').setPermissionsByRecord({ getId: function () { return protocolId; } });
            me.getEditPermissionAspect(protocolType).setPermissionsByRecord({ getId: function () { return protocolId; } });

            me.unmask();
        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', e.message || e.Message, 'error');
        });
    },

    onClickAdd: function () {
        this.editRecord();
    },

    init: function () {
        var me = this;
        me.getStore('realityobj.decision_protocol.Decision').on('beforeload', me.onBeforeDecisionStoreLoad, me);

        me.getAvailableProtocolTypes();

        me.control({
            'protocolgrid': {
                'render': function (grid) {
                    grid.getStore().on('beforeload', me.onBeforeLoad, me);
                },
                'rowaction': {
                    fn: me.onRowAction,
                    scope: me
                }
            },
            'protocolgrid b4addbutton': {
                'click': { fn: me.onClickAdd, scope: me }

            },
            'decisionedit b4combobox[name=DecisionType]': {
                'change': { fn: me.onDecisionTypeSelect, scope: me }
            },
            'protodecisiongrid checkbox[name=showNonActive]': {
                'change': { fn: me.reloadDecisionGrid, scope: me }
            },
            'button[action=nskdecision]': {
                click: { fn: me.onNskDecisionClick, scope: me }
            },
            'nskdecisionedit checkbox': {
                change: { fn: me.onNskDecisionCheckboxChange, scope: me }
            },
            'nskdecisionedit b4savebutton': {
                click: { fn: me.onSubmitBtnClick, scope: me }
            },
            'protocoledit': {
                render: { fn: me.onProtocolEditFormRender, scope: me }
            },
            'form[entity=CrFundFormationDecision] b4enumcombo': {
                change: { fn: me.onCrFundFormatDecisionSelect, scope: me }
            },
            'nskdecisionedit b4enumcombo[name=DecisionType]': {
                change: { fn: me.onAccountOwnerDecisionTypeSelect, scope: me }
            },
            'nskdecisionedit [name=ProtocolType]': {
                change: { fn: me.onChangeProtocolType, scope: me }
            },
            //При выборе "Способ формирования фонда КР" (Checked=true) Принятое решение="Специальный счет".
            'nskdecisionedit form[entity="CrFundFormationDecision"] checkbox[name="IsChecked"]': {
                change: {
                    fn: me.onChangeFundFormatDecChkBox,
                    scope: me
                }
            },
            'nskdecisionedit form[entity="CrFundFormationDecision"] b4enumcombo': {
                change: {
                    fn: me.onChangeFundFormatDecision,
                    scope: me
                }
            },
            'nskdecisionedit form[entity="AccountOwnerDecision"] checkbox[name="IsChecked"]': {
                change: {
                    fn: me.onChangeAccountOwnerCheckbox,
                    scope: me
                }
            },
            '#formconfirmbtn': {
                click: { fn: me.onFormConfirm, scope: me }
            },
            '#downloadContract': {
                click: { fn: me.onContractDownload, scope: me }
            },
            'nskdecisionaddconfirm b4savebutton': {
                click: { fn: me.saveNotification, scope: me }
            },
            'nskdecisionaddconfirm #btSend': {
                click: { fn: me.sendNotification, scope: me }
            },
            'jobyeardecision checkbox': {
                change: { fn: me.jobYearDecisionActivated, scope: me }
            },
            'monthlyfeedecision checkbox': {
                change: { fn: me.monthlyFeeDecisionActivated, scope: me }
            },
            'nskdecisionaddconfirm b4filefield[name=ProtocolFile]': {
                render: { fn: me.confirmProtocolShow, scope: me }
            },
            'penaltydelaydecision checkbox': {
                change: { fn: me.penaltyDelayActivated, scope: me }
            },
            'nskdecisionedit': {
                afterrender: {
                    fn: me.onAfterrenderEditPanel,
                    scope: me
                }
            },
        });

        me.callParent(arguments);
    },

    onAfterrenderEditPanel: function (panel) {
        var protocolTypeField = panel.down('[name=ProtocolType]');
        panel.onProtocolTypeChange(protocolTypeField, protocolTypeField.getValue());

    },

    onChangeProtocolType: function (cb, newValue) {
        this.getEditPermissionAspect(newValue).setPermissionsByRecord({ getId: function () { return cb.up('window').protocolId; } });
    },

    onChangeAccountOwnerCheckbox: function (checkbox, newValue) {
        var me = this,
            win = me.getNskDecisionEdit(),
            isCreditOrgCheckbox = win.down('form[entity=CreditOrgDecision]'),
            decisionType = win.down('form[entity=AccountOwnerDecision] b4enumcombo[name="DecisionType"]');

        if (newValue == true) {
            isCreditOrgCheckbox.setDisabled(false);
            if (!decisionType.getValue()) {
                decisionType.setValue(B4.enums.AccountOwnerDecisionType.RegOp);             
            }
        } else {
            decisionType.setValue(B4.enums.AccountOwnerDecisionType.RegOp);
            isCreditOrgCheckbox.setDisabled(true);
        }
    },

    onChangeFundFormatDecision: function (field, newValue) {
        var me = this,
            win = me.getNskDecisionEdit(),
            accOwnerDecForm = win.down('form[entity=AccountOwnerDecision]'),
            creditOrgDecForm = win.down('form[entity=CreditOrgDecision]'),
            accOwnerDecCheckbox = accOwnerDecForm.down('checkbox'),
            creditOrgDecCheckbox = creditOrgDecForm.down('checkbox'),
            isSpecAcc = newValue == B4.enums.CrFundFormationDecisionType.SpecialAccount;

        accOwnerDecCheckbox.setValue(isSpecAcc);
        accOwnerDecCheckbox.setDisabled(!isSpecAcc);
        creditOrgDecCheckbox.setDisabled(!isSpecAcc);

        if (!isSpecAcc) {
            creditOrgDecCheckbox.setValue(isSpecAcc);
        }
    },

    onChangeFundFormatDecChkBox: function (field, newValue) {
        var win = field.up('nskdecisionedit'),
            accOwnerDecForm = win.down('form[entity=AccountOwnerDecision]'),
            fields;

        if (newValue == true) {
            //чтобы при загрузке не устанавливалось значение по умолчанию
            if (!win.down('form[entity="CrFundFormationDecision"] b4enumcombo[name="Decision"]').getValue()) {
                win.down('form[entity="CrFundFormationDecision"] b4enumcombo[name="Decision"]').setValue(B4.enums.CrFundFormationDecisionType.SpecialAccount);
                accOwnerDecForm.down('checkbox').setValue(newValue);
                accOwnerDecForm.down('checkbox').enable();
            }
        } else {
            fields = Ext.ComponentQuery.query('form[entity=CreditOrgDecision] [col=right] field', win);

            Ext.each(fields, function (f) {
                f.setValue(null);
            });

            accOwnerDecForm.down('checkbox').setValue(newValue);
            accOwnerDecForm.down('checkbox').disable();
        }
    },

    sendNotification: function (btn) {
        var cbOriginalIncome = Ext.ComponentQuery.query('nskdecisionaddconfirm #cbOriginalIncome')[0];
        if (cbOriginalIncome) {
            cbOriginalIncome.enable();
        }

        var cbCopyIncome = Ext.ComponentQuery.query('nskdecisionaddconfirm #cbCopyIncome')[0];
        if (cbCopyIncome) {
            cbCopyIncome.enable();
        }

        var cbCopyProtocolIncome = Ext.ComponentQuery.query('nskdecisionaddconfirm #cbCopyProtocolIncome')[0];
        if (cbCopyProtocolIncome) {
            cbCopyProtocolIncome.enable();
        }
    },

    onContractDownload: function (btn) {
        var protocolId = parseInt(btn.up('nskdecisionedit').down('[fname=Protocol]').getValue());

        Ext.DomHelper.append(document.body, {
            tag: 'iframe',
            id: 'downloadIframe',
            frameBorder: 0,
            width: 0,
            height: 0,
            css: 'display:none;visibility:hidden;height:0px;',
            src: B4.Url.action('DownloadContract', 'DecisProt', { decisionProtocolId: protocolId })
        });
    },

    saveNotification: function (btn) {
        var me = this,
            frm = btn.up('nskdecisionaddconfirm'),
            rec, decisionEdit = me.getNskDecisionEdit(),
            protocolId = decisionEdit.down('[fname="Protocol"]').getValue() || me.protocolId;

        var value = frm.getValues();

        frm.getForm().updateRecord();
        rec = frm.getForm().getRecord();

        var isValid = frm.getForm().isValid();

        if (!isValid) {
            Ext.Msg.alert('Ошибка', 'Неверно введены данные');
            return;
        }

        frm.submit({
            url: rec.getProxy().getUrl({ action: 'update' }),
            params: {
                records: Ext.encode([rec.getData()])
            },
            success: function () {
                var sendButton = Ext.ComponentQuery.query('nskdecisionaddconfirm #btSend')[0];
                if (sendButton) {
                    sendButton.enable();
                }

                var frmConfirm = Ext.ComponentQuery.query('nskdecisionaddconfirm')[0];

                frmConfirm.getForm().setValues(value.toDateString);

                B4.Ajax.request({
                    url: B4.Url.action('GetConfirm', 'DecisionStraightForward'),
                    params: {
                        protocolId: protocolId
                    },
                    method: 'GET'
                }).next(function (response) {
                    var decoded = Ext.JSON.decode(response.responseText),
                        model = me.getModel('DecisionNotification'),
                        id = decoded && decoded.data ? decoded.data : 0;

                    if (id > 0) {
                        model.load(id, {
                            success: function (newRec) {
                                var win = Ext.ComponentQuery.query('nskdecisionaddconfirm')[0];
                                win.loadRecord(newRec);
                            }
                        });
                    }

                    me.unmask();
                }).error(function (form, action) {
                    var json = Ext.JSON.decode(action.response.responseText);
                    B4.QuickMsg.msg('Ошибка', json.message, 'error');
                    me.unmask();
                });

                Ext.Msg.alert('Успешно', 'Сохранение прошло успешно!');
            },
            failure: function (form, action) {
                var json = Ext.JSON.decode(action.response.responseText);
                Ext.Msg.alert('Ошибка', json && json.message ? json.message : 'Ошибка при сохранении!');
                me.unmask();
            }
        });
    },

    onFormConfirm: function (btn) {
        var me = this,
            protocolId = +me.getProtocolWindow().down('[name=Id]').getValue();

        if (!protocolId) {
            Ext.Msg.alert('Внимание!', 'Перед формированием необходимо сохранить форму!');
        } else {
            me.mask('Уведомление...');

            B4.Ajax.request({
                url: B4.Url.action('GetConfirm', 'DecisionStraightForward'),
                params: {
                    protocolId: protocolId
                },
                method: 'GET'
            }).next(function (response) {
                var decoded = Ext.JSON.decode(response.responseText),
                    model = me.getModel('DecisionNotification'),
                    id = decoded && decoded.data ? decoded.data : 0;

                if (id > 0) {
                    model.load(id, {
                        success: function (newRec) {
                            var win = Ext.ComponentQuery.query('nskdecisionaddconfirm')[0];
                            if (!win) {
                                win = Ext.create('B4.view.realityobj.decision_protocol.NskDecisionAddConfirm', {
                                    constrain: true,
                                    renderTo: B4.getBody().getActiveTab().getEl(),
                                    closeAction: 'destroy'
                                });
                                win.loadRecord(newRec);
                                win.show();
                            }
                        }
                    });
                }

                me.unmask();
            }).error(function (form, action) {
                var json = Ext.JSON.decode(action.response.responseText);
                B4.QuickMsg.msg('Ошибка', json.message, 'error');
                me.unmask();
            });
        }
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('protocolpanel'),
            entityStore = view.down('entityhistoryinfogrid').getStore(),
            protocolId = 0;

        me.params.realityObjectId = id;

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        entityStore.setGroup(B4.enums.EntityHistoryType.DecisionProtocol);

        entityStore.load();

        if (me.params && me.params.protocolId) {
            protocolId = me.params.protocolId;
            me.getAspect('protocolDecisionAspect').editRecord(null, protocolId);
        }
        me.getAspect('protocolStatePermissionAspect').setPermissionsByRecord({ getId: function () { return protocolId; } });
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        if (me.params) {
            operation.params.roId = me.params.realityObjectId;
        }
    },

    onAccountOwnerDecisionTypeSelect: function (selField, val) {
        var win = selField.up('nskdecisionedit'),
            downloadContr = win.down('#downloadContract');


        downloadContr.setDisabled(val != B4.enums.AccountOwnerDecisionType.RegOp);
        if (isCreditOrgCheckbox) {
            if (val === B4.enums.AccountOwnerDecisionType.RegOp) {
                isCreditOrgCheckbox.setValue(false);
            }

            isCreditOrgCheckbox.setDisabled(val === undefined); //(val === B4.enums.AccountOwnerDecisionType.RegOp);
        }
    },

    onDecisionTypeSelect: function (combo, val) {
        var me = this,
            prevWin = combo.up('window'),
            type = combo.getStore().findRecord('Code', val),
            protocolId = combo.up('form').down('[name=Protocol]').getValue(),
            cls = type.get('Js'),
            win;


        win = Ext.create('B4.view.realityobj.decision_protocol.DecisionWindow', {
            childCls: cls,
            protocolId: protocolId,
            decisionTypeCode: val
        });
        win.on('close', me.reloadDecisionGrid, me);
        prevWin.close();
        win.show();
    },

    onBeforeDecisionStoreLoad: function (store, operation) {
        var onlyActive = !this.getShowNonActiveCheckbox().getValue();
        operation.params.protocolId = this.getProtocolIdField().getValue();
        operation.params.onlyActive = onlyActive;
    },

    reloadDecisionGrid: function () {
        this.getDecisionGrid().getStore().load();
    },

    onNskDecisionClick: function () {
        var protoId = this.getProtocolIdField().getValue(),
            win = Ext.widget('nskdecisionedit', {
                protocolId: protoId
            });
        win.show();
    },

    onNskDecisionCheckboxChange: function (chb, val) {
        var form = chb.up('form'),
            fields;

        if (form.entity === 'JobYearDecision' || form.entity === 'MonthlyFeeAmountDecision') {
            var grid = form.down('grid');
            grid.setDisabled(!val);
        } else {
            // изменил из-за того что форма поменялась убрал лишние компонетны.
            fields = Ext.ComponentQuery.query('[col=right] field', form);

            Ext.each(fields, function (f) {
                f.setDisabled(!val);
            });

            fields = Ext.ComponentQuery.query('[col=right][isFormField=true]', form);
            Ext.each(fields, function (f) {
                f.setDisabled(!val);
            });
        }
    },

    onProtocolEditFormRender: function (win) {
        var protocolId = +win.down('hiddenfield[name=Id]').getValue(),
            btn = win.down('button[action=nskdecision]');

        btn.setDisabled(!protocolId);
    },

    onSubmitBtnClick: function (btn) {
        var controller = this,
            me = btn.up('window'),
            win = btn.up('nskdecisionedit'),
            type = win.down('[name=ProtocolType]').getValue(),
            form = type == B4.enums.CoreDecisionType.Owners
                ? win
                : (type == B4.enums.CoreDecisionType.Government ? win.down('form[ownerType=1]') : win.down('form[ownerType=2]')),
            subforms = Ext.ComponentQuery.query('form[disabled=false][entity]', form),
            params = {},
            convertDecision = { 'MkdManagementDecision': true, 'AccountOwnerDecision': true, 'CreditOrgDecision': true },
            isValid = win.getForm().isValid(),
            protocolId = win.down('hiddenfield[name=Id]').getValue(),
            mainForm = win.getForm(),
            protocolFile, errorMessage;

        params.protocolT = type;
        params.roId = controller.params.realityObjectId;
        params.protocolId = protocolId;

        if (type == B4.enums.CoreDecisionType.Owners) {
            var govTab = win.down('form[ownerType=1]');
            protocolFile = govTab.down('b4filefield[name=ProtocolFile]');

            if (protocolFile) {
                protocolFile.name = 'HiddenProtocolFile';
                protocolFile.setDisabled(true);
                protocolFile.reset();
            }

            Ext.each(subforms, function (f) {
                if (Ext.isDefined(params[f.entity])) {
                    Ext.apply(params[f.entity], f.getValues());
                } else {
                    params[f.entity] = f.getValues() || {};
                }

                params[f.entity].Protocol = { Id: me.protocolId };
                if (convertDecision[f.entity]) {
                    params[f.entity].Decision = { Id: params[f.entity].Decision };
                }

                var isChekedField = f.down('checkbox[name="IsChecked"]');
                if (isChekedField) {
                    params[f.entity].IsChecked = isChekedField.getValue();
                }

                isValid = isValid && (Ext.isFunction(f.isValid) ? f.isValid() : f.getForm().isValid());

                if (!isValid) {
                    errorMessage = controller.validateForm(win.getForm());
                }
            });

        }
        else if (type == B4.enums.CoreDecisionType.Government) {
            var otherTab = win.down('form[ownerType=2]');
            
            protocolFile = otherTab.down('b4filefield[name=ProtocolFile]');

            if (protocolFile) {
                protocolFile.name = 'HiddenProtocolFile';
                protocolFile.setDisabled(true);
                protocolFile.reset();
            }
            
            Ext.apply(params, { records: Ext.encode([mainForm.getValues()]) });

            errorMessage = controller.validateForm(win.getForm());
        }

        Ext.Object.each(params, function(key, value) {
            if (typeof (value) == 'object') {
                params[key] = Ext.JSON.encode(value);
            }
        });
       
        me.mask('Сохранение...');
        mainForm.submit({
            url: B4.Url.action('SaveOrUpdateDecisions', 'RealityObjectBothProtocol'),
            params: params,
            success: function (f, action) {
                me.unmask();

                var json = Ext.JSON.decode(action.response.responseText),
                    protocol,
                    prId;

                B4.QuickMsg.msg('Сохранение', 'Сохранение прошло успешно', 'success');

                Ext.ComponentQuery.query('protocolgrid')[0].getStore().load();
                if (json.data && (json.data.Protocol || json.data.Id)) {
                    protocol = json.data.Protocol
                        ? json.data.Protocol
                        : json.data.Id;

                    prId = protocol.Id || protocol;

                    controller.loadProtocol(prId, type, win);

                    controller.getProtocolStateBtn().setVisible(true);
                    controller.getCreateNotifBtn().setVisible(true);
                    controller.protocolId = protocol.Id;
                }
            },
            failure: function (f, action) {
                if (action.result) {
                    errorMessage = action.result.message;
                }
                Ext.Msg.alert('Ошибка', errorMessage);

                me.unmask();
            }
        });
    },

    validateForm: function(form) {
        var fields = form.getFields(),
            invalidFields = '';

        Ext.each(fields.items, function (field) {
            if (!field.isValid()) {
                invalidFields += '<br>' + field.fieldLabel;
            }
        });

        return 'Не заполнены (или заполнены неверно) обязательные поля: <b>' + invalidFields + '</b>';
    },

    loadNskDecisionEditValues: function (form, protocolId, roId) {
        var me = this,
            protocolField = me.getNskDecisionEdit().down('[name=ProtocolType]');

        me.mask('Загрузка...', form);

        form.protocolId = protocolId;

        B4.Ajax.request({
            url: B4.Url.action('Get', 'RealityObjectBothProtocol'),
            params: {
                protocolType: form.protocolType,
                protocolId: protocolId,
                roId: roId
            },
            method: 'GET'
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText);
            form.loadValues(json.data);
            protocolField.setValue(json.protocolType);
            me.unmask();
        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', e.message || e.Message, 'error');
        });

    },

    onCrFundFormatDecisionSelect: function (selField, val, oldVal, opts) {
        var win = selField.up('nskdecisionedit'),
            confirmButton = win.down('#formconfirmbtn'),
            downloadContr = win.down('#downloadContract'),
            decisionTypeEnum = win.down('b4enumcombo[name=DecisionType]'),
            chbxs = Ext.ComponentQuery.query('checkbox[specialacc=true]', win),
            spec = B4.enums.CrFundFormationDecisionType.getStore().findRecord('Description', 'SpecialAccount'),
            regop = B4.enums.CrFundFormationDecisionType.getStore().findRecord('Description', 'RegOpAccount');

        // если способ формирования фонда сменили с регоператора на любой другой
        // выводим alert, что дом будет отвязан от регоператора
        if (val !== oldVal && selField.componentLayoutCounter > 2) {

            var msge = '';
            if (oldVal === regop.get('Value')) {
                msge = 'Дом привязан к расчетному счету регионального оператора, при сохранениии и переводе в статус (Утверждено) привязка будет удалена. Продолжить?';
            } else {
                msge = 'Дом привязан к специальному счету регионального оператора, при сохранении и переводе в статус (Утверждено) привязка будет удалена. Продолжить?';
            }
            selField.suspendCheckChange++;
            selField.setValue(oldVal);
            selField.lastValue = oldVal;
            selField.suspendCheckChange--;
            Ext.Msg.alert({
                title: 'Внимание!',
                msg: msge,
                buttons: Ext.MessageBox.YESNO,
                icon: Ext.MessageBox.WARNING,
                width: 300,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closable: false,
                fn: function (btn) {
                    if (btn == 'yes') {
                        selField.suspendCheckChange++;
                        selField.setValue(val);
                        selField.lastValue = val;

                        if (!spec || val === spec.get('Value')) {
                            // выбран спецсчет
                            Ext.each(chbxs, function (chb) {
                                if (chb.forceSelect) {
                                    chb.setValue(true);
                                }
                                chb.enable();
                            });

                            if (decisionTypeEnum.value == B4.enums.AccountOwnerDecisionType.RegOp) {
                                downloadContr.enable();
                            }
                            confirmButton.enable();

                        } else if (!regop || val === regop.get('Value')) {
                            Ext.each(chbxs, function (chb) {
                                chb.setValue(false);
                                chb.disable();
                            });
                            downloadContr.enable();
                            confirmButton.disable();
                        } else {
                            Ext.each(chbxs, function (chb) {
                                chb.setValue(false);
                                chb.disable();
                            });
                            confirmButton.disable();
                            downloadContr.disable();
                        }
                        selField.suspendCheckChange--;
                    }
                }
            });
        }
    },

    jobYearDecisionActivated: function (checkbox) {
        var store = checkbox.up('jobyeardecision').down('grid').getStore();
        store.each(function (rec) {
            if (checkbox.getValue()) {
                rec.set('UserYear', rec.get('PlanYear'));
            } else {
                rec.set('UserYear', '');
            }
        });
    },

    penaltyDelayActivated: function (checkbox) {
        var grid = checkbox.up('penaltydelaydecision').down('grid'),
            store;

        if (!checkbox.getValue()) {
            store = grid.getStore();
            store.removeAll();
        } else {
            grid.setDisabled(false);
        }
    },

    monthlyFeeDecisionActivated: function (checkbox) {
        var store;
        if (!checkbox.getValue()) {
            store = checkbox.up('monthlyfeedecision').down('grid').getStore();
            store.removeAll();
        }
    },

    confirmProtocolShow: function (field) {
        // спрятать кнопку выбора файла
        if (field) {
            var elems = field.triggerCell.elements;

            if (elems.length > 0) {
                elems[0].setWidth(0);
            }

        }
    },

    getDeleletePermissionAspect: function (protocolType) {
        var me = this,
            permissionAspect;

        switch (protocolType) {
            case B4.enums.CoreDecisionType.Owners:
                permissionAspect = me.getAspect('deleteDecisionProtocolStatePerm');
                break;
            case B4.enums.CoreDecisionType.Government:
                permissionAspect = me.getAspect('deleteGovProtocolDecisionStatePerm');
                break;
            case B4.enums.CoreDecisionType.CrFund:
                permissionAspect = me.getAspect('deleteCrFundDecisionProtocolStatePerm');
                break;
            case B4.enums.CoreDecisionType.MkdManagementType:
                permissionAspect = me.getAspect('deleteMkdManagementTypeDecisionProtocolStatePerm');
                break;
            case B4.enums.CoreDecisionType.ManagementOrganization:
                permissionAspect = me.getAspect('deleteManagementOrganizationDecisionProtocolStatePerm');
                break;
            case B4.enums.CoreDecisionType.TariffApproval:
                permissionAspect = me.getAspect('deleteTariffApprovalDecisionProtocolStatePerm');
                break;
            case B4.enums.CoreDecisionType.OoiManagement:
                permissionAspect = me.getAspect('deleteOoiManagementDecisionProtocolStatePerm');
                break;
        }

        return permissionAspect;
    },

    getEditPermissionAspect: function (protocolType) {
        var me = this,
            permissionAspect;

        switch (protocolType) {
            case B4.enums.CoreDecisionType.Owners:
                permissionAspect = me.getAspect('editDecisionProtocolStatePerm');
                break;
            case B4.enums.CoreDecisionType.Government:
                permissionAspect = me.getAspect('editGovProtocolDecisionStatePerm');
                break;
            case B4.enums.CoreDecisionType.CrFund:
                permissionAspect = me.getAspect('editCrFundDecisionProtocolStatePerm');
                break;
            case B4.enums.CoreDecisionType.MkdManagementType:
                permissionAspect = me.getAspect('editMkdManagementTypeDecisionProtocolStatePerm');
                break;
            case B4.enums.CoreDecisionType.ManagementOrganization:
                permissionAspect = me.getAspect('editManagementOrganizationDecisionProtocolStatePerm');
                break;
            case B4.enums.CoreDecisionType.TariffApproval:
                permissionAspect = me.getAspect('editTariffApprovalDecisionProtocolStatePerm');
                break;
            case B4.enums.CoreDecisionType.OoiManagement:
                permissionAspect = me.getAspect('editOoiManagementDecisionProtocolStatePerm');
                break;
        }

        return permissionAspect;
    }
});