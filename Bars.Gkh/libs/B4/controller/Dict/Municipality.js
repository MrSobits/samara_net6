Ext.define('B4.controller.dict.Municipality', {
    extend: 'B4.base.Controller',

    municipalityId: 0,
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.Municipality',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    models: ['dict.Municipality', 'dict.MunicipalitySourceFinancing'],
    stores: ['dict.Municipality', 'dict.MunicipalitySourceFinancing', 'dict.FiasStreet', 'dict.FiasForSelected'],
    views: [
        'dict.municipality.Grid',
        'dict.municipality.EditWindow',
        'dict.municipality.SourceFinancingGrid',
        'dict.municipality.SourceFinancingEditWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.municipality.Grid',
    mainViewSelector: 'municipalityGrid',

    //Селектор окна котоырй потом используется
    editWindowSelector :'#municipalityEditWindow',
    refs: [
        {
            ref: 'mainView',
            selector: 'municipalityGrid'
        }
    ],

    aspects: [
        {
            xtype: 'municipalitydictperm'
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'municipalityGridWindowAspect',
            gridSelector: 'municipalityGrid',
            editFormSelector: '#municipalityEditWindow',
            storeName: 'dict.Municipality',
            modelName: 'dict.Municipality',
            editWindowView: 'dict.municipality.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record.getId());
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    asp.controller.setCurrentId(record.getId());

                    var fias = record.get('DinamicFias');

                    var fiasCmp = this.getForm().down('#municipalityFiasComboBox');

                    if (fias) {
                        fiasCmp.getStore().insert(0, fias);
                        fiasCmp.setValue(fias);
                    } else {
                        fiasCmp.setValue(null);
                    }
                },
                getdata: function (asp, record) {
                    var fiasId = this.getForm().down('#municipalityFiasComboBox').getValue();

                    if (fiasId) {
                        record.set('FiasId', fiasId);
                    } else {
                        record.set('FiasId', null);
                    }
                }
            },
            
            otherActions: function (actions) {
                actions['municipalityGrid #btnFromFias'] = {
                    click: { fn: this.onOpenLoaderFromFias, scope: this }
                },

                actions['municipalityFiasLoadWindow b4savebutton'] = {
                    click: { fn: this.onLoadFromFias, scope: this }
                },
                
                actions['municipalityFiasLoadWindow b4closebutton'] = {
                    click: { fn: this.onCloseFiasWindow, scope: this }
                }
            },
            
            onCloseFiasWindow: function() {
                var loadWindow = Ext.ComponentQuery.query('municipalityFiasLoadWindow')[0];
                
                loadWindow.close();
            },

            onOpenLoaderFromFias: function () {

                var worktree = Ext.ComponentQuery.query('municipalityFiasLoadWindow')[0];

                if (worktree) {
                    worktree.show();
                } else {
                    worktree = Ext.create('B4.view.dict.municipality.FiasLoader');
                    worktree.show();
                }
            },

            onLoadFromFias: function () {

                var loadWindow = Ext.ComponentQuery.query('municipalityFiasLoadWindow')[0];
                var fiasField = Ext.ComponentQuery.query('municipalityFiasLoadWindow #tfMunicipalityFias')[0];

                var me = this;

                if (fiasField) {
                    var ids = fiasField.getValue();

                    B4.Ajax.request({
                        url: B4.Url.action('AddMoFromFias', 'Municipality'),
                        params: {
                            fiasIds: ids
                        }
                    }).next(function () {
                        me.controller.getStore('dict.Municipality').load();
                        loadWindow.close();
                    }).error(function () {
                        Ext.Msg.alert('Добавление МО', 'Произошла ошибка при добавлении МО из ФИАС');
                    });

                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'municipalitySourceFinancingGridWindowAspect',
            gridSelector: '#municipalitySourceFinancingGrid',
            editFormSelector: '#municipalitySourceFinancingEditWindow',
            storeName: 'dict.MunicipalitySourceFinancing',
            modelName: 'dict.MunicipalitySourceFinancing',
            editWindowView: 'dict.municipality.SourceFinancingEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Municipality', this.controller.municipalityId);
                    }
                }
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'FiasLoadMunMultiselectwindowaspect',
            fieldSelector: 'municipalityFiasLoadWindow #tfMunicipalityFias',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#FiasLoadMunSelectWindow',
            storeSelect: 'dict.FiasStreet',
            storeSelected: 'dict.FiasForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' }
                },
                {
                    header: 'Подчинен', xtype: 'gridcolumn', dataIndex: 'ParentName', flex: 1, filter: { xtype: 'textfield' }
                },
                { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'OKATO', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        }
    ],

    init: function () {
        this.getStore('dict.MunicipalitySourceFinancing').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('municipalityGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.Municipality').load();
    },

    setCurrentId: function (id) {
        this.municipalityId = id;

        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];
        editWindow.down('.tabpanel').setActiveTab(0);

        var sourceStore = this.getStore('dict.MunicipalitySourceFinancing');
        sourceStore.removeAll();

        if (id > 0) {
            editWindow.down('#municipalitySourceFinancingGrid').setDisabled(false);
            sourceStore.load();
        } else {
            editWindow.down('#municipalitySourceFinancingGrid').setDisabled(true);
        }
    },

    onBeforeLoad: function (store, operation) {
        operation.params.municipalityId = this.municipalityId;
    }
});