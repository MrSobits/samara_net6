Ext.define('B4.controller.dict.MunicipalityTree', {
    extend: 'B4.base.Controller',

    municipalityId: 0,
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.Municipality',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: [
        'dict.Municipality',
        'dict.MunicipalityTree',
        'dict.MunicipalitySourceFinancing',
        'dict.MunicipalityFiasOktmo'
    ],
    stores: [
        'dict.Municipality',
        'dict.MunicipalityTree',
        'dict.MunicipalitySourceFinancing',
        'dict.MunicipalityFiasOktmo',
        'dict.FiasStreet',
        'dict.FiasForSelected'
    ],
    views: [
        'dict.municipalitytree.Tree',
        'dict.municipalitytree.EditWindow',
        'dict.municipality.SourceFinancingGrid',
        'dict.municipality.SourceFinancingEditWindow',
        'dict.municipalitytree.MunicipalityFiasOktmoGrid',
        'dict.municipalitytree.MunicipalityFiasOktmoEditWindow',
        'SelectWindow.MultiSelectWindow',
        'dict.municipalitytree.UnionMoWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.municipalitytree.Tree',
    mainViewSelector: 'municipalityTree',

    //Селектор окна котоырй потом используется
    editWindowSelector: '#municipalityEditWindow',
    refs: [
        {
            ref: 'mainView',
            selector: 'municipalityTree'
        }
    ],

    aspects: [
        {
            xtype: 'municipalitydictperm'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Dictionaries.MunicipalityTree.UnionMoBtn', applyTo: 'button[action=UnionMo]', selector: 'municipalityTree',
                    applyBy: function (component, allowed) {
                        if (allowed) component.setDisabled(false);
                        else component.setDisabled(true);
                    }
                }
            ]
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'municipalityGridWindowAspect',
            gridSelector: '#municipalityTreeGrid',
            editFormSelector: '#municipalityEditWindow',
            storeName: 'dict.Municipality',
            modelName: 'dict.Municipality',
            editWindowView: 'dict.municipalitytree.EditWindow',
            onSaveSuccess: function (asp, record) {

                var sfMunicipality = asp.getForm().down('#sfMunicipality');
              
                if (sfMunicipality && record) {

                    B4.Ajax.request({
                        url: B4.Url.action('SetMoParent', 'Municipality'),
                        params: {
                            moId: record.raw.Id,
                            parentMoId: sfMunicipality.getValue()
                        }
                    }).next(function() {

                    }).error(function() {
                        Ext.Msg.alert('Добавление МО', 'Произошла ошибка при сохранении родительского МО');
                    });
                    
                }
                
                var grid = asp.getGrid();
                if (grid) {
                    var store = grid.getStore();
                    if (store) {
                        store.load();
                    }
                }


                asp.controller.setCurrentId(record.getId());
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    asp.controller.setCurrentId(record.getId());
                    
                    var str = asp.getGrid().getStore();

                    if (record) {
                        var parentMo = record.raw.ParentMo;

                        if (str && parentMo) {
                            var parent = str.getById(parentMo.Id);

                            var sfMunicipality = asp.getForm().down('#sfMunicipality');

                            if (sfMunicipality) {
                                
                                if (parent) {
                                    sfMunicipality.value = parent.raw.id;
                                    sfMunicipality.setRawValue(parent.raw.Name);
                                }
                            }
                        }
                    }

                    // var fias = record.get('DinamicFias');
                    //var fiasCmp = this.getForm().down('#municipalityFiasComboBox');

                    //if (fias) {
                    //    fiasCmp.getStore().insert(0, fias);
                    //    fiasCmp.setValue(fias);
                    //} else {
                    //    fiasCmp.setValue(null);
                    //}
                },
                getdata: function (asp, record) {
                    //var fiasId = this.getForm().down('#municipalityFiasComboBox').getValue();

                    //if (fiasId) {
                    //    record.set('FiasId', fiasId);
                    //} else {
                    //    record.set('FiasId', null);
                    //}
                }
            },

            otherActions: function (actions) {
                actions['municipalityGrid #btnFromFias'] = {
                    click: { fn: this.onOpenLoaderFromFias, scope: this }
                },                
                actions['#municipalityTreeGrid b4deletecolumn'] = {
                    click: { fn: this.onDeleteRecord, scope: this }
                },
                actions['municipalityFiasLoadWindow b4savebutton'] = {
                    click: { fn: this.onLoadFromFias, scope: this }
                },
                actions['municipalityFiasLoadWindow b4closebutton'] = {
                    click: { fn: this.onCloseFiasWindow, scope: this }
                },
                actions['municipalityTree b4updatebutton'] = {
                    click: { fn: this.onRefreshTree, scope: this }
                },
                actions['municipalityTree button[action=UnionMo]'] = {
                    click: { fn: this.onUnionMo, scope: this }
                },     
                actions['#municipalityTreeGrid b4editcolumn'] = {
                    click: { fn: this.onEditRecord, scope: this }
                },
                actions['unionMoWindow [name=CopyFrom]'] = {
                    change: { fn: this.onCopyFromChange, scope: this }
                },
                actions['unionMoWindow button[action=SaveBtn]'] = {
                    click: { fn: this.onSaveBtn, scope: this }
                },
                actions['unionMoWindow treeselectfield[name=UnionMos]'] = {
                    beforeload: { fn: this.onBeforeLoadUnionMos, scope: this }
                }
            },
            
            onEditRecord: function (a,b,t,y,r,rec ) {
                window.asp = this;
                asp.editRecord(rec);
            },
            
            onDeleteRecord: function (a, b, t, y, r, rec) {
                window.asp = this;
                asp.deleteRecord(rec);
            },

            onCloseFiasWindow: function () {
                var loadWindow = Ext.ComponentQuery.query('municipalityFiasLoadWindow')[0];

                loadWindow.close();
            },

            onRefreshTree: function () {

                var searchAr = Ext.ComponentQuery.query('#municipalityTreeGrid textfield');
                
                if (searchAr.length > 0) {
                    var searchBox = searchAr[0];

                    var filter = searchBox.getValue();
                                       
                    var grid = this.getGrid();
                    
                    if (grid) {
                        var store = grid.getStore();
                        if (store) {
                            
                            if (filter) {
                                store.load({
                                    params: {
                                        name: filter
                                    }
                                });
                            } else {
                                store.load();
                            }
                        }
                    }
                } 
            },

            onBeforeLoadUnionMos: function (field, options, store) {
                var me = this,
                    win = field.up('window'),
                    copyFromFld = win.down('[name=CopyFrom]'),
                    searchFld;

                options = options || {};
                options.params = options.params || {};

                options.params.parentMoName = copyFromFld.getText();

                if (field.selectWindow) {
                    searchFld = field.selectWindow.down('[name=tfSearch]');
                    options.params.search = searchFld.getValue();
                }
                
                return true;
            },

            onUnionMo: function () {
                var me = this,
                    unionMoWindow = Ext.create('B4.view.dict.municipalitytree.UnionMoWindow');

                unionMoWindow.show();
            },

            onCopyFromChange: function (fld, newValue) {
                var me = this,
                    win = fld.up('window'),
                    unionMosFld = win.down('[name=UnionMos]');

                if (newValue) {
                    unionMosFld.setValue();
                    unionMosFld.setDisabled(false);
                } else {
                    unionMosFld.setValue();
                    unionMosFld.setDisabled(true);
                }
            },

            onSaveBtn: function (btn, ev) {
                var me = this,
                    tree = me.controller.getMainView(),
                    treeGrid = tree.down('#municipalityTreeGrid'),
                    treeStore = treeGrid.getStore(),
                    win = btn.up('window'),
                    copyFromFld = win.down('[name=CopyFrom]'),
                    nameFld = win.down('[name=Name]'),
                    unionMosFld = win.down('[name=UnionMos]');

                if (copyFromFld.getValue() == null) {
                    Ext.Msg.alert('Внимание!', 'Поле "Скопировать с" обязательно для заполнения');
                    return;
                }
                else if (nameFld.getValue() == '') {
                    Ext.Msg.alert('Внимание!', 'Поле "Наименование" обязательно для заполнения');
                    return;
                }
                else if (unionMosFld.getValue() == null) {
                    Ext.Msg.alert('Внимание!', 'Поле "МО для объединения" обязательно для заполнения');
                    return;
                }

                me.mask('Объединение МО', me.controller.getMainComponent());
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('UnionMo', 'Municipality'),
                    params: {
                        unionMos: unionMosFld.getValue(),
                        name: nameFld.getValue(),
                        copyFromId: copyFromFld.getValue()
                    }
                }).next(function () {
                    me.unmask();
                    B4.QuickMsg.msg('Внимание!', 'Объединение МО выполнено успешно', 'success');
                    win.close();
                    treeStore.load();
                    return true;
                    }).error(function (response) {
                    me.unmask();
                    B4.QuickMsg.msg('Внимание!', 'При объединении МО возникла ошибка', 'error');
                    win.close();
                    });
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
            xtype: 'grideditwindowaspect',
            name: 'municipalityfiasoktmogridwindowaspect',
            gridSelector: 'municipalityfiasoktmogrid',
            editFormSelector: 'municipalityfiasoktmoeditwindow',
            storeName: 'dict.MunicipalityFiasOktmo',
            modelName: 'dict.MunicipalityFiasOktmo',
            editWindowView: 'dict.municipalitytree.MunicipalityFiasOktmoEditWindow',
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' [name=FiasGuid]'] = { 'beforeload': { fn: me.controller.onBeforeLoad, scope: me.controller } };
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Municipality', this.controller.municipalityId);
                    }
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.getStore('dict.MunicipalityFiasOktmo').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('municipalityTree');
        this.bindContext(view);
        this.application.deployView(view);
    },

    setCurrentId: function (id) {
        this.municipalityId = id;

        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];
        editWindow.down('.tabpanel').setActiveTab(0);

        var sourceStore = this.getStore('dict.MunicipalitySourceFinancing');
        sourceStore.removeAll();

        var fiasOktmoStore = this.getStore('dict.MunicipalityFiasOktmo');
        fiasOktmoStore.removeAll();

        if (id > 0) {
            editWindow.down('#municipalitySourceFinancingGrid').setDisabled(false);
            sourceStore.load();

            editWindow.down('municipalityfiasoktmogrid').setDisabled(false);
            fiasOktmoStore.load();
        } else {
            editWindow.down('#municipalitySourceFinancingGrid').setDisabled(true);
            editWindow.down('municipalityfiasoktmogrid').setDisabled(true);
        }
    },

    onBeforeLoad: function (store, operation) {
        operation.params.municipalityId = this.municipalityId;
    }
});