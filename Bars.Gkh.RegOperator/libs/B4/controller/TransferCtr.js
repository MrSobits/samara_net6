Ext.define('B4.controller.TransferCtr', {
    /*
    * Контроллер раздела заявка на перечисление ден. средств
    */
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.StateGridWindowColumn',
        'B4.form.ComboBox',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.SelectField',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu',
        'B4.enums.regop.LoanFormationType'
    ],

    models: [
        'transferrf.TransferCtr',
        'transferctr.PaymentDetail'
    ],

    stores: [
        'transferrf.TransferCtr',
        'transferrf.MunicipalityForSelect',
        'transferrf.MunicipalityForSelected',
        'transferrf.PersonalAccount',
        'contragent.Bank',
        'transferrf.PersonalAccount',
        'ObjectCr',
        'transferctr.PaymentDetail'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'transferctr.RequestGrid',
        'transferctr.RequestEditWindow',
        'transferctr.RequestPanel',
        'transferctr.PaymentDetailGrid',
        'transferctr.RequestInfoPanel',
        'transferctr.RequestPaymentInfoPanel'
    ],

    aspects: [        
        /* пермишен Заявки на перечисление средств по роли */
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhRf.TransferCtr.Create', applyTo: 'b4addbutton', selector: 'requesttransferctrgrid' },
                { name: 'GkhRf.TransferCtr.Delete', applyTo: 'b4deletecolumn', selector: 'requesttransferctrgrid' },
                { name: 'GkhRf.TransferCtr.ExportToTxt', applyTo: '[action=ExportToTxt]', selector: 'requesttransferctrgrid',
                    applyBy: function (cmp, allowed) {
                        cmp.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.ExportToTxt', applyTo: '[action=ExportToTxt]', selector: '#requestTransferCtrEditWindow',
                    applyBy: function (cmp, allowed) {
                        cmp.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Column.IsExport', applyTo: '[dataIndex=IsExport]', selector: 'requesttransferctrgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Column.PaidSum', applyTo: '[dataIndex=PaidSum]', selector: 'requesttransferctrgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Column.CalcAccNumber', applyTo: '[dataIndex=CalcAccNumber]', selector: 'requesttransferctrgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'transferCtrStatePermissionAspect',
            editFormAspectName: 'requestTransferCtrGridEditWindow',
            setPermissionEvent: 'aftersetformdata',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.enable();
                } else {
                    component.disable();
                }
            },
            
            // Отличие от базового: убрали preDisable(), чтобы была возможность добавлять новый объект (без статуса) 
            init: function (controller) {
                var me = this,
                    editFormAspect;
                me.controller = controller;

                // Выключаем аспект, если не передан нужный конфиг
                if ((!me.permissions || me.permissions.length == 0)) {
                    return;
                }

                if (me.editFormAspectName) {
                    editFormAspect = controller.getAspect(me.editFormAspectName);
                    editFormAspect.on(me.setPermissionEvent, me.setPermissions, me);
                }
            },
            permissions: [
                { name: 'GkhRf.TransferCtr.Edit', applyTo: 'b4savebutton', selector: '#requestTransferCtrEditWindow' },
                {
                    name: 'GkhRf.TransferCtr.Field.DocumentNum_View',
                    applyTo: '#tfDocumentNum',
                    selector: '#requestTransferCtrEditWindow',
                    applyBy: function(cmp, allowed) {
                        cmp.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Field.DocumentNum_Edit',
                    applyTo: '#tfDocumentNum',
                    selector: '#requestTransferCtrEditWindow',
                    applyBy: function(cmp, allowed) {
                        cmp.setReadOnly(!allowed);
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Field.PaidSum_View', applyTo: '[name=PaidSum]', selector: '#requestTransferCtrEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Field.PaymentDate_View', applyTo: '[name=PaymentDate]', selector: '#requestTransferCtrEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'GkhRf.TransferCtr.Field.DateFrom_Edit', applyTo: '#dfDateFrom', selector: '#requestTransferCtrEditWindow' },
                { name: 'GkhRf.TransferCtr.Field.TypeProgramRequest_Edit', applyTo: '[name=ProgramCrType]', selector: '#requestTransferCtrEditWindow' },
                { name: 'GkhRf.TransferCtr.Field.ProgramCr_Edit', applyTo: '#sfProgramCr', selector: '#requestTransferCtrEditWindow' },
                { name: 'GkhRf.TransferCtr.Field.ObjectCr_Edit', applyTo: '#sfObjectCr', selector: '#requestTransferCtrEditWindow' },
                { name: 'GkhRf.TransferCtr.Field.Builder_Edit', applyTo: '#sfBuilder', selector: '#requestTransferCtrEditWindow' },
                { name: 'GkhRf.TransferCtr.Field.ContragentBank_Edit', applyTo: '#sfContragentBank', selector: '#requestTransferCtrEditWindow' },
                { name: 'GkhRf.TransferCtr.Field.Contract_Edit', applyTo: '#sfContract', selector: '#requestTransferCtrEditWindow' },
                { name: 'GkhRf.TransferCtr.Field.PaymentType_Edit', applyTo: '[name=PaymentType]', selector: '#requestTransferCtrEditWindow' },
                { name: 'GkhRf.TransferCtr.Field.PaymentType_View', applyTo: '[name=PaymentType]', selector: '#requestTransferCtrEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'GkhRf.TransferCtr.Field.Perfomer_Edit', applyTo: '[name=Perfomer]', selector: '#requestTransferCtrEditWindow' },
                { name: 'GkhRf.TransferCtr.Field.File_Edit', applyTo: '[name=File]', selector: '#requestTransferCtrEditWindow' },
                { name: 'GkhRf.TransferCtr.Field.Comment_Edit', applyTo: '[name=Comment]', selector: '#requestTransferCtrEditWindow' },
                {  name: 'GkhRf.TransferCtr.Field.Comment_View', applyTo: '[name=Comment]', selector: '#requestTransferCtrEditWindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Field.TypeWorkCr_View',
                    applyTo: '[name=TypeWorkCr]',
                    selector: '#requestTransferCtrEditWindow',
                    applyBy: function(component, allowed) {
                        component.allowBlank = !allowed;
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Field.TypeWorkCr_Edit',
                    applyTo: '[name=TypeWorkCr]',
                    selector: '#requestTransferCtrEditWindow',
                    applyBy: function (component, allowed) {
                        component.allowBlank = !allowed;
                        component.setDisabled(!allowed);
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Field.PaymentPurposeDescription_View',
                    applyTo: '[name=PaymentPurposeDescription]',
                    selector: '#requestTransferCtrEditWindow',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Field.PaymentPurposeDescription_Edit',
                    applyTo: '[name=PaymentPurposeDescription]',
                    selector: '#requestTransferCtrEditWindow',
                    applyBy: function (component, allowed) {
                        component.setDisabled(!allowed);
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Field.RegOperator_View',
                    applyTo: '[name=RegOperator]',
                    selector: '#requestTransferCtrEditWindow',
                    applyBy: function(component, allowed) {
                        component.allowBlank = !allowed;
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Field.RegOperator_Edit',
                    applyTo: '[name=RegOperator]',
                    selector: '#requestTransferCtrEditWindow',
                    applyBy: function (component, allowed) {
                        component.allowBlank = !allowed;
                        component.setDisabled(!allowed);
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Field.RegopCalcAccount_View',
                    applyTo: '[name=RegopCalcAccount]',
                    selector: '#requestTransferCtrEditWindow',
                    applyBy: function(component, allowed) {
                        component.allowBlank = !allowed;
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Field.RegopCalcAccount_Edit',
                    applyTo: '[name=RegopCalcAccount]',
                    selector: '#requestTransferCtrEditWindow',
                    applyBy: function (component, allowed) {
                        component.allowBlank = !allowed;
                        component.setDisabled(!allowed);
                    }
                },
                { name: 'GkhRf.TransferCtr.Field.KindPayment_Edit', applyTo: '[name=KindPayment]', selector: '#requestTransferCtrEditWindow' },
                {
                    name: 'GkhRf.TransferCtr.Field.KindPayment_View',
                    applyTo: '[name=KindPayment]',
                    selector: '#requestTransferCtrEditWindow',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Field.PayDetail.Amount_View', applyTo: '[dataIndex=Amount]', selector: 'transferctrpaydetailgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRf.TransferCtr.Field.PayDetail.Amount_Edit', applyTo: '[dataIndex=Amount]', selector: 'transferctrpaydetailgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.editor = {
                                xtype: 'numberfield',
                                minValue: 0,
                                decimalSeparator: ','
                            };
                        } else {
                            component.editor = null;
                        }
                    }
                }
            ]
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'requestTransferCtrStateButtonAspect',
            stateButtonSelector: '#requestTransferCtrEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    var editWindowAspect = asp.controller.getAspect('requestTransferCtrGridEditWindow'),
                        model = asp.controller.getModel('transferrf.TransferCtr');

                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState);
                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    editWindowAspect.updateGrid();
                    if (entityId) {

                        model.load(entityId, {
                            success: function(rec) {
                                editWindowAspect.setFormData(rec);
                            },
                            scope: asp
                        });
                    }

                    
                }
            }
        },
        {
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'requestTransferCtrStateTransferAspect',
            gridSelector: 'requesttransferctrgrid',
            stateType: 'rf_transfer_ctr',
            menuSelector: 'requestTransferCtrGridStateMenu'
        },
        {
            /*
            * аспект взаимодействия таблицы заявок и окна редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'requestTransferCtrGridEditWindow',
            gridSelector: '#requestTransferCtrGrid',
            editFormSelector: '#requestTransferCtrEditWindow',
            storeName: 'transferrf.TransferCtr',
            modelName: 'transferrf.TransferCtr',
            editWindowView: 'transferctr.RequestEditWindow',
            onSaveSuccess: function() {
            },
            listeners: {
                /*
                * 1.Проставляем статус,
                * 2.включаем/выключаем грид
                * 3. ставим поле Дата ПП редактируемым, если уже было когда то заполнено
                */
                aftersetformdata: function(asp, record) {
                    var recId = record.get('Id'),
                        grid = asp.getForm().down('transferctrpaydetailgrid'),
                        tfTransferCtrSum = asp.getForm().down('numberfield[name=Sum]'),
                        permAspect = asp.controller.getAspect('transferCtrStatePermissionAspect'),
                        dateFromPp = asp.getForm().down('requestinfopanel').down('#dfDateFromPp'),
                        objectCr = asp.getForm().down('requestinfopanel').down('#sfObjectCr'),
                        dateFromPpValue = record.get('DateFromPp'),
                        loanFormationByTransferCtr = Gkh.config.RegOperator.GeneralConfig.LoanConfig.LoanFormationType === B4.enums.regop.LoanFormationType.ByTransferCtr;

                    if (recId != 0) {
                        asp.controller.getAspect('requestTransferCtrStateButtonAspect').setStateData(record.get('Id'), record.get('State')); //1
                        permAspect.preDisable();

                        grid.setDisabled(false);
                        grid.getStore().load();
                    } else {
                        grid.setDisabled(true);
                        grid.getStore().removeAll();
                    }

                    if (dateFromPp) {
                        dateFromPp.setReadOnly(!dateFromPpValue);
                    }

                    if (tfTransferCtrSum) {
                        tfTransferCtrSum.setReadOnly(!loanFormationByTransferCtr);
                    }

                    if (!record.PaymentDate) {
                        objectCr.setReadOnly(true);
                    }

                    grid.recId = recId;
                }
            },
            saveRecordHasUpload: function (rec) {
                var me = this,
                    frm = me.getForm(),
                    grid = frm.down('transferctrpaydetailgrid'),
                    details = [],
                    requestSum = 0,
                    loanFormationByTransferCtr = Gkh.config.RegOperator.GeneralConfig.LoanConfig.LoanFormationType === B4.enums.regop.LoanFormationType.ByTransferCtr;;

                Ext.each(grid.getStore().data.items, function(item) {
                    details.push(item.getData());
                    requestSum += item.get('Amount');
                });

                if (loanFormationByTransferCtr && rec.get('Sum') !== 0 && requestSum !== 0 && requestSum > rec.get('Sum')) {
                    Ext.Msg.alert('Ошибка', 'Сумма заявки не может быть меньше сумм распределяемых оплат (столбец "Оплата")');
                    return;
                }

                me.mask('Сохранение', frm);
                frm.submit({
                    url: B4.Url.action('SaveWithDetails', 'TransferCtr'),
                    params: {
                        records: Ext.encode([rec.getData()]),
                        details: Ext.encode(details)
                    },
                    success: function(form, action) {
                        var model, id;
                        me.unmask();
                        me.updateGrid();

                        model = me.getModel(rec);

                        if (action.result.data.length > 0) {
                            id = action.result.data[0] instanceof Object ? action.result.data[0].Id : action.result.data[0];
                            model.load(id, {
                                success: function(newRec) {
                                    me.setFormData(newRec);
                                    me.fireEvent('savesuccess', me, newRec);
                                }
                            });
                        }
                    },
                    failure: function(form, action) {
                        me.unmask();
                        me.fireEvent('savefailure', rec, action.result.message);
                        Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                    }
                });
            },
            otherActions: function (actions) {
                var me = this;

                actions[me.editFormSelector + ' transferctrpaydetailgrid'] = {
                    'afterrender': function (grid) {
                        var controller = this,
                            editWindowAspect = controller.getAspect('requestTransferCtrGridEditWindow'),
                            model = controller.getModel('transferrf.TransferCtr');

                        grid.getStore().on('beforeload', function(store, operation) {
                            operation.params.transferCtrId = me.getForm().down('[name=Id]').getValue();
                        });

                        if (grid.recId) {

                            model.load(grid.recId, {
                                success: function (rec) {
                                    editWindowAspect.setFormData(rec);            
                                }
                            });
                        }

                        if (!grid.disabled) {
                            grid.getStore().load();
                        }
                    }
                };

                //вешаемся
                actions[me.editFormSelector + ' #sfContragentBank'] = {
                    'change': { fn: me.onChangeContrBank, scope: me },

                    'beforeload': function (store, operation) {
                        var builderId = me.getForm().down('#sfBuilder').getValue();
                        operation.params.builderId = builderId;
                    }
                };

                actions[me.editFormSelector + ' #sfContract'] = {
                    'beforeload': function (store, operation) {
                        var objectCrId = me.getForm().down('[name=ObjectCr]').getValue();
                        operation.params.objectCrId = objectCrId;
                    }
                };

                actions[me.editFormSelector + ' #sfBuilder'] = {
                    'change': { fn: me.onChangeBuilder, scope: me },

                    'beforeload': function (store, operation) {
                        var objectCrId = me.getForm().down('[name=ObjectCr]').getValue();
                        operation.params.objectCrId = objectCrId;
                    }
                };

                actions[me.editFormSelector + ' #sfProgramCr'] = {
                    'change': { fn: me.onChangeProgram, scope: me }
                };

                actions[me.editFormSelector + ' #sfObjectCr'] = {
                    'change': { fn: me.onChangeObjectCr, scope: me },

                    'beforeload': function (store, operation) {
                        var programId = me.getForm().down('#sfProgramCr').getValue();
                        operation.params.programId = programId;
                    }
                };
                
                actions[me.editFormSelector + ' [name=TypeWorkCr]'] = {
                    'beforeload': function(store, operation) {
                        var objectCrId = me.getForm().down('[name=ObjectCr]').getValue();
                        operation.params.objectCrId = objectCrId;
                    }
                };

                actions[me.editFormSelector + ' [name=RegOperator]'] = {
                    'hide': { fn: me.onHideRegopFieldset, scope: me },
                    'show': {fn: me.onShowRegopFieldset, scope: me}
                };

                actions[me.editFormSelector + ' [name=RegopCalcAccount]'] = {
                    'beforeload': { fn: me.onBeforeRegopCalcAccount, scope: me },
                    'hide': { fn: me.onHideRegopFieldset, scope: me },
                    'show': { fn: me.onShowRegopFieldset, scope: me }
                };

                actions['#requestTransferCtrGrid [action=ExportToTxt]'] = { 'click': { fn: me.exportToTxt, scope: me } };
                actions['#requestTransferCtrEditWindow [action=ExportToTxt]'] = { 'click': { fn: me.exportToTxt, scope: me } };
                actions['#requestTransferCtrEditWindow [name=IsEditPurpose]'] = { 'change': { fn: me.changeEditPurpose, scope: me } };
            },
            
            //при смене программы кр очищаем поле "объект кр"
            onChangeProgram: function(obj, newValue) {
                var editForm = this.getForm(),
                    sfObjectCr;

                if (Ext.isEmpty(editForm)) {
                    return;
                }

                //если ничего не изменилось, ничего не делаем
                //т.к. копирование текста - операция изменения поля
                if (obj.lastValue == newValue) {
                    return;
                }

                sfObjectCr = editForm.down('#sfObjectCr');
                sfObjectCr.setValue(null);

                if (Ext.isEmpty(newValue)) {
                    sfObjectCr.setReadOnly(true);
                } else {
                    sfObjectCr.setReadOnly(false);
                }
            },

            //при смене подрядной организации изменяем или чистим ее readonly поля (ИНН, КПП, Телефон)
            onChangeBuilder: function (obj, newValue) {
                var editForm = this.getForm(),
                    tfInn,
                    tfKpp,
                    tfPhone,
                    sfContragentBank;

                if (Ext.isEmpty(editForm)) {
                    return;
                }

                //если ничего не изменилось, ничего не делаем
                //т.к. копирование текста - операция изменения поля
                if (obj.lastValue == newValue) {
                    return;
                }

                //обнуляем банк контрагента
                sfContragentBank = editForm.down('#sfContragentBank');
                tfInn = editForm.down('#tfInn');
                tfKpp = editForm.down('#tfKpp');
                tfPhone = editForm.down('#tfPhone');

                sfContragentBank.setValue(null);

                if (Ext.isEmpty(newValue)) {
                    tfInn.setValue(null);
                    tfKpp.setValue(null);
                    tfPhone.setValue(null);

                    sfContragentBank.setReadOnly(true);
                } else {
                    tfInn.setValue(newValue.Inn);
                    tfKpp.setValue(newValue.Kpp);
                    tfPhone.setValue(newValue.Phone);

                    sfContragentBank.setReadOnly(false);
                }
            },
            //при смене объекта Кр
            //очищаем поле "подрядчик", "договор подряда" и "вид работы"
            onChangeObjectCr: function (obj, newValue) {
                var editForm = this.getForm(),
                    sfBuilder,
                    sfContract,
                    sfTypeWork;

                if (Ext.isEmpty(editForm)) {
                    return;
                }

                //если ничего не изменилось, ничего не делаем
                //т.к. копирование текста - операция изменения поля
                if (obj.lastValue == newValue) {
                    return;
                }

                sfBuilder = editForm.down('#sfBuilder');
                sfContract = editForm.down('#sfContract');
                sfTypeWork = editForm.down('[name=TypeWorkCr]');
                        
                sfBuilder.setValue(null);
                sfContract.setValue(null);
                sfTypeWork.setValue(null);
                
                if (Ext.isEmpty(newValue)) {
                    sfBuilder.setReadOnly(true);
                    sfContract.setReadOnly(true);
                    sfTypeWork.setReadOnly(true);
                } else {
                    sfBuilder.setReadOnly(false);
                    sfContract.setReadOnly(false);
                    sfTypeWork.setReadOnly(false);
                }
            },
            //При смене банка заменяем или чистим поля (Расчетный счет, Корр. счет и БИК)
            onChangeContrBank: function(obj, newValue) {
                var editForm = this.getForm(),
                    tfSettlementAccount,
                    tfCorrAccount,
                    tfBik;

                if (Ext.isEmpty(editForm)) {
                    return;
                }

                //если ничего не изменилось, ничего не делаем
                //т.к. копирование текста - операция изменения поля
                if (obj.lastValue == newValue) {
                    return;
                }

                tfSettlementAccount = editForm.down('#tfSettlementAccount');
                tfCorrAccount = editForm.down('#tfCorrAccount');
                tfBik = editForm.down('#tfBik');

                if (Ext.isEmpty(newValue)) {
                    tfSettlementAccount.setValue(null);
                    tfCorrAccount.setValue(null);
                    tfBik.setValue(null);
                } else {
                    tfSettlementAccount.setValue(newValue.SettlementAccount);
                    tfCorrAccount.setValue(newValue.CorrAccount);
                    tfBik.setValue(newValue.Bik);
                }
            },
            
            onBeforeRegopCalcAccount: function (store, operation) {
                var me = this,
                    regOpId = me.getForm().down('[name=RegOperator]').value.ContragentId;

                operation.params.ownerId = regOpId;
            },

            onHideRegopFieldset: function () {
                var me = this,
                    view = me.getForm(),
                    regOpField = view.down('[name=RegOperator]'),
                    regOpCalcAccField = view.down('[name=RegopCalcAccount]'),
                    fieldset = view.down('[fieldSetType=Regop]');

                if (regOpField.isHidden() && regOpCalcAccField.isHidden()) {
                    fieldset.hide();
                }
            },

            onShowRegopFieldset: function () {
                var me = this,
                    view = me.getForm(),
                    regOpField = view.down('[name=RegOperator]'),
                    regOpCalcAccField = view.down('[name=RegopCalcAccount]'),
                    fieldset = view.down('[fieldSetType=Regop]');

                if (!regOpField.isHidden() || !regOpCalcAccField.isHidden()) {
                    fieldset.show();
                }
            },

            exportToTxt: function (btn) {
                var me = this,
                    grid = btn.up('requesttransferctrgrid'),
                    window = btn.up('requesttransferctreditwin'),
                    ids = [],
                    selection,
                    msg;

                if (grid) {
                    selection = grid.getSelectionModel().getSelection();
                    selection.forEach(function(entry) {
                        ids.push(entry.getId());

                        if (entry.get('Document')) {
                            msg = 'Для некоторых выбранных заявок уже сформированы документы. Заменить документы на новые?';
                        }
                    });
                }

                if (msg && ids.length === 1) {
                    msg = 'Для выбранной заявки уже сформирован документ. Заменить документ на новый?';
                }

                if (window) {
                    ids.push(window.getRecord().getId());
                }

                if (ids.length > 0) {
                    if (msg) {
                        Ext.Msg.confirm('Внимание', msg, function(res) {
                            if (res === 'yes') {
                                me.exportInternal(grid, ids, false);
                            } else {
                                me.exportInternal(grid, ids, true);
                            }
                        });
                    } else {
                        me.exportInternal(grid, ids, false);
                    }

                } else {
                    Ext.Msg.alert('Ошибка', "Необходимо выбрать заявку!");
                }
            },
            exportInternal: function (grid, ids, isPrintOldDoc) {
                var me = this;

                me.mask("Обработка...", me.getMainView);
                B4.Ajax.request({
                    url: B4.Url.action('ExportToTxt', 'TransferCtr', {
                        transferIds: ids.join(','),
                        isPrintOldDoc: isPrintOldDoc
                    }),
                    timeout: 9999999
                }).next(function (resp) {
                    var tryDecoded, data, message, frame;
                    me.unmask();

                    try {
                        tryDecoded = Ext.JSON.decode(resp.responseText);
                    } catch (e) {
                        tryDecoded = {};
                    }

                    data = resp.data ? resp.data : tryDecoded;
                    message = resp.message ? resp.message : tryDecoded.message;

                    if (data && data.Id) {
                        frame = Ext.get('downloadIframe');
                        if (frame != null) {
                            Ext.destroy(frame);
                        }

                        Ext.DomHelper.append(document.body, {
                            tag: 'iframe',
                            id: 'downloadIframe',
                            frameBorder: 0,
                            width: 0,
                            height: 0,
                            css: 'display:none;visibility:hidden;height:0px;',
                            src: B4.Url.action('Download', 'FileUpload', { Id: data.Id })
                        });
                    } else {
                        me.unmask();
                        Ext.Msg.alert('Внимание', message);
                    }

                    if (grid) {
                        grid.getStore().load();
                    }

                }).error(function (err) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка', err.message || err.message || err);
                });
            },
            changeEditPurpose: function (fld, newValue) {
                var window = fld.up('window'),
                    purposeFld = window.down('[name=PaymentPurposeDescription]');

                if (!newValue) {
                    Ext.Msg.confirm('Внимание', 'При автоматическом заполнении поля ручные корректировки будут потеряны. Включить автоматическое формирование поля?', function (res) {
                        if (res === 'yes') {
                            purposeFld.setValue('');
                            purposeFld.setReadOnly(true);
                        } else {
                            purposeFld.setReadOnly(false);
                            fld.setValue(true);
                        }
                    });
                }
            }
        },
        {
            /*
            * аспект взаимодействия триггер-поля фильтрации мун. образований и таблицы объектов КР
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'requesttransferctrmultiselectwindowaspect',
            fieldSelector: '#requestTransferCtrFilterPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#municipalitySelectWindow',
            storeSelect: 'transferrf.MunicipalityForSelect',
            storeSelected: 'transferrf.MunicipalityForSelected',
            columnsGridSelect: [
                { header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Группа', xtype: 'gridcolumn', dataIndex: 'Group', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Федеральный номер', xtype: 'gridcolumn', dataIndex: 'FederalNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'OKATO', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'RequestTransferRfButtonExportAspect',
            gridSelector: 'requesttransferctrgrid',
            buttonSelector: 'requesttransferctrgrid #btnRequestTransferCtrExport',
            controllerName: 'TransferCtr',
            actionName: 'Export'
        }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'transferctr.RequestPanel',
    mainViewSelector: 'requestTransferCtrPanel',
    mainViewItemId: 'requestTransferCtrPanel',

    editWindowSelector: '#requestTransferCtrEditWindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'requestTransferCtrPanel'
        },
        {
            ref: 'grid',
            selector: 'requestTransferCtrPanel requesttransferctrgrid'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            '#requestTransferCtrFilterPanel #updateGrid': {
                click: function() {
                    me.getGrid().getStore().load();
                }
            }
        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView();
        me.params = {};

        if (!view) {
            view = Ext.widget('requestTransferCtrPanel');
            me.bindContext(view);
            me.application.deployView(view);

            view.down('grid').getStore().on('beforeload', me.onBeforeLoadRequest, me);
        }

        view.down('grid').getStore().load();
    },

    onBeforeLoad: function(store, operation) {
        operation.params.requestTransferRfId = this.requestTransferCtrId;
    },

    onBeforeLoadRequest: function (store, operation) {
        var me = this,
            view = me.getMainView();
    
        operation.params.municipalities = view.down('[name=Municipality]').getValue();
        operation.params.dateStart = view.down('#dfDateStart').getValue();
        operation.params.dateEnd = view.down('#dfDateEnd').getValue();
    },

    onBeforeLoadProgramCr: function (store, operation) {
        operation.params.activePrograms = true;
        operation.params.onlyFull = true;
    }
});