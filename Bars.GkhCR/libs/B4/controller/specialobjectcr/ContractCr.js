Ext.define('B4.controller.specialobjectcr.ContractCr', {
    /*
    * Контроллер раздела договора кап. ремонта
    */
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.permission.specialobjectcr.ContractCr',
        'B4.aspects.permission.specialobjectcr.ContractCrType',
        'B4.aspects.permission.specialobjectcr.ContractCrView',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.StateContextButton',
        'B4.aspects.permission.specialobjectcr.BuildContract',
        'B4.aspects.permission.specialobjectcr.BuildContractView',
        'B4.aspects.permission.specialobjectcr.BuildContractType',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.model.Contragent',
        'B4.enums.BuildContractState',
        'B4.enums.YesNo'
    ],

    models: [
        'specialobjectcr.ContractCr',
        'specialobjectcr.BuildContract',
        'specialobjectcr.BuildContractBuilder',
        'specialobjectcr.BuildContractTypeWork',
        'specialobjectcr.ContractCrTypeWork',
        'Contragent'
    ],
    stores: [
        'specialobjectcr.ContractCr',
        'specialobjectcr.BuildContract',
        'specialobjectcr.BuildContractBuilder',
        'specialobjectcr.BuildContractTypeWork',
        'specialobjectcr.ContractCrTypeWork',
        'specialobjectcr.TypeWorkCrForSelect',
        'specialobjectcr.TypeWorkCrForSelected'
    ],
    views: [
        'specialobjectcr.ContractCrGrid',
        'specialobjectcr.ContractCrEditWindow',
        'specialobjectcr.ContractPanel',
        'specialobjectcr.BuildContractEditWindow',
        'specialobjectcr.BuildContractGrid',
        'specialobjectcr.BuildContractTypeWorkGrid',
        'specialobjectcr.ContractCrTypeWorkGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.specialobjectcr.Navi',
    regOpContragent: null,

    mainView: 'specialobjectcr.ContractPanel',
    mainViewSelector: 'specialobjectcrcontractpanel',

    aspects: [
        {
            xtype: 'contractspecialobjectcrperm',
            name: 'contracteditPermissionAspect',
            editFormAspectName: 'contractCrGridWindowAspect',
            setPermissionEvent: 'aftersetformdata'
        },
        //аспект пермишена типов Договора на услугу
        {
            xtype: 'contractspecialobjectcrtypeperm',
            name: 'contractTypePermissionAspect',
            editFormAspectName: 'contractCrGridWindowAspect',
            setPermissionEvent: 'aftersetformdata'
        },
        {
            xtype: 'contractspecialobjectcrviewperm',
            name: 'contractViewPermissionAspect',
            editFormAspectName: 'contractCrGridWindowAspect',
            setPermissionEvent: 'aftersetformdata'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'contractcreatepermissionaspect',
            permissions: [
                { name: 'GkhCr.SpecialObjectCr.Register.ContractCrViewCreate.Create', applyTo: 'b4addbutton', selector: 'specialobjectcrcontractgrid' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'contractdeletepermissionaspect',
            permissions: [
               { name: 'GkhCr.SpecialObjectCr.Register.ContractCr.Delete' }
            ]
        },
        {
            /**
            * Аспект смены статуса в гриде
            */
            xtype: 'b4_state_contextmenu',
            name: 'contractStateTransferAspect',
            gridSelector: 'specialobjectcrcontractgrid',
            stateType: 'special_cr_obj_contract',
            menuSelector: 'contractCrGridStateMenu'
        },
        {
            /**
            * Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statecontextbuttonaspect',
            name: 'contractStateButtonAspect',
            stateButtonSelector: 'specialobjectcrcontractwin #btnState',
            listeners: {
                transfersuccess: function (me, entityId, newState) {
                    var aspect = me.controller.getAspect('contractCrGridWindowAspect'),
                        model = me.controller.getModel(aspect.modelName);
                    //Если статус изменен успешно, то проставляем новый статус
                    me.setStateData(entityId, newState);

                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    aspect.updateGrid();

                    model.load(entityId, {
                        success: function(rec) {
                            aspect.setFormData(rec);
                        }
                    });
                }
            }
        },
        {
            /**
            * Аспект взаимодействия таблицы и формы редактирования договора кап. ремонта
            */
            xtype: 'grideditctxwindowaspect',
            name: 'contractCrGridWindowAspect',
            gridSelector: 'specialobjectcrcontractgrid',
            editFormSelector: 'specialobjectcrcontractwin',
            modelName: 'specialobjectcr.ContractCr',
            editWindowView: 'specialobjectcr.ContractCrEditWindow',
            onSaveSuccess: function (aspect) {
                aspect.controller.getAspect('ContrCrTypeWrkMultiSelectWindowAspect').save();
            },
            listeners: {
                getdata: function(asp, record) {
                    if (!record.data.Id) {
                        record.data.ObjectCr = asp.controller.getContextValue(asp.controller.getMainView(), 'objectcrId');
                    }
                },
                beforesetformdata: function (asp, rec) {
                    var me = asp.controller,
                        customer = rec.get('Customer');

                    if (!customer) {
                        rec.set('Customer', me.regOpContragent);
                    }
                },
                aftersetformdata: function (asp, rec) {
                    var form = asp.getForm(),
                        typeWorkGrid = form.down('contracttypeworkspecialcrgrid'),
                        typeWorkStore = typeWorkGrid.getStore();

                    this.controller.getAspect('contractStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                    if (!typeWorkGrid.params) {
                        typeWorkGrid.params = {};
                    }

                    typeWorkGrid.params.contractCrId = rec.get('Id');
                    typeWorkStore.clearFilter(true);
                    typeWorkStore.filter('contractCrId', rec.get('Id'));

                    typeWorkGrid.setDisabled(rec.phantom);
                }
            },
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' [name=Contragent]'] = { 'beforeload': { fn: me.onBeforeLoadContragent, scope: me } };
            },
            deleteRecord: function (record) {
                var me = this;
                if (record.getId()) {
                    me.controller.getAspect('contractdeletepermissionaspect').loadPermissions(record)
                        .next(function(response) {
                            var grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                                    var model, rec;
                                    if (result == 'yes') {
                                        model = me.getModel(record),
                                        rec = new model({ Id: record.getId() });

                                        me.mask('Удаление', B4.getBody());
                                        rec.destroy()
                                            .next(function() {
                                                me.fireEvent('deletesuccess', me);
                                                me.updateGrid();
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
            },
            onBeforeLoadContragent: function (field, options) {
                if (options.params == null) {
                    options.params = {};
                }
                options.params.showAll = true;
            }
        },
        {
            xtype: 'buildcontractspecialobjectcrperm',
            name: 'buildContractPermissionAspect',
            editFormAspectName: 'buildContractGridWindowAspect',
            setPermissionEvent: 'aftersetformdata'
        },
        {
            xtype: 'buildcontractspecialobjectcrviewperm',
            name: 'buildContractViewPermissionAspect',
            editFormAspectName: 'buildContractGridWindowAspect',
            setPermissionEvent: 'aftersetformdata'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'buildcontractcreatepermissionaspect',
            permissions: [
                { name: 'GkhCr.SpecialObjectCr.Register.BuildContractViewCreate.Create', applyTo: 'b4addbutton', selector: 'specialobjectcrbuildcontractgrid' },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.BuildContractViewCreate.Column.Sum', applyTo: '[dataIndex=Sum]', selector: 'specialobjectcrbuildcontractgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'buildcontractdeletepermissionaspect',
            permissions: [
                { name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Delete' }
            ]
        },
        //аспект пермишена типов Договора подряда
        {
            xtype: 'specialobjectcrbuildcontracttypeperm',
            name: 'buildContractTypePermissionAspect',
            editFormAspectName: 'buildContractGridWindowAspect',
            setPermissionEvent: 'aftersetformdata'
        },
        {
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'buildContractStateTransferAspect',
            gridSelector: 'specialobjectcrbuildcontractgrid',
            stateType: 'special_cr_obj_build_contract',
            menuSelector: 'buildContractGridStateMenu'
        },
        {
            /*
            * Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statecontextbuttonaspect',
            name: 'buildContractStateButtonAspect',
            stateButtonSelector: 'specialobjectcrbuildcontracteditwindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState);

                    var editWindowAspect = asp.controller.getAspect('buildContractGridWindowAspect');

                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    editWindowAspect.updateGrid();

                    var model = asp.controller.getModel(editWindowAspect.modelName);
                    model.load(entityId, {
                        success: function (rec) {
                            editWindowAspect.setFormData(rec);
                        }
                    });
                }
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'buildContractPrintAspect',
            buttonSelector: 'specialobjectcrbuildcontracteditwindow #btnPrint',
            codeForm: 'BuildContractCr',
            getUserParams: function () {
                var param = { BuildContractId: this.controller.params.buildContractId };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования договоров подряда
            */
            xtype: 'grideditctxwindowaspect',
            name: 'buildContractGridWindowAspect',
            gridSelector: 'specialobjectcrbuildcontractgrid',
            editFormSelector: 'specialobjectcrbuildcontracteditwindow',
            modelName: 'specialobjectcr.BuildContract',
            editWindowView: 'specialobjectcr.BuildContractEditWindow',
            onSaveSuccess: function (aspect) {
                aspect.controller.getAspect('buildContrTypeWrkMultiSelectWindowAspect').save();
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ObjectCr', asp.controller.getContextValue(asp.controller.getMainView(), 'objectcrId'));
                    }
                },
                beforesetformdata: function (asp, rec) {
                    var me = asp.controller,
                        contragent = rec.get('Contragent');

                    if (!contragent) {
                        rec.set('Contragent', me.regOpContragent);
                    }
                },
                aftersetformdata: function (asp, rec) {
                    this.controller.getAspect('buildContractStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                    var form = asp.getForm(),
                        typeWorkGrid = form.down('contracttypeworkspecialcrgrid'),
                        builderField = form.down('[name=Builder]'),
                        typeWorkStore = typeWorkGrid.getStore();

                    builderField.store.on('beforeload', function (store, operation) {
                        operation.params.objectCrId = asp.controller.getContextValue(asp.controller.getMainView(), 'objectcrId');
                    });

                    if (!typeWorkGrid.params) {
                        typeWorkGrid.params = {};
                    }

                    typeWorkGrid.params.buildContractId = rec.get('Id');
                    typeWorkStore.clearFilter(true);
                    typeWorkStore.filter('buildContractId', rec.get('Id'));

                    typeWorkGrid.setDisabled(rec.phantom);

                    this.controller.getAspect('buildContractPrintAspect').loadReportStore();
                }
            },
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' [name=TypeContractBuild]'] = { 'refresh': { fn: me.setTypeContractPermission, scope: me } };
                actions[me.editFormSelector + ' b4enumcombo[name=BuildContractState]'] = { 'change': { fn: me.onBuildContractStateChange, scope: me } };
                actions[me.editFormSelector + ' b4enumcombo[name=IsLawProvided]'] = { 'change': { fn: me.onIsLawProvidedChange, scope: me } };
            },
            setTypeContractPermission: function (field) {
                var str = field.getStore(),
                    picker = field.picker,
                    value = field.getValue();

                field.notAllowed.forEach(function(item) {
                    var rec = str.findRecord('Name', item);
                    if (rec && rec.get('Value') !== value) {
                        var node = picker.getNodeByRecord(rec);
                        node.style.display = 'none';
                        picker.updateLayout();
                    }
                });
            },
            onBuildContractStateChange: function (field, newValue) {
                var window = field.up('window'),
                    terminationPanel = window.down('panel[name=TerminationContractTab]');

                if (terminationPanel) {
                    Ext.each(terminationPanel.query('field'), function (f) {
                        f.allowBlank = newValue !== B4.enums.BuildContractState.Terminated;
                    });
                }
                window.getForm().isValid();
            },
            onIsLawProvidedChange: function (field, newValue) {
                var asp = this,
                    webSiteField = field.up('window').down('field[name=WebSite]'),
                    isDisabled = newValue !== B4.enums.YesNo.Yes;

                if (webSiteField && !webSiteField.manualDisabled) {
                    webSiteField.setDisabled(isDisabled);
                }
            },
            deleteRecord: function (record) {
                if (record.getId()) {
                    this.controller.getAspect('buildcontractdeletepermissionaspect').loadPermissions(record)
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
            }
        },
        {
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'buildContrTypeWrkMultiSelectWindowAspect',
            gridSelector: 'contracttypeworkspecialcrgrid',
            storeName: 'specialobjectcr.BuildContractTypeWork',
            modelName: 'specialobjectcr.BuildContractTypeWork',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#buildContrSpecTypeWrkMultiSelectWindow',
            storeSelect: 'specialobjectcr.TypeWorkCrForSelect',
            storeSelected: 'specialobjectcr.TypeWorkCrForSelected',
            titleSelectWindow: 'Выбор видов работ',
            titleGridSelect: 'Виды работ для выбора',
            titleGridSelected: 'Выбранные виды работ',
            columnsGridSelect: [
                 { text: 'Вид работы', dataIndex: 'WorkName', flex: 1 },
                 { text: 'Разрез финансирования', dataIndex: 'FinanceSourceName', flex: 1 }
            ],
            columnsGridSelected: [
                 { text: 'Вид работы', dataIndex: 'WorkName', flex: 1 },
                 { text: 'Разрез финансирования', dataIndex: 'FinanceSourceName', flex: 1 }
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.objectCrId = this.controller.getContextValue(this.controller.getMainView(), 'objectcrId');
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [],
                        buildContractId = asp.getGrid().params.buildContractId;

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainView());
                        B4.Ajax.request(B4.Url.action('AddTypeWorks', 'SpecialBuildContract', {
                            objectIds: recordIds,
                            buildContractId: buildContractId
                        })).next(function () {
                            asp.controller.unmask();
                            asp.getGrid().getStore().load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать виды работ');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'ContrCrTypeWrkMultiSelectWindowAspect',
            gridSelector: 'contracttypeworkspecialcrgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#contrSpecCrTypeWrkMultiSelectWindow',
            storeSelect: 'specialobjectcr.TypeWorkCrForSelect',
            storeSelected: 'specialobjectcr.TypeWorkCrForSelected',
            titleSelectWindow: 'Выбор видов услуг',
            titleGridSelect: 'Виды услуг для выбора',
            titleGridSelected: 'Выбранные виды услуг',
            columnsGridSelect: [
                { text: 'Вид работы', dataIndex: 'WorkName', flex: 1 },
                { text: 'Разрез финансирования', dataIndex: 'FinanceSourceName', flex: 1 }
            ],
            columnsGridSelected: [
                { text: 'Вид работы', dataIndex: 'WorkName', flex: 1 },
                { text: 'Разрез финансирования', dataIndex: 'FinanceSourceName', flex: 1 }
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.objectCrId = this.controller.getContextValue(this.controller.getMainView(), 'objectcrId');
                operation.params.onlyServices = true;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [],
                        contractCrId = asp.getGrid().params.contractCrId;

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainView());
                        B4.Ajax.request(B4.Url.action('AddTypeWorks', 'SpecialContractCr', {
                            objectIds: recordIds,
                            contractCrId: contractCrId
                        })).next(function () {
                            asp.controller.unmask();
                            asp.getGrid().getStore().load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать вид работ');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;
        var actions = {};

        actions['specialobjectcrbuildcontractgrid'] = { 'store.beforeload': { fn: me.onBeforeLoad, scope: me } };
        actions['specialobjectcrcontractgrid'] = { 'store.beforeload': { fn: me.onBeforeLoad, scope: me } };

        me.loadRegOpContragent();
        me.control(actions);
        me.callParent(arguments);
    },

    index: function(id) {
        var me = this;

        var view = me.getMainView() || Ext.widget('specialobjectcrcontractpanel'),
            store,
            storeContractCr;

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'specialobjectcr_info');

        store = view.down('specialobjectcrbuildcontractgrid').getStore();
        store.clearFilter(true);
        store.filter('objectCrId', id);

        storeContractCr = view.down('specialobjectcrcontractgrid').getStore();
        storeContractCr.clearFilter(true);
        storeContractCr.filter('objectCrId', id);

        me.getAspect('contractcreatepermissionaspect').setPermissionsByRecord({ getId: function () { return id; } });
        me.getAspect('buildcontractcreatepermissionaspect').setPermissionsByRecord({ getId: function () { return id; } });
    },

    onBeforeLoad: function(store, operation) {
        var objectId = this.getContextValue(this.getMainComponent(), 'objectcrId');
        operation.params.objectCrId = objectId;
    },

    loadRegOpContragent: function () {
        var me = this;

        B4.Ajax.request({
                url: B4.Url.action('List', 'RegOperator'),
                method: 'POST'
            })
            .next(function(response) {
                var obj = Ext.JSON.decode(response.responseText),
                    model = me.getModel('B4.model.Contragent'),
                    contragentId;

                if (obj.success && obj.data[0]) {
                    contragentId = obj.data[0].ContragentId;

                    model.load(contragentId,
                    {
                        scope: me,
                        success: function(record) {
                            me.regOpContragent = record.data;
                        }
                    });
                }
            });
    }

});