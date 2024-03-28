Ext.define('B4.controller.DpkrDocument', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.DpkrDocument',
        'B4.aspects.StateContextMenu',
        'B4.form.ComboBox',
        'B4.ux.grid.filter.YesNo'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'DpkrDocument',
        'dpkrdocument.ProgramVersion'
    ],

    stores: [
        'DpkrDocument',
        'dpkrdocument.ProgramVersion',
        'dpkrdocument.ProgramVersionSelect',
        'dpkrdocument.ProgramVersionSelected'
    ],

    views: [
        'dpkrdocument.Grid',
        'dpkrdocument.VersionGrid',
        'dpkrdocument.EditWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'dpkrdocument.Grid',
    mainViewSelector: 'dpkrdocumentgrid',
    
    aspects: [
        {
            xtype: 'dpkrdocumentperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'dpkrDocumentGridWindowAspect',
            gridSelector: 'dpkrdocumentgrid',
            editFormSelector: 'dpkrdocumenteditwindow',
            modelName: 'DpkrDocument',
            editWindowView: 'dpkrdocument.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.dpkrDocumentId = record.getId();
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var versionGrid = form.down('dpkrdocumentversiongrid'),
                        includedRoGrid = form.down('#includedRealityObjectGrid'),
                        excludedRoGrid = form.down('#excludedRealityObjectGrid'),
                        gridArray = [versionGrid, includedRoGrid, excludedRoGrid],
                        docExists = record.getId() !== 0;

                    asp.controller.dpkrDocumentId = record.getId();

                    gridArray.forEach(function(grid){
                        grid.setDisabled(!docExists);
                    });

                    if (docExists) {
                        gridArray.forEach(function(grid){
                            grid.getStore().load();
                        });
                    }
                }
            }
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'dpkrDocumentStateTransferAspect',
            gridSelector: 'dpkrdocumentgrid',
            menuSelector: 'dpkrdocumentgridStateMenu',
            stateType: 'ovrhl_dpkr_documents'
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'dpkrDocumentProgramVersionAspect',
            gridSelector: 'dpkrdocumentversiongrid',
            storeName: 'dpkrdocument.ProgramVersion',
            modelName: 'dpkrdocument.ProgramVersion',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#dpkrDocumentProgramVersionAspectMultiSelectWindow',
            storeSelect: 'dpkrdocument.ProgramVersionSelect',
            storeSelected: 'dpkrdocument.ProgramVersionSelected',
            titleSelectWindow: 'Версии программы',
            rightGridConfig: {
                hidden: true
            },
            saveButtonText: 'Выбрать',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    dataIndex: 'Municipality',
                    flex: 0.5,
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
                { header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Дата', xtype: 'datecolumn', dataIndex: 'VersionDate', flex: 0.25, format: 'd.m.Y', filter: { xtype: 'datefield', operand: CondExpr.operands.eq, format: 'd.m.Y' } },
                { header: 'Основная', dataIndex: 'IsMain', flex: 0.25, filter: { xtype: 'b4dgridfilteryesno' }, renderer: function (v) { return v ? 'Да' : 'Нет'; } }
            ],
            listeners: {
                getdata: function (asp, records) {
                    if (records.items.length === 0)
                        return true;

                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.getForm());
                    B4.Ajax.request(B4.Url.action('AddProgramVersions', 'DpkrDocumentProgramVersion', {
                        ids: Ext.encode(recordIds),
                        dpkrDocumentId: asp.controller.dpkrDocumentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранено!', 'Версии ДПКР сохранены успешно');
                        asp.getGrid().getStore().load();
                        return true;
                    }).error(function (e) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка!', e.message || 'При сохранении версий ДПКР произошла ошибка!');
                    });

                    return true;
                }
            },
            onBeforeLoad: function (store, operation) {
                this.controller.onBeforeLoad(store, operation);
            }
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'dpkrdocumentversiongrid': {
                'afterrender': function (grid) {
                    grid.getStore().on('beforeload', me.onBeforeLoad, me);
                }
            },
            'dpkrdocumentversiongrid #btnAddRealityObjects': {
                'click': { fn: this.onAddRealityObjectButtonClick, scope: this }
            },
            'dpkrdocumentrealityobjectcontainer': {
                'afterrender': function (container) {
                    var includedRoGrid = container.down('#includedRealityObjectGrid'),
                        excludedRoGrid = container.down('#excludedRealityObjectGrid'),
                        gridArray = [includedRoGrid, excludedRoGrid];

                    gridArray.forEach(function (grid) {
                        grid.getStore().on('beforeload', me.onRealityObjectStoreBeforeLoad, me);
                    });
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.dpkrDocumentId = this.dpkrDocumentId;
    },

    onRealityObjectStoreBeforeLoad: function (store, operation) {
        var me = this;

        me.onBeforeLoad(store, operation);
        operation.params.isExcluded = store.isExcluded;
    },

    onAddRealityObjectButtonClick: function (btn) {
        var me = this,
            form = btn.up('dpkrdocumenteditwindow');

        me.mask('Формирование перечня домов...', form);
        B4.Ajax.request({
            url: B4.Url.action('AddRealityObjects', 'DpkrDocumentRealityObject'),
            params: {
                dpkrDocumentId: me.dpkrDocumentId
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            Ext.Msg.alert('Успешно!', 'Перечень включенных и исключенных домов сформирован успешно');

            var includedRoGrid = form.down('#includedRealityObjectGrid'),
                excludedRoGrid = form.down('#excludedRealityObjectGrid'),
                gridArray = [includedRoGrid, excludedRoGrid];

            gridArray.forEach(function (grid) {
                grid.getStore().load();
            });
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', e.message || 'При формировании перечня произошла ошибка!');
        });
    }
});