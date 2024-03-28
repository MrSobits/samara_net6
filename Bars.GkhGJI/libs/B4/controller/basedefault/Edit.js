Ext.define('B4.controller.basedefault.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiInspection',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.Ajax', 'B4.Url'
    ],

    models: ['BaseDefault'],

    stores: [
        'basedefault.RealityObject',
        'realityobj.ByTypeOrg',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected'
    ],

    views: ['SelectWindow.MultiSelectWindow', 'basedefault.EditPanel'],

    mainView: 'basedefault.EditPanel',
    mainViewSelector: '#baseDefaultEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },
    
    baseDefaultEditPanelSelector: '#baseDefaultEditPanel',

    aspects: [
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'baseDefaultStateButtonAspect',
            stateButtonSelector: '#baseDefaultEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('baseDefaultEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            /*
            * Аспект основной панели проверки без основания
            */
            xtype: 'gjiinspectionaspect',
            name: 'baseDefaultEditPanelAspect',
            editPanelSelector: '#baseDefaultEditPanel',
            modelName: 'BaseDefault',
            listeners: {
                aftersetpaneldata: function (asp, rec, panel) {
                    this.controller.getAspect('baseDefaultStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                }
            }
        },
        {
            /*
            * Аспект взаимодействия кнопки таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'baseDefaultRealityObjectGjiAspect',
            gridSelector: '#baseDefaultRealityObjectGrid',
            storeName: 'basedefault.RealityObject',
            modelName: 'RealityObjectGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseDefaultRealityObjectMultiSelectWindow',
            storeSelect: 'realityobj.ByTypeOrg',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальный район', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
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
                var editPanel = Ext.ComponentQuery.query(this.controller.baseDefaultEditPanelSelector)[0];
                operation.params.contragentId = editPanel.down('#sfContragent').getValue();
                operation.params.typeJurPerson = 10;
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
                        }).next(function() {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function() {
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
        }
    ],

    init: function () {
        this.getStore('basedefault.RealityObject').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('baseDefaultEditPanelAspect').setData(this.params.inspectionId);

            var mainView = this.getMainComponent();
            if (mainView)
                mainView.setTitle(this.params.title);

            //Обновляем список домов в проверке
            this.getStore('basedefault.RealityObject').load();
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.inspectionId = this.params.inspectionId;
        }
    }
});