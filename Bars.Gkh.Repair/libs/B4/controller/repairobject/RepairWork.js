Ext.define('B4.controller.repairobject.RepairWork', {
    /*
    * Контроллер раздела видов работ
    */
    extend: 'B4.base.Controller',
    params: null,
    requires:
    [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.repairobject.RepairWork'
    ],

    models: ['repairobject.RepairWork'],
    stores: ['repairobject.RepairWork'],
    views: ['repairobject.repairwork.Grid',
            'repairobject.repairwork.EditWindow'],

    mainView: 'repairobject.repairwork.Grid',
    mainViewSelector: 'repairWorkGrid',
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'repairworkperm',
            name: 'repairWorkPerm'
        },
            /*
            * Аспект взаимодействия таблицы и формы редактирования видов работ
            */
        {
            xtype: 'grideditwindowaspect',
            name: 'repairWorkGridWindowAspect',
            gridSelector: 'repairWorkGrid',
            editFormSelector: 'repairworkEditWindow',
            storeName: 'repairobject.RepairWork',
            modelName: 'repairobject.RepairWork',
            editWindowView: 'repairobject.repairwork.EditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.params && !record.data.Id) {
                        record.data.RepairObject = this.controller.params.get('Id');
                    }
                },
                afterSetFormData: function (asp, record) {
                    var sfWork = asp.getForm().down('#sfWork');

                    if (sfWork) {
                        sfWork.setDisabled(record.getId() != 0);
                    }
                }
            }
        }
    ],

    init: function () {
        this.getStore('repairobject.RepairWork').on('beforeload', this.onBeforeLoad, this);

        this.control({
            '#sfWork': {
                change: function (field, newItem, oldItem) {
                    var unitMeasureField = Ext.ComponentQuery.query('#unitMeasure')[0];
                    var typeWorkField = Ext.ComponentQuery.query('#typeWork')[0];
                    if (newItem) {
                        if (unitMeasureField) {
                            unitMeasureField.setValue(newItem.UnitMeasure);
                        }
                        if (typeWorkField) {
                            typeWorkField.setValue(newItem.TypeWork);
                        }
                    }
                }
            }
        });

        this.callParent(arguments);
    },
    onLaunch: function () {
        this.getStore('repairobject.RepairWork').load();
        if (this.params) {
            this.getAspect('repairWorkPerm').setPermissionsByRecord(this.params);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.repairObjectId = this.params.get('Id');
        }
    }
});