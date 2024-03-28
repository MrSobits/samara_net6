Ext.define('B4.controller.dict.Municipality', {
    extend: 'B4.base.Controller',
    municipalityId: 0,
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.dict.Municipality',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    models: ['dict.Municipality'],
    
    stores: [
        'dict.Municipality',
        'dict.FiasStreet',
        'dict.FiasForSelected'
    ],
    
    views: [
        'dict.municipality.Grid',
        'dict.municipality.EditWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'dict.municipality.Grid',
    mainViewSelector: 'municipalityGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'EditWindow',
            selector: '#municipalityEditWindow'
        },
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
            listeners: {
                aftersetformdata: function (asp, record) {
                    var fias = record.get('DinamicFias'),
                        region = {
                            RegionGuid: '',
                            RegionName: record.get('RegionName') ? record.get('RegionName') : ''
                        };

                    var fiasCmp = this.getForm().down('#municipalityFiasComboBox');
                    var regionCmp = this.getForm().down('#regionFiasComboBox');

                    if (region.RegionName != '') {
                        regionCmp.disable();
                    }
                    
                    asp.fillCmp(fiasCmp, fias);
                    asp.fillCmp(regionCmp, region);                    
                },
                getdata: function(asp, record) {
                    var fiasId = this.getForm().down('#municipalityFiasComboBox').getValue();
                    var regionName = this.getForm().down('#regionFiasComboBox').getRawValue();

                    asp.setField(record, 'FiasId', fiasId);
                    asp.setField(record, 'RegionName', regionName);
                }
            },
            gridAction: function (grid, action) {
                var me = this;
                
                if (this.fireEvent('beforegridaction', this, grid, action) !== false) {
                    switch (action.toLowerCase()) {
                        case 'add':
                            {
                                B4.Ajax.request({
                                    url: B4.Url.action('GetRegion', 'Municipality')
                                }).next(function (resp) {
                                    var response = Ext.decode(resp.responseText);
                                    
                                    if (response.success) {
                                        var regionCmp = me.getForm().down('#regionFiasComboBox');
                                        regionCmp.disable();
                                        me.fillCmp(regionCmp, { RegionGuid: '', RegionName: response.data });
                                    }
                                }).error(function () {
                                    Ext.Msg.alert('Добавление МО', 'Произошла ошибка при получении региона МО');
                                });
                                
                                this.editRecord();

                            }
                            break;
                        case 'update':
                            this.updateGrid();
                            break;
                    }
                }
                
            },
            setField: function (record, field, value) {
                if (value) {
                    record.set(field, value);
                } else {
                    record.set(field, null);
                }
            },
            fillCmp: function(cmp, record) {
                if (record) {
                    cmp.getStore().insert(0, record);
                    cmp.setValue(record);
                } else {
                    cmp.setValue(null);
                }
            },

            otherActions: function (actions) {
                actions['municipalityGrid #btnFromFias'] = {
                    click: { fn: this.onOpenLoaderFromFias, scope: this }
                };

                actions['municipalityFiasLoadWindow b4savebutton'] = {
                    click: { fn: this.onLoadFromFias, scope: this }
                };

                actions['municipalityFiasLoadWindow b4closebutton'] = {
                    click: { fn: this.onCloseFiasWindow, scope: this }
                };
            },
            
            onCloseFiasWindow: function () {
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

    index: function () {
        var view = this.getMainView() || Ext.widget('municipalityGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.Municipality').load();
    }
});