Ext.define('B4.controller.dict.RepairProgram', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.RepairProgram',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],
    
    models: ['dict.RepairProgram'],
    stores: [
        'dict.RepairProgram',
        'dict.RepairProgramMunicipality',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],
    views: [
        'dict.repairprogram.Grid',
        'dict.repairprogram.EditWindow',
        'SelectWindow.MultiSelectWindow'
    ],
   
    mainView: 'dict.repairprogram.Grid',
    mainViewSelector: 'repairProgramGrid',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    editWindowSelector: '#repairProgramEditWindow',
    
    refs: [
        {
            ref: 'mainView',
            selector: 'repairProgramGrid'
        }
    ],
    
    aspects: [
        {
            xtype: 'repairprogramperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'repairProgramGridWindowAspect',
            gridSelector: 'repairProgramGrid',
            editFormSelector: '#repairProgramEditWindow',
            storeName: 'dict.RepairProgram',
            modelName: 'dict.RepairProgram',
            editWindowView: 'dict.repairprogram.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record.getId());
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    asp.controller.setCurrentId(record.getId());
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'repProgramMunicipalityGridAspect',
            gridSelector: '#repProgramMunicipalityGrid',
            storeName: 'dict.RepairProgramMunicipality',
            modelName: 'dict.RepairProgramMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#repProgramMunicipalityGridMultiSelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            titleSelectWindow: 'Выбор муниципального образования',
            titleGridSelect: 'Муниципальные образования',
            titleGridSelected: 'Выбранные муниципальные образования',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddMunicipality', 'RepairProgramMunicipality', {
                            muIds: recordIds,
                            repairProgramId: asp.controller.repairProgramId
                        })).next(function () {
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.unmask();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать муниципальные образования');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],
    
    init: function () {
        this.getStore('dict.RepairProgramMunicipality').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('repairProgramGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.RepairProgram').load();
    },
    
    onBeforeLoad: function (store, operation) {
        operation.params.repairProgramId = this.repairProgramId;
    },
    
    setCurrentId: function (id) {
        this.repairProgramId = id;

        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];

        var storeRepairProgramMunicipality = this.getStore('dict.RepairProgramMunicipality');
        storeRepairProgramMunicipality.removeAll();

        if (id > 0) {
            editWindow.down('#repProgramMunicipalityGrid').setDisabled(false);
            storeRepairProgramMunicipality.load();
        } else {
            editWindow.down('#repProgramMunicipalityGrid').setDisabled(true);
        }
    }
});