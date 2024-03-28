///!!!Править аккуратно!!!
Ext.define('B4.controller.Service', {
    extend: 'B4.base.Controller',

    requires:
    [
        'B4.Ajax',
        'B4.Url',
        'B4.Date',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.permission.service.State',
        'B4.aspects.GkhEditPanel'
    ],

    models:
    [
        'service.Base',
        'service.Communal',
        'service.CapitalRepair',
        'service.Repair',
        'service.Housing',
        'service.Control',
        'service.Additional',
        'service.TariffForConsumers',
        'service.WorkCapRepair',
        'service.WorkRepairList',
        'service.WorkRepairDetail',
        'service.TariffForRso',
        'service.CostItem',
        'dict.GroupWorkPpr',
        'dict.WorkPpr',
        'dict.WorkTo',
        'service.WorkRepairTechServ',
        'service.ProviderService',
        'service.ConsumptionNormsNpa',
        'menu.ManagingOrgRealityObjDataMenu'
    ],
    stores:
    [
        'service.Base',
        'service.TariffForConsumers',
        'service.TariffForConsumersCapRep',
        'service.TariffForConsumersRepair',
        'service.TariffForConsumersHousing',
        'service.TariffForConsumersControl',
        'service.TariffForConsumersAdditional',
        'service.TariffForConsumersRepair',
        'service.WorkCapRepair',
        'service.WorkRepairList',
        'service.WorkRepairDetail',
        'service.HousingCostItem',
        'service.GroupWorkPprSelect',
        'service.GroupWorkPprSelected',
        'service.WorkPprSelect',
        'service.WorkPprSelected',
        'service.TariffForRso',
        'service.WorkToSelected',
        'service.WorkToSelect',
        'dict.WorkSelect',
        'dict.WorkSelected',
        'service.WorkRepairTechServ',
        'service.ProviderServiceAdditional',
        'service.ProviderServiceHousing',
        'service.ProviderServiceCommunal',
        'service.ProviderServiceCapRepair',
        'service.ProviderServiceRepair',
        'service.ConsumptionNormsNpa',
        'service.ProviderServiceControl',
        'menu.ManagingOrgRealityObjDataMenuServ',
        'menu.ManagingOrgRealityObjDataMenuServSelect',
        'menu.ManagingOrgRealityObjDataMenuServSelected'
    ],
    views:
    [
        'service.Grid',
        'service.CustomServiceWindow',
        'service.communal.EditWindow',
        'service.caprepair.EditWindow',
        'service.housing.EditWindow',
        'service.control.EditWindow',
        'service.additional.EditWindow',
        'service.repair.EditWindow',
        'SelectWindow.MultiSelectWindow',
        'SelectWindow.MultiSelectWindowTree',
        'service.additional.ProviderServiceEditWindow',
        'service.additional.ProviderServiceEditWindow',
        'service.control.ProviderServiceEditWindow',
        'service.housing.ProviderServiceEditWindow',
        'service.communal.ProviderServiceEditWindow',
        'service.caprepair.ProviderServiceEditWindow',
        'service.repair.ProviderServiceEditWindow',
        'service.CopyServiceWindow'
    ],

    refs: [
        {
            ref: 'serviceWindow',
            selector: '#customServiceWindow'

        },
        {
            ref: 'housingEditWindow',
            selector: '#housingServiceEditWindow'
        },
        {
            ref: 'housingCostittemGrid',
            selector: 'housingcostitemgrid'
        },
        {
            ref: 'housTarifGrid',
            selector: 'tariffforconsumhousinggrid'
        },
        {
            ref: 'housProviderGrid',
            selector: 'housproviderservicegrid'
        }
    ],

    treePanelSelector: '#workRepairTechServMultiSelectWindow #tpSelect',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'service.Grid',
    mainViewSelector: '#serviceGrid',

    config: {

    },

    validateFacade: function () {
        var grid = Ext.ComponentQuery.query('#workRepairListGrid')[0],
            me = this;

        var cbTypeOfProvision = Ext.ComponentQuery.query('#cbTypeOfProvisionService')[0];

        if (cbTypeOfProvision) {
            var typeOfProvisionValue = cbTypeOfProvision.getValue();
            if (typeOfProvisionValue == 30) {
                return true;
            }
        }

        if (grid) {
            var val = grid.down('[name="ScheduledPreventiveMaintanance"]').getRawValue();
            switch (val) {
                case 'Не задано':
                    Ext.Msg.alert("Сообщение", "Выберите значение поля наличия планово-предупредительных работ");
                    break;
                case 'Нет':
                    me.checkToAndTariff();
                    break;
                case 'Да':
                    if (me.checkToAndTariff()) {
                        me.checkPprAndDetails();
                    }
                    break;
            }
        }

        return true;
    },

    checkToAndTariff: function () {
        var gridToAny = Ext.ComponentQuery.query('#workRepairTechServGrid')[0].getStore().getCount() > 0,
            gridTariffAny = Ext.ComponentQuery.query('#tariffForConsumersRepairGrid')[0].getStore().getCount() > 0,
            form = Ext.ComponentQuery.query('#repairServiceEditWindow')[0];

        var valid = form.getForm().isValid() && gridToAny && gridTariffAny;
        if (!valid) {
            Ext.Msg.alert("Сообщение", "Не заполнены обязательные поля или не указана  информация о тарифах и работах по ТО");
        }

        return valid;
    },

    checkPprAndDetails: function () {
        var grid = Ext.ComponentQuery.query('#workRepairListGrid')[0],
            gridDetailsAny;

        var idx = grid.getStore().findBy(function (rec) {
            return rec.get('PlannedCost') && rec.get('DateStart');
        });

        if (idx == -1) {
            Ext.Msg.alert("Сообщение", "Не заполнены обязательные поля ППР");
            return false;
        }

        asp.controller.mask('Проверка', asp.controller.getMainComponent());
        B4.Ajax.request(B4.Url.action('HasDetailAllWorkRepair', 'WorkRepairList', {
            baseServiceId: this.baseServiceId
        })).next(function (response) {
            asp.controller.unmask();

            gridDetailsAny = Ext.JSON.decode(response.responseText);

            var valid = gridDetailsAny;
            if (!valid) {
                Ext.Msg.alert("Сообщение", "Не заполнены детализации ППР");
            }

            return valid;
        }).error(function () {
            asp.controller.unmask();
            return false;
        });
    },

    setVisibleForNumberContract: function (year, form) {
        var isYearGte2015 = false;
        try {
            var yearFromParams = parseInt(year.substring(0, 4));
            if (yearFromParams >= 2015) {
                isYearGte2015 = true;
            }
        } catch (e) {
        }
        // NumberContract
        // 2015 и более - показать
        // 2014 и менее - спрятать
        var tfNumberContract = form.down("textfield[name=NumberContract]");
        if (tfNumberContract) {
            tfNumberContract.setVisible(isYearGte2015);
            tfNumberContract.allowBlank = !isYearGte2015;
        }
    },

    aspects: [
        {
            xtype: 'servicestateperm',
            name: 'servicePermissionAspect'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'additProviderServiceGridWindowAspect',
            gridSelector: '#providerServiceAdditionalGrid',
            editFormSelector: '#additionalProviderServiceEditWindow',
            storeName: 'service.ProviderServiceAdditional',
            modelName: 'service.ProviderService',
            editWindowView: 'service.additional.ProviderServiceEditWindow',

            listeners: {
                beforesave: function (asp, record) {
                    record.set('BaseService', asp.controller.baseServiceId);
                    return true;
                },
                deletesuccess: function (asp) {
                    asp.controller.updateProviderService(asp);
                },
                aftersetformdata: function (asp, record, form) {
                    asp.controller.setVisibleForNumberContract(asp.controller.params.year, form);
                }
            },
            onSaveSuccess: function (asp) {
                asp.controller.updateProviderService(asp);
                asp.getForm().close();
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'controlProviderServiceGridWindowAspect',
            gridSelector: '#providerServiceControlGrid',
            editFormSelector: '#controlProviderServiceEditWindow',
            storeName: 'service.ProviderServiceControl',
            modelName: 'service.ProviderService',
            editWindowView: 'service.control.ProviderServiceEditWindow',

            listeners: {
                beforesave: function (asp, record) {
                    record.set('BaseService', asp.controller.baseServiceId);
                    return true;
                },
                deletesuccess: function (asp) {
                    asp.controller.updateProviderService(asp);
                },
                aftersetformdata: function (asp, record, form) {
                    asp.controller.setVisibleForNumberContract(asp.controller.params.year, form);
                }
            },
            onSaveSuccess: function (asp) {
                asp.controller.updateProviderService(asp);
                asp.getForm().close();
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'housingProviderServiceGridWindowAspect',
            gridSelector: '#providerServiceHousingGrid',
            editFormSelector: '#housingProviderServiceEditWindow',
            storeName: 'service.ProviderServiceHousing',
            modelName: 'service.ProviderService',
            editWindowView: 'service.housing.ProviderServiceEditWindow',

            listeners: {
                beforesave: function (asp, record) {
                    record.set('BaseService', asp.controller.baseServiceId);
                    return true;
                },
                deletesuccess: function (asp) {
                    asp.controller.updateProviderService(asp);
                },
                aftersetformdata: function (asp, record, form) {
                    asp.controller.setVisibleForNumberContract(asp.controller.params.year, form);
                }
            },
            onSaveSuccess: function (asp) {
                asp.controller.updateProviderService(asp);
                asp.getForm().close();
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'communalProviderServiceGridWindowAspect',
            gridSelector: '#providerServiceCommunalGrid',
            editFormSelector: '#communalProviderServiceEditWindow',
            storeName: 'service.ProviderServiceCommunal',
            modelName: 'service.ProviderService',
            editWindowView: 'service.communal.ProviderServiceEditWindow',

            listeners: {
                beforesave: function (asp, record) {
                    record.set('BaseService', asp.controller.baseServiceId);
                    return true;
                },
                deletesuccess: function (asp) {
                    asp.controller.updateProviderService(asp);
                },
                aftersetformdata: function (asp, record, form) {
                    asp.controller.setVisibleForNumberContract(asp.controller.params.year, form);
                }
            },
            onSaveSuccess: function (asp) {
                asp.controller.updateProviderService(asp);
                asp.getForm().close();
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'repairProviderServiceGridWindowAspect',
            gridSelector: '#providerServiceRepairGrid',
            editFormSelector: '#repairProviderServiceEditWindow',
            storeName: 'service.ProviderServiceRepair',
            modelName: 'service.ProviderService',
            editWindowView: 'service.repair.ProviderServiceEditWindow',

            listeners: {
                beforesave: function (asp, record) {
                    record.set('BaseService', asp.controller.baseServiceId);
                    return true;
                },
                deletesuccess: function (asp) {
                    asp.controller.updateProviderService(asp);
                },
                aftersetformdata: function (asp, record, form) {
                    asp.controller.setVisibleForNumberContract(asp.controller.params.year, form);
                }
            },
            onSaveSuccess: function (asp) {
                asp.controller.updateProviderService(asp);
                asp.getForm().close();
            },
            setFormData: function (rec) {
                if (rec.data.Id == 0 && rec.data.IsActive != undefined) {
                    rec.data.IsActive = true;
                }
                var form = this.getForm();
                if (this.fireEvent('beforesetformdata', this, rec, form) !== false) {
                    form.loadRecord(rec);
                    form.getForm().updateRecord();
                    form.getForm().isValid();
                }

                this.fireEvent('aftersetformdata', this, rec, form);
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'capRepairProviderServiceGridWindowAspect',
            gridSelector: '#providerServiceCapRepairGrid',
            editFormSelector: '#capRepairProviderServiceEditWindow',
            storeName: 'service.ProviderServiceCapRepair',
            modelName: 'service.ProviderService',
            editWindowView: 'service.caprepair.ProviderServiceEditWindow',

            listeners: {
                beforesave: function (asp, record) {
                    record.set('BaseService', asp.controller.baseServiceId);
                    return true;
                },
                deletesuccess: function (asp) {
                    asp.controller.updateProviderService(asp);
                },
                aftersetformdata: function (asp, record, form) {
                    asp.controller.setVisibleForNumberContract(asp.controller.params.year, form);
                }
            },
            onSaveSuccess: function (asp) {
                asp.controller.updateProviderService(asp);
                asp.getForm().close();
            }
        },
        {
            //Аспект выбора работ по ТО
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'workRepairTechServAspect',
            gridSelector: '#workRepairTechServGrid',
            storeName: 'service.WorkRepairTechServ',
            modelName: 'service.WorkRepairTechServ',
            multiSelectWindow: 'SelectWindow.MultiSelectWindowTree',
            multiSelectWindowSelector: '#workRepairTechServMultiSelectWindow',
            storeSelect: 'service.WorkToSelect',
            storeSelected: 'service.WorkToSelected',
            titleSelectWindow: 'Выбор работ',
            titleGridSelect: 'Работы',
            titleGridSelected: 'Выбранные работы',
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            otherActions: function (actions) {
                actions['#tpSelect #btnMarkAll'] = { 'click': { fn: this.onClickMarkAll, scope: this } };
                actions['#tpSelect #btnUnmarkAll'] = { 'click': { fn: this.onClickUnmarkAll, scope: this } };
                actions['#tpSelect #btnUpdateTree'] = { 'click': { fn: this.onClickUpdate, scope: this } };
                actions['#tpSelect #tfSearchName'] = { 'change': { fn: this.onChangeNameFilter, scope: this } };
                actions['#tpSelect'] = { 'checkchange': { fn: this.onCheckRec, scope: this } };
            },
            onCheckRec: function (node, checked) {
                var grid = this.getSelectedGrid(),
                    storeSelected = grid.getStore(),
                    model = this.controller.getModel('dict.WorkTo');
                //если элемент конечный то добавляем в стор выбранных
                if (grid && node.get('leaf')) {
                    if (checked) {
                        if (storeSelected.find('Id', node.get('id'), 0, false, false, true) == -1)
                            storeSelected.add(new model({ Id: node.get('id'), Name: node.get('text') }));
                    } else {
                        storeSelected.remove(storeSelected.getById(node.get('id')));
                    }
                }
            },
            updateSelectGrid: function () {
                this.controller.getStore('service.WorkToSelect').load();
            },
            onBeforeLoad: function (store, operation) {
                if (this.controller.templateServiceId > 0) {
                    operation.params.baseServiceId = this.controller.baseServiceId;
                }
            },

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddWorks', 'WorkRepairTechServ', {
                            objectIds: recordIds,
                            baseServiceId: asp.controller.baseServiceId
                        })).next(function () {
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.unmask();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать работы');
                        return false;
                    }
                    return true;
                }
            },
            onClickMarkAll: function (btn) {
                var me = this;

                var tree = Ext.ComponentQuery.query(me.controller.treePanelSelector)[0];

                tree.getRootNode().cascadeBy(function () {
                    if (!Ext.isEmpty(this.get('checked'))) {
                        this.set('checked', true);
                    }
                    me.onCheckRec(this, true);
                });
            },
            onClickUnmarkAll: function (btn) {
                var me = this;
                var tree = Ext.ComponentQuery.query(me.controller.treePanelSelector)[0];

                tree.getRootNode().cascadeBy(function () {
                    if (!Ext.isEmpty(this.get('checked'))) {
                        this.set('checked', false);
                    }
                    me.onCheckRec(this, false);
                });
            },
            onChangeNameFilter: function (field, newValue, oldValue) {
                this.controller.filterName = newValue;
            },
            onClickUpdate: function (btn) {
                this.clearFilter();

                this.filterBy(this.controller.filterName, 'text');
            },
            clearFilter: function () {

                var me = this,
                    treePanel = Ext.ComponentQuery.query(me.controller.treePanelSelector)[0],
                    view = treePanel.getView();

                treePanel.collapseAll();
                treePanel.getRootNode().cascadeBy(function (tree, view) {
                    var uiNode = view.getNodeByRecord(this);
                    if (uiNode) {
                        Ext.get(uiNode).setDisplayed('table-row');
                    }
                }, null, [this, view]);
            },
            filterBy: function (text, by) {
                var me = this,
                    treePanel = Ext.ComponentQuery.query(me.controller.treePanelSelector)[0],
                    view = treePanel.getView(),
                    nodesAndParents = [];

                // Find the nodes which match the search term, expand them.
                // Then add them and their parents to nodesAndParents.
                treePanel.getRootNode().cascadeBy(function (tree, view) {
                    var currNode = this;
                    if (currNode && currNode.get('leaf') && currNode.data[by] && currNode.data[by].toString().toLowerCase().indexOf(text.toLowerCase()) > -1) {
                        treePanel.expandPath(currNode.getPath());

                        if (currNode.hasChildNodes()) {
                            currNode.eachChild(function (child) {
                                nodesAndParents.push(child.id);
                            });
                        }
                        while (currNode.parentNode) {
                            nodesAndParents.push(currNode.id);
                            currNode = currNode.parentNode;
                        }
                    }
                }, null, [treePanel, view]);

                // Hide all of the nodes which aren't in nodesAndParents
                treePanel.getRootNode().cascadeBy(function (tree, view) {
                    var uiNode = view.getNodeByRecord(this);
                    if (uiNode && !Ext.Array.contains(nodesAndParents, this.id)) {
                        Ext.get(uiNode).setDisplayed('none');
                    }
                }, null, [treePanel, view]);
            }
        },
        {
            //Аспект выбора работ по детализации
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'workRepairDetailAspect',
            gridSelector: '#workRepairDetailGrid',
            storeName: 'service.WorkRepairDetail',
            modelName: 'service.WorkRepairDetail',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#workRepairDetailMultiSelectWindow',
            storeSelect: 'service.WorkPprSelect',
            storeSelected: 'service.WorkPprSelected',
            titleSelectWindow: 'Выбор под-работ капремонта',
            titleGridSelect: 'Работы',
            titleGridSelected: 'Выбранные работы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            otherActions: function (actions) {
                var me = this;
                actions[me.gridSelector + ' #btnSave'] = {
                    'click': {
                        fn: function (btn) {
                            var store = btn.up(me.gridSelector).getStore(),
                                result = true;

                            Ext.each(store.data.items, function (item) {
                                return result = !Ext.isEmpty(item.get('UnitMeasure'));
                            });

                            if (!result) {
                                B4.QuickMsg.msg('Ошибка', 'Необходимо заполнить поле "Единица измерения"', 'error');
                                return;
                            }

                            store.sync({
                                callback: function () {
                                    store.load();
                                }
                            });
                        }
                    }
                };
            },
            onBeforeLoad: function (store, operation) {
                    operation.params.groupWorkPprId = this.controller.currentGroupWorkPpr;
            },

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddWorks', 'WorkRepairDetail', {
                            objectIds: recordIds,
                            baseServiceId: asp.controller.baseServiceId
                        })).next(function () {
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.unmask();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать работы');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            //Аспект выбора списка работ
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'workRepairListAspect',
            gridSelector: '#workRepairListGrid',
            saveButtonSelector: '#workRepairListGrid #workRepairListSaveButton',
            storeName: 'service.WorkRepairList',
            modelName: 'service.WorkRepairList',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#workRepairListMultiSelectWindow',
            storeSelect: 'service.GroupWorkPprSelect',
            storeSelected: 'service.GroupWorkPprSelected',
            titleSelectWindow: 'Выбор работ капремонта',
            titleGridSelect: 'Работы',
            titleGridSelected: 'Выбранные работы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                { header: 'Норматив', xtype: 'gridcolumn', dataIndex: 'Normative', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],

            onBeforeLoad: function (store, operation) {
                operation.params.baseServiceId = this.controller.baseServiceId;
            },

            otherActions: function (actions) {
                var me = this;
                actions['#workRepairListGrid'] = {
                    render: function (grid) {
                        grid.down('[name="ScheduledPreventiveMaintanance"]').setValue(me.controller.ScheduledPreventiveMaintanance);
                    }
                };
            },

            listeners: {
                //В данном методе принимаем массив записей из формы выбора и вставляем их в сторе грида без сохранения                
                getdata: function (asp, records) {

                    var repairListIds = [];

                    var st = asp.controller.getStore(asp.storeName);
                    st.each(function (rec) {
                        if (rec.get('GroupWorkPpr')) {
                            repairListIds.push(rec.get('GroupWorkPpr'));
                        }
                    });

                    records.each(function (rec) {
                        if (Ext.Array.indexOf(repairListIds, rec.get('Id')) == -1) {
                            //создаем рекорд модели RepairList
                            var recordRepairList = asp.controller.getModel(asp.modelName).create();
                            recordRepairList.set('GroupWorkPpr', rec.get('Id'));
                            recordRepairList.set('Name', rec.get('Name'));
                            recordRepairList.set('BaseService', asp.controller.baseServiceId);

                            st.insert(0, recordRepairList);
                        }
                    });
                    return true;
                }
            },

            beforeSave: function (asp, store) {
                var form = Ext.ComponentQuery.query('#repairServiceEditWindow')[0];

                form.getForm().updateRecord();
                var rec = form.getRecord();
                rec.save({ id: asp.controller.baseServiceId });

                return asp.controller.validateFacade();
            }
        },
        {
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'workCapRepairAspect',
            gridSelector: '#workCapRepairGrid',
            saveButtonSelector: '#workCapRepairGrid #workCapRepairSaveButton',
            storeName: 'service.WorkCapRepair',
            modelName: 'service.WorkCapRepair',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#workCapRepairMultiSelectWindow',
            storeSelect: 'dict.WorkSelect',
            storeSelected: 'dict.WorkSelected',
            titleSelectWindow: 'Выбор работ капремонта',
            titleGridSelect: 'Работы капремонта',
            titleGridSelected: 'Выбранные работы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                { header: 'Норматив', xtype: 'gridcolumn', dataIndex: 'Normative', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],

            listeners: {
                //В данном методе принимаем массив записей из формы выбора и вставляем их в сторе грида без сохранения                
                getdata: function (asp, records) {
                    var recordIds = [];

                    var st = asp.controller.getStore(asp.storeName);
                    st.each(function (rec) {
                        if (rec.get('WorkId')) {
                            recordIds.push(rec.get('WorkId'));
                        }
                    });

                    records.each(function (rec) {
                        if (Ext.Array.indexOf(recordIds, rec.get('Id')) == -1) {
                            //создаем рекорд модели CapRepair
                            var recordCapRepair = asp.controller.getModel(asp.modelName).create();
                            recordCapRepair.set('Work', rec.get('Id'));
                            recordCapRepair.set('Name', rec.get('Name'));
                            recordCapRepair.set('BaseService', asp.controller.baseServiceId);

                            st.insert(0, recordCapRepair);
                        }
                    });
                    return true;
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'tariffForConsumersCapRepGridAspect',
            storeName: 'service.TariffForConsumersCapRep',
            modelName: 'service.TariffForConsumers',
            gridSelector: '#tariffForConsumersCapRepGrid',
            saveButtonSelector: '#tariffForConsumersCapRepGrid #tariffForConsumersCapRepSaveButton',
            listeners: {
                beforeaddrecord: function (asp, rec) {
                    rec.set('DateStart', new Date());
                },
                beforesave: function (asp, store) {
                    if (asp.controller.baseServiceId) {
                        store.each(function (rec) {
                            if (!rec.get('Id')) {
                                rec.set('BaseService', asp.controller.baseServiceId);
                            }
                        });
                        return true;
                    } else {
                        Ext.Msg.alert('Ошибка сохранения', 'Выберите услугу заново!');
                        return false;
                    }
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'tariffForConsumersRepairGridAspect',
            storeName: 'service.TariffForConsumersRepair',
            modelName: 'service.TariffForConsumers',
            gridSelector: '#tariffForConsumersRepairGrid',
            saveButtonSelector: '#tariffForConsumersRepairGrid #tariffForConsumersRepairSaveButton',
            listeners: {
                beforeaddrecord: function (asp, rec) {
                    var store = Ext.ComponentQuery.query('#tariffForConsumersRepairGrid')[0].getStore(),
                        lastRec = store.getCount() > 0 ? store.getAt(store.getCount() - 1) : null;
                    if (lastRec) {
                        if (lastRec.get('DateEnd')) {
                            rec.set('DateStart', B4.Date.nextDayFor(lastRec.get('DateEnd')));
                        } else {
                            lastRec.set('DateEnd', B4.Date.previousDayFor(new Date()));
                            rec.set('DateStart', new Date());
                        }
                    } else {
                        rec.set('DateStart', new Date());
                    }
                },
                beforesave: function (asp, store) {
                    var periods;
                    if (asp.controller.baseServiceId) {
                        store.each(function (rec) {
                            if (!rec.get('Id')) {
                                rec.set('BaseService', asp.controller.baseServiceId);
                            }
                        });
                    } else {
                        Ext.Msg.alert('Ошибка сохранения', 'Выберите услугу заново!');
                        return false;
                    }

                    periods = Ext.Array.map(store.getRange(), function (r) {
                        return { DateStart: r.get('DateStart'), DateEnd: r.get('DateEnd') };
                    });
                    if (!B4.Date.isPeriodRange(periods, true)) {
                        Ext.Msg.alert('Ошибка сохранения', 'Некорректно задан период действия тарифа');
                        return false;
                    }

                    return true;
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'tariffForConsumersAdditionalGridAspect',
            storeName: 'service.TariffForConsumersAdditional',
            modelName: 'service.TariffForConsumers',
            gridSelector: '#tariffForConsumersAdditionalGrid',
            saveButtonSelector: '#tariffForConsumersAdditionalGrid #tariffForConsumersAdditionalSaveButton',
            listeners: {
                beforeaddrecord: function (asp, rec) {
                    rec.set('DateStart', new Date());
                },
                beforesave: function (asp, store) {
                    if (asp.controller.baseServiceId) {
                        store.each(function (rec) {
                            if (!rec.get('Id')) {
                                rec.set('BaseService', asp.controller.baseServiceId);
                            }
                        });
                        return true;
                    } else {
                        Ext.Msg.alert('Ошибка сохранения', 'Выберите услугу заново!');
                        return false;
                    }
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'tariffForConsumersControlGridAspect',
            storeName: 'service.TariffForConsumersControl',
            modelName: 'service.TariffForConsumers',
            gridSelector: '#tariffForConsumersControlGrid',
            saveButtonSelector: '#tariffForConsumersControlGrid #tariffForConsumersControlSaveButton',
            listeners: {
                beforeaddrecord: function (asp, rec) {
                    rec.set('DateStart', new Date());
                },
                beforesave: function (asp, store) {
                    if (asp.controller.baseServiceId) {
                        store.each(function (rec) {
                            if (!rec.get('Id')) {
                                rec.set('BaseService', asp.controller.baseServiceId);
                            }
                        });
                        return true;
                    } else {
                        Ext.Msg.alert('Ошибка сохранения', 'Выберите услугу заново!');
                        return false;
                    }
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'housingCostItemGridAspect',
            storeName: 'service.HousingCostItem',
            modelName: 'service.CostItem',
            gridSelector: '#housingCostItemGrid',
            saveButtonSelector: '#housingCostItemGrid #housingCostItemSaveButton',
            listeners: {
                beforesave: function (asp, store) {
                    if (asp.controller.baseServiceId) {
                        store.each(function (rec) {
                            if (!rec.get('Id')) {
                                rec.set('BaseService', asp.controller.baseServiceId);
                            }
                        });
                        return true;
                    } else {
                        Ext.Msg.alert('Ошибка сохранения', 'Сначала необходимо сохранить услугу!');
                        return false;
                    }
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'tariffForRsoGridAspect',
            storeName: 'service.TariffForRso',
            modelName: 'service.TariffForRso',
            gridSelector: '#tariffForRsoCommunalGrid',
            saveButtonSelector: '#tariffForRsoCommunalGrid #tariffForRsoSaveButton',
            listeners: {
                beforeaddrecord: function (asp, rec) {
                    rec.set('DateStart', new Date());
                },

                beforesave: function (asp, store) {
                    if (asp.controller.baseServiceId) {
                        store.each(function (rec) {
                            if (!rec.get('Id')) {
                                rec.set('BaseService', asp.controller.baseServiceId);
                            }
                        });
                        return true;
                    } else {
                        Ext.Msg.alert('Ошибка сохранения', 'Выберите услугу заново!');
                        return false;
                    }
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'consumptionNormsNpaAspect',
            storeName: 'service.ConsumptionNormsNpa',
            modelName: 'service.ConsumptionNormsNpa',
            gridSelector: 'consumptionnormsnpagrid',
            saveButtonSelector: 'consumptionnormsnpagrid [typeaction=consumptionNormsNpaSave]',
            listeners: {
                beforeaddrecord: function (asp, rec) {
                    rec.set('NpaDate', new Date());
                },

                beforesave: function (asp, store) {
                    if (asp.controller.baseServiceId) {
                        store.each(function (rec) {
                            if (!rec.get('Id')) {
                                rec.set('BaseService', asp.controller.baseServiceId);
                            }
                        });
                        return true;
                    } else {
                        Ext.Msg.alert('Ошибка сохранения', 'Выберите услугу заново!');
                        return false;
                    }
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'tariffForConsumersGridAspect',
            storeName: 'service.TariffForConsumers',
            modelName: 'service.TariffForConsumers',
            gridSelector: '#tariffForConsumersCommunalGrid',
            saveButtonSelector: '#tariffForConsumersCommunalGrid #tariffForConsumersSaveButton',
            listeners: {
                beforeaddrecord: function (asp, rec) {
                    rec.set('DateStart', new Date());
                },
                beforesave: function (asp, store) {
                    var dataStartMoredataEnd = true;
                    Ext.each(store.data.items, function (rec) {
                        if (rec.data != null && rec.data.DateStart != null && rec.data.DateEnd != null) {
                            var dataStart = new Date(rec.data.DateStart.toString());
                            var dataEnd = new Date(rec.data.DateEnd.toString());
                            if (dataStart > dataEnd) {
                                dataStartMoredataEnd = false;
                                return false;
                            }
                        }
                    });
                    if (!dataStartMoredataEnd) {
                        Ext.Msg.alert('Ошибка сохранения', 'Дата начала не может быть больше даты окончания действия');
                        return false;
                    }
                    if (asp.controller.baseServiceId) {
                        store.each(function (rec) {
                            if (!rec.get('Id')) {
                                rec.set('BaseService', asp.controller.baseServiceId);
                            }
                        });
                        return true;
                    } else {
                        Ext.Msg.alert('Ошибка сохранения', 'Выберите услугу заново!');
                        return false;
                    }
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'tariffForConsumersHousingGridAspect',
            storeName: 'service.TariffForConsumersHousing',
            modelName: 'service.TariffForConsumers',
            gridSelector: '#tariffForConsumersHousingGrid',
            saveButtonSelector: '#tariffForConsumersHousingGrid #tariffForConsumersHousingSaveButton',
            listeners: {
                beforeaddrecord: function (asp, rec) {
                    rec.set('DateStart', new Date());
                },
                beforesave: function (asp, store) {
                    var dataStartMoredataEnd = true;
                    Ext.each(store.data.items, function (rec) {
                        if (rec.data != null && rec.data.DateStart != null && rec.data.DateEnd != null) {
                            var dataStart = new Date(rec.data.DateStart.toString());
                            var dataEnd = new Date(rec.data.DateEnd.toString());
                            if (dataStart > dataEnd) {
                                dataStartMoredataEnd = false;
                                return false;
                            }
                        }
                    });
                    if (!dataStartMoredataEnd) {
                        Ext.Msg.alert('Ошибка сохранения', 'Дата начала не может быть больше даты окончания действия');
                        return false;
                    }
                    if (asp.controller.baseServiceId) {
                        store.each(function (rec) {
                            if (!rec.get('Id')) {
                                rec.set('BaseService', asp.controller.baseServiceId);
                            }
                        });
                        return true;
                    } else {
                        Ext.Msg.alert('Ошибка сохранения', 'Выберите услугу заново!');
                        return false;
                    }
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'copyServGridMultiSelectAspect',
            gridSelector: '#realityObjCopyServGrid',
            storeName: 'menu.ManagingOrgRealityObjDataMenuServ',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#copyServMultiSelectWindow',
            storeSelect: 'menu.ManagingOrgRealityObjDataMenuServSelect',
            storeSelected: 'menu.ManagingOrgRealityObjDataMenuServSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Жилые дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                { header: 'Адрес дома', xtype: 'gridcolumn', dataIndex: 'AddressName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес дома', xtype: 'gridcolumn', dataIndex: 'AddressName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.disclosureInfoId = this.controller.params.disclosureInfoId;
                operation.params.disclosureInfoRealityObjId = this.controller.params.disclosureInfoRealityObjId;
                operation.params.year = this.controller.params.year;
            },
            listeners: {
                getdata: function (asp, records) {
                    var store = this.controller.getStore('menu.ManagingOrgRealityObjDataMenuServ');
                    store.removeAll();
                    store.add(records.items);
                }
            },
            rowAction: function (grid, action, record) {
                if (action.toLowerCase() == 'delete') {
                    this.deleteRecord(record);
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'serviceGridWindowAspect',
            gridSelector: '#serviceGrid',
            storeName: 'service.Base',
            customWindowView: 'service.CustomServiceWindow',
            customFormSelector: '#customServiceWindow',
            copyWindowView: 'service.CopyServiceWindow',
            copyFormSelector: '#copyServiceWindow',
            hasSaveTabPanel: false,

            onSaveSuccess: function (asp, record) {

                asp.controller.setCurrentId(record);

                var form = asp.getForm();
                form.down('#sflProvider').readOnly = true;
                form.down('#changeProviderButton').setDisabled(false);
                form.doLayout();

                asp.hasSaveTabPanel = false;
            },

            //Данный метод перекрываем для того чтобы вместо целого объекта Provider
            // передать только Id на сохранение, поскольку если на сохранение уйдет Provider целиком,
            //то это поле тоже сохраниться и поля для записи Provider будут потеряны
            getRecordBeforeSave: function (record) {
                var provider = record.get('Provider');
                if (provider && provider.Id > 0) {
                    record.set('Provider', provider.Id);
                }

                return record;
            },

            listeners: {
                beforesave: function (asp, record) {
                    record.set('DisclosureInfoRealityObj', asp.controller.params.disclosureInfoRealityObjId);
                    if (record.getId() && !asp.validateGrid(asp, record)) {
                        return false;
                    }
                    asp.hasSaveTabPanel = true;
                    return asp.controller.validateFacade();
                },
                beforerowaction: function (asp, grid, action, rec) {
                    if (action.toLowerCase() == 'edit' || action.toLowerCase() == 'doubleclick') {
                        asp.controller.customServiceBtnClick(null, null, { asp: asp }, rec);
                        return false;
                    }
                    if (action.toLowerCase() == 'copy') {
                        asp.controller.copingServiceId = rec.getId();
                        asp.controller.copingServiceKind = rec.get('KindServiceDi');
                        asp.controller.getCopyWindow(asp).show();
                        asp.controller.getStore('menu.ManagingOrgRealityObjDataMenuServ').removeAll();
                    }
                    return true;
                },
                beforesaverequest: function (asp, record, form) {
                    var me = this,
                        controller = asp.controller,
                        win = controller.getHousingEditWindow();

                    if (win) {
                        var cbTypeOfProvisionService = win.down('#cbTypeOfProvisionService');

                        if (cbTypeOfProvisionService.getValue() === 10 && win.SavebuttonisEnabled) {
                            Ext.Msg.alert('Ошибка сохранения!', 'Необходимо добавить хотя бы одну запись в разделе "Статьи затрат" с указанием суммы');
                            return false;
                        }
                    }
                    return true;
                },
                //перед вставкой данных получаем коллекцию настраиваемых полей
                beforesetformdata: function (asp, record, form) {
                    delete asp.controller.optionFields;

                    var templateServiceId = 0;
                    if (asp.controller.templateServiceId) {
                        templateServiceId = asp.controller.templateServiceId;
                    } else if (record.get('TemplateService')) {
                        templateServiceId = record.get('TemplateService').Id;
                    }

                    asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('GetOptionsFields', 'TemplateService', {
                        templateServiceId: templateServiceId
                    })).next(function (response) {
                        var obj = Ext.JSON.decode(response.responseText);
                        asp.controller.optionFields = obj;
                        //Применяем сокрытие настраиваемых полей
                        var cbTypeOfProvisionService = form.down('#cbTypeOfProvisionService');
                        if (cbTypeOfProvisionService && cbTypeOfProvisionService.getValue()) {
                            cbTypeOfProvisionService.fireEvent('change', cbTypeOfProvisionService);
                        } else {
                            asp.acceptOptionFields(asp.controller.optionFields, form, false);
                            form.doLayout();
                        }
                        asp.controller.unmask();
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                },
                aftersetformdata: function (asp, record, form) {
                    var recordId = record.getId();
                    if (recordId == 0) {
                        form.down('#sflProvider').readOnly = false;
                        form.down('#changeProviderButton').setDisabled(true);
                        form.doLayout();
                    } else {
                        form.down('#sflProvider').readOnly = true;
                        form.down('#changeProviderButton').setDisabled(false);
                        form.doLayout();
                    }

                    var controller = this.controller,
                        gridCostItem = controller.getHousingCostittemGrid(),
                        gridTarif = controller.getHousTarifGrid(),
                        gridProvider = controller.getHousProviderGrid(),
                        compSaveButton = gridCostItem ? gridCostItem.down('#housingCostItemSaveButton') : null;

                    if (gridCostItem) {
                        gridTarif.setDisabled(recordId === 0);
                        gridProvider.setDisabled(recordId === 0);
                        compSaveButton.setDisabled(recordId === 0);
                    }
                    form.doLayout();

                    var templateService = record.get('TemplateService');
                    var sflUnitMeasure = form.down('#sflUnitMeasure');
                    if (templateService) {
                        if (templateService.KindServiceDi == '60' && record.get('Provider')) {
                            form.down('#tfOgrn').setValue(record.get('Provider').Ogrn);
                            form.down('#dfDateRegistartion').setValue(record.get('Provider').ActivityDateStart);
                        }
                    } else { //Заход в первый раз (после выбора в маленькой форме)
                        templateService = asp.controller.templateService;
                        record.set('TemplateService', asp.controller.templateService);

                        asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('GetUnitMeasure', 'TemplateService', {
                            templateServiceId: asp.controller.templateServiceId ? asp.controller.templateServiceId : record.get('TemplateService').Id
                        })).next(function (response) {
                            //десериализуем полученную строку
                            var obj = Ext.JSON.decode(response.responseText);
                            sflUnitMeasure.setValue(obj.UnitMeasure);
                            record.set('UnitMeasure', obj.UnitMeasure);
                            if (!obj.Changeable) {
                                sflUnitMeasure.setDisabled(true);
                            } else {
                                sflUnitMeasure.setDisabled(false);
                            }
                            asp.controller.unmask();
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    //лочим если ед измер не изменяемая
                    if (!templateService.Changeable) {
                        sflUnitMeasure.setDisabled(true);
                    } else {
                        sflUnitMeasure.setDisabled(false);
                    }

                    form.setTitle(templateService.Name);

                    asp.controller.setCurrentId(record);

                    if (asp.hasSaveTabPanel) {
                        asp.saveTabPanel();
                    }

                    asp.controller.onUpdateTbp();

                    asp.controller.ScheduledPreventiveMaintanance = record.get('ScheduledPreventiveMaintanance') || 30 /*Не задано*/;
                }
            },
            saveTabPanel: function () {
                var housingCostItemGridAspect = this.controller.getAspect('housingCostItemGridAspect');
                if (housingCostItemGridAspect.getGrid()) {
                    housingCostItemGridAspect.save();
                }
            },

            otherActions: function (actions) {
                actions['#serviceGrid #serviceAddButton'] = { 'click': { fn: this.controller.addBtnClick, scope: this } };
                actions['#customServiceWindow #customServiceButton'] = { 'click': { fn: this.controller.customServiceBtnClick, scope: this, asp: this } };
                actions['#customServiceWindow b4closebutton'] = { 'click': { fn: this.onCloseCustomWindow, scope: this } };

                actions['#copyServiceWindow #copyServiceButton'] = { 'click': { fn: this.controller.copyServiceBtnClick, scope: this, asp: this } };
                actions['#copyServiceWindow #btnCopyServClear'] = { 'click': { fn: this.controller.btnCopyServClearClick, scope: this } };
                actions['#copyServiceWindow b4closebutton'] = { 'click': { fn: this.controller.closeCopyWindow, scope: this, asp: this } };

                //Подписка на события выбора вида шаблонной услуги и шаблонной услуги
                actions['#customServiceWindow #templateService'] = { 'beforeload': { fn: this.controller.onBeforeLoadTemplateService, scope: this } };
                actions['#customServiceWindow #KindServiceDi'] = { 'change': { fn: this.controller.onChangeKindServiceDi, scope: this } };
                actions['#customServiceWindow'] = { 'show': { fn: this.controller.onShowCustomServiceWindow, scope: this } };

                actions['#communalServiceEditWindow #cbTypeOfProvisionService'] = { 'change': { fn: this.changeTypeOfProvisionCommunal, scope: this } };
                actions['#capitalRepairServiceEditWindow #cbTypeOfProvisionService'] = { 'change': { fn: this.changeTypeOfProvisionCapRepair, scope: this } };
                actions['#repairServiceEditWindow #cbTypeOfProvisionService'] = { 'change': { fn: this.changeTypeOfProvisionRepair, scope: this } };
                actions['#housingServiceEditWindow #cbTypeOfProvisionService'] = { 'change': { fn: this.changeTypeOfProvisionHousing, scope: this } };

                //подписка на изменение поставщика
                actions['#communalServiceEditWindow #changeProviderButton'] = { 'click': { fn: this.changeProvider, scope: this, asp: 'communalProviderServiceGridWindowAspect' } };
                actions['#capitalRepairServiceEditWindow #changeProviderButton'] = { 'click': { fn: this.changeProvider, scope: this, asp: 'capRepairProviderServiceGridWindowAspect' } };
                actions['#repairServiceEditWindow #changeProviderButton'] = { 'click': { fn: this.changeProvider, scope: this, asp: 'repairProviderServiceGridWindowAspect' } };
                actions['#housingServiceEditWindow #changeProviderButton'] = { 'click': { fn: this.changeProvider, scope: this, asp: 'housingProviderServiceGridWindowAspect' } };
                actions['#controlServiceEditWindow #changeProviderButton'] = { 'click': { fn: this.changeProvider, scope: this, asp: 'controlProviderServiceGridWindowAspect' } };
                actions['#additionalServiceEditWindow #changeProviderButton'] = { 'click': { fn: this.changeProvider, scope: this, asp: 'additProviderServiceGridWindowAspect' } };

                //подписка на сохранение для различных форм услуг
                actions['#communalServiceEditWindow b4savebutton'] = { 'click': { fn: this.saveRequestHandler, scope: this } };
                actions['#capitalRepairServiceEditWindow b4savebutton'] = { 'click': { fn: this.saveRequestHandler, scope: this } };
                actions['#repairServiceEditWindow b4savebutton'] = { 'click': { fn: this.saveRequestHandler, scope: this } };
                actions['#housingServiceEditWindow b4savebutton'] = { 'click': { fn: this.saveRequestHandler, scope: this } };
                actions['#controlServiceEditWindow b4savebutton'] = { 'click': { fn: this.saveRequestHandler, scope: this } };
                actions['#additionalServiceEditWindow b4savebutton'] = { 'click': { fn: this.saveRequestHandler, scope: this } };

                //подписка на закрытие для различных форм услуг
                actions['#communalServiceEditWindow b4closebutton'] = { 'click': { fn: this.closeWindowHandler, scope: this } };
                actions['#capitalRepairServiceEditWindow b4closebutton'] = { 'click': { fn: this.closeWindowHandler, scope: this } };
                actions['#repairServiceEditWindow b4closebutton'] = { 'click': { fn: this.closeWindowHandler, scope: this } };
                actions['#housingServiceEditWindow b4closebutton'] = { 'click': { fn: this.closeWindowHandler, scope: this } };
                actions['#controlServiceEditWindow b4closebutton'] = { 'click': { fn: this.closeWindowHandler, scope: this } };
                actions['#additionalServiceEditWindow b4closebutton'] = { 'click': { fn: this.closeWindowHandler, scope: this } };

                //Подписка на событие выбора поставщика(Выбирается либо поставщик жилищных услуг либо ресурсосбер орг взависимоти от типа услуги)
                actions['#additionalServiceEditWindow #sflProvider'] = {
                    'beforeload': { fn: this.controller.onBeforeLoadProvider, scope: this },
                    'change': { fn: this.onChangeProvider, scope: this }
                };
                actions['#communalServiceEditWindow #sflProvider'] = { 'beforeload': { fn: this.controller.onBeforeLoadProvider, scope: this } };
                actions['#capitalRepairServiceEditWindow #sflProvider'] = { 'beforeload': { fn: this.controller.onBeforeLoadProvider, scope: this } };
                actions['#housingServiceEditWindow #sflProvider'] = { 'beforeload': { fn: this.controller.onBeforeLoadProvider, scope: this } };
                actions['#housingServiceEditWindow #CitizenSuggestion'] = { 'load': { fn: this.controller.onBeforeLoadProvider, scope: this } };
                actions['#controlServiceEditWindow #sflProvider'] = { 'beforeload': { fn: this.controller.onBeforeLoadProvider, scope: this } };
                actions['#serviceGrid #btnUnfilledMandatoryServs'] = { 'click': { fn: this.unfillMandServBtnClick, scope: this } };

                actions['#communalServiceEditWindow'] = {
                    'render': function (w) {
                        var me = this,
                            params = me.params || {},
                            selectedDate = params.year,
                            selectedYear = 0;

                        if (selectedDate) {
                            selectedYear = new Date(selectedDate).getFullYear();
                        }

                        if (selectedYear >= 2015) {
                            w.down('#tbCommunalGrids').child('#tbContainer').tab.show();
                        } else {
                            w.down('#tbCommunalGrids').child('#tbContainer').tab.hide();
                        }

                    }
                };
            },

            unfillMandServBtnClick: function () {
                this.controller.mask('Загрузка', this.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('GetUnfilledMandatoryServs', 'Service', {
                    disclosureInfoRealityObjId: this.controller.params.disclosureInfoRealityObjId
                })).next(function (response) {
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText);
                    Ext.Msg.alert('Сообщение!', obj.message);
                    this.controller.unmask();
                }, this)
                    .error(function () {
                        this.controller.unmask();
                    }, this);
            },

            onChangeProvider: function (field, newValue, oldValue) {
                var form = this.getForm();
                form.down('#tfOgrn').setValue('');
                form.down('#dfDateRegistartion').setValue('');
                if (newValue) {

                    form.down('#tfOgrn').setValue(newValue.Ogrn);
                    form.down('#dfDateRegistartion').setValue(newValue.ActivityDateStart);
                }
            },

            //делает видимые элементы для данного fieldset невидимыми взависимости от настройки IsHidden
            //isFieldSet - поле находится в филдсете и на него уже наложена логика сокрытия отображения
            //поэтому в данном случае только скрываем, а за отображение отвечает логика сокрытия в филдсете
            //Иначе скрываем и отображаем
            acceptOptionFields: function (objCollection, form, isFieldSet) {
                Ext.Array.each(objCollection, function (obj) {
                    var field = form.getForm().findField(obj.FieldName);
                    if (field) {
                        if (isFieldSet) {
                            if (obj.IsHidden) {
                                field.hide();
                                field.allowBlank = true;
                            }
                        } else {
                            obj.IsHidden ? field.hide() : field.show();
                        }
                    }
                });
            },

            changeProvider: function (btn, event, providerAspect) {
                if (!Ext.isEmpty(this.controller.baseServiceId) && this.controller.baseServiceId != 0) {
                    var aspect = this.controller.getAspect(providerAspect.asp);
                    aspect.editRecord(null);

                } else {
                    Ext.Msg.alert('Внимание!', 'Перед тем как задать поставщика следует сохранить услугу');
                }
            },

            changeTypeOfProvisionCommunal: function (newValue) {
                var form = this.getForm();
                switch (newValue.getValue()) {
                    case 10:
                        form.down('#fsService').setTitle('Услуга предоставляется через УО');
                        form.down('#sflUnitMeasure').show();
                        form.down('#changeProviderButton').show();
                        form.down('#sflProvider').allowBlank = false;
                        form.down('#sflProvider').show();
                        form.down('#nfProfit').show();
                        form.down('#cbKindElectricitySupply').show();
                        form.down('#nfVolumePurchasedResources').show();
                        form.down('#nfVolumePurchasedResources').allowBlank = false;
                        form.down('#nfPricePurchasedResources').show();
                        form.down('#nfPricePurchasedResources').allowBlank = false;
                        form.down('#fsService').show();
                        break;
                    case 20:
                        form.down('#fsService').setTitle('Услуга предоставляется без участия УО');
                        form.down('#sflUnitMeasure').hide();
                        form.down('#changeProviderButton').show();
                        form.down('#sflProvider').allowBlank = false;
                        form.down('#sflProvider').show();
                        form.down('#nfProfit').hide();
                        form.down('#cbKindElectricitySupply').hide();
                        form.down('#nfVolumePurchasedResources').hide();
                        form.down('#nfVolumePurchasedResources').allowBlank = true;
                        form.down('#nfPricePurchasedResources').hide();
                        form.down('#nfPricePurchasedResources').allowBlank = true;
                        form.down('#fsService').show();
                        break;
                    case 30:
                        form.down('#fsService').hide();
                        form.down('#sflUnitMeasure').hide();
                        form.down('#changeProviderButton').hide();
                        form.down('#sflProvider').allowBlank = true;
                        form.down('#sflProvider').hide();
                        form.down('#nfProfit').hide();
                        form.down('#cbKindElectricitySupply').hide();
                        form.down('#nfVolumePurchasedResources').hide();
                        form.down('#nfVolumePurchasedResources').allowBlank = true;
                        form.down('#nfPricePurchasedResources').hide();
                        form.down('#nfPricePurchasedResources').allowBlank = true;
                        break;
                    default:
                        form.down('#sflUnitMeasure').hide();
                        form.down('#changeProviderButton').hide();
                        form.down('#sflProvider').allowBlank = true;
                        form.down('#sflProvider').hide();
                        form.down('#nfProfit').hide();
                        form.down('#cbKindElectricitySupply').hide();
                        form.down('#nfVolumePurchasedResources').hide();
                        form.down('#nfVolumePurchasedResources').allowBlank = true;
                        form.down('#nfPricePurchasedResources').hide();
                        form.down('#nfPricePurchasedResources').allowBlank = true;
                        form.down('#fsService').hide();
                }
                this.acceptOptionFields(this.controller.optionFields, form, true);
                form.doLayout();
            },

            changeTypeOfProvisionCapRepair: function (newValue) {
                var form = this.getForm();
                switch (newValue.getValue()) {
                    case 10:
                        form.down('#fsService').setTitle('Услуга предоставляется через УО');
                        form.down('#sflUnitMeasure').show();
                        form.down('#changeProviderButton').show();
                        form.down('#sflProvider').show();
                        form.down('#sflProvider').allowBlank = false;
                        form.down('#nfProfit').show();
                        form.down('#cbRegionalFund').show();
                        form.down('#cbRegionalFund').allowBlank = false;
                        form.down('#fsService').show();
                        break;
                    case 20:
                        form.down('#fsService').setTitle('Услуга предоставляется без участия УО');
                        form.down('#sflUnitMeasure').hide();
                        form.down('#changeProviderButton').show();
                        form.down('#sflProvider').show();
                        form.down('#sflProvider').allowBlank = false;
                        form.down('#nfProfit').hide();
                        form.down('#cbRegionalFund').hide();
                        form.down('#cbRegionalFund').allowBlank = true;
                        form.down('#fsService').show();
                        break;
                    case 30:
                        form.down('#fsService').hide();
                        form.down('#sflUnitMeasure').hide();
                        form.down('#changeProviderButton').hide();
                        form.down('#sflProvider').hide();
                        form.down('#sflProvider').allowBlank = true;
                        form.down('#nfProfit').hide();
                        form.down('#cbRegionalFund').hide();
                        form.down('#cbRegionalFund').allowBlank = true;
                        break;
                    default:
                        form.down('#sflUnitMeasure').hide();
                        form.down('#changeProviderButton').hide();
                        form.down('#sflProvider').hide();
                        form.down('#sflProvider').allowBlank = true;
                        form.down('#nfProfit').hide();
                        form.down('#cbRegionalFund').hide();
                        form.down('#cbRegionalFund').allowBlank = true;
                        form.down('#fsService').hide();
                }
                this.acceptOptionFields(this.controller.optionFields, form, true);
                form.doLayout();
            },

            changeTypeOfProvisionRepair: function (newValue) {
                var form = this.getForm();
                switch (newValue.getValue()) {
                    case 10:
                        form.down('#fsService').setTitle('Услуга предоставляется через УО');
                        form.down('#sflUnitMeasure').show();
                        form.down('#changeProviderButton').show();
                        form.down('#sflProvider').show();
                        form.down('#sflProvider').allowBlank = true;
                        form.down('#nfProfit').show();
                        form.down('#fsService').show();
                        break;
                    case 20:
                        form.down('#fsService').setTitle('Услуга предоставляется без участия УО');
                        form.down('#sflUnitMeasure').hide();
                        form.down('#changeProviderButton').show();
                        form.down('#sflProvider').show();
                        form.down('#sflProvider').allowBlank = true;
                        form.down('#nfProfit').hide();
                        form.down('#fsService').show();
                        break;
                    case 30:
                        form.down('#fsService').hide();
                        form.down('#sflUnitMeasure').hide();
                        form.down('#changeProviderButton').hide();
                        form.down('#sflProvider').hide();
                        form.down('#sflProvider').allowBlank = true;
                        form.down('#nfProfit').hide();
                        form.down('#sumWorkTo').allowBlank = true;
                        break;
                    default:
                        form.down('#sflUnitMeasure').hide();
                        form.down('#changeProviderButton').hide();
                        form.down('#sflProvider').hide();
                        form.down('#sflProvider').allowBlank = true;
                        form.down('#nfProfit').hide();
                        form.down('#fsService').hide();
                }
                this.acceptOptionFields(this.controller.optionFields, form, true);
                form.doLayout();
            },

            changeTypeOfProvisionHousing: function (newValue) {
                var form = this.getForm();
                switch (newValue.getValue()) {
                    case 10:
                        form.down('#fsService').setTitle('Услуга предоставляется через УО');
                        form.down('#sflUnitMeasure').show();
                        form.down('#changeProviderButton').show();
                        form.down('#sflProvider').allowBlank = false;
                        form.down('#sflProvider').show();
                        form.down('#nfProfit').show();
                        form.down('#sflPeriodicity').show();
                        form.down('#sflPeriodicity').allowBlank = false;
                        form.down('#nfProtocolNumber').hide();
                        form.down('#nfProtocolNumber').allowBlank = true;
                        form.down('#dfProtocolFrom').hide();
                        form.down('#ffProtocol').hide();
                        form.down('#ffProtocol').allowBlank = true;
                        form.down('#fsService').show();
                        form.down('#cbEquipment').show();
                        break;
                    case 20:
                        form.down('#fsService').setTitle('Услуга предоставляется без участия УО');
                        form.down('#sflUnitMeasure').hide();
                        form.down('#changeProviderButton').show();
                        form.down('#sflProvider').allowBlank = false;
                        form.down('#sflProvider').show();
                        form.down('#nfProfit').hide();
                        form.down('#sflPeriodicity').hide();
                        form.down('#sflPeriodicity').allowBlank = true;
                        form.down('#nfProtocolNumber').hide();
                        form.down('#nfProtocolNumber').allowBlank = true;
                        form.down('#dfProtocolFrom').hide();
                        form.down('#ffProtocol').hide();
                        form.down('#ffProtocol').allowBlank = true;
                        form.down('#fsService').show();
                        form.down('#cbEquipment').hide();
                        break;
                    case 40:
                        form.down('#fsService').setTitle('Собственники отказались от предоставления услуги');
                        form.down('#sflUnitMeasure').hide();
                        form.down('#changeProviderButton').hide();
                        form.down('#sflProvider').hide();
                        form.down('#sflProvider').allowBlank = true;
                        form.down('#nfProfit').hide();
                        form.down('#sflPeriodicity').hide();
                        form.down('#sflPeriodicity').allowBlank = true;
                        form.down('#nfProtocolNumber').show();
                        form.down('#nfProtocolNumber').allowBlank = false;
                        form.down('#dfProtocolFrom').show();
                        form.down('#ffProtocol').show();
                        form.down('#ffProtocol').allowBlank = false;
                        form.down('#fsService').show();
                        form.down('#cbEquipment').hide();
                        break;
                    default:
                        form.down('#sflUnitMeasure').hide();
                        form.down('#sflUnitMeasure').hide();
                        form.down('#changeProviderButton').hide();
                        form.down('#sflProvider').hide();
                        form.down('#sflProvider').allowBlank = true;
                        form.down('#nfProfit').hide();
                        form.down('#sflPeriodicity').hide();
                        form.down('#sflPeriodicity').allowBlank = true;
                        form.down('#nfProtocolNumber').hide();
                        form.down('#nfProtocolNumber').allowBlank = true;
                        form.down('#dfProtocolFrom').hide();
                        form.down('#ffProtocol').hide();
                        form.down('#ffProtocol').allowBlank = true;
                        form.down('#fsService').hide();
                        form.down('#cbEquipment').hide();
                }
                this.acceptOptionFields(this.controller.optionFields, form, true);
                form.doLayout();
            },

            onCloseCustomWindow: function () {
                var customWindow = this.controller.getServiceWindow(this);
                customWindow.close();
            },

            validateGrid: function (asp, record) {
                var storeTariffConsumers,
                    storeProviderService,
                    storeTariffForRso,
                    form = asp.getForm(),
                    result = false,
                    msg = '',
                    kindServiceValue = record.get('TemplateService').KindServiceDi,
                    provider = record.get('Provider'),
                    provisionServ = record.get('TypeOfProvisionService');

                if (provisionServ != 10 && provisionServ) {
                    //Услуга предоставляется не через УО
                    result = true;
                } else {
                    switch (kindServiceValue) {
                        //Коммунальная
                        case 10:
                            var resultConsumers, resultRso;
                            storeTariffConsumers = form.down('#tariffForConsumersCommunalGrid').getStore();
                            if (storeTariffConsumers.getCount() > 0) {
                                storeTariffConsumers.each(function (rec) {
                                    if (!rec.phantom) {
                                        if (rec.get('DateStart') && rec.get('TariffIsSetFor') && !Ext.isEmpty(rec.get('Cost'))) {
                                            resultConsumers = true;
                                        }
                                    }
                                });
                            }
                            storeTariffForRso = form.down('#tariffForRsoCommunalGrid').getStore();
                            if (storeTariffForRso.getCount() > 0) {
                                storeTariffForRso.each(function (rec) {
                                    if (!rec.phantom) {
                                        if (rec.get('DateStart')
                                            && rec.get('NumberNormativeLegalAct')
                                            && rec.get('OrganizationSetTariff')
                                            && rec.get('DateNormativeLegalAct')
                                            && !Ext.isEmpty(rec.get('Cost'))) {
                                            resultRso = true;
                                        }
                                    }
                                });
                            }

                            result = resultConsumers && resultRso;
                            if (!result) {
                                msg = 'Не заполнены обязательные поля или не указана информация по тарифам.';
                            }

                            var noPhantomRecCommunalCount = 0;
                            storeProviderService = form.down('#providerServiceCommunalGrid').getStore();
                            if (storeProviderService.getCount() > 0) {
                                storeProviderService.each(function (rec) {
                                    if (!rec.phantom) {
                                        noPhantomRecCommunalCount++;
                                    }
                                });
                            }
                            if (noPhantomRecCommunalCount <= 0 && !provider) {
                                msg += 'Необходимо добавить поставщика услуг.';
                            }
                            result = result && noPhantomRecCommunalCount > 0;

                            break;
                            //Жилищная
                        case 20:
                            storeTariffConsumers = form.down('#tariffForConsumersHousingGrid').getStore();
                            if (storeTariffConsumers.getCount() > 0) {
                                storeTariffConsumers.each(function (rec) {
                                    if (!rec.phantom) {
                                        if (!Ext.isEmpty(rec.get('Cost'))) {
                                            result = true;
                                        }
                                    }
                                });
                            }
                            if (!result) {
                                msg = 'Не заполнены обязательные поля или не указана информация по тарифам.';
                            }

                            var noPhantomRecHousingCount = 0;
                            storeProviderService = form.down('#providerServiceHousingGrid').getStore();
                            if (storeProviderService.getCount() > 0) {
                                storeProviderService.each(function (rec) {
                                    if (!rec.phantom) {
                                        noPhantomRecHousingCount++;
                                    }
                                });
                            }
                            if (noPhantomRecHousingCount <= 0 && !provider) {
                                msg += 'Необходимо добавить поставщика услуг.';
                            }

                            result = result && noPhantomRecHousingCount > 0;

                            break;
                            //Ремонт
                        case 30:
                            var resultRepConsumers;
                            storeTariffConsumers = form.down('#tariffForConsumersRepairGrid').getStore();
                            if (storeTariffConsumers.getCount() > 0) {
                                storeTariffConsumers.each(function (rec) {
                                    if (!rec.phantom) {
                                        if (!Ext.isEmpty(rec.get('Cost'))) {
                                            resultRepConsumers = true;
                                        }
                                    }
                                });
                            }

                            var noPhantomRecRepairCount = 0;
                            storeProviderService = form.down('#providerServiceRepairGrid').getStore();
                            if (storeProviderService.getCount() > 0) {
                                storeProviderService.each(function (rec) {
                                    if (!rec.phantom) {
                                        noPhantomRecRepairCount++;
                                    }
                                });
                            }
                            if (noPhantomRecRepairCount <= 0 && !provider) {
                                msg += 'Необходимо добавить поставщика услуг.';
                            }

                            result = noPhantomRecRepairCount > 0;

                            break;
                            //Кап. ремонт
                        case 40:
                            storeTariffConsumers = form.down('#tariffForConsumersCapRepGrid').getStore();
                            if (storeTariffConsumers.getCount() > 0) {
                                storeTariffConsumers.each(function (rec) {
                                    if (!rec.phantom) {
                                        if (!Ext.isEmpty(rec.get('Cost'))) {
                                            result = true;
                                        }
                                    }
                                });
                            }

                            if (!result) {
                                msg = 'Не заполнены обязательные поля или не указана информация по тарифам.';
                            }

                            var noPhantomRecCapRepairCount = 0;
                            storeProviderService = form.down('#providerServiceCapRepairGrid').getStore();
                            if (storeProviderService.getCount() > 0) {
                                storeProviderService.each(function (rec) {
                                    if (!rec.phantom) {
                                        noPhantomRecCapRepairCount++;
                                    }
                                });
                            }
                            if (noPhantomRecCapRepairCount <= 0 && !provider) {
                                msg += 'Необходимо добавить поставщика услуг.';
                            }

                            result = result && noPhantomRecCapRepairCount > 0;

                            break;
                            //Управление МКД
                        case 50:
                            storeTariffConsumers = form.down('#tariffForConsumersControlGrid').getStore();
                            if (storeTariffConsumers.getCount() > 0) {
                                storeTariffConsumers.each(function (rec) {
                                    if (!rec.phantom) {
                                        if (rec.get('DateStart') && rec.get('TariffIsSetFor') && !Ext.isEmpty(rec.get('Cost')) && rec.get('TypeOrganSetTariffDi')) {
                                            result = true;
                                        }
                                    }
                                });
                            }
                            if (!result) {
                                msg = 'Не заполнены обязательные поля или не указана информация по тарифам.';
                            }

                            var noPhantomRecControlCount = 0;
                            storeProviderService = form.down('#providerServiceControlGrid').getStore();
                            if (storeProviderService.getCount() > 0) {
                                storeProviderService.each(function (rec) {
                                    if (!rec.phantom) {
                                        noPhantomRecControlCount++;
                                    }
                                });
                            }
                            if (noPhantomRecControlCount <= 0 && !provider) {
                                msg += 'Необходимо добавить поставщика услуг.';
                            }

                            result = result && noPhantomRecControlCount > 0;

                            break;
                            //Дополнительная                            
                        case 60:
                            storeTariffConsumers = form.down('#tariffForConsumersAdditionalGrid').getStore();
                            if (storeTariffConsumers.getCount() > 0) {
                                storeTariffConsumers.each(function (rec) {
                                    if (!rec.phantom) {
                                        if (rec.get('DateStart') && rec.get('TariffIsSetFor') && !Ext.isEmpty(rec.get('Cost'))) {
                                            result = true;
                                        }
                                    }
                                });
                            }

                            if (!result) {
                                msg = 'Не заполнены обязательные поля или не указана информация по тарифам.';
                            }

                            var noPhantomRecAdditCount = 0;
                            storeProviderService = form.down('#providerServiceAdditionalGrid').getStore();
                            if (storeProviderService.getCount() > 0) {
                                storeProviderService.each(function (rec) {
                                    if (!rec.phantom) {
                                        noPhantomRecAdditCount++;
                                    }
                                });
                            }
                            if (noPhantomRecAdditCount <= 0 && !provider) {
                                msg += 'Необходимо добавить поставщика услуг.';
                            }

                            result = result && noPhantomRecAdditCount > 0;

                            break;
                    }
                }

                if (!result) {
                    Ext.Msg.alert('Внимание!', msg);
                }

                return result;
            },

            getModel: function (record) {
                asp = this;
                if (record) {
                    switch (record.get('TemplateService') ? record.get('TemplateService').KindServiceDi : record.get('KindServiceDi')) {
                        //Коммунальная
                        case 10:
                            asp.modelName = 'service.Communal';
                            break;
                            //Жилищная
                        case 20:
                            asp.modelName = 'service.Housing';
                            break;
                            //Ремонт
                        case 30:
                            asp.modelName = 'service.Repair';
                            break;
                            //Кап. ремонт
                        case 40:
                            asp.modelName = 'service.CapitalRepair';
                            break;
                            //Управление МКД
                        case 50:
                            asp.modelName = 'service.Control';
                            break;
                            //Дополнительная
                        case 60:
                            asp.modelName = 'service.Additional';
                            break;
                    }
                }
                return asp.controller.getModel(asp.modelName);
            }
        }
    ],

    init: function () {
        var me = this,
            actions = {};
        actions[me.mainViewSelector] = { 'afterrender': { fn: me.onMainViewAfterRender, scope: me } };

        me.control(actions);

        me.getStore('service.Base').on('beforeload', me.onBeforeLoad, me);

        me.getStore('service.TariffForConsumers').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.TariffForRso').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.ConsumptionNormsNpa').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.TariffForConsumersCapRep').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.TariffForConsumersRepair').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.TariffForConsumersHousing').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.TariffForConsumersControl').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.TariffForConsumersAdditional').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.HousingCostItem').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.WorkCapRepair').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.WorkRepairList').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.WorkRepairTechServ').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.WorkRepairDetail').on('beforeload', me.onBeforeLoadTariff, me, 'withSelModel');

        me.getStore('service.ProviderServiceAdditional').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.ProviderServiceControl').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.ProviderServiceHousing').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.ProviderServiceCommunal').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.ProviderServiceRepair').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.ProviderServiceCapRepair').on('beforeload', me.onBeforeLoadTariff, me);

        me.getStore('service.HousingCostItem').on('update', me.lockSaveButton, me);

        me.getStore('service.HousingCostItem').on('load', me.lockSaveButton, me);

        me.callParent(arguments);
    },

    lockSaveButton: function () {
        var me = this,
            store = me.getHousingCostittemGrid() ? me.getHousingCostittemGrid().getStore() : null,
            length = store !== null ? store.data.length : 0,
            view = me.getHousingEditWindow();

        if (view) {
            view.SavebuttonisEnabled = length === 0;
        }
    },

    onLaunch: function () {
        if (this.params) {
            this.getStore('service.Base').load();

            var me = this;
            me.params.getId = function () { return me.params.disclosureInfoId; };
            this.getAspect('servicePermissionAspect').setPermissionsByRecord(this.params);
        }
    },

    onMainViewAfterRender: function () {

        var mainGrid = this.getMainComponent();
        if (mainGrid && this.params) {
            var controller = this;
            this.mask('Загрузка', mainGrid);
            B4.Ajax.request(B4.Url.action('GetCountMandatory', 'Service', {
                disclosureInfoRealityObjId: controller.params.disclosureInfoRealityObjId
            })).next(function (response) {
                //десериализуем полученную строку
                var obj = Ext.JSON.decode(response.responseText);
                var text = obj.countDifficit > 0 ? 'Количество не заполненных обязательных услуг ' + obj.countDifficit : '';
                mainGrid.down('#lbCountDifficit').setText(text);
                this.unmask();
            }, this)
                .error(function () {
                    this.unmask();
                }, this);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params)
            operation.params.disclosureInfoRealityObjId = this.params.disclosureInfoRealityObjId;
    },

    onBeforeLoadTemplateService: function (field, options) {
        if (this.controller.params) {
            options.params = {};
            options.params.kindTemplateService = this.controller.params.kindTemplateService;
            options.params.isTemplateService = true;
        }
    },

    onBeforeLoadProvider: function (field, options) {
        options.params = {};
        if (this.controller.typeProvider == 'ResourceOrg') {
            options.params.typeJurOrg = 20;
            options.params.typeServOrg = 0;
        } else if (this.controller.typeProvider == 'ServiceOrg') {
            options.params.typeJurOrg = 0;
            options.params.typeServOrg = 20;
        }
    },

    onBeforeLoadTariff: function (store, operation, opts) {
        operation.params = {};
        operation.params.baseServiceId = this.baseServiceId;

        if (opts == 'withSelModel') {
            operation.params.groupWorkPprId = this.currentGroupWorkPpr;
        }
    },

    onChangeKindServiceDi: function (newValue) {
        var win = this.controller.getServiceWindow(this);

        this.controller.params.kindTemplateService = newValue.getValue();
        win.down('#templateService').setValue(null);
    },

    onShowCustomServiceWindow: function (win) {
        win.down('#KindServiceDi').clearValue();
        win.down('#templateService').setValue(null);
    },

    addBtnClick: function () {
        var customWindow = this.controller.getCustomWindow(this);
        customWindow.show();
    },

    customServiceBtnClick: function (btn, event, aspect, record) {
        var asp = aspect.asp;
        var customWindow = asp.controller.getServiceWindow(asp);
        var kindServiceValue;
        if (customWindow && !customWindow.isHidden()) {
            kindServiceValue = customWindow.down('#KindServiceDi').getValue();
            asp.controller.templateService = customWindow.down('#templateService').value;

            if (!asp.controller.templateService || !kindServiceValue) {
                Ext.Msg.alert('Внимание!', 'Необходимо выбрать услугу');
                return;
            }

            asp.controller.templateServiceId = asp.controller.templateService.Id;
        }
        if (record && record.get('KindServiceDi')) {
            kindServiceValue = record.get('KindServiceDi');
        }

        if (kindServiceValue) {
            switch (kindServiceValue) {
                //Коммунальная
                case 10:
                    asp.modelName = 'service.Communal';
                    asp.editFormSelector = '#communalServiceEditWindow';
                    asp.editWindowView = 'service.communal.EditWindow';

                    asp.controller.tbpnl = '#tbCommunalGrids';
                    asp.controller.typeProvider = 'ResourceOrg';
                    break;
                    //Жилищная
                case 20:
                    asp.modelName = 'service.Housing';
                    asp.editFormSelector = '#housingServiceEditWindow';
                    asp.editWindowView = 'service.housing.EditWindow';

                    asp.controller.tbpnl = '#tbHousingGrids';
                    asp.controller.typeProvider = 'ServiceOrg';
                    break;
                    //Ремонт
                case 30:
                    asp.modelName = 'service.Repair';
                    asp.editFormSelector = '#repairServiceEditWindow';
                    asp.editWindowView = 'service.repair.EditWindow';

                    asp.controller.tbpnl = '#tbRepairGrids';
                    asp.controller.typeProvider = 'ServiceOrg';
                    break;
                    //Кап. ремонт
                case 40:
                    asp.modelName = 'service.CapitalRepair';
                    asp.editFormSelector = '#capitalRepairServiceEditWindow';
                    asp.editWindowView = 'service.caprepair.EditWindow';

                    asp.controller.tbpnl = '#tbCapRepairGrids';
                    asp.controller.typeProvider = 'ServiceOrg';
                    break;
                    //Управление МКД
                case 50:
                    asp.modelName = 'service.Control';
                    asp.editFormSelector = '#controlServiceEditWindow';
                    asp.editWindowView = 'service.control.EditWindow';

                    asp.controller.tbpnl = '#tbControlGrids';
                    asp.controller.typeProvider = 'ServiceOrg';
                    break;
                    //Дополнительная                            
                case 60:
                    asp.modelName = 'service.Additional';
                    asp.editFormSelector = '#additionalServiceEditWindow';
                    asp.editWindowView = 'service.additional.EditWindow';

                    asp.controller.tbpnl = '#tbAdditionalGrids';
                    asp.controller.typeProvider = 'ServiceOrg';
                    break;
            }

            asp.editRecord(record);
        }

        if (customWindow) {
            customWindow.close();
        }
    },

    updateProviderService: function (aspect) {
        var aspectService = aspect.controller.getAspect('serviceGridWindowAspect');
        var model = aspectService.getModel();
        model.load(aspect.controller.baseServiceId, {
            success: function (rec) {
                aspectService.getForm().down('#sflProvider').setValue(rec.get('Provider'));
            },
            scope: aspectService
        });
    },

    copyServiceBtnClick: function (btn, event, aspect, record) {
        var asp = aspect.asp;
        Ext.Msg.confirm('Копирование услуг!', 'Скопировать услугу в выбранные дома?', function (result) {
            if (result == 'yes') {
                var copyWindow = asp.controller.getCopyWindow(asp);
                var realityObjIds = [];
                asp.controller.getStore('menu.ManagingOrgRealityObjDataMenuServ').each(function (obj) {
                    realityObjIds.push(obj.get('Id'));
                });

                if (realityObjIds.length > 0) {
                    copyWindow.close();
                    asp.controller.mask('Копирование', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        method: 'POST',
                        url: B4.Url.action('CopyService', 'Service'),
                        params: {
                            realityObjIds: realityObjIds.join(','),
                            copingServiceId: asp.controller.copingServiceId,
                            copingServiceKind: asp.controller.copingServiceKind
                        },
                        timeout: 1200000
                    }).next(function (response) {
                        var resp = Ext.decode(response.responseText),
                            message = resp.message,
                            logFileId = resp.data,
                            url,
                            logLink = '';

                        if (logFileId) {
                            url = B4.Url.content(Ext.String.format('{0}/{1}?id={2}', 'FileUpload', 'Download', logFileId));
                            logLink = '<a href="' + url + '" target="_blank" style="color: #04468C !important; float: right;">Скачать подробный лог</a>';
                        }

                        Ext.Msg.show({
                            title: 'Копирование',
                            msg: Ext.String.format('{0}&nbsp;{1}', message, logLink),
                            width: 300,
                            buttons: Ext.Msg.OK,
                            icon: Ext.window.MessageBox.INFO
                        });

                        asp.controller.unmask();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });
                } else {
                    Ext.Msg.alert('Внимание!', 'Необходимо выбрать один или несколько домов');
                }
            }
        }, asp);
    },

    closeCopyWindow: function (btn, event, aspect, record) {
        var asp = aspect.asp;
        asp.controller.getCopyWindow(asp).close();
    },

    btnCopyServClearClick: function () {
        this.controller.getStore('menu.ManagingOrgRealityObjDataMenuServ').removeAll();
    },

    getCustomWindow: function (asp) {
        if (asp.customFormSelector) {
            var customWindow = Ext.ComponentQuery.query(asp.customFormSelector)[0];

            if (customWindow) {
                customWindow.Close();
            }

            if (!customWindow) {
                customWindow = asp.controller.getView(asp.customWindowView).create();
            }

            return customWindow;
        }
        return null;
    },

    getCopyWindow: function (asp) {
        if (asp.copyFormSelector) {
            var copyWindow = Ext.ComponentQuery.query(asp.copyFormSelector)[0];
            if (!copyWindow) {
                copyWindow = asp.controller.getView(asp.copyWindowView).create();
            }

            return copyWindow;
        }
        return null;
    },

    setCurrentId: function (rec) {
        var me = this,
            service = rec.get('TemplateService');
        me.templateServiceId = null;
        me.baseServiceId = rec.getId();
    },

    onUpdateTbp: function () {
        var me = this,
            tbpnl = Ext.ComponentQuery.query(me.tbpnl)[0],

            storeTariffForConsumers = me.getStore('service.TariffForConsumers'),
            storeTariffForConsumersCapRep = me.getStore('service.TariffForConsumersCapRep'),
            storeTariffForConsumersRepair = me.getStore('service.TariffForConsumersRepair'),
            storeTariffForConsumersHousing = me.getStore('service.TariffForConsumersHousing'),
            storeTariffForConsumersControl = me.getStore('service.TariffForConsumersControl'),
            storeTariffForConsumersAdditional = me.getStore('service.TariffForConsumersAdditional'),
            storeProviderServiceAdditional = me.getStore('service.ProviderServiceAdditional'),
            storeProviderServiceControl = me.getStore('service.ProviderServiceControl'),
            storeProviderServiceHousing = me.getStore('service.ProviderServiceHousing'),
            storeProviderServiceRepair = me.getStore('service.ProviderServiceRepair'),
            storeProviderServiceCapRepair = me.getStore('service.ProviderServiceCapRepair'),
            storeProviderServiceCommunal = me.getStore('service.ProviderServiceCommunal'),
            storeHousingCostItem = me.getStore('service.HousingCostItem'),
            storeWorkCapRepair = me.getStore('service.WorkCapRepair'),
            storeTariffForRso = me.getStore('service.TariffForRso'),
            storeConsumptionNormsNpa = me.getStore('service.ConsumptionNormsNpa'),
            storeWorkRepairList = me.getStore('service.WorkRepairList'),
            storeWorkRepairDetail = me.getStore('service.WorkRepairDetail'),
            storeWorkRepairTechServ = me.getStore('service.WorkRepairTechServ');

        storeTariffForConsumers.removeAll();
        storeTariffForConsumersCapRep.removeAll();
        storeTariffForConsumersRepair.removeAll();
        storeTariffForConsumersHousing.removeAll();
        storeTariffForConsumersControl.removeAll();
        storeTariffForConsumersAdditional.removeAll();
        storeProviderServiceAdditional.removeAll();
        storeProviderServiceControl.removeAll();
        storeProviderServiceHousing.removeAll();
        storeProviderServiceRepair.removeAll();
        storeProviderServiceCapRepair.removeAll();
        storeProviderServiceCommunal.removeAll();
        storeHousingCostItem.removeAll();
        storeWorkCapRepair.removeAll();
        storeTariffForRso.removeAll();
        storeConsumptionNormsNpa.removeAll();
        storeWorkRepairList.removeAll();
        storeWorkRepairDetail.removeAll();
        storeWorkRepairTechServ.removeAll();

        if (me.baseServiceId > 0) {
            if (tbpnl.down('#workRepairDetailAddButton')) {
                tbpnl.down('#workRepairDetailAddButton').setDisabled(true);
            }
            tbpnl.setDisabled(false);

            switch (me.tbpnl) {
                case '#tbControlGrids':
                    storeTariffForConsumersControl.load();
                    storeProviderServiceControl.load();
                    break;
                case '#tbCommunalGrids':
                    storeTariffForConsumers.load();
                    storeTariffForRso.load();
                    storeConsumptionNormsNpa.load();
                    storeProviderServiceCommunal.load();
                    break;
                case '#tbHousingGrids':
                    storeTariffForConsumersHousing.load();
                    storeHousingCostItem.load();
                    storeProviderServiceHousing.load();
                    break;
                case '#tbCapRepairGrids':
                    storeTariffForConsumersCapRep.load();
                    storeWorkCapRepair.load();
                    storeProviderServiceCapRepair.load();
                    break;
                case '#tbAdditionalGrids':
                    storeTariffForConsumersAdditional.load();
                    storeProviderServiceAdditional.load();
                    break;
                case '#tbRepairGrids':
                    storeTariffForConsumersRepair.load();
                    storeWorkRepairList.load();
                    storeWorkRepairTechServ.load();
                    storeProviderServiceRepair.load();
                    break;
            }

            var gridList = Ext.ComponentQuery.query('#workRepairListGrid')[0];
            var gridDetail = Ext.ComponentQuery.query('#workRepairDetailGrid')[0];
            if (gridList) {
                gridList.getSelectionModel().on('selectionchange', function (sm, selectedRecord) {
                    if (selectedRecord.length > 0) {
                        me.currentGroupWorkPpr = selectedRecord[0].get('GroupWorkPpr');
                        tbpnl.down('#workRepairDetailAddButton').setDisabled(false);
                        gridDetail.getStore().load();
                    } else {
                        tbpnl.down('#workRepairDetailAddButton').setDisabled(true);
                        gridDetail.getStore().removeAll();
                    }
                });
            }

        } else {
            var store = me.getHousingCostittemGrid().getStore();
            store.load();
        }
    }
});