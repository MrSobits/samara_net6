Ext.define('B4.controller.baselicenseapplicants.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiInspection',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.Ajax',
        'B4.Url',
        'B4.enums.LicenseRequestType'
    ],

    models: [
        'Disposal',
        'BaseLicenseApplicants',
        'RealityObjectGji'
    ],

    stores: [
        'baselicenseapplicants.RealityObject',
        'realityobj.ByTypeOrg',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'baselicenseapplicants.RealityObjectGrid',
        'baselicenseapplicants.EditPanel'
    ],

    mainView: 'baselicenseapplicants.EditPanel',
    mainViewSelector: 'baselicenseappeditpanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    baseStatementEditPanelSelector: 'baselicenseappeditpanel',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhGji.Inspection.BaseLicApplicants.Edit', applyTo: 'b4savebutton', selector: 'baselicenseappeditpanel' },
                { name: 'GkhGji.Inspection.BaseLicApplicants.Field.InspectionNumber_Edit', applyTo: '#tfInspectionNumber', selector: 'baselicenseappeditpanel' },
                {
                    name: 'GkhGji.Inspection.BaseLicApplicants.Register.RealityObject.ShowAll_View', applyTo: '[name=ShowAll]', selector: '#baseLicenseAppRealObjMultiSelectWindow',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) component.show();
                            else component.hide();
                        }
                    }
                },
                 {
                     name: 'GkhGji.Inspection.BaseStatement.Field.ReasonErpChecking_Edit', applyTo: 'b4enumcombo[name=ReasonErpChecking]', selector: '#baselicenseappeditpanel',
                     applyBy: function (component, allowed) {
                         component.setVisible(allowed);
                         component.setDisabled(!allowed);
                     }
                 }

            ]
        },
        {
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'baseLicenseAppCreateButtonAspect',
            buttonSelector: 'baselicenseappeditpanel gjidocumentcreatebutton',
            containerSelector: 'baselicenseappeditpanel',
            typeBase: 130 // Тип проверка обращения
        },
        {
            xtype: 'statebuttonaspect',
            name: 'baseLicenseAppStateButtonAspect',
            stateButtonSelector: 'baselicenseappeditpanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('baseLicenseAppEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            /*
            Аспект основной панели Проверки по обращению граждан
            */
            xtype: 'gjiinspectionaspect',
            name: 'baseLicenseAppEditPanelAspect',
            editPanelSelector: 'baselicenseappeditpanel',
            modelName: 'BaseLicenseApplicants',
            otherActions: function (actions) {
                var me = this;
                actions[me.editPanelSelector + ' #btnDelete'] = { 'click': { fn: me.btnDeleteClick, scope: me } };
            },
            
            onSaveSuccess: function (asp, record) {
                asp.controller.setInspectionId(record.get('Id'));
            },

            listeners: {
                aftersetpaneldata: function (asp, rec, panel) {
                    var me = this,
                        inspId = rec.get('Id'),
                        requestType = rec.get('ManOrgLicenseRequest') && rec.get('ManOrgLicenseRequest').Type;

                    asp.controller.params = asp.controller.params || {};

                    // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                    // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                    var callbackUnMask = asp.controller.params.callbackUnMask;
                    if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                        callbackUnMask.call();
                    }

                    if (requestType) {
                        panel.down('[name=InspectionType]').setVisible(
                            requestType === B4.enums.LicenseRequestType.GrantingLicense ||
                            requestType === B4.enums.LicenseRequestType.RenewalLicense  ||
                            requestType === B4.enums.LicenseRequestType.TerminationActivities);
                    }
                                 
                    asp.controller.setInspectionId(inspId);
                    
                    //Обновляем статусы
                    me.controller.getAspect('baseLicenseAppStateButtonAspect').setStateData(inspId, rec.get('State'));
                    //Обновляем кнопку Сформировать
                    me.controller.getAspect('baseLicenseAppCreateButtonAspect').setData(inspId);
                }
            },
            btnDeleteClick: function () {
                var me = this,
                    panel = me.getPanel(),
                    record = panel.getForm().getRecord();

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить документ?', function (result) {
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
            }
            
        },
        {
            /* 
            Аспект взаимодействия таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'baseLicenseAppRealityObjectAspect',
            gridSelector: 'baselicenseapprealobjgrid',
            storeName: 'basestatement.RealityObject',
            modelName: 'RealityObjectGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseLicenseAppRealObjMultiSelectWindow',
            storeSelect: 'realityobj.ByTypeOrg',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            toolbarItems: [
                {
                    xtype: 'checkbox',
                    margin: '0 5 0 0',
                    name: 'ShowAll',
                    fieldLabel: 'Показать все дома',
                    labelWidth: 120,
                    labelAlign: 'right'
                }
            ],
            columnsGridSelect: [
                {
                    header: 'Муниципальный район',
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
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],
            onBeforeLoad: function (store, operation) {
                var asp = this,
                    panel = asp.controller.getMainView(),
                    showAll = asp.getForm().down('[name=ShowAll]').getValue();
                if (!showAll) {
                    operation.params.typeJurPerson = panel.down('[name=TypeJurPerson]').getValue();
                    operation.params.contragentId = panel.down('[name=Contragent]').getValue();
                }
                
                operation.params.isPhysicalPerson = panel.down('[name=PersonInspection]').getValue() === 10 ? true : false;
            },
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
                            asp.controller.getStore('baselicenseapplicants.RealityObject').load();
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
            },
            otherActions: function (actions) {
                actions[this.multiSelectWindowSelector + ' [name=ShowAll]'] = {
                    'change': {
                        fn: function () {
                            this.getSelectGrid().getStore().load();
                        }, scope: this
                    }
                };
            }
        }
    ],

    init: function () {
        var me = this;
        me.getStore('baselicenseapplicants.RealityObject').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;

        if (me.params) {
            me.getAspect('baseLicenseAppEditPanelAspect').setData(me.params.inspectionId);

            var mainView = me.getMainComponent();
            if (mainView)
                mainView.setTitle(me.params.title);
        }
        me.getStore('baselicenseapplicants.RealityObject').load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        if (me.params && me.params.inspectionId > 0)
            operation.params.inspectionId = me.params.inspectionId;
    },

    setInspectionId: function (id) {
        var me = this;

        if (me.params) {
            me.params.inspectionId = id;
        }
    }
});