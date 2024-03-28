/*
* Контроллер раздела Акты выполненных работ
*/
Ext.define('B4.controller.workscr.PerfAct', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.StateButton',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhButtonImportAspect',
        'B4.aspects.permission.typeworkcr.PerformedWorkAct',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu'
    ],

    models: [
        'objectcr.PerformedWorkAct',
        'objectcr.performedworkact.Record',
        'objectcr.performedworkact.Payment'
    ],

    stores: [
        'objectcr.PerformedWorkAct',
        'objectcr.performedworkact.Record',
        'objectcr.performedworkact.Payment',
        'objectcr.performedworkact.TypeWorkCr',
        'objectcr.TypeWorkCr'
    ],

    views: [
        'objectcr.performedworkact.Grid',
        'objectcr.performedworkact.EditWindow',
        'objectcr.performedworkact.RecGrid',
        'objectcr.performedworkact.PaymentGrid',
        'objectcr.TypeWorkCrGrid',
        'B4.view.Import.Window'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'objectcr.performedworkact.Grid',
    mainViewSelector: 'perfactgrid',

    refs: [
        {
            ref: 'grid',
            selector: 'perfactgrid'
        },
        {
            ref: 'performedWorkActGrid',
            selector: 'perfactgrid'
        },
        {
            ref: 'editWindow',
            selector: 'perfactwin'
        },
        {
            ref: 'recordGrid',
            selector: 'perfworkactrecgrid'
        },
        {
            ref: 'paymentGrid',
            selector: 'perfworkactpaymentgrid'
        }
    ],

    aspects: [
        /* пермишшены по статусы Акта выполненных работ */
        {
            xtype: 'performedworkacttypeworkcrstateperm',
            editFormAspectName: 'performedWorkActGridEditWindowAspect',
            setPermissionEvent: 'aftersetformdata',
            name: 'performedWorkActObjectCrStatePerm'
        },
        /* пермишшен по статусу Объекта КР */
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'performedWorkActObjectCrCreatePerm',
            permissions: [
                { name: 'GkhCr.TypeWorkCr.Register.PerformedWorkActViewCreate.Create', applyTo: 'b4addbutton', selector: 'perfactgrid' }
            ]
        },
        /* пермишшен по удалению записи Акта выполненных работ(по его статусы), вынесен в отдельный аспект для  удобства */
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'GkhCr.TypeWorkCr.Register.PerformedWorkAct.Delete' }],
            name: 'deletePerformedWorkActStatePerm'
        },
        {
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'performedWorkActStateTransferAspect',
            gridSelector: 'perfactgrid',
            stateType: 'cr_obj_performed_work_act',
            menuSelector: 'performedWorkActGridStateMenu'
        },
        {
            /* Вешаем аспект смены статуса в карточке редактирования */
            xtype: 'statebuttonaspect',
            name: 'performedWorkActStateButtonAspect',
            stateButtonSelector: 'perfactwin #btnState',
            listeners: {
                transfersuccess: function(me, entityId, newState) {
                    var aspect = me.controller.getAspect('performedWorkActGridEditWindowAspect'),
                        model = me.controller.getModel('objectcr.PerformedWorkAct');

                    //Если статус изменен успешно, то проставляем новый статус
                    me.setStateData(entityId, newState);

                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    aspect.updateGrid();

                    entityId ? model.load(entityId, {
                        success: function(rec) {
                            aspect.setFormData(rec);
                            me.controller.getAspect('performedWorkActObjectCrStatePerm').setPermissionsByRecord(rec);
                        },
                        scope: me
                    }) : me.controller.getAspect('performedWorkActObjectCrStatePerm').setPermissionsByRecord(new model({ Id: 0 }));
                }
            }
        },
        {
            /*
            * аспект для импорта
            */
            xtype: 'gkhbuttonimportaspect',
            name: 'performedWorkActImportAspect',
            buttonSelector: 'perfworkactrecgrid #btnImport',
            ownerWindowSelector: 'perfactwin',
            codeImport: 'PerformedWorkAct',
            getUserParams: function() {
                this.params = { PerformedWorkActId: this.controller.performedWorkActId };
            },
            refreshData: function() {
                this.controller.getStore('objectcr.performedworkact.Record').load();
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы акта вып. работ и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'performedWorkActGridEditWindowAspect',
            gridSelector: 'perfactgrid',
            editFormSelector: 'perfactwin',
            storeName: 'objectcr.PerformedWorkAct',
            modelName: 'objectcr.PerformedWorkAct',
            editWindowView: 'objectcr.performedworkact.EditWindow',
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var workField = form.down('b4selectfield[name=TypeWorkCr]');
                    asp.setCurrentId(record.get('Id'));
                    asp.controller.getAspect('performedWorkActStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                    asp.controller.getAspect('performedWorkActImportAspect').loadImportStore();

                    workField.setVisible(false);
                    workField.allowBlank = true;

                    B4.Ajax.request(B4.Url.action('GetInfo', 'PerformedWorkAct', {
                        objectCrId: asp.controller.getObjectId()
                    })).next(function(response) {
                        var obj = Ext.JSON.decode(response.responseText);
                        form.down('#tfObjectCr').setValue(obj.objCrProgram);
                    });
                },
                getdata: function(asp, record) {
                    if (!+record.get('Id')) {
                        record.set('ObjectCr', asp.controller.getObjectId());
                        record.set('TypeWorkCr', asp.controller.getTypeWorkId());
                    }
                }
            },
            onSaveSuccess: function(asp, record) {
                asp.setCurrentId(record.get('Id'));
            },
            setCurrentId: function(id) {
                var me = this,
                    win = me.controller.getEditWindow(),
                    recGrid = win.down('perfworkactrecgrid'),
                    paymentTab = win.down('#tabPayment');

                recGrid.setDisabled(!id);
                paymentTab.setDisabled(!id);

                if (id > 0) {
                    recGrid.getStore().load();
                    paymentTab.down('grid').getStore().load();
                } else {
                    recGrid.removeAll();
                    paymentTab.down('grid').removeAll();
                }
            },
            deleteRecord: function(record) {
                var me = this,
                    model, rec, grants;
                if (record.getId()) {
                    me.controller.getAspect('deletePerformedWorkActStatePerm').loadPermissions(record)
                        .next(function(response) {
                            grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                                    if (result == 'yes') {
                                        model = me.getModel(record);

                                        rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', B4.getBody());
                                        rec.destroy()
                                            .next(function() {
                                                me.fireEvent('deletesuccess', me);
                                                me.controller.getStore(me.storeName).load();
                                                me.unmask();
                                            })
                                            .error(function(result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            });
                                    }
                                });
                            }
                        });
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
            otherActions: function(actions) {
                var me = this;
                actions[me.gridSelector + ' #cbShowEstimMatch'] = { 'change': { fn: me.onChangeShowEstimMatch, scope: me } };
            },
            onChangeShowEstimMatch: function() {
                this.controller.getStore('objectcr.performedworkact.Record').load();
            },
            listeners: {
                beforesave: function(asp, store) {
                    var perfActId = asp.controller.getPerfActId();
                    Ext.each(store.data.items, function(rec) {
                        if (!rec.get('Id')) {
                            rec.set('PerformedWorkAct', perfActId);
                        }
                    });
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'PerformedWorkActPaymentGridEditWindowAspect',
            gridSelector: 'perfworkactpaymentgrid',
            storeName: 'objectcr.performedworkact.Payment',
            modelName: 'objectcr.performedworkact.Payment',
            saveButtonSelector: 'perfworkactpaymentgrid [action=savePayments]',
            listeners: {
                beforesave: function(asp, store) {
                    var res = true;

                    Ext.each(store.data.items, function(rec) {
                        if (rec.get('Sum') == 0) {
                            Ext.Msg.alert('Ошибка сохранения', 'Сумма оплаты должна быть больше 0!');
                            res = false;
                        }

                        if (!rec.get('DatePayment')) {
                            Ext.Msg.alert('Ошибка сохранения', 'Необходимо заполнить дату оплаты!');
                            res = false;
                        }

                        if (!rec.get('Id')) {
                            rec.set('PerformedWorkAct', asp.controller.performedWorkActId);
                        }

                        return res;
                    });

                    return res;
                }
            }
        }
    ],

    init: function() {
        var me = this;

        me.control({
            'perfworkactrecgrid': {
                'afterrender': function(grid) {
                    grid.getStore().on('beforeload', function(s, operation) {
                        operation.params.performedWorkActId = me.getPerfActId();
                        operation.params.showEstimMatch = me.getRecordGrid().down('checkbox[itemId=cbShowEstimMatch]').getValue();
                    });
                }
            },
            'perfworkactpaymentgrid': {
                'afterrender': function(grid) {
                    grid.getStore().on('beforeload', function(s, operation) {
                        operation.params.performedWorkActId = me.getPerfActId();
                    });
                }
            }
        });

        me.callParent(arguments);
    },

    index: function(id, objectId) {
        var me = this,
            view = me.getMainView();

        if (!view) {
            view = Ext.widget('perfactgrid');

            view.getStore().on('beforeload',
                function(s, operation) {
                    operation.params.twId = id;
                    operation.params.objectCrId = objectId;
                });
        }

        me.bindContext(view);
        me.setContextValue(view, 'twId', id);
        me.setContextValue(view, 'objectId', objectId);
        me.application.deployView(view, 'works_cr_info');

        view.getStore().load();
    },

    getTypeWorkId: function() {
        var me = this;
        return me.getContextValue(me.getMainView(), 'twId');
    },

    getObjectId: function() {
        var me = this;
        return me.getContextValue(me.getMainView(), 'objectId');
    },

    getPerfActId: function() {
        var editWindow = this.getEditWindow();

        if (editWindow) {
            return editWindow.down('hidden[name=Id]').getValue();
        }

        return null;
    }
});