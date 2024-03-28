Ext.define('B4.controller.manorg.RealityObject', {
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.manorg.RealityObject',
        'B4.Ajax',
        'B4.Url'
    ],

    models: ['manorg.RealityObject'],
    stores: [
        'manorg.RealityObject',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected',
        'realityobj.ExceptDirectManag'
    ],
    views: [
        'SelectWindow.MultiSelectWindow',
        'manorg.RealityObjectGrid'
    ],

    parentCtrlCls: 'B4.controller.manorg.Navigation',

    aspects: [
        {
            xtype: 'manorgrealobjperm'
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'manorgRealobjGridAspect',
            gridSelector: 'realityobjectgrid',
            storeName: 'manorg.RealityObject',
            modelName: 'manorg.RealityObject',
            selectGridSelector: '#realityObjectMultiSelectGrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#realityObjectMultiSelectWindow',
            multiSelectWindowWidth: 950,
            storeSelect: 'realityobj.ExceptDirectManag',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            leftGridConfig: {
                enableColumnHide: true,
                stateful: true,
                itemId: 'realityObjectMultiSelectGrid',
                provideStateId: function () {
                    return this.itemId;
                }
            },
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
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                {
                    header: 'Адрес', 
                    xtype: 'gridcolumn', 
                    dataIndex: 'Address', 
                    flex: 1, 
                    filter: { xtype: 'textfield'}
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ActiveManagingOrganization',
                    flex: 1,
                    hidden: true,
                    text: 'Управляющая организация',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ActiveInn',
                    flex: 1,
                    hidden: true,
                    text: 'ИНН',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ActiveDateStart',
                    flex: 1,
                    format: 'd.m.Y',
                    hidden: true,
                    text: 'Дата начала управления',
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ActiveLicenseDate',
                    flex: 1,
                    format: 'd.m.Y',
                    hidden: true,
                    text: 'Дата включения в реестр лицензий',
                    filter: { xtype: 'datefield' }
                }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddRealityObjects', 'ManagingOrgRealityObject', {
                            objectIds: Ext.encode(recordIds),
                            manorgId: asp.controller.params.manorgId
                        })).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            Ext.Msg.alert('Сохранение!', 'Жилые дома сохранены успешно');
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
        }
    ],

    params: {},
    mainView: 'manorg.RealityObjectGrid',
    mainViewSelector: 'realityobjectgrid',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    init: function () {
        this.getStore('manorg.RealityObject').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.manorgId = this.params.id;
            operation.params.showNotValid = this.getMainView().down('[name=ShowNotValidCheckbox]').getValue();
        }
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjectgrid');
        me.params.id = id;

        me.bindContext(view);
        me.setContextValue(view, 'manorgId', id);
        me.application.deployView(view, 'manorgId_info');
        
        me.params.manorgId = me.params.id;
        this.getStore('manorg.RealityObject').load();
    }
});