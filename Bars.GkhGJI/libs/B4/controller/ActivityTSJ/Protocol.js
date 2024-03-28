Ext.define('B4.controller.activitytsj.Protocol', {
    extend: 'B4.base.Controller',
    
    params: null,

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.Ajax', 'B4.Url'
    ],

    models: ['activitytsj.Protocol'],
    stores: ['activitytsj.ProtocolRealObj', 'activitytsj.Protocol'],
    mainView: 'activitytsj.ProtocolGrid',
    mainViewSelector: '#activityTsjProtocolGrid',
    views: [
       'activitytsj.ProtocolGrid',
       'activitytsj.ProtocolEditWindow',
       'activitytsj.ProtocolRealObjGrid',
       'SelectWindow.MultiSelectWindow'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
    
    activityTsjProtocolEditWindowSelector: '#activityTsjProtocolEditWindow',
    
    aspects: [
         {
             xtype: 'grideditwindowaspect',
             name: 'activityTsjProtocolGridEditWindow',
             storeName: 'activitytsj.Protocol',
             modelName: 'activitytsj.Protocol',
             gridSelector: '#activityTsjProtocolGrid',
             editFormSelector: '#activityTsjProtocolEditWindow',
             editWindowView: 'activitytsj.ProtocolEditWindow',
             onSaveSuccess: function (asp, record) {
                 asp.controller.setCurrentId(record.get('Id'));
             },
             listeners: {
                 getdata: function (asp, record) {
                     if (this.controller.params && !record.get('Id')) {
                         record.set('ActivityTsj', this.controller.params.get('Id'));
                     }
                 },
                 aftersetformdata: function (asp, record, form) {
                     asp.controller.setCurrentId(record.get('Id'));
                 }
             }
         },
        {

            //Аспект взаимодействия таблицы списка домов протокола ТСЖ с массовой формой выбора жилых домов
            //При добавлении открывается форма массового выбора домов. После выбора список получается через подписку 
            //на событие getdata идет добавление записей в сторе

            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'activityTsjProtocolRealObjAspect',
            gridSelector: '#activityTsjProtocolRealObjGrid',
            storeName: 'activitytsj.ProtocolRealObj',
            modelName: 'activitytsj.ProtocolRealObj',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#activityTsjProtocolRealObjMultiSelectWindow',
            storeSelect: 'activitytsj.RealityObjectForSelect',
            storeSelected: 'activitytsj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } },
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
                }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false },
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, sortable: false }
            ],

            onBeforeLoad: function (store, operation, type) {
                operation.params.realityObjIds = this.controller.realityObjIds;
            },

            listeners: {
                //В данном методе принимаем массив записей из формы выбора и вставляем их в сторе грида списка домов
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddRealityObjects', 'ActivityTsjProtocolRealObj', {
                            objectIds: recordIds,
                            protocol: asp.controller.protocolId
                        })).next(function (response) {
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
        }
    ],

    init: function () {
        this.getStore('activitytsj.Protocol').on('beforeload', this.onBeforeLoad, this);
        this.getStore('activitytsj.ProtocolRealObj').on('beforeload', this.onBeforeLoadProtocol, this);
        
        this.callParent(arguments);
    },
    
    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.activityTSJ = this.params.get('Id');
        }
    },

    onBeforeLoadProtocol: function (store, operation) {
        if (!Ext.isEmpty(this.protocolId)) {
            operation.params.protocol = this.protocolId;
        }
    },

    onLaunch: function () {
        this.getStore('activitytsj.Protocol').load();
    },

    setCurrentId: function (id) {
        this.protocolId = id;

        var editWindow = Ext.ComponentQuery.query(this.activityTsjProtocolEditWindowSelector)[0];
        editWindow.down('.tabpanel').setActiveTab(0);

        var storeProtocolRealObj = this.getStore('activitytsj.ProtocolRealObj');
        storeProtocolRealObj.removeAll();

        if (id > 0) {
            editWindow.down('#activityTsjProtocolRealObjGrid').setDisabled(false);
            storeProtocolRealObj.load();
        } else {
            editWindow.down('#activityTsjProtocolRealObjGrid').setDisabled(true);
        }
    }
});