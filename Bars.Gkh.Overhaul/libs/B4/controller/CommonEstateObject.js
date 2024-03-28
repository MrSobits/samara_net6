Ext.define('B4.controller.CommonEstateObject', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.cmnestateobj.Grid',
        'B4.view.cmnestateobj.EditPanel',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
   
    models: [
        'CommonEstateObject',
        'cmnestateobj.StructuralElementGroup',
        'cmnestateobj.StructuralElementGroupAttribute',
        'cmnestateobj.StructuralElement',
        'cmnestateobj.StructuralElementWork',
        'cmnestateobj.GroupAttributeWithResolvers',
        'B4.model.cmnestateobj.StructuralElementFeatureViol',
        'FormulaParam'
    ],
    
    stores: [
        'CommonEstateObject',
        'cmnestateobj.StructuralElementGroup',
        'cmnestateobj.StructuralElementGroupAttribute',
        'cmnestateobj.StructuralElement',
        'cmnestateobj.StructuralElementWork',
        'cmnestateobj.GroupAttributeWithResolvers',
        'dict.FeatureViolGji'
    ],
    
    views: [
        'cmnestateobj.Grid',
        'cmnestateobj.EditPanel',
        
        'cmnestateobj.group.attributes.Grid',
        'cmnestateobj.group.attributes.EditWindow',
        
        'cmnestateobj.group.structelement.Grid',
        'cmnestateobj.group.structelement.EditWindow',
        
        'cmnestateobj.group.structelement.WorkGrid',
        
        'cmnestateobj.group.formula.ParamEditWindow',
        'cmnestateobj.group.formula.ParamGrid',
        'cmnestateobj.group.structelement.FeatureViolGrid',
        
        'SelectWindow.MultiSelectWindow'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'groupattributesaspect',
            modelName: 'cmnestateobj.StructuralElementGroupAttribute',
            gridSelector: 'groupattributesgrid',
            editFormSelector: 'groupattributeseditwindow',
            editWindowView: 'cmnestateobj.group.attributes.EditWindow',
            getRecordBeforeSave: function(record) {
                var objId = this.controller.getInfoPanel().down('grouppanel').down('structelgrouppanel').objectId;
                record.set('Group', objId);
                return record;
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'groupelementsaspect',
            modelName: 'cmnestateobj.StructuralElement',
            gridSelector: 'groupelementsgrid',
            editFormSelector: 'groupelementseditwindow',
            editWindowView: 'cmnestateobj.group.structelement.EditWindow',
            getRecordBeforeSave: function(record) {
                var objId = this.controller.getInfoPanel().down('grouppanel').down('structelgrouppanel').objectId;
                record.set('Group', objId);
                return record;
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'groupelementworksaspect',
            gridSelector: '#groupElementWorksGrid',
            storeName: 'cmnestateobj.StructuralElementWork',
            modelName: 'cmnestateobj.StructuralElementWork',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#groupElementWorksMultiSelectWindow',
            storeSelect: 'dict.JobSelect',
            storeSelected: 'dict.JobSelected',
            titleSelectWindow: 'Выбор работ',
            titleGridSelect: 'Работы',
            titleGridSelected: 'Выбранные работы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],

            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];
                    records.each(function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    var sm = this.controller.getInfoPanel().down('grouppanel').down('structelgrouppanel').down('structelgroupelementspanel').down('groupelementsgrid').getSelectionModel();
                    var rec = sm.getSelection()[0];
                    var elementId = rec.getId();
                    var grid = asp.getGrid();

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', grid);
                        B4.Ajax.request({
                            method: 'POST',
                            url: B4.Url.action('AddWorks', 'CommonEstateObject'),
                            params: {
                                objectIds: Ext.encode(recordIds),
                                elementId: elementId
                            }
                        }).next(function() {
                            grid.getStore().load();
                            asp.controller.unmask();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать услуги');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'groupelementfeatureViolaspect',
            gridSelector: 'featureviolgrid',
            modelName: 'cmnestateobj.StructuralElementFeatureViol',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#structuralElementFeatureViolMultiSelectWindow',
            storeSelect: 'dict.FeatureViolGji',
            storeSelected: 'dict.FeatureViolGji',
            titleSelectWindow: 'Выбор работ',
            titleGridSelect: 'Работы',
            titleGridSelected: 'Выбранные работы',
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

                    var sm = this.controller.getInfoPanel().down('grouppanel').down('structelgrouppanel').down('structelgroupelementspanel').down('groupelementsgrid').getSelectionModel();
                    var rec = sm.getSelection()[0];
                    var elementId = rec.getId();
                    var grid = asp.getGrid();

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', grid);
                        B4.Ajax.request({
                            method: 'POST',
                            url: B4.Url.action('AddFeatureViol', 'CommonEstateObject'),
                            params: {
                                objectIds: Ext.encode(recordIds),
                                elementId: elementId
                            }
                        }).next(function () {
                            grid.getStore().load();
                            asp.controller.unmask();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать услуги');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Ovrhl.Dictionaries.CommonEstateObject.Create', applyTo: 'b4addbutton', selector: 'cmnestateobjgrid' },
                { name: 'Ovrhl.Dictionaries.CommonEstateObject.Edit', applyTo: 'b4savebutton', selector: 'cmnestateobjeditpanel' },
                { name: 'Ovrhl.Dictionaries.CommonEstateObject.Delete', applyTo: 'b4deletecolumn', selector: 'cmnestateobjgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                //---группы
                {
                    name: 'Ovrhl.Dictionaries.CommonEstateObject.Group.Create',
                    applyTo: 'b4addbutton',
                    selector: 'cmnestateobjeditpanel grouppanel structelgroupgrid'
                },
                {
                    name: 'Ovrhl.Dictionaries.CommonEstateObject.Group.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'cmnestateobjeditpanel grouppanel structelgrouppanel'
                },
                {
                    name: 'Ovrhl.Dictionaries.CommonEstateObject.Group.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'cmnestateobjeditpanel grouppanel structelgroupgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                //---характеристики групп
                {
                    name: 'Ovrhl.Dictionaries.CommonEstateObject.Group.Attribute.Create',
                    applyTo: 'b4addbutton',
                    selector: 'groupattributesgrid'
                },
                {
                    name: 'Ovrhl.Dictionaries.CommonEstateObject.Group.Attribute.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'groupattributeseditwindow'
                },
                {
                    name: 'Ovrhl.Dictionaries.CommonEstateObject.Group.Attribute.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'groupattributesgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                //---конструктивные элементы
                {
                    name: 'Ovrhl.Dictionaries.CommonEstateObject.Group.StructEl.Create',
                    applyTo: 'b4addbutton',
                    selector: 'groupelementsgrid'
                },
                {
                    name: 'Ovrhl.Dictionaries.CommonEstateObject.Group.StructEl.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'groupelementseditwindow'
                },
                {
                    name: 'Ovrhl.Dictionaries.CommonEstateObject.Group.StructEl.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'groupelementsgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Ovrhl.Dictionaries.CommonEstateObject.Group.StructEl.ReformCode_Edit',
                    applyTo: '[name=ReformCode]',
                    selector: 'groupelementseditwindow',
                    applyBy: function (component, allowed) {
                        component.setDisabled(!allowed);
                    }
                },
                //---работы конструктивного элемента
                {
                    name: 'Ovrhl.Dictionaries.CommonEstateObject.Group.StructEl.Work.Create',
                    applyTo: 'b4addbutton',
                    selector: '#groupElementWorksGrid'
                },
                {
                    name: 'Ovrhl.Dictionaries.CommonEstateObject.Group.StructEl.Work.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: '#groupElementWorksGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                //--- параметры формул
                {
                    name: 'Ovrhl.Dictionaries.CommonEstateObject.Group.Formula.Create',
                    applyTo: 'b4addbutton',
                    selector: 'cmnestateobjeditpanel grouppanel structelgrouppanel structelgroupformulapanel groupformulaparamsgrid'
                },
                {
                    name: 'Ovrhl.Dictionaries.CommonEstateObject.Group.Formula.Edit',
                    applyTo: 'b4savebutton',
                    selector: '#groupFormulaParamEditWindow'
                },
                {
                    name: 'Ovrhl.Dictionaries.CommonEstateObject.Group.Formula.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'cmnestateobjeditpanel grouppanel structelgrouppanel structelgroupformulapanel groupformulaparamsgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    refs: [
        { ref: 'mainPanel', selector: 'cmnestateobjgrid' },
        { ref: 'infoPanel', selector: 'cmnestateobjeditpanel' }
    ],

    // Init
    init: function() {
        var me = this;

        this.control({
            'cmnestateobjgrid b4addbutton': { click: { fn: me.addRecord, scope: me } },

            'cmnestateobjgrid b4updatebutton': { click: { fn: me.updateGrid, scope: me } },
            
            'cmnestateobjgrid button[action=export]': { click: { fn: me.exportGrid, scope: me } },

            'cmnestateobjgrid': { rowaction: { fn: this.rowAction, scope: this } },

            'cmnestateobjeditpanel cmnestateobjmaininfo b4savebutton': { click: { fn: me.saveCmnEstateObj, scope: me } },
            
            'cmnestateobjeditpanel grouppanel structelgroupgrid b4addbutton': { click: { fn: me.addGroup, scope: me } },
            
            'cmnestateobjeditpanel grouppanel structelgroupgrid b4updatebutton': { click: { fn: me.updateGroups, scope: me } },

            'cmnestateobjeditpanel grouppanel structelgroupgrid b4deletecolumn': { click: { fn: me.removeGroup, scope: me } },

            'cmnestateobjeditpanel grouppanel structelgrouppanel b4savebutton': { click: { fn: me.saveGroup, scope: me } },

            'cmnestateobjeditpanel grouppanel structelgroupgrid': { selectionchange: { fn: me.editGroup, scope: me } },
            
            'cmnestateobjeditpanel grouppanel structelgrouppanel structelgroupelementspanel groupelementsgrid': { selectionchange: { fn: me.reloadWorks, scope: me } },
            
            'cmnestateobjeditpanel grouppanel structelgrouppanel structelgroupformulapanel groupformulaparamsgrid b4addbutton': { click: { fn: me.showParamEditWindow, scope: me } },
            
            'cmnestateobjeditpanel grouppanel structelgrouppanel structelgroupformulapanel groupformulaparamsgrid b4updatebutton': { click: { fn: me.updateParams, scope: me } },
            
            'cmnestateobjeditpanel grouppanel structelgrouppanel structelgroupformulapanel groupformulaparamsgrid b4editcolumn': { click: { fn: me.editFormulaParam, scope: me } },
            
            'cmnestateobjeditpanel grouppanel structelgrouppanel structelgroupformulapanel groupformulaparamsgrid b4deletecolumn': { click: { fn: me.removeFormulaParam, scope: me } },
            
            'cmnestateobjeditpanel grouppanel structelgrouppanel structelgroupformulapanel button[action=getparams]': { click: { fn: me.getParamsList, scope: me } },
            
            'cmnestateobjeditpanel grouppanel structelgrouppanel structelgroupformulapanel button[action=checkformula]': { click: { fn: me.checkFormula, scope: me } },
            
            'cmnestateobjeditpanel grouppanel structelgrouppanel structelgroupformulapanel textarea[name=Formula]': { change: { fn: me.duplicateFormula, scope: me } },
            
            'groupformulaparameditwindow b4savebutton': { click: { fn: me.saveParam, scope: me } },
            
            'groupformulaparameditwindow b4closebutton': { click: { fn: me.closeParamWindow, scope: me } }
        });

        this.callParent(arguments);
    },
    
    // Action Index - shows CommonEstateObject grid
    index: function() {
        var view = this.getMainPanel() || Ext.widget('cmnestateobjgrid');

        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    },

    // Action Create - shows form for creating CommonEstateObject
    create: function() {
        var view = this.getInfoPanel() || Ext.widget('cmnestateobjeditpanel', {
            title: 'Новый ООИ'
        }),
            infopanel = view.down('cmnestateobjmaininfo'),
            model = this.getModel('CommonEstateObject'),
            rec = new model({ Id: 0 });

        if (!infopanel.getRecord()) {
            infopanel.loadRecord(rec);
            this.reloadDependedValues(view);
        }

        this.bindContext(view);
        this.application.deployView(view);
    },

    // Action Edit - shows form for edit CommonEstateObject
    edit: function(id) {
        var me = this,
            model = this.getModel('CommonEstateObject'),
            view = this.getInfoPanel() || Ext.widget('cmnestateobjeditpanel', {
                objectId: id
            }),
            infopanel = view.down('cmnestateobjmaininfo');

        if (!infopanel.getRecord()) {
            model.load(id, {
                success: function(rec) {
                    infopanel.loadRecord(rec);
                    view.down('grouppanel').setDisabled(false);
                    view.setTitle('ООИ: ' + rec.get('Name'));
                    me.reloadDependedValues(view);
                },
                scope: me
            });
        }

        this.bindContext(view);
        this.application.deployView(view);
    },
    
    exportGrid: function () {
        var frame = Ext.get('downloadIframe'), url;
        if (frame != null) {
            Ext.destroy(frame);
        }
        url = Ext.urlAppend(rootUrl + 'CommonEstateObject/Export');

        Ext.DomHelper.append(document.body, {
            tag: 'iframe',
            id: 'downloadIframe',
            frameBorder: 0,
            width: 0,
            height: 0,
            css: 'display:none;visibility:hidden;height:0px;',
            src: url
        });
    },

    // RowAction event handling
    rowAction: function(grid, action, record) {
        switch (action.toLowerCase()) {
        case 'edit':
            this.editRecord(record.getId());
            break;
        case 'delete':
            this.deleteRecord(record.getId(), grid);
            break;
        }
    },
    
    // Reload grid store
    updateGrid: function(btn) {
        btn.up('cmnestateobjgrid').getStore().load();
    },
    
    // Deleting CommonEstateObject record by Id
    deleteRecord: function(recId, grid) {
        var me = this;

        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
            if (result == 'yes') {
                var model = this.getModel('CommonEstateObject');

                var rec = new model({ Id: recId });
                me.mask('Удаление', B4.getBody());
                rec.destroy()
                    .next(function(success) {
                        grid.getStore().load();
                        me.unmask();
                    }, this)
                    .error(function(failure) {
                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(failure.responseData) ? failure.responseData : failure.responseData.message);
                        me.unmask();
                    }, this);
            }
        }, me);
    },
    
    // Add button (CommonEstateObject) click handling
    addRecord: function() {
        Ext.History.add('commonestateobj_create');
    },
    
    // Edit column (CommonEstateObject) click handling
    editRecord: function(recId) {
        Ext.History.add('commonestateobj_edit/' + recId);
    },

    // Save button (CommonEstateObject) click handling
    saveCmnEstateObj: function(btn, e, eOpts) {
        var me = this,
            editPanel = btn.up('cmnestateobjmaininfo'),
            cmnEstateObjectEditPanel = editPanel.up('cmnestateobjeditpanel'),
            rec,
            id;

        if (!me.formValidation(editPanel)) {
            return;
        }

        editPanel.getForm().updateRecord();
        rec = editPanel.getRecord();
        id = rec.getId();

        me.mask('Сохранение', editPanel);

        rec.save({ id: id })
            .next(function(result) {
                me.unmask();
                cmnEstateObjectEditPanel.objectId = result.record.getId();
                editPanel.up('cmnestateobjeditpanel').down('grouppanel').setDisabled(false);
                me.reloadDependedValues(cmnEstateObjectEditPanel);
            }, this)
            .error(function(result) {
                me.unmask();
                Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            }, this);
    },

    // Reload commonestateobject depended values
    reloadDependedValues: function(view) {
        var id = view.objectId,
            groupStore = view.down('structelgroupgrid').getStore();

        if (id) {
            groupStore.clearFilter(true);
            groupStore.filter('commonestateobject', view.objectId);
        } else {
            groupStore.removeAll(true);
        }
    },
    
    // Add Group button click handling
    addGroup: function(btn) {
        var grid = btn.up('structelgroupgrid'),
            panel = grid.up('grouppanel'),
            groupPanel = panel.down('structelgrouppanel'),
            groupGrid = panel.down('structelgroupgrid'),
            infoPanel = groupPanel.down('structelgroupmaininfopanel'),
            atrsGrid = infoPanel.down('groupattributesgrid'),
            strucelpanel = groupPanel.down('structelgroupelementspanel'),
            formulapanel = groupPanel.down('structelgroupformulapanel'),
            model = this.getModel('cmnestateobj.StructuralElementGroup'),
            rec = new model({ Id: 0 });

        groupPanel.objectId = 0;
        groupPanel.loadRecord(rec);
        groupPanel.setDisabled(false);
        groupGrid.getSelectionModel().deselectAll();
        //groupPanel.down('tabpanel').setActiveTab(0);
        strucelpanel.setDisabled(true);
        formulapanel.setDisabled(true);
        atrsGrid.setDisabled(true);
        this.reloadAttributes(groupPanel);
        this.reloadElements(groupPanel);
        this.reloadParams(groupPanel);
    },
    
    // Save Group button handler
    saveGroup: function(btn) {
        var me = this,
            groupPanel = btn.up('structelgrouppanel'),
            panel = groupPanel.up('grouppanel'),
            cmnEstateObjPanel = panel.up('cmnestateobjeditpanel'),
            formulaPanel = groupPanel.down('structelgroupformulapanel'),
            paramsGrid = formulaPanel.down('groupformulaparamsgrid'),
            paramsStore = paramsGrid.getStore(),
            params = Ext.Array.pluck(paramsStore.getRange(), 'data'),
            rec, id, grid, store;

        if (!me.formValidation(groupPanel)) {
            return;
        }

        Ext.each(params, function(item) {
            if (item.Attribute && item.Attribute.ValueResolverCode) {
                item.ValueResolverCode = item.Attribute.ValueResolverCode;
                item.ValueResolverName = item.Attribute.ValueResolverName;
                item.Attribute = null;
            }
        });

        grid = panel.down('structelgroupgrid');
        store = grid.getStore();
        groupPanel.getForm().updateRecord();
        rec = groupPanel.getRecord();
        rec.set('CommonEstateObject', cmnEstateObjPanel.objectId);
        rec.set('FormulaParams', params);
        id = rec.getId();

        me.mask('Сохранение', groupPanel);

        rec.save({ id: id })
            .next(function(result) {
                me.unmask();
                groupPanel.objectId = result.record.getId();
                groupPanel.down('structelgroupmaininfopanel').down('groupattributesgrid').setDisabled(false);
                groupPanel.down('structelgroupelementspanel').setDisabled(false);
                groupPanel.down('structelgroupformulapanel').setDisabled(false);
                paramsStore.commitChanges();
                me.reloadAttributes(groupPanel);
                me.reloadElements(groupPanel);
                me.reloadParams(groupPanel, rec);
                store.load();
            }, this)
            .error(function(result) {
                me.unmask();
                Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            }, this);
    },
    
    // Group grid selection change event handler
    editGroup: function(sm, records) {
        var me = this,
            panel = sm.view.up('grouppanel'),
            groupPanel = panel.down('structelgrouppanel'),
            tabpanel = groupPanel.down('tabpanel'),
            infoPanel = groupPanel.down('structelgroupmaininfopanel'),
            atrsGrid = infoPanel.down('groupattributesgrid'),
            strucelpanel = groupPanel.down('structelgroupelementspanel'),
            formulapanel = groupPanel.down('structelgroupformulapanel'),
            rec = records[0],
            id = rec ? rec.getId() : null;
        
        //При любом изменении группового элемента всегда делаем активной первую вкладку
        //tabpanel.setActiveTab(0);

        if (id && id != groupPanel.objectId) {
            me.mask('Обновление', groupPanel);
            groupPanel.objectId = id;
            groupPanel.loadRecord(rec);
            groupPanel.setDisabled(false);
            strucelpanel.setDisabled(false);
            formulapanel.setDisabled(false);
            atrsGrid.setDisabled(false);
            this.reloadAttributes(groupPanel);
            this.reloadElements(groupPanel);
            this.reloadParams(groupPanel, rec);
            me.unmask();
        }
    },
    
    // Remove group button click handler
    removeGroup: function (grid, action, record) {
        var me = this,
            panel = grid.up('grouppanel'),
            groupPanel = panel.down('structelgrouppanel');
        
        Ext.Msg.confirm('Удаление группы!', 'Вы действительно хотите удалить группу конструктивых элементов?', function (result) {
            if (result == 'yes') {
                var model = this.getModel('cmnestateobj.StructuralElementGroup');

                var rec = new model({ Id: grid.getStore().getAt(record).getId() });
                me.mask('Удаление', B4.getBody());
                rec.destroy()
                    .next(function (success) {
                        grid.getStore().load();
                        groupPanel.setDisabled(true);
                        me.unmask();
                    }, this)
                    .error(function (failure) {
                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(failure.responseData) ? failure.responseData : failure.responseData.message);
                        me.unmask();
                    }, this);
            }
        }, this);
    },
    
    // Group grid update button click handler
    updateGroups: function(btn) {
        btn.up('structelgroupgrid').getStore().load();
    },
    
    // Reload attributes grid
    reloadAttributes: function(panel) {
        var infoPanel = panel.down('structelgroupmaininfopanel'),
            grid = infoPanel.down('groupattributesgrid'),
            store = grid.getStore(),
            id = panel.objectId;

        if (id) {
            store.clearFilter(true);
            store.filter('group', id);
        } else {
            store.removeAll();
        }
    },
    
    // Reload elements grid
    reloadElements: function(panel) {
        var infoPanel = panel.down('structelgroupelementspanel'),
            grid = infoPanel.down('groupelementsgrid'),
            store = grid.getStore(),
            id = panel.objectId;

        if (id) {
            store.clearFilter(true);
            store.filter('group', id);
        } else {
            store.removeAll();
        }
    },
    
    // Reload attributes grid+
    reloadParams: function (panel, rec) {
        var infoPanel = panel.down('structelgroupformulapanel'),
            grid = infoPanel.down('groupformulaparamsgrid'),
            store = grid.getStore(),
            id = panel.objectId;

        store.removeAll();
        if (id) {
            store.add(rec.get('FormulaParams'));
        }
    },

    // Reload element works
    reloadWorks: function(sm, records) {
        var rec = records[0],
            id = rec ? rec.getId() : 0,
            umId = rec ? rec.get('UnitMeasure').Id : 0,
            panel = sm.view.up('structelgroupelementspanel'),
            grid = panel.down('groupelementworksgrid'),
            featureViolGrid = panel.down('featureviolgrid'),
            store = grid.getStore(),
            featureViolStore = featureViolGrid.getStore();
        
        if (id) {
            panel.selectedUnitMeasureId = umId;
            grid.setDisabled(false);
            featureViolGrid.setDisabled(false);
            store.clearFilter(true);
            store.filter('element', id);
            featureViolStore.clearFilter(true);
            featureViolStore.filter('element', id);
        } else {
            grid.setDisabled(true);
            store.removeAll();
            featureViolStore.removeAll();
        }
    },
    
    // Shows formula param edit window
    showParamEditWindow: function(btn) {
        var form = this.getForm(),
            panel = this.getInfoPanel(),
            grid = panel.down('groupformulaparamsgrid'),
            store = grid.getStore(),
            model = this.getModel('FormulaParam'),
            rec = new model({ Id: store.getCount() + 1 });

        form.loadRecord(rec);
        
        form.show();
    },
    
    // Get Form for formula params edit
    getForm: function () {
        var me = this,
            panel = this.getInfoPanel(),
            groupPanel = panel.down('structelgrouppanel'),
            grid = groupPanel.down('groupformulaparamsgrid'),
            fieldStore;

        var editWindow = Ext.ComponentQuery.query('groupFormulaParamEditWindow')[0];

        if (editWindow && !editWindow.getBox().width) {
            editWindow = editWindow.destroy();
        }

        if (!editWindow) {
            
            editWindow = me.getView('cmnestateobj.group.formula.ParamEditWindow').create(
                {
                    constrain: true,
                    renderTo: B4.getBody().getActiveTab().getEl()
                });

            fieldStore = editWindow.down('b4selectfield[name=Attribute]').getStore();
            fieldStore.clearFilter(true);
            fieldStore.filter('group', groupPanel.objectId);

            editWindow.on('beforedestroy', function () {
                if (grid.floatingItems) {
                    grid.floatingItems.removeAtKey(editWindow.getId()); //#warning необходимость в этом отпадет с версией 4.2    
                }
            });
        }

        return editWindow;
    },
    
    // Add formula param to store
    saveParam: function(btn) {
        var form = btn.up('groupformulaparameditwindow'),
            panel = this.getInfoPanel(),
            grid = panel.down('groupformulaparamsgrid'),
            store = grid.getStore(),
            me = this,
            rec, name, id, findedIndex, findedRec;
        
        if (!me.formValidation(form)) {
            return;
        }
        
        name = form.down('[name=Name]').getValue();
        id = form.down('[name=Id]').getValue();
        findedIndex = store.find('Name', name);
        
        if (findedIndex >= 0) {
            findedRec = store.getAt(findedIndex);
            if (findedRec.getId() != id) {
                Ext.Msg.alert('Ошибка!', 'Параметр с таким именем уже есть!');
                return;
            }
        }

        form.getForm().updateRecord();
        rec = form.getRecord();

        var recInStore = store.getById(rec.getId());
        if (!recInStore) {
            store.add(rec);
        }
        
        form.close();
    },
    
    // Formula param edit window close button click handler
    closeParamWindow: function(btn) {
        btn.up('groupformulaparameditwindow').close();
    },
    
    // Formula param grid editcolumn click handler
    editFormulaParam: function(grid, action, record) {
        var form = this.getForm(),
            rec = grid.getStore().getAt(record);
        
        if (!rec.get('Attribute')) {
            rec.set('Attribute', {
                Id: rec.get('ValueResolverCode'),
                Name: rec.get('ValueResolverName')
            });
        }

        form.loadRecord(rec);

        form.show();
    },
    
    // Formula param grid update button click handler
    updateParams: function(btn) {
        var paramsGrid = btn.up('groupformulaparamsgrid'),
            paramsStore = paramsGrid.getStore(),
            groupPanel = paramsGrid.up('structelgrouppanel'),
            objectId = groupPanel.objectId,
            panel = groupPanel.up('grouppanel'),
            grid = panel.down('structelgroupgrid'),
            store = grid.getStore(),
            index, rec;

        index = store.find('Id', objectId);
        rec = store.getAt(index);

        paramsStore.removeAll();
        paramsStore.add(rec.get('FormulaParams'));
    },
    
    // Get params list for formula
    getParamsList: function (btn) {
        var formulapanel = btn.up('structelgroupformulapanel'),
            grid = formulapanel.down('groupformulaparamsgrid'),
            store = grid.getStore(),
            formulaField = formulapanel.down('textarea[name=Formula]'),
            formula = formulaField.getValue(),
            model = this.getModel('FormulaParam');
        
        B4.Ajax.request({
            url: B4.Url.action('GetParamsList', 'StructuralElementGroup'),
            params: {
                formula: formula
            }
        }).next(function (resp) {
            var response = Ext.decode(resp.responseText);
            if (response.success) {
                Ext.each(response.data, function (item) {
                    var findedIndex = store.find('Name', item);
                    if (findedIndex < 0) {
                        store.add(new model({ Id: store.getCount() + 1, Name: item }));
                    }
                });
            } else {
                Ext.Msg.alert('Ошибка!', response.message);
            }
        }).error(function () {
            Ext.Msg.alert('Ошибка!', 'При получении параметров произошла ошибка!');
        });
    },
    
    // Check: is formula valid
    checkFormula: function (btn) {
        var formulapanel = btn.up('structelgroupformulapanel'),
            formulaField = formulapanel.down('textarea[name=Formula]'),
            formula = formulaField.getValue(),
            displayField = formulapanel.down('displayfield[name=FormulaMsg]');

        B4.Ajax.request({
            url: B4.Url.action('CheckFormula', 'StructuralElementGroup'),
            params: {
                formula: formula
            }
        }).next(function (resp) {
            var response = Ext.decode(resp.responseText);
            displayField.setValue(response.message);
        }).error(function () {
            Ext.Msg.alert('Ошибка!', 'При проверке формулы произошла ошибка!');
        });
    },
    
    // Remove param
    removeFormulaParam: function (grid, action, record) {
        Ext.Msg.confirm('Удаление!', 'Вы действительно хотите удалить параметр формулы?', function (result) {
            if (result == 'yes') {
                grid.getStore().removeAt(record);
            }
        }, this);   
    },
    
    // Duplicate Formula to field
    duplicateFormula: function(field, newValue, oldValue) {
        var panel = field.up('structelgrouppanel'),
            infopanel = panel.down('structelgroupmaininfopanel'),
            duplicateField = infopanel.down('textarea[name=FormulaDuplicate]');

        duplicateField.setValue(newValue);
    },

    formValidation: function (form) {
        if (form.getForm().isValid()) {
            return true;
        }

        var fields = form.getForm().getFields();

        var invalidFields = '';

        Ext.each(fields.items, function (field) {
            if (!field.isValid()) {
                invalidFields += '<br>' + field.fieldLabel;
            }
        });

        Ext.Msg.alert('Ошибка заполнения формы!', 'Не заполнены обязательные поля: ' + invalidFields);
        return false;

    }
});