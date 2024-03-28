Ext.define('B4.controller.activitytsj.Inspection', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: [
        'BaseActivityTsj'
    ],

    store: [
        'activitytsj.RealityObjectForSelect',
        'activitytsj.RealityObjectForSelected',
        'activitytsj.RealObj',
        'BaseActivityTsj'
    ],

    views: [
        'activitytsj.InspectionGrid',
        'SelectWindow.MultiSelectWindow'
    ],
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },
    
    mainView: 'activitytsj.InspectionGrid',
    mainViewSelector: '#activityTsjInspectionGrid',
    
    aspects: [
         {
            //Аспект взаимодействия таблицы документы деятельности ТСЖ с массовой формой выбора жилых домов
            //При добавлении открывается форма массового выбора домов. После выбора список получается через подписку 
            //на событие getdata вызывается открытие карточки обследований в которую передаются выбранные до этого дома.

            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'activityTsjDocRealObjAspect',
            gridSelector: '#activityTsjInspectionGrid',
            storeName: 'BaseActivityTsj',
            modelName: 'BaseActivityTsj',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#activityTsjDocRealObjMultiSelectWindow',
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

            //Переопределены методы. Убрал проверку на наличие editFormSelector (т.к. на него есть подписки (сохранить, закрыть) в аспекте которые в данном случае не нужны)
            rowAction: function (grid, action, record) {
                switch (action.toLowerCase()) {
                    case 'edit':
                        this.editRecord(record);
                        break;
                }
            },
            rowDblClick: function (view, record) {
                this.editRecord(record);
            },

            listeners: {
                //В данном методе принимаем массив записей из формы выбора, формируем запись модели и передаем ее в метод editRecord
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    var model = this.controller.getModel(this.modelName);
                    var recordActivityTsj = new model({ Id: 0, RealityObjects: recordIds});
                    
                    if (recordIds[0] > 0) {
                        this.editRecord(recordActivityTsj);
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            },

            //Если запись существует и открыта на редактирование то вызываем метод показа формы. Иначе сохраняем предварительно новое основание проверки
            editRecord: function (record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model;

                if (id > 0) {
                    me.showInspectionForm(record);
                } else {
                    //Тут мы сами создаем модель проверки и открываем форму редактирования
                    model = this.controller.getModel(this.modelName);
                    var newRec = new model({
                        Id: 0,
                        ActivityTsj: this.controller.params.get('Id'),
                        TypeBase: 70,
                        RealityObjects: record.get('RealityObjects')
                    });

                    newRec.save({ id: newRec.getId() })
                        .next(function (result) {
                            this.controller.getStore(this.storeName).load();
                            this.showInspectionForm(result.record);
                        }, this)
                        .error(function (result) {
                            Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                        }, this);
                }
            },

            showInspectionForm: function (record) {
                var me = this,
                    inspectionId = record.get('Id'),
                    portal = me.controller.getController('PortalController'),
                    controllerEditName = 'B4.controller.baseactivitytsj.Navigation',
                    model = me.controller.getModel('InspectionGji'),
                    inspection = new model({ Id: inspectionId });

                portal.loadController(controllerEditName, inspection, portal.containerSelector);
            }
        }
    ],

    init: function () {
        this.getStore('BaseActivityTsj').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('BaseActivityTsj').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.activityTSJ = this.params.get('Id');
        }
    }
});