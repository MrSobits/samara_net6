Ext.define('B4.controller.inspectionactionisolated.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiInspection',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.inspectionactionisolated.EditPanel',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.Ajax',
        'B4.store.RealityObjectGji',
        'B4.Url'
    ],

    models: [
        'RealityObjectGji',
        'inspectionactionisolated.InspectionActionIsolated'
    ],

    stores: [
        'realityobj.ByTypeOrg',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected',
        'Contragent',
        'inspectionactionisolated.JointInspectionStore'
    ],

    views: [
        'inspectionactionisolated.EditPanel',
        'inspectiongji.RiskPrevWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'inspectionactionisolated.EditPanel',
    mainViewSelector: '#inspectionActionIsolatedEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'inspectionactionisolatededitpanelperm',
            editFormAspectName: 'inspectionActionIsolatedEditPanelAspect'
        },
        {
            /*
            Аспект формирвоания документов для данного основания проверки
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'disposalCreateButtonAspect',
            buttonSelector: '#inspectionActionIsolatedEditPanel gjidocumentcreatebutton',
            containerSelector: '#inspectionActionIsolatedEditPanel',
            typeBase: 180 // Тип Проверки по мероприятиям без взаимодействия с контролируемыми лицами
        },
        {
            /*
            Аспект основной панели проверки по поручению руководства
            */
            xtype: 'gjiinspectionaspect',
            name: 'inspectionActionIsolatedEditPanelAspect',
            editPanelSelector: '#inspectionActionIsolatedEditPanel',
            modelName: 'inspectionactionisolated.InspectionActionIsolated',
            otherActions: function (actions) {
                var me = this;
                actions[me.editPanelSelector + ' #btnDelete'] = { 'click': { fn: me.btnDeleteClick, scope: me } };
                actions[me.editPanelSelector + ' [name=RiskCategory]'] = { change: { fn: me.onChangeRiskCategoryData, scope: me } };
                actions[me.editPanelSelector + ' [name=RiskCategoryStartDate]'] = { change: { fn: me.onChangeRiskCategoryData, scope: me } };
                actions[me.editPanelSelector + ' [name=AllCategory]'] = {
                    click: {
                        fn: function () {
                            var record = me.getRecord(),
                                contragentId = record.get('ContragentId');

                            Ext.History.add(Ext.String.format('contragentedit/{0}/risk', contragentId));
                        },
                        scope: me
                    }
                };
                actions[me.editPanelSelector + ' [name=PrevCategory]'] = {
                    click: {
                        fn: function () {
                            var prevWindow = Ext.create('B4.view.inspectiongji.RiskPrevWindow', {
                                renderTo: B4.getBody().getActiveTab().getEl(),
                                inspectionId: me.controller.params.inspectionId
                            });

                            prevWindow.on('beforeclose', function (win) {
                                if (win.saved) {
                                    me.setData(me.controller.params.inspectionId);
                                }
                            });

                            prevWindow.show();
                        },
                        scope: me
                    }
                };
            },
            listeners: {
                aftersetpaneldata: function (asp, rec, panel) {
                    this.controller.getAspect('inspectionActionIsolatedStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                    this.controller.getAspect('disposalCreateButtonAspect').setData(rec.get('Id'));

                    var typeObject = rec.get('TypeObject'),
                        jurPersonField =  panel.down('b4selectfield[name=JurPerson]'),
                        typeJurPersonField =  panel.down('b4enumcombo[name=TypeJurPerson]'),
                        personNameField =  panel.down('textfield[name=PersonName]'),
                        innField =  panel.down('[name=Inn]');
                    
                    switch(typeObject){
                        case B4.enums.TypeObjectAction.Individual:
                            jurPersonField.setVisible(false);
                            typeJurPersonField.setVisible(false);
                            personNameField.setVisible(true);
                            innField.setVisible(true);
                            break;
                        case B4.enums.TypeObjectAction.Legal:
                            jurPersonField.setVisible(true);
                            typeJurPersonField.setVisible(true);
                            personNameField.setVisible(false);
                            innField.setVisible(false);
                            break;
                        case B4.enums.TypeObjectAction.Official:
                            jurPersonField.setVisible(true);
                            typeJurPersonField.setVisible(true);
                            personNameField.setVisible(true);
                            innField.setVisible(true);
                            break;
                    }
                }
            },
            btnDeleteClick: function () {
                var me = this,
                    panel = me.getPanel(),
                    record = panel.getForm().getRecord();

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить проверку со всеми дочерними документами?', function (result) {
                    if (result == 'yes') {
                        me.mask('Удаление', B4.getBody());
                        record.destroy()
                            .next(function (result) {
                                //Обновляем дерево меню
                                me.unmask();
                                var tree = Ext.ComponentQuery.query(me.controller.params.treeMenuSelector)[0];
                                tree.getStore().load();
                            })
                            .error(function (result) {
                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                me.unmask();
                            });
                    }
                });
            },
            onChangeRiskCategoryData: function (field) {
                var riskCategory = field.up().down('[name=RiskCategory]'),
                    riskCategoryStartDate = field.up().down('[name=RiskCategoryStartDate]'),
                    allowBlank = Ext.isEmpty(riskCategory.getValue()) && Ext.isEmpty(riskCategoryStartDate.getValue());

                riskCategory.allowBlank = allowBlank;
                riskCategoryStartDate.allowBlank = allowBlank;

                riskCategory.validate();
                riskCategoryStartDate.validate();
            }
        },
        {
            xtype: 'statebuttonaspect',
            name: 'inspectionActionIsolatedStateButtonAspect',
            stateButtonSelector: '#inspectionActionIsolatedEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    asp.controller.getAspect('inspectionActionIsolatedEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'inspectionActionIsolatedRealityObjectAspect',
            gridSelector: '#inspectionActionIsolatedRealityObjectGrid',
            storeName: 'inspectionactionisolated.RealityObject',
            modelName: 'RealityObjectGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#binspectionActionIsolatedRealityObjectMultiSelectWindow',
            storeSelect: 'realityobj.ByTypeOrg',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.getId());
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddRealityObjects', 'InspectionGjiRealityObject'),
                            method: 'POST',
                            params: {
                                objectIds: Ext.encode(recordIds),
                                inspectionId: asp.controller.params.inspectionId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
            Аспект взаимодействия кнопки таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'inspectionActionIsolatedJointInspectionAspect',
            gridSelector: '#inspectionActionIsolatedJointInspectionGrid',
            storeName: 'inspectionactionisolated.JointInspectionStore',
            modelName: 'inspectiongji.InspectionBaseContragent',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#inspectionActionIsolatedJointInspectionMultiSelectWindow',
            storeSelect: 'Contragent',
            storeSelected: 'Contragent',
            titleSelectWindow: 'Выбор органов проверки',
            titleGridSelect: 'Контрагенты для отбора',
            titleGridSelected: 'Выбранные контрагенты',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddContragents', 'InspectionBaseContragent'),
                            method: 'POST',
                            params: {
                                contragentIds: Ext.encode(recordIds),
                                inspectionId: asp.controller.params.inspectionId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function (e) {
                            asp.controller.unmask();
                            Ext.Msg.alert('Ошибка!', e.message);
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать контрагентов');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.getStore('inspectionactionisolated.RealityObject').on('beforeload', me.onBeforeLoad, me);
        me.getStore('inspectionactionisolated.JointInspectionStore').on('beforeload', me.onBeforeLoad, me);
        me.getStore('inspectionactionisolated.RealityObject').on('load', me.onAfterLoad, me);
        me.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('inspectionActionIsolatedEditPanelAspect').setData(this.params.inspectionId);
        }

        this.getStore('inspectionactionisolated.RealityObject').load();
        this.getStore('inspectionactionisolated.JointInspectionStore').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params)
            operation.params.inspectionId = this.params.inspectionId;
    },

    onAfterLoad: function (store) {
        this.getMainView().down('gjidocumentcreatebutton').menu.items.items.forEach(function (item) {
            if (item.actionUrl === 'B4.controller.Disposal') {
                item.setDisabled(store.data.items.length === 0);
            }
        });
    }
});