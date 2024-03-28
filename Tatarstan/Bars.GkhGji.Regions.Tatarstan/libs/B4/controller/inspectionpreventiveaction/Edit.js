Ext.define('B4.controller.inspectionpreventiveaction.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiInspection',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.inspectionpreventiveaction.EditPanel',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'inspectionpreventiveaction.InspectionPreventiveAction'
    ],

    stores: [
        'Contragent',
        'RealityObjectGji',
        'realityobj.ByTypeOrg',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected',
        'inspectionpreventiveaction.JointInspectionStore'
    ],

    views: [
        'inspectionpreventiveaction.EditPanel',
        'inspectiongji.RiskPrevWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'inspectionpreventiveaction.EditPanel',
    mainViewSelector: '#inspectionPreventiveActionEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'inspectionpreventiveactioneditpanelperm',
            editFormAspectName: 'inspectionPreventiveActionEditPanelAspect'
        },
        {
            // Аспект формирвоания документов для данного основания проверки
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'inspectionPreventiveActionCreateButtonAspect',
            buttonSelector: '#inspectionPreventiveActionEditPanel gjidocumentcreatebutton',
            containerSelector: '#inspectionPreventiveActionEditPanel',
            typeBase: 200 // InspectionPreventiveAction (Проверка по профилактическому мероприятию)
        },
        {
            // Аспект основной панели проверки
            xtype: 'gjiinspectionaspect',
            name: 'inspectionPreventiveActionEditPanelAspect',
            editPanelSelector: '#inspectionPreventiveActionEditPanel',
            modelName: 'inspectionpreventiveaction.InspectionPreventiveAction',
            otherActions: function (actions) {
                var me = this;
                actions[me.editPanelSelector + ' #btnDelete'] = { 'click': { fn: me.btnDeleteClick, scope: me } };
                actions[me.editPanelSelector + ' [name=RiskCategory]'] = { change: { fn: me.onChangeRiskCategoryData, scope: me } };
                actions[me.editPanelSelector + ' [name=RiskCategoryStartDate]'] = { change: { fn: me.onChangeRiskCategoryData, scope: me } };
            },
            listeners: {
                aftersetpaneldata: function (asp, rec, panel) {
                    var id = rec.getId();
                    this.controller.getAspect('inspectionPreventiveActionStateButtonAspect').setStateData(id, rec.get('State'));
                    this.controller.getAspect('inspectionPreventiveActionCreateButtonAspect').setData(id);
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
            name: 'inspectionPreventiveActionStateButtonAspect',
            stateButtonSelector: '#inspectionPreventiveActionEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    asp.controller.getAspect('inspectionPreventiveActionEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'inspectionPreventiveActionRealityObjectAspect',
            gridSelector: '#inspectionPreventiveActionRealityObjectGrid',
            storeName: 'inspectionpreventiveaction.RealityObject',
            modelName: 'RealityObjectGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#binspectionPreventiveActionRealityObjectMultiSelectWindow',
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
            name: 'inspectionPreventiveActionJointInspectionAspect',
            gridSelector: '#inspectionPreventiveActionJointInspectionGrid',
            storeName: 'inspectionpreventiveaction.JointInspectionStore',
            modelName: 'inspectiongji.InspectionBaseContragent',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#inspectionPreventiveActionJointInspectionMultiSelectWindow',
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
        me.getStore('inspectionpreventiveaction.RealityObject').on('beforeload', me.onBeforeLoad, me);
        me.getStore('inspectionpreventiveaction.JointInspectionStore').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('inspectionPreventiveActionEditPanelAspect').setData(this.params.inspectionId);

        }

        this.getStore('inspectionpreventiveaction.RealityObject').load();
        this.getStore('inspectionpreventiveaction.JointInspectionStore').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params)
            operation.params.inspectionId = this.params.inspectionId;
    }
});