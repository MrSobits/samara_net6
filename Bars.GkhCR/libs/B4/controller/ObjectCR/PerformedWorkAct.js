Ext.define('B4.controller.objectcr.PerformedWorkAct', {
    /*
    * Контроллер раздела Акты выполненных работ
    */
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.StateContextButton',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhButtonImportAspect',
        'B4.aspects.permission.objectcr.PerformedWorkAct',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu'
    ],

    models: [
        'objectcr.PerformedWorkAct',
        'objectcr.performedworkact.Record',
        'objectcr.performedworkact.PerformedWorkActPhoto',
        'objectcr.performedworkact.Payment'
    ],

    stores: [
        'objectcr.PerformedWorkAct',
        'objectcr.performedworkact.Record',
        'objectcr.performedworkact.Payment',
        'objectcr.performedworkact.TypeWorkCr',
        'objectcr.performedworkact.PerformedWorkActPhoto',
        'objectcr.TypeWorkCr'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    views: [
        'objectcr.performedworkact.Grid',
        'objectcr.performedworkact.EditWindow',
        'objectcr.performedworkact.PaymentDetailsWindow',
        'objectcr.performedworkact.RecGrid',
        'objectcr.performedworkact.PaymentGrid',
        'objectcr.performedworkact.PhotoGrid',
        'objectcr.performedworkact.PhotoEditWindow',
        'objectcr.TypeWorkCrGrid',
        'B4.view.Import.Window'
    ],

    mainView: 'objectcr.performedworkact.Grid',
    mainViewSelector: 'perfactgrid',

    perfWorkActEditWindowSelector: 'perfactwin',
    perfWorkActRecGridSelector: 'perfworkactrecgrid',

    refs: [
        {
            ref: 'performedWorkActGrid',
            selector: 'perfactgrid'
        },
        {
            ref: 'performedWorkActWin',
            selector: 'perfactwin'
        },
        {
            ref: 'performedWorkActRecGrid',
            selector: 'perfworkactrecgrid'
        }
    ],

    parentCtrlCls: 'B4.controller.objectcr.Navi',

    aspects: [
        /* пермишшены по статусы Акта выполненных работ */
        {
            xtype: 'performedworkactobjectcrstateperm',
            editFormAspectName: 'performedWorkActGridEditWindowAspect',
            setPermissionEvent: 'aftersetformdata',
            name: 'performedWorkActObjectCrStatePerm'
        },
        /* пермишшен по статусу Объекта КР */
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'performedWorkActObjectCrCreatePerm',
            permissions: [
                { name: 'GkhCr.ObjectCr.Register.PerformedWorkActViewCreate.Create', applyTo: 'b4addbutton', selector: 'perfactgrid' }
            ]
        },
        /* пермишшен по удалению записи Акта выполненных работ(по его статусы), вынесен в отдельный аспект для  удобства */
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Delete' }],
            name: 'deletePerformedWorkActStatePerm'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'performedWorkActStateTransferAspect',
            gridSelector: 'perfactgrid',
            stateType: 'cr_obj_performed_work_act',
            menuSelector: 'performedWorkActGridStateMenu'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statecontextbuttonaspect',
            name: 'performedWorkActStateButtonAspect',
            stateButtonSelector: 'perfactwin #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState);
                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    var editWindowAspect = asp.controller.getAspect('performedWorkActGridEditWindowAspect');
                    editWindowAspect.updateGrid();
                    var model = this.controller.getModel('objectcr.PerformedWorkAct');
                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            editWindowAspect.setFormData(rec);
                            this.controller.getAspect('performedWorkActObjectCrStatePerm').setPermissionsByRecord(rec);
                        },
                        scope: this
                    }) : this.controller.getAspect('performedWorkActObjectCrStatePerm').setPermissionsByRecord(new model({ Id: 0 }));
                }
            }
        },
        {
            /*
            *аспект для импорта
            */
            xtype: 'gkhbuttonimportaspect',
            name: 'performedWorkActImportAspect',
            buttonSelector: 'perfworkactrecgrid #btnImport',
            ownerWindowSelector: 'perfactwin',
            codeImport: 'PerformedWorkAct',
            getUserParams: function () {
                var me = this,
                    actId = me.controller.getPerformedWorkActId();

                me.params = { PerformedWorkActId: actId };
            },
            refreshData: function () {
                this.controller.getStore('objectcr.performedworkact.Record').load();
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы акта вып. работ и формы редактирования
            */
            xtype: 'grideditctxwindowaspect',
            name: 'performedWorkActGridEditWindowAspect',
            gridSelector: 'perfactgrid',
            editFormSelector: 'perfactwin',
            modelName: 'objectcr.PerformedWorkAct',
            editWindowView: 'objectcr.performedworkact.EditWindow',
            otherActions: function (actions) {
                actions['perfactwin #sfTypeWorkCr'] = { 'beforeload': { fn: this.onBeforeLoadTypeWork, scope: this } };
                actions['perfactwin field[name=RepresentativeSigned]'] = { 'change': { fn: this.onRepresentativeSignedChange, scope: this } };
            },
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record);
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    asp.controller.setCurrentId(record);
                    this.controller.getAspect('performedWorkActStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                    this.controller.getAspect('performedWorkActImportAspect').loadImportStore();

                    var me = this,
                        actId = me.controller.getPerformedWorkActId(),
                        store = form.down('perfworkactphotogrid').getStore();

                    store.clearFilter(true);
                    store.filter([
                        { property: 'performedWorkActId', value: actId }
                    ]);

                    B4.Ajax.request(B4.Url.action('GetInfo', 'PerformedWorkAct', {
                        objectCrId: asp.controller.getContextValue(this.controller.getMainView(), 'objectcrId')
                    })).next(function (response) {
                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText);
                        form.down('#tfObjectCr').setValue(obj.objCrProgram);
                    });
                },
                getdata: function (asp, record) {
                    if (this.controller && !record.get('Id')) {
                        record.set('ObjectCr', this.controller.getContextValue(this.controller.getMainView(), 'objectcrId'));
                    }
                }
            },
            onBeforeLoadTypeWork: function (field, options) {
                options.params = {};
                options.params.objectCrId = this.controller.getContextValue(this.controller.getMainComponent(), 'objectcrId');
            },
            onRepresentativeSignedChange: function(field, newValue) {
                var asp = this,
                    representativeContainer = field.up('window').down('[name=RepresentativeNameContainer]'),
                    isDisabled = newValue !== B4.enums.YesNo.Yes;

                if (representativeContainer && !representativeContainer.manualDisabled) {
                    Ext.each(representativeContainer.query('field'), function(f) {
                        f.setDisabled(isDisabled);
                    });
                }
            },
            deleteRecord: function (record) {
                if (record.getId()) {
                    this.controller.getAspect('deletePerformedWorkActStatePerm').loadPermissions(record)
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
                                        me.mask('Удаление', B4.getBody());
                                        rec.destroy()
                                            .next(function () {
                                                this.fireEvent('deletesuccess', this);
                                                me.controller.getMainView().getStore().load();
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
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы акта вып. работ и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'performedWorkActPhotoGridEditWindowAspect',
            gridSelector: 'perfworkactphotogrid',
            editFormSelector: 'perfworkactphotowin',
            modelName: 'objectcr.performedworkact.PerformedWorkActPhoto',
            editWindowView: 'objectcr.performedworkact.PhotoEditWindow',

            listeners: {
                beforesave: function (asp, store) {
                    if (store.get('Id') == 0) {
                        store.set('PerformedWorkAct', asp.controller.getPerformedWorkActId());
                    }
                }
            }
        },
        {
            /*
            * Аспект взаимодействия субтаблицы записей акта вып. работ и формы редактирования
            */
            xtype: 'gkhinlinegridaspect',
            name: 'PerformedWorkActRecGridEditWindowAspect',
            gridSelector: 'perfworkactrecgrid',
            storeName: 'objectcr.performedworkact.Record',
            modelName: 'objectcr.performedworkact.Record',
            saveButtonSelector: 'perfworkactrecgrid #btnSaveRecs',
            otherActions: function (actions) {
                actions[this.gridSelector + ' #cbShowEstimMatch'] = { 'change': { fn: this.onChangeShowEstimMatch, scope: this } };
            },
            onChangeShowEstimMatch: function (checkbox, newValue) {
                var me = this,
                    actId = me.controller.getPerformedWorkActId(),
                    gridRec = me.getGrid(),
                    storeRecords = gridRec.getStore();

                storeRecords.clearFilter(true);
                storeRecords.filter([
                    { property: 'performedWorkActId', value: actId },
                    { property: 'showEstimNotMatch', value: gridRec.down('#cbShowEstimMatch').getValue() }
                ]);
            },
            listeners: {
                beforesave: function (asp, store) {
                    var me = this,
                        actId = me.controller.getPerformedWorkActId();

                    Ext.each(store.data.items, function (rec) {
                        if (!rec.get('Id')) {
                            rec.set('PerformedWorkAct', actId);
                        }
                    });
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'PerformedWorkActPaymentGridEditWindowAspect',
            gridSelector: 'perfworkactpaymentgrid',
            modelName: 'objectcr.performedworkact.Payment',
            saveButtonSelector: 'perfworkactpaymentgrid [action=savePayments]',
            editFormSelector: 'perfworkactpaymentdetailwindow',
            editWindowView: 'B4.view.objectcr.performedworkact.PaymentDetailsWindow',
            saveRecordHasNotUpload: function (rec) {
                var me = this,
                    frm = me.getForm(),
                    store = frm.down('b4grid').getStore(),
                    details = Ext.Array.map(store.getRange(), function (r) {
                        var result = r.getData();
                        result.OldAmount = r.raw.Amount;
                        return result;
                    }),
                    validationResult = me.validateDetails(details);

                if (validationResult) {
                    B4.QuickMsg.msg('Ошибка', validationResult, 'error');
                    return;
                }

                me.controller.mask('Сохранение', frm);

                if (typeof rec.get('PerformedWorkAct') == 'object') {
                    rec.set('PerformedWorkAct', rec.get('PerformedWorkAct').Id);
                }

                B4.Ajax.request({
                    url: B4.Url.action('BatchSaveWithOrder', 'PaymentOrderDetail'),
                    method: 'POST',
                    params: {
                        details: Ext.JSON.encode(details),
                        payment: Ext.JSON.encode(rec.getData())
                    },
                    timeout: 9999999
                }).next(function () {
                    me.controller.unmask();
                    frm.close();
                    me.getGrid().getStore().load();
                }).error(function (e) {
                    me.controller.unmask();
                    B4.QuickMsg.msg('Ошибка', e.message || 'Возникла ошибка при формировании распоряжения', 'error');
                });
            },

            validateDetails: function (details) {
                for (var i = 0; i < details.length; i++) {
                    if (details[i].Balance + details[i].OldAmount < details[i].Amount) {
                        return 'Недостаточно денег у источника финансирования ' + details[i].WalletName;
                    }
                }
                return false;
            },

            listeners: {
                beforesave: function (asp, store) {
                    var res = true;

                    if (store.get('Id') == 0) {
                        store.set('PerformedWorkAct', asp.controller.getPerformedWorkActId());
                    }

                    Ext.each(store.data.items, function (rec) {
                        if (rec.get('Sum') == 0) {
                            Ext.Msg.alert('Ошибка сохранения', 'Сумма оплаты должна быть больше 0!');
                            res = false;
                        }

                        if (!rec.get('DatePayment')) {
                            Ext.Msg.alert('Ошибка сохранения', 'Необходимо заполнить дату оплаты!');
                            res = false;
                        }

                        if (!rec.get('Id')) {
                            rec.set('PerformedWorkAct', asp.controller.getPerformedWorkActId());
                        }
                    });

                    return res;
                },

                aftersetformdata: function (aspect, rec, form) {
                    var me = this,
                        actId = me.controller.getPerformedWorkActId(),
                        store = form.down('b4grid').getStore();

                    store.clearFilter(true);
                    store.filter([
                        { property: 'paymentId', value: rec.getId() },
                        { property: 'performedWorkActId', value: actId }
                    ]);
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'perfactwin': {
                'close': function () {
                    me.getPerformedWorkActGrid().getStore().load();
                }
            }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('perfactgrid'),
            token = Ext.History.getToken(),
            splitted = token.split('?'),
            store = view.getStore();

        if (splitted.length > 1) {
            me.getAspect('performedWorkActGridEditWindowAspect').editRecord({ getId: function () { return splitted[1]; } });

            me.application.redirectTo(splitted[0]);
        }

        me.getAspect('performedWorkActObjectCrCreatePerm').setPermissionsByRecord({ getId: function () { return id; } });

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'objectcr_info');

        store.on('beforeload', me.onBeforeLoad, me);
        store.load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.objectCrId = this.getContextValue(this.getMainView(), 'objectcrId');
    },

    getPerformedWorkActId: function () {
        var me = this,
            win = me.getPerformedWorkActWin();

        return win.getForm().getRecord().get('Id');
    },

    setCurrentId: function (record) {
        var me = this,
            editWindow = me.getPerformedWorkActWin(),
            gridRec = editWindow.down('perfworkactrecgrid'),
            gridPayment = editWindow.down('perfworkactpaymentgrid'),
            storeRecords = gridRec.getStore(),
            storePayments = gridPayment.getStore(),
            num = editWindow.down('[name=DocumentNum]');

        storeRecords.removeAll();

        if (record.getId() > 0) {
            num.setValue(record.get("DocumentNum"));
            num.setReadOnly(false);

            editWindow.down('perfworkactrecgrid').setDisabled(false);
            editWindow.down('#tabPayment').setDisabled(false);

            storeRecords.clearFilter(true);
            storeRecords.filter([
                { property: 'performedWorkActId', value: record.get('Id') },
                { property: 'showEstimMatch', value: gridRec.down('[actionName=cbShowEstimMatch]').getValue() }
            ]);

            storePayments.clearFilter(true);
            storePayments.filter('performedWorkActId', record.get('Id'));
        } else {
            num.setReadOnly(true);

            editWindow.down('perfworkactrecgrid').setDisabled(true);
            editWindow.down('#tabPayment').setDisabled(true);
        }
    }
});