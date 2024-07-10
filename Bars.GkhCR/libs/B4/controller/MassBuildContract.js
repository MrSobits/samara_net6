Ext.define('B4.controller.MassBuildContract', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu',
        'B4.aspects.StateContextButton',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.store.dict.Work',
        'B4.enums.TypeWork',
        'B4.aspects.permission.objectcr.BuildContract',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    programCrId: null,
    massBCObjectCr: null,
    buildContractId: null,


    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    stores: [
        'objectcr.MassBuildContract',
        'objectcr.MassBuildContractWork',
        'objectcr.MassBuildContractObjectCr',
        'objectcr.MassBuildContractObjectCrWork',
        'objectcr.BuildContractForMassBuild'
    ],

    models: [
        'objectcr.MassBuildContract',
        'objectcr.MassBuildContractWork',
        'objectcr.MassBuildContractObjectCr',
        'objectcr.MassBuildContractObjectCrWork',
        'objectcr.BuildContractForMassBuild'
    ],

    views: [
        'objectcr.MassBuildContractEditWindow',
        'objectcr.MassBuildContractGrid',
        'objectcr.MassBuildContractObjectCrGrid',
        'objectcr.MassBuildContractObjectCrEditWindow',
        'objectcr.MassBuildContractObjectCrWorkGrid',
        'objectcr.BuildContractForMassBuildGrid'
    ],

    mainView: 'objectcr.MassBuildContractGrid',
    mainViewSelector: 'massbuildcontractgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'massbuildcontractgrid'
        },
        {
            ref: 'objectCRGridGrid',
            selector: 'massbuildcontractcrobjectgrid'
        },
        {
            ref: 'buildContractForMassBuild',
            selector: 'buildcontractformassbuildgrid'
        }
    ],

    aspects: [
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'massbuildcontractStateTransferAspect',
            gridSelector: 'massbuildcontractgrid',
            stateType: 'cr_mass_build_contract',
            menuSelector: 'massbuildcontractstatecontextmenu'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'buildContractForMassBuildStateTransferAspect',
            gridSelector: 'buildcontractformassbuildgrid',
            stateType: 'cr_obj_build_contract',
            menuSelector: 'buildcontractformassbuildstatecontextmenu'
        },
        {
            xtype: 'statecontextbuttonaspect',
            name: 'buildContractForMassBuildStateButtonAspect',
            stateButtonSelector: 'buildcontractformassbuildgrid #btnState',
            listeners: {
                transfersuccess: function (me, entityId, newState) {
                    var grid = me.controller.getGrid('objectcr.BuildContractForMassBuildGrid');
                        model = me.controller.getModel('objectcr.BuildContractForMassBuild');
                    //Если статус изменен успешно, то проставляем новый статус
                    me.setStateData(entityId, newState);

                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    me.updateGrid('objectcr.BuildContractForMassBuildGrid');
                }
            }
        },
        {
             xtype: 'grideditwindowaspect',
             name: 'massbuildcontractGridWindowAspect',
             gridSelector: 'massbuildcontractgrid',
             editFormSelector: 'massbuildcontracteditwindow',
             modelName: 'objectcr.MassBuildContract',
             storeName: 'objectcr.MassBuildContract',
             editWindowView: 'objectcr.MassBuildContractEditWindow',
             //otherActions: function (actions) {
 
             //    actions['overhaulproposeeditwindow #sfProgramCr'] = { 'change': { fn: this.onSelectProgram, scope: this } };
             //    actions['overhaulproposeeditwindow #sfObjectCr'] = { 'beforeload': { fn: this.onBeforeLoadObjectCr, scope: this } };
             //},  
             onSaveSuccess: function () {
                 // перекрыл метод потому что ненужно чтобы форма закрывалась                 

             },
             onSelectProgram: function (field, newValue) {
                 var form = this.getForm();
                 var thisrecord = form.getRecord();
                 if (thisrecord.data.Id == null || thisrecord.data.Id == "")
                {
                    if (newValue != null && newValue != "") {
                        selectedProgram = newValue.Id;
                        form.down('#sfObjectCr').setValue(null);
                        form.down('#sfObjectCr').setDisabled(false);
                    }
                    else {
                        form.down('#sfObjectCr').setDisabled(true);
                    }
                }
                else
                 {  
                     objectCrId = thisrecord.data.ObjectCr.Id;
                     form.down('#sfProgramCr').setDisabled(true);
                     form.down('#sfObjectCr').setDisabled(true);
                }
               
             },         

             onBeforeLoadObjectCr: function (store, operation) {
                 operation = operation || {};
                 operation.params = operation.params || {};

                 operation.params.programId = selectedProgram;
             },

             onBeforeLoadContracts: function (store, operation) {
                 operation = operation || {};
                 operation.params = operation.params || {};

                 operation.params.buildContractId = buildContractId;
             },

             listeners: {
                 aftersetformdata: function (asp, rec, form) {
                     var me = this;
                   
                     buildContractId = rec.getId();
                     if (rec.getId()) {
                         var grid = form.down('massbuildcontractworkgrid'),
                             store = grid.getStore();
                         var gridOcr = form.down('massbuildcontractcrobjectgrid'),
                             storeOcr = gridOcr.getStore();
                         var gridContr = form.down('buildcontractformassbuildgrid'),
                             storeContr = gridContr.getStore();

                         storeContr.on('beforeload', this.onBeforeLoadContracts);

                         grid.setDisabled(false);
                         gridOcr.setDisabled(false);
                         gridContr.setDisabled(false);
                         store.filter('buildContractId', rec.getId());
                         storeOcr.filter('buildContractId', rec.getId());
                         storeContr.filter('buildContractId', rec.getId());
                         programCrId = rec.get('ProgramCr');
                     }
                     else
                     {
                         var grid = form.down('massbuildcontractworkgrid');
                         grid.setDisabled(true);                        
                     }
                 }
             }
        },
        {
             /* 
             Аспект взаимодействия таблицы предоставляемых документов с массовой формой выбора предоставляемых документов
             По нажатию на Добавить открывается форма выбора предоставляемых документов.
             По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
             И сохранение предоставляемых документов
             */
             xtype: 'gkhinlinegridmultiselectwindowaspect',
             name: 'massbuildcontractworkGridAspect',
             gridSelector: 'massbuildcontractworkgrid',
             storeName: 'objectcr.MassBuildContractWork',
             modelName: 'objectcr.MassBuildContractWork',
             multiSelectWindow: 'SelectWindow.MultiSelectWindow',
             multiSelectWindowSelector: '#MassBuildContractWorkMultiSelectWindow',
             storeSelect: 'objectcr.DistinctWorksByProgramIdForSelect',
             storeSelected: 'objectcr.DistinctWorksByProgramIdForSelected',
             titleSelectWindow: 'Выбор работ',
             titleGridSelect: 'Работы',
             titleGridSelected: 'Выбранные работы',
             columnsGridSelect: [
                 { header: 'Тип', xtype: 'gridcolumn', dataIndex: 'TypeWork', flex: 1, renderer: function (val) { return B4.enums.TypeWork.displayRenderer(val); } },
                 { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 2 },
                 { header: 'Единица измерения', xtype: 'gridcolumn', dataIndex: 'UnitMeasure', flex: 1 },
             ],
             columnsGridSelected: [
                 { header: 'Тип', xtype: 'gridcolumn', dataIndex: 'TypeWork', flex: 1, renderer: function (val) { return B4.enums.TypeWork.displayRenderer(val); } },
                 { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
             ],
             onBeforeLoad: function (store, operation) {              
                 operation.params.programCrId = programCrId;
             },
           
             listeners: {

                 getdata: function (asp, records) {
                     var recordIds = [];
                     records.each(function (rec, index) {
                         recordIds.push(rec.get('Id'));
                     });
                     if (recordIds[0] > 0) {
                         asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                         B4.Ajax.request(B4.Url.action('AddMassBuilderContractWork', 'ObjectCr', {
                             objectCrWorkIds: recordIds,
                             parentId: buildContractId
                         })).next(function (response) {
                             asp.controller.unmask();
                             asp.controller.getStore(asp.storeName).load();
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
            /* 
            Аспект взаимодействия таблицы предоставляемых документов с массовой формой выбора предоставляемых документов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'massbuildcontractcrobjectGridAspect',
            gridSelector: 'massbuildcontractcrobjectgrid',
            modelName: 'objectcr.MassBuildContractObjectCr',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#MassBuildContractObjectCrMultiSelectWindow',
            storeSelect: 'objectcr.ObjectCRForMassBuilderForSelect',
            storeSelected: 'objectcr.ObjectCRForMassBuilderForSelected',
            titleSelectWindow: 'Выбор объектов',
            titleGridSelect: 'Объекты КР',
            titleGridSelected: 'Выбранные объекты',
            editFormSelector: '#massBuildContractObjectCrEditWindow',
            editWindowView: 'objectcr.MassBuildContractObjectCrEditWindow',
            columnsGridSelect: [
                { header: 'МО', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' }},
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 2, filter: { xtype: 'textfield' } },
            ],
            columnsGridSelected: [
                { header: 'МО', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1 },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 2 },
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.buildContractId = buildContractId;
            },
            saveRecord: function (rec) {
                var me = this,
                    frm = me.getEditForm().getForm();

                rec.set('Sum', me.getEditForm().down('gkhdecimalfield[name=Sum]').getValue());

                frm.submit({
                    url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
                    params: {
                        records: Ext.encode([rec.getData()])
                    },
                    success: function () {
                        me.updateGrid();
                        me.getEditForm().close();
                    },
                    failure: function (form, action) {
                        me.fireEvent('savefailure', rec, action.result.message);
                        Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                    }
                });
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    var ocrGrid = asp.controller.getObjectCRGridGrid();
                    
                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddMassBuilderContractObjectCr', 'ObjectCr', {
                            objectCrIds: recordIds,
                            parentId: buildContractId
                        })).next(function (response) {
                            asp.controller.unmask();
                            ocrGrid.getStore().load();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать работы');
                        return false;
                    }
                    return true;
                },
                aftersetformdata: function (asp, record) {
                    var me = this,
                        grid = me.getGrid();
                    massBCObjectCr = record.getId();
                    var form = asp.getEditForm(),
                        archGrid = form.down('massbuildcontractobjectcrworkgrid'),
                        archStore = archGrid.getStore();

                    archStore.on('beforeload', function (store, operation) {
                        operation.params.buildContractCRId = massBCObjectCr;
                    },
                        me);
                    archStore.load();                    

                }
            },
            getEditForm: function () {
                var me = this,
                    editWindow;

                if (me.editFormSelector) {

                    if (me.componentQuery) {
                        editWindow = me.componentQuery(me.editFormSelector);
                    }

                    if (!editWindow) {
                        editWindow = Ext.ComponentQuery.query(me.editFormSelector)[0];
                    }

                    if (!editWindow) {

                        editWindow = me.controller.getView(me.editWindowView).create(
                        {
                            constrain: true,
                            autoDestroy: true,
                            closeAction: 'destroy',
                            renderTo: B4.getBody().getActiveTab().getEl(),
                            ctxKey: me.controller.getCurrentContextKey ? me.controller.getCurrentContextKey() : ''
                        });

                        editWindow.show();
                    }

                    return editWindow;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы предоставляемых документов с массовой формой выбора предоставляемых документов
            По нажатию на Добавить открывается форма выбора предоставляемых документов.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение предоставляемых документов
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'massbuildcontractcrworkInlineGridAspect',
            gridSelector: 'massbuildcontractobjectcrworkgrid',
            storeName: 'objectcr.MassBuildContractObjectCrWork',
            modelName: 'objectcr.MassBuildContractObjectCrWork',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#MassBuildContractCRWorkMultiSelectWindow',
            storeSelect: 'objectcr.DistinctWorksByProgramIdForSelect',
            storeSelected: 'objectcr.DistinctWorksByProgramIdForSelected',
            titleSelectWindow: 'Выбор работ',
            titleGridSelect: 'Работы',
            titleGridSelected: 'Выбранные работы',
            columnsGridSelect: [
                { header: 'Тип', xtype: 'gridcolumn', dataIndex: 'TypeWork', flex: 1, renderer: function (val) { return B4.enums.TypeWork.displayRenderer(val); } },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 2 },
                { header: 'Единица измерения', xtype: 'gridcolumn', dataIndex: 'UnitMeasure', flex: 1 },
            ],
            columnsGridSelected: [
                { header: 'Тип', xtype: 'gridcolumn', dataIndex: 'TypeWork', flex: 1, renderer: function (val) { return B4.enums.TypeWork.displayRenderer(val); } },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.programCrId = programCrId;
            },

            listeners: {

                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddMassBuilderContractCRWork', 'ObjectCr', {
                            objectCrWorkIds: recordIds,
                            parentId: massBCObjectCr
                        })).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
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
        }
         
         
    ],


    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('massbuildcontractgrid');
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('objectcr.MassBuildContract').load();
    },

    init: function () {
        var me = this;
        this.getStore('objectcr.MassBuildContractWork').on('beforeload', this.onBeforeLoadSub, this);
        this.getStore('objectcr.MassBuildContractObjectCr').on('beforeload', this.onBeforeLoadSub, this);
        //this.getStore('objectcr.BuildContractForMassBuild').on('beforeload', this.onBeforeLoadSub, this);
   //     this.getStore('OverhaulProposal').on('beforeload', this.onBeforeLoad, this);
      //  this.getStore('OverhaulProposal').load();
        me.callParent(arguments);
    },

    onBeforeLoadSub: function (store, operation) {
        operation.params.buildContractId = buildContractId;
    },

    onBeforeLoad: function (store, operation) {
        var filterPanel = this.getOverhaulProposeFilterPanel();
        if (filterPanel) {
            operation.params.programId = filterPanel.down('#tfProgramCr').getValue();
            operation.params.municipalityId = filterPanel.down('#tfMunicipality').getValue();
        }
    }
});