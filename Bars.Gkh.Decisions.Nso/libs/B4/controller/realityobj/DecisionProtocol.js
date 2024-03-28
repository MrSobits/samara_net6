Ext.define('B4.controller.realityobj.DecisionProtocol', {
    extend: 'B4.controller.MenuItemController',
    params: {},

    requires: [
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
        'B4.enums.AccountOwnerDecisionType',
        'B4.Ajax',
        'B4.Url',
        'B4.QuickMsg',
        'B4.enums.CrFundFormationDecisionType',
        'B4.model.DecisionNotification',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.GkhStatePermissionAspect'
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
        'DecisionNotification'
    ],

    stores: [
        'realityobj.decision_protocol.Protocol',
        'realityobj.decision_protocol.Decision'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    parentCtrlCls: 'B4.controller.realityobj.Navi',

    refs: [
        {
            ref: 'mainView',
            selector: 'protocolpanel'
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
        }
    ],

    aspects: [
        {
            xtype: 'statecontextbuttonaspect',
            name: 'protocolStateBtnAspect',
            stateButtonSelector: '#protocolStateBtn',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    var editWindowAspect = asp.controller.getAspect('protocolDecisionAspect'),
                        permissionAspect = asp.controller.getAspect('protocolStatePermissionAspect'),
                        model = asp.controller.getModel('RealityObjectDecisionProtocol');

                    asp.setStateData(entityId, newState);
                    asp.controller.getProtocolDecisionForm().state = newState;

                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            editWindowAspect.setFormData(rec);
                            permissionAspect.setPermissionsByRecord(rec);
                        },
                        scope: this
                    }) : permissionAspect.setPermissionsByRecord(new model({ Id: 0 }));
                }
            },
            changeState: function (newStateId) {
                var asp = this,
                    id = asp.getEntityId(),
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
            xtype: 'gkhstatepermissionaspect',
            name: 'protocolStatePermissionAspect',
            editFormAspectName: 'protocolDecisionAspect',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                {
                    name: 'Gkh.RealityObject.Register.DecisionProtocolsEditDelete.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'nskdecisionedit'
                },
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
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'Gkh.RealityObject.Register.DecisionProtocolsEditDelete.Delete' }],
            name: 'deleteDecisionProtocolStatePerm'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.RealityObject.Register.DecisionProtocolsViewCreate.Create',
                    applyTo: 'b4addbutton', selector: 'protocolgrid'
                }
            ]
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'protocolStateTransferAspect',
            gridSelector: 'protocolgrid',
            menuSelector: 'protocolgridStateMenu',
            stateType: 'gkh_real_obj_dec_protocol'
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'protocolDecisionAspect',
            gridSelector: 'protocolgrid',
            storeName: 'realityobj.decision_protocol.Protocol',
            modelName: 'RealityObjectDecisionProtocol',
            editWindowView: 'realityobj.decision_protocol.NskDecisionEdit',
            editFormSelector: 'nskdecisionedit',
            getGrid: function () {
                return Ext.ComponentQuery.query(this.gridSelector)[0];
            },
            listeners: {
                beforesetformdata: function (asp, record) {
                    if (this.controller.params) {
                        record.set('RealityObject', this.controller.params.realityObjectId);
                    }
                    if (!record.get('Id')) {
                        record.set('ProtocolDate', new Date());
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var controller = asp.controller,
                        protoId = +controller.getProtocolIdField().getValue(),
                        downloadBtn = controller.getDownloadContrBtn();
                    record.set('Id', protoId);
                    controller.loadNskDecisionEditValues(form, protoId, controller.params.realityObjectId);
                    controller.getAspect('protocolStateBtnAspect').setStateData(record.get('Id'), record.raw.State);
                    controller.getProtocolStateBtn().setVisible(record && record.get('Id'));
                    controller.getCreateNotifBtn().setVisible(record && record.get('Id'));
                    if (downloadBtn) {
                        downloadBtn.setVisible(record && record.get('Id'));
                    }
                }
            },
            editRecord: function (record, protocolId) {
                var me = this,
                    id = record ? record.getId() : protocolId,
                    model;

                model = this.getModel(record);

                B4.Ajax.request({
                    url: B4.Url.action('GetOwnerType', 'DecisionStraightForward'),
                    params: {
                        roId: me.controller.params.realityObjectId
                    },
                    method: 'GET'
                }).next(function (response) {
                    Ext.ClassManager.get('B4.enums.AccountOwnerDecisionType').Meta.Custom.Display = Ext.decode(response.responseText).data;

                    id ? model.load(id, {
                        success: function (rec) {
                            me.setFormData(rec);
                        },
                        scope: me
                    }) : me.setFormData(new model({ Id: 0 }));
                });
            },
            deleteRecord: function (record) {
                if (record.getId()) {
                    this.controller.getAspect('deleteDecisionProtocolStatePerm').loadPermissions(record)
                        .next(function (response) {
                            var me = this,
                                grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                                    if (result == 'yes') {
                                        var model = this.getModel(record);

                                        var rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
                                        rec.destroy()
                                            .next(function () {
                                                this.fireEvent('deletesuccess', this);
                                                me.updateGrid();
                                                me.unmask();
                                            }, this)
                                            .error(function (result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, this);
                                    }
                                }, me);
                            }

                        }, this);
                }
            },
            saveRequestHandler: function () {
                // pass
            }
        },
        {
            xtype: 'entityhistoryfieldwindowaspect',
            getParentId: function () {
                var me = this.controller,
                    view = me.getMainView();

                return me.getContextValue(view, 'realityObjectId');
            }
        }
    ],

    init: function () {
        var me = this;
        me.getStore('realityobj.decision_protocol.Protocol').on('beforeload', me.onBeforeLoad, me);
        me.getStore('realityobj.decision_protocol.Decision').on('beforeload', me.onBeforeDecisionStoreLoad, me);

        me.control({
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
            'jobyeardecision checkbox': {
                change: { fn: me.jobYearDecisionActivated, scope: me }
            },
            'monthlyfeedecision checkbox': {
                change: { fn: me.monthlyFeeDecisionActivated, scope: me }
            },
            'nskdecisionaddconfirm b4filefield[name=ProtocolFile]': {
                render: { fn: me.confirmProtocolShow, scope: me }
            }
        });

        me.callParent(arguments);
    },

    onChangeAccountOwnerCheckbox: function (checkbox, newValue) {
        var me = this,
            win = me.getNskDecisionEdit(),
            decisionType = win.down('form[entity=AccountOwnerDecision] b4enumcombo[name="DecisionType"]');

        if (newValue == true) {
            if (!decisionType.getValue()) {
                decisionType.setValue(B4.enums.AccountOwnerDecisionType.RegOp);
            }
        } else {
            decisionType.setValue(B4.enums.AccountOwnerDecisionType.RegOp);
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
            win.down('form[entity="CrFundFormationDecision"] b4enumcombo[name="Decision"]').setValue(B4.enums.CrFundFormationDecisionType.SpecialAccount);
        } else {
            fields = Ext.ComponentQuery.query('form[entity=CreditOrgDecision] [col=right] field', win);

            Ext.each(fields, function (f) {
                f.setValue(null);
            });
        }

        accOwnerDecForm.down('checkbox').setValue(newValue);
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
            src: B4.Url.action('DownloadContract', 'Decision', { decisionProtocolId: protocolId })
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

        frm.submit({
            url: rec.getProxy().getUrl({ action: 'update' }),
            params: {
                records: Ext.encode([rec.getData()])
            },
            success: function () {
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
            protocolId = parseInt(btn.up('nskdecisionedit').down('[fname=Protocol]').getValue()) || me.protocolId;

        if (!protocolId || protocolId == 0) {
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
            }).error(function () {
                var json = Ext.JSON.decode(action.response.responseText);
                B4.QuickMsg.msg('Ошибка', json.message, 'error');
                me.unmask();
            });
        }
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('protocolpanel');

        me.params.realityObjectId = id;

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        if (me.params && me.params.protocolId) {
            me.getAspect('protocolDecisionAspect').editRecord(null, me.params.protocolId);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.roId = this.params.realityObjectId;
        }
    },

    onAccountOwnerDecisionTypeSelect: function (selField, val) {
        var win = selField.up('nskdecisionedit'),
            downloadContr = win.down('#downloadContract');

        downloadContr.setDisabled(val != B4.enums.AccountOwnerDecisionType.RegOp);
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
            grid.setDisabled(true);
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
            form = btn.up('nskdecisionedit').down(' > form'),
            subforms = Ext.ComponentQuery.query('form', form),
            params = {},
            convertDecision = { 'MkdManagementDecision': true, 'AccountOwnerDecision': true, 'CreditOrgDecision': true },
            isValid = form.getForm().isValid(),
            validationMessage = null;

        Ext.each(subforms, function (f) {
            params[f.entity] = f.getValues() || {};
            params[f.entity].Protocol = { Id: me.protocolId };
            if (convertDecision[f.entity]) {
                params[f.entity].Decision = { Id: params[f.entity].Decision };
            }

            var isChekedField = f.down('checkbox[name="IsChecked"]');
            if (isChekedField) {
                params[f.entity].IsChecked = isChekedField.getValue();
            }

            params[f.entity] = Ext.JSON.encode(params[f.entity]);

            isValid = isValid && (Ext.isFunction(f.isValid) ? f.isValid() : f.getForm().isValid());
            if (!isValid) {
                validationMessage = f.validationMessage;
            }
            return isValid;
        });

        if (isValid) {

            me.mask('Сохранение...');

            form.submit({
                url: B4.Url.action('SaveOrUpdateDecisions', 'DecisionStraightForward'),
                params: params,
                success: function (f, action) {
                    var json = Ext.JSON.decode(action.response.responseText),
                        protocol,
                        prId;

                    B4.QuickMsg.msg('Сохранение', 'Сохранение прошло успешно', 'success');
                    Ext.ComponentQuery.query('protocolgrid')[0].getStore().load();
                    if (json.data && json.data.Protocol) {
                        protocol = json.data.Protocol;

                        prId = protocol.Id;

                        controller.loadNskDecisionEditValues(me, protocol.Id, controller.params.realityObjectId);
                        controller.getAspect('protocolStateBtnAspect').setStateData(protocol.Id, protocol.State);

                        controller.getAspect('protocolStatePermissionAspect').setPermissionsByRecord({ getId: function () { return prId; } });

                        controller.getProtocolStateBtn().setVisible(true);
                        controller.getCreateNotifBtn().setVisible(true);
                        controller.protocolId = protocol.Id;
                    }

                    // Обновляем кнопку смены статуса
                    me.unmask();
                },
                failure: function (f, action) {
                    var json = Ext.JSON.decode(action.response.responseText);
                    B4.QuickMsg.msg('Сохранение', json.message, 'error');
                    me.unmask();
                }
            });
        } else {
            Ext.Msg.alert('Ошибка', validationMessage || 'Неверно введены данные');
        }
    },

    loadNskDecisionEditValues: function (nskdec, protocolId, roId) {
        var me = this;
        me.mask('Загрузка...', nskdec);

        nskdec.protocolId = protocolId;

        B4.Ajax.request({
            url: B4.Url.action('Get', 'DecisionStraightForward'),
            params: {
                protocolId: protocolId,
                roId: roId
            },
            method: 'GET'
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText);
            nskdec.loadValues(json.data);
            me.unmask();
        }).error(function () {
            var json = Ext.JSON.decode(action.response.responseText);
            B4.QuickMsg.msg('Ошибка', json.message, 'error');
            me.unmask();
        });

    },

    onCrFundFormatDecisionSelect: function (selField, val) {
        var win = selField.up('nskdecisionedit'),
            confirmButton = win.down('#formconfirmbtn'),
            downloadContr = win.down('#downloadContract'),
            decisionTypeEnum = win.down('b4enumcombo[name=DecisionType]'),
            chbxs = Ext.ComponentQuery.query('checkbox[specialacc=true]', win),
            spec = B4.enums.CrFundFormationDecisionType.getStore().findRecord('Description', 'SpecialAccount'),
            regop = B4.enums.CrFundFormationDecisionType.getStore().findRecord('Description', 'RegOpAccount');
        if (!spec || val === spec.get('Value')) {
            // выбран спецсчет
            Ext.each(chbxs, function (chb) {
                if (chb.forceSelect) {
                    chb.setValue(true);
                }
                chb.enable();
            });

            if (downloadContr && decisionTypeEnum.value == B4.enums.AccountOwnerDecisionType.RegOp) {
                downloadContr.enable();
            }
            confirmButton.enable();

        } else if (!regop || val === regop.get('Value')) {
            Ext.each(chbxs, function (chb) {
                chb.setValue(false);
                chb.disable();
            });
            if (downloadContr) {
                downloadContr.enable();
            }
            confirmButton.disable();
        } else {
            Ext.each(chbxs, function (chb) {
                chb.setValue(false);
                chb.disable();
            });
            confirmButton.disable();
            if (downloadContr) {
                downloadContr.disable();
            }
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

    monthlyFeeDecisionActivated: function (checkbox) {
        var store = checkbox.up('monthlyfeedecision').down('grid').getStore();
        if (!checkbox.getValue()) {
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
    }
});