Ext.define('B4.controller.specialobjectcr.PerformedWorkAct', {
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
        'B4.aspects.permission.specialobjectcr.PerformedWorkAct',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu'
    ],

    models: [
        'specialobjectcr.PerformedWorkAct',
        'specialobjectcr.performedworkact.Record',
    ],

    stores: [
        'specialobjectcr.PerformedWorkAct',
        'specialobjectcr.performedworkact.Record',
        'specialobjectcr.performedworkact.TypeWorkCr',
        'specialobjectcr.TypeWorkCr'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    views: [
        'specialobjectcr.performedworkact.Grid',
        'specialobjectcr.performedworkact.EditWindow',
        'specialobjectcr.performedworkact.RecGrid',
        'specialobjectcr.TypeWorkCrGrid',
        'B4.view.Import.Window'
    ],

    mainView: 'specialobjectcr.performedworkact.Grid',
    mainViewSelector: 'specialobjectcrperfactgrid',

    perfWorkActEditWindowSelector: 'specialobjectcrperfactwin',
    perfWorkActRecGridSelector: 'specialobjectcrperfworkactrecgrid',

    refs: [
        {
            ref: 'performedWorkActGrid',
            selector: 'specialobjectcrperfactgrid'
        },
        {
            ref: 'performedWorkActWin',
            selector: 'specialobjectcrperfactwin'
        },
        {
            ref: 'performedWorkActRecGrid',
            selector: 'specialobjectcrperfworkactrecgrid'
        }
    ],

    parentCtrlCls: 'B4.controller.specialobjectcr.Navi',

    aspects: [
        /* пермишшены по статусы Акта выполненных работ */
        {
            xtype: 'performedworkactspecialobjectcrstateperm',
            editFormAspectName: 'performedWorkActGridEditWindowAspect',
            setPermissionEvent: 'aftersetformdata',
            name: 'performedWorkActObjectCrStatePerm'
        },
        /* пермишшен по статусу Объекта КР */
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'performedWorkActObjectCrCreatePerm',
            permissions: [
                { name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkActViewCreate.Create', applyTo: 'b4addbutton', selector: 'specialobjectcrperfactgrid' }
            ]
        },
        /* пермишшен по удалению записи Акта выполненных работ(по его статусы), вынесен в отдельный аспект для  удобства */
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [
                { name: 'GkhCr.SpecialObjectCr.Register.PerformedWorkAct.Delete' }
            ],
            name: 'deletePerformedWorkActStatePerm'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'performedWorkActStateTransferAspect',
            gridSelector: 'specialobjectcrperfactgrid',
            stateType: 'special_cr_obj_performed_work_act',
            menuSelector: 'performedWorkActGridStateMenu'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statecontextbuttonaspect',
            name: 'performedWorkActStateButtonAspect',
            stateButtonSelector: 'specialobjectcrperfactwin #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState);
                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    var editWindowAspect = asp.controller.getAspect('performedWorkActGridEditWindowAspect');
                    editWindowAspect.updateGrid();
                    var model = this.controller.getModel('specialobjectcr.PerformedWorkAct');
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
            buttonSelector: 'specialobjectcrperfworkactrecgrid #btnImport',
            ownerWindowSelector: 'specialobjectcrperfactwin',
            codeImport: 'PerformedWorkAct',
            getUserParams: function () {
                var me = this,
                    actId = me.controller.getPerformedWorkActId();

                me.params = { PerformedWorkActId: actId };
            },
            refreshData: function () {
                this.controller.getStore('specialobjectcr.performedworkact.Record').load();
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы акта вып. работ и формы редактирования
            */
            xtype: 'grideditctxwindowaspect',
            name: 'performedWorkActGridEditWindowAspect',
            gridSelector: 'specialobjectcrperfactgrid',
            editFormSelector: 'specialobjectcrperfactwin',
            modelName: 'specialobjectcr.PerformedWorkAct',
            editWindowView: 'specialobjectcr.performedworkact.EditWindow',
            otherActions: function (actions) {
                actions[this.editFormSelector + ' [name=TypeWorkCr]'] = { 'beforeload': { fn: this.onBeforeLoadTypeWork, scope: this } };
                actions[this.editFormSelector + ' field[name=RepresentativeSigned]'] = { 'change': { fn: this.onRepresentativeSignedChange, scope: this } };
            },
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record);
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    asp.controller.setCurrentId(record);
                    this.controller.getAspect('performedWorkActStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                    this.controller.getAspect('performedWorkActImportAspect').loadImportStore();

                    B4.Ajax.request(B4.Url.action('GetInfo', 'SpecialPerformedWorkAct', {
                        objectCrId: asp.controller.getContextValue(this.controller.getMainView(), 'objectcrId')
                    })).next(function (response) {
                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText);
                        form.down('[name=ObjectCrName]').setValue(obj.objCrProgram);
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
            * Аспект взаимодействия субтаблицы записей акта вып. работ и формы редактирования
            */
            xtype: 'gkhinlinegridaspect',
            name: 'PerformedWorkActRecGridEditWindowAspect',
            gridSelector: 'specialobjectcrperfworkactrecgrid',
            storeName: 'specialobjectcr.performedworkact.Record',
            modelName: 'specialobjectcr.performedworkact.Record',
            saveButtonSelector: 'specialobjectcrperfworkactrecgrid #btnSaveRecs',
            otherActions: function (actions) {
                actions[this.gridSelector + ' [actionName=cbShowEstimMatch]'] = { 'change': { fn: this.onChangeShowEstimMatch, scope: this } };
            },
            onChangeShowEstimMatch: function (checkbox, newValue) {
                var me = this,
                    actId = me.controller.getPerformedWorkActId(),
                    gridRec = me.getGrid(),
                    storeRecords = gridRec.getStore();

                storeRecords.clearFilter(true);
                storeRecords.filter([
                    { property: 'performedWorkActId', value: actId },
                    { property: 'showEstimNotMatch', value: gridRec.down('[actionName=cbShowEstimMatch]').getValue() } //??
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
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'specialobjectcrperfactgrid': {
                'beforerender': me.onLoad
            },
            'specialobjectcrperfactwin': {
                'close': function () {
                    me.getPerformedWorkActGrid().getStore().load();
                }
            }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('specialobjectcrperfactgrid'),
            token = Ext.History.getToken(),
            splitted = token.split('?'),
            store;

        if (splitted.length > 1) {
            me.getAspect('performedWorkActGridEditWindowAspect').editRecord({ getId: function () { return splitted[1]; } });

            me.application.redirectTo(splitted[0]);
        }

        me.getAspect('performedWorkActObjectCrCreatePerm').setPermissionsByRecord({ getId: function() { return id; } });

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'specialobjectcr_info');
    },

    onLoad: function (view) {
        var me = this,
            objectCrId = me.getContextValue(view, 'objectcrId');

        view.getStore().on({
            beforeload: function (curStore, operation) {
                operation.params = operation.params || {};
                operation.params.objectCrId = objectCrId;
            }
        });
        view.getStore().load();
    },

    getPerformedWorkActId: function () {
        var me = this,
            win = me.getPerformedWorkActWin();

        return win.getForm().getRecord().get('Id');
    },

    setCurrentId: function (record) {
        var me = this,
            editWindow = me.getPerformedWorkActWin(),
            gridRec = editWindow.down('specialobjectcrperfworkactrecgrid'),
            storeRecords = gridRec.getStore(),
            num = editWindow.down('[name=DocumentNum]'),
            isCreated = record.getId() > 0;

        storeRecords.removeAll();
        gridRec.setDisabled(!isCreated);
        num.setReadOnly(!isCreated);

        if (isCreated) {
            num.setValue(record.get("DocumentNum"));

            storeRecords.clearFilter(true);
            storeRecords.filter([
                { property: 'performedWorkActId', value: record.get('Id') },
                { property: 'showEstimMatch', value: gridRec.down('[actionName=cbShowEstimMatch]').getValue() }
            ]);
        } 
    }
});