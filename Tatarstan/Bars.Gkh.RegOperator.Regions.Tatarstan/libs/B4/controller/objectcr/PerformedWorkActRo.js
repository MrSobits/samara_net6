// Данный контроллер вынесен поскольку необходимо переделать грид с оплатами и правки по обязательности полей 

Ext.define('B4.controller.objectcr.PerformedWorkActRo', {
    /*
    * Контроллер раздела Акты выполненных работ
    */
    extend: 'B4.base.Controller',
    performedWorkActId: 0,
    params: null,
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.StateButton',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhButtonImportAspect',
        'B4.aspects.permission.objectcr.PerformedWorkAct',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu',
        'B4.aspects.PerformedWorkActDetails',
        'B4.QuickMsg'
    ],

    models: [
        'objectcr.PerformedWorkAct',
        'objectcr.performedworkact.Record',
        'objectcr.performedworkact.Payment',
        'PaymentOrderDetail'
    ],

    stores: [
        'objectcr.PerformedWorkAct',
        'objectcr.performedworkact.Record',
        'objectcr.performedworkact.Payment',
        'objectcr.performedworkact.TypeWorkCr',
        'objectcr.TypeWorkCr',
        'workactregister.Details'
    ],

    views: [
        'objectcr.performedworkact.Grid',
        'objectcr.performedworkact.EditWindow',
        'objectcr.performedworkact.RecGrid',
        'objectcr.performedworkact.PaymentGrid',
        'objectcr.performedworkact.PaymentDetailsWindow',
        'objectcr.performedworkact.PaymentDetailsGrid',
        'objectcr.TypeWorkCrGrid',
        'workactregister.DetailsPanel',
        'B4.view.Import.Window'
    ],

    mixins: {
        //context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'objectcr.performedworkact.Grid',
    mainViewSelector: '#performedWorkActGrid',

    perfWorkActEditWindowSelector: 'perfactwin',
    perfWorkActRecGridSelector: 'perfworkactrecgrid',

    refs: [
        {
            ref: 'performedWorkActGrid',
            selector: '#performedWorkActGrid'
        }
    ],

    aspects: [
        /* пермишшены по статусы Акта выполненных работ */
        {
            xtype: 'gkhstatepermissionaspect',
            editFormAspectName: 'performedWorkActGridEditWindowAspect',
            setPermissionEvent: 'aftersetformdata',
            name: 'performedWorkActObjectCrStatePerm',
            permissions: [
                { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Edit', applyTo: 'b4savebutton', selector: 'perfactwin' },
                { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Work', applyTo: '#sfTypeWorkCr', selector: 'perfactwin' },
                { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.DocumentNum', applyTo: '#tfDocumentNum', selector: 'perfactwin' },
                { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Sum', applyTo: '#nfSum', selector: 'perfactwin' },
                { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Volume', applyTo: '#nfVolume', selector: 'perfactwin' },
                { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.DateFrom', applyTo: '#dfDateFrom', selector: 'perfactwin' },
                {
                    name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.CostFile', applyTo: 'b4filefield[name="CostFile"]', selector: 'perfactwin',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.DocumentFile', applyTo: 'b4filefield[name="DocumentFile"]', selector: 'perfactwin',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.AdditionFile', applyTo: 'b4filefield[name="AdditionFile"]', selector: 'perfactwin',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Rec.Edit', applyTo: '#btnSaveRecs', selector: 'perfworkactrecgrid' },
                {
                    name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Rec.Delete', applyTo: 'b4deletecolumn', selector: 'perfworkactrecgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Payment.View', applyTo: '#tabPayment', selector: 'perfactwin',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.tab.show();
                        } else {
                            component.tab.hide();
                        }
                    }
                },
                { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Payment.Create', applyTo: 'b4addbutton', selector: 'perfworkactpaymentgrid' },
                { name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Payment.Edit', applyTo: '[action=savePayments]', selector: 'perfworkactpaymentgrid' },
                {
                    name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Payment.Delete', applyTo: 'b4deletecolumn', selector: 'perfworkactpaymentgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'GkhCr.ObjectCr.Register.PerformedWorkAct.Field.Payment.Export', applyTo: '[action=export]', selector: 'perfworkactpaymentgrid',
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
        /* пермишшен по статусу Объекта КР */
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'performedWorkActObjectCrCreatePerm',
            permissions: [
                { name: 'GkhCr.ObjectCr.Register.PerformedWorkActViewCreate.Create', applyTo: 'b4addbutton', selector: '#performedWorkActGrid' }
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
            gridSelector: '#performedWorkActGrid',
            stateType: 'cr_obj_performed_work_act',
            menuSelector: 'performedWorkActGridStateMenu'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'performedWorkActStateButtonAspect',
            stateButtonSelector: 'perfactwin #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState);
                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    var editWindowAspect = asp.controller.getAspect('performedWorkActGridEditWindowAspect');
                    editWindowAspect.updateGrid();
                    var model = asp.controller.getModel('objectcr.PerformedWorkAct');
                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            editWindowAspect.setFormData(rec);
                            asp.controller.getAspect('performedWorkActObjectCrStatePerm').setPermissionsByRecord(rec);
                        },
                        scope: this
                    }) : asp.controller.getAspect('performedWorkActObjectCrStatePerm').setPermissionsByRecord(new model({ Id: 0 }));
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
                this.params = { PerformedWorkActId: this.controller.performedWorkActId };
            },
            refreshData: function () {
                this.controller.getStore('objectcr.performedworkact.Record').load();
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы акта вып. работ и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'performedWorkActGridEditWindowAspect',
            gridSelector: '#performedWorkActGrid',
            editFormSelector: 'perfactwin',
            storeName: 'objectcr.PerformedWorkAct',
            modelName: 'objectcr.PerformedWorkAct',
            editWindowView: 'objectcr.performedworkact.EditWindow',
            otherActions: function (actions) {
                actions['perfactwin #sfTypeWorkCr'] = {
                    'beforeload': { fn: this.onBeforeLoadTypeWork, scope: this },
                    'change': { fn: this.onChangeTypeWork, scope: this }
                };
            },
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record);
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var volumeField = form.down('[name=Volume]');

                    asp.controller.setCurrentId(record);
                    asp.controller.getAspect('performedWorkActStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                    asp.controller.getAspect('performedWorkActImportAspect').loadImportStore();

                    B4.Ajax.request(B4.Url.action('GetInfo', 'PerformedWorkAct', {
                        objectCrId: asp.controller.params.get('Id')
                    })).next(function (response) {
                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText);
                        form.down('#tfObjectCr').setValue(obj.objCrProgram);
                    });

                    volumeField.allowBlank = !record.get('IsWork');
                    volumeField.validate();
                },
                getdata: function (asp, record) {
                    if (this.controller.params && !record.get('Id')) {
                        record.set('ObjectCr', this.controller.params.get('Id'));
                    }
                }
            },
            onBeforeLoadTypeWork: function (field, options) {
                options.params = {};
                options.params.objectCrId = this.controller.params.get("Id");
            },
            onChangeTypeWork: function (field, newValue) {
                var volumeField = this.getForm().down('[name=Volume]');
                if (newValue) {
                    volumeField.allowBlank = newValue.TypeWork != 10;
                    volumeField.validate();
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
                                                me.controller.getStore(me.storeName).load();
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
                this.controller.getStore('objectcr.performedworkact.Record').load();
            },
            listeners: {
                beforesave: function (asp, store) {
                    Ext.each(store.data.items, function (rec) {
                        if (!rec.get('Id')) {
                            rec.set('PerformedWorkAct', asp.controller.performedWorkActId);
                        }
                    });
                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы оплат с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'PerformedWorkActPaymentGridEditWindowAspect',
            gridSelector: 'perfworkactpaymentgrid',
            editFormSelector: 'perfworkactpaymentdetailwindow',
            storeName: 'objectcr.performedworkact.Payment',
            modelName: 'objectcr.performedworkact.Payment',
            editWindowView: 'objectcr.performedworkact.PaymentDetailsWindow',
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

                if (typeof rec.get('PerformedWorkAct') == "object") {
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
                aftersetformdata: function (asp, rec, form) {
                    var store = form.down('b4grid').getStore();

                    store.clearFilter(true);

                    store.on('beforeload', function (self, operation) {
                        operation.params = operation.params || {};
                        operation.params.paymentId = rec.getId();
                        operation.params.performedWorkActId = asp.controller.performedWorkActId;
                    });

                    store.load();
                },
                beforesave: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('PerformedWorkAct', { Id: asp.controller.performedWorkActId });
                    }
                }
            }
        },
        {
            xtype: 'performedworkactdetails',
            name: 'performedWorkActDetailsAspect',
            showDetailsButtonSelector: 'perfactgrid button[action=ShowDetails]',
            listeners: {
                aftersetformdata: function (asp, detailsPanel) {
                    var me = this,
                        programField = detailsPanel.down('[name=Program]'),
                        municipalityField = detailsPanel.down('[name=Municipality]'),
                        addressField = detailsPanel.down('[name=Address]');

                    programField.setValue(me.controller.params.get('ProgramCrName') || '-');
                    municipalityField.setValue(me.controller.params.get('Municipality') || '-');
                    addressField.setValue(me.controller.params.get('RealityObjName') || '-');
                }
            },
            onBeforeLoad: function (store, operation) {
                var me = this;
                if (me.controller.params) {
                    operation.params.objectCrId = me.controller.params.get('Id');
                }
            },
            getParams: function() {
                var me = this,
                    params = {};

                if (me.controller.params) {
                    params.objectCrId = me.controller.params.get('Id');
                }

                return params;
            }
        }

    ],

    init: function () {
        var me = this;
        me.getStore('objectcr.PerformedWorkAct').on('beforeload', me.onBeforeLoad, me);
        me.getStore('objectcr.PerformedWorkAct').on('beforeload', me.onBeforeLoad, me);
        me.getStore('objectcr.performedworkact.Record').on('beforeload', me.onBeforeLoadRec, me);
        me.getStore('objectcr.performedworkact.Payment').on('beforeload', me.onBeforeLoadPayment, me);

        me.control({
            'perfactwin': {
                'close': function () {
                    me.getPerformedWorkActGrid().getStore().load();
                }
            },
            'perfworkactpaymentgrid button[action=export]': {
                'click': { fn: me.exportToTxt, scope: me }
            }
        });
        me.callParent(arguments);
    },

    exportToTxt: function (btn) {
        var me = this,
            grid = btn.up('grid'),
            win = btn.up('window'),
            record = grid.getSelectionModel().getSelection()[0];

        var selection = grid.getSelectionModel().getSelection();
        var ids = [];
        selection.forEach(function (entry) {
            ids.push(entry.getId());
        });

        if (record) {
            me.mask("Обработка...", win);
            B4.Ajax.request({
                url: B4.Url.action('ExportToTxt', 'PerfWorkActPayment', { paymentId: ids.join(',') }),
                timeout: 9999999
            }).next(function (resp) {
                me.unmask();

                var tryDecoded;
                try {
                    tryDecoded = Ext.JSON.decode(resp.responseText);
                } catch (e) {
                    tryDecoded = {};
                }

                var data = resp.data ? resp.data : tryDecoded,
                    message = resp.message ? resp.message : tryDecoded.message;

                if (data && data.Id) {
                    var frame = Ext.get('downloadIframe');
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
            }).error(function (err) {
                me.unmask();
                Ext.Msg.alert('Ошибка', err.message || err.message || err);
            });
        } else {
            Ext.Msg.alert('Ошибка', "Необходимо выбрать оплату!");
        }
    },

    onLaunch: function () {
        this.getStore('objectcr.PerformedWorkAct').load();

        if (this.params && this.params.record) {
            this.getAspect('performedWorkActGridEditWindowAspect').editRecord(this.params.record);

            delete this.params.record;
            delete this.params.childController;
        }

        if (this.params) {
            this.getAspect('performedWorkActObjectCrCreatePerm').setPermissionsByRecord(this.params);
        }
    },

    setCurrentId: function (record) {
        this.performedWorkActId = record.getId();

        var editWindow = Ext.ComponentQuery.query(this.perfWorkActEditWindowSelector)[0];

        var storeRecords = this.getStore('objectcr.performedworkact.Record');
        var storePayments = this.getStore('objectcr.performedworkact.Payment');
        storeRecords.removeAll();

        var num = editWindow.down("#tfDocumentNum");
        if (this.performedWorkActId > 0) {
            num.setValue(record.get("DocumentNum"));
            num.setReadOnly(false);

            editWindow.down('perfworkactrecgrid').setDisabled(false);
            editWindow.down('#tabPayment').setDisabled(false);
            storeRecords.load();
            storePayments.load();
        } else {
            num.setReadOnly(true);

            editWindow.down('perfworkactrecgrid').setDisabled(true);
            editWindow.down('#tabPayment').setDisabled(true);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.objectCrId = this.params.get('Id');
        }
    },

    onBeforeLoadRec: function (store, operation) {
        operation.params.performedWorkActId = this.performedWorkActId;

        var recGrid = Ext.ComponentQuery.query(this.perfWorkActRecGridSelector)[0];
        if (recGrid) {
            operation.params.showEstimMatch = recGrid.down('#cbShowEstimMatch').getValue();
        }
    },

    onBeforeLoadPayment: function (store, operation) {
        operation.params.performedWorkActId = this.performedWorkActId;
    }
});