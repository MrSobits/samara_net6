Ext.define('B4.controller.ConstructionObject', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateContextMenu'
    ],

    views: [
        'constructionobject.Grid',
        'constructionobject.AddWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    models: ['ConstructionObject'],

    stores: ['ConstructionObject'],
    
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'constructionobject.Grid',
    mainViewSelector: 'constructionobjectgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'constructionobjectgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhgrideditformaspect',
            name: 'constructobjGridWindowAspect',
            gridSelector: 'constructionobjectgrid',
            editFormSelector: 'constructionobjectaddwindow',
            storeName: 'ConstructionObject',
            modelName: 'ConstructionObject',
            editWindowView: 'constructionobject.AddWindow',
            controllerEditName: 'B4.controller.constructionobject.Navi',
            editRecord: function (record) {
                var me = this,
                    id = record ? record.get('Id') : null,
                    model;

                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('constructionobjectedit/{0}/', id));
                    } else {
                        model.load(id, {
                            success: function (rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            }
        },
        {
            /*
             * аспект взаимодействия триггер-поля фильтрации мун. районов и таблицы объектов строительства
             */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'constructObjectfieldmultiselectwindowaspect',
            fieldSelector: '#constructionObjFilterPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#municipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
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
                { header: 'Федеральный номер', xtype: 'gridcolumn', dataIndex: 'FederalNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'Okato', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранные записи',
            listeners: {
                getdata: function (asp, records) {
                    if (records.length > 0) {
                        var filterPanel = asp.controller.getFilterPanel(),
                            municipality = filterPanel.down('#tfMunicipality');

                        municipality.setValue(asp.parseRecord(records, asp.valueProperty));
                        municipality.updateDisplayedText(asp.parseRecord(records, asp.textProperty));
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать одно или несколько муниципальных образования');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'constructojectresettlementprogrammultiselectwindowaspect',
            fieldSelector: '#constructionObjFilterPanel #tfResettlementProgram',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#resettlementProgramSelectWindow',
            storeSelect: 'dict.ResettlementProgram',
            storeSelected: 'dict.ResettlementProgramForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранные записи',
            listeners: {
                getdata: function (asp, records) {
                    if (records.length > 0) {
                        var filterPanel = asp.controller.getFilterPanel(),
                            program = filterPanel.down('#tfResettlementProgram');

                        program.setValue(asp.parseRecord(records, asp.valueProperty));
                        program.updateDisplayedText(asp.parseRecord(records, asp.textProperty));
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать одну или несколько программ переселения');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'constructObjectStateTransferAspect',
            gridSelector: 'constructionobjectgrid',
            menuSelector: 'constructObjGridStateMenu',
            stateType: 'gkh_construct_obj'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'constructionobjecsmrStateTransferAspect',
            gridSelector: 'constructionobjectgrid',
            menuSelector: 'constructionobjecGridMonitoringSmrMenu',
            stateType: 'gkh_construct_obj_smr',
            stateProperty: 'MonitoringSmrState',
            entityIdProperty: 'MonitoringSmrId',
            cellClickAction: function(grid, e, action, record) {
                switch (action.toLowerCase()) {
                    case 'statechangesmr':
                        e.stopEvent();
                        var menu = this.getContextMenu();
                        this.currentRecord = record;
                        menu.updateData(record, e.xy, this.stateType, this.stateProperty);
                        break;
                }
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('constructionobjectgrid');

        view.getStore().load();

        this.bindContext(view);
        this.application.deployView(view);
    },

    init: function() {
        var me = this;

        me.getStore('ConstructionObject').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    getFilterPanel: function() {
        var panel = Ext.ComponentQuery.query('#constructionObjFilterPanel')[0];
        return panel;
    },

    onBeforeLoad: function (store, operation) {
        var filterPanel = this.getFilterPanel();
        if (filterPanel) {
            operation.params.municipalityId = filterPanel.down('#tfMunicipality').getValue();
            operation.params.resettlementProgramId = filterPanel.down('#tfResettlementProgram').getValue();
        }
    }
});