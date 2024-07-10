// Сторы в разных местах создаются. Некотоыре прямо в Гриде , некотоыре через Контроллер. Некотоыре сторы передают параметры через filter, 
// другие сторы передают параметры через конфиг в момент create, а другие сторы передают параметры через подписку на beforeload. Устал искать где ошибки.

// Невозможно дорабатывать
Ext.define('B4.controller.realityobj.StructElement', {
    extend: 'B4.controller.MenuItemController',
    params: {},

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextButton',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.StateContextMenu',
        'B4.aspects.FieldRequirementAspect',
        'B4.Ajax',
        'B4.Url'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'RealityObject',
        'realityobj.StructuralElement',
        'realityobj.StructuralElementAttributeValue',
        'realityobj.StructuralElementWork',
        'realityobj.MissingCeo'
    ],

    stores: [
        'RealityObject',
        'realityobj.StructuralElement',
        'realityobj.StructuralElementWork',
        'realityobj.StructuralElementTree',
        'realityobj.MissingCeo',
        'realityobj.CeoSelected',
        'realityobj.CeoSelect'
    ],

    views: [
        'realityobj.structelement.Panel',
        'realityobj.structelement.Grid',
        'realityobj.structelement.TreeMultiSelect',
        'realityobj.structelement.EditWindow',
        'SelectWindow.MultiSelectWindow',
        'realityobj.missingceo.Grid',
        'realityobj.structelement.HistoryDetailWindow',
        'realityobj.structelement.HistoryDetailGrid',
        'realityobj.structelement.HistoryGrid'
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',

    refs: [
        {
            ref: 'mainView',
            selector: 'structelementpanel'
        },
        {
            ref: 'treeGrid',
            selector: '#tpSelect'
        },
        {
            ref: 'elGridInfoLabel',
            selector: 'structelementgrid toolbar label[name="info"]'
        },
        {
            ref: 'chBoxOnlyRequired',
            selector: 'structelselect toolbar checkbox[name="OnlyRequired"]'
        },
        {
            ref: 'cbDetailed',
            selector: 'structelselect toolbar checkbox[name="Detailed"]'
        },
        {
            ref: 'tfFind',
            selector: 'structelselect textfield[name="Find"]'
        }
    ],

    aspects: [
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'Gkh.RealityObject.StructElem.Field.LastOverhaulYear_Rqrd', applyTo: '[name=LastOverhaulYear]', selector: 'rostructeleditwin' },
                { name: 'Gkh.RealityObject.StructElem.Field.Wearout_Rqrd', applyTo: '[name=Wearout]', selector: 'rostructeleditwin' },
                { name: 'Gkh.RealityObject.StructElem.Field.Volume_Rqrd', applyTo: '[name=Volume]', selector: 'rostructeleditwin' },
                { name: 'Gkh.RealityObject.StructElem.Field.Condition_Rqrd', applyTo: '[name=Condition]', selector: 'rostructeleditwin' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'structelstatepermission',
            permissions: [
                { name: 'Gkh.RealityObject.Register.StructElem.Create', applyTo: 'b4addbutton', selector: 'structelementgrid' },
                { name: 'Gkh.RealityObject.Register.StructElem.Edit', applyTo: 'b4savebutton', selector: 'rostructeleditwin' },
                {
                    name: 'Gkh.RealityObject.Register.StructElem.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'structelementgrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.StructElem.PlanRepairYear_View',
                    applyTo: 'gridcolumn[dataIndex=PlanRepairYear]',
                    selector: 'structelementgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.StructElem.AdjustedYear_View',
                    applyTo: 'gridcolumn[dataIndex=AdjustedYear]',
                    selector: 'structelementgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.RealityObject.Register.StructElem.Create', applyTo: 'b4addbutton', selector: 'missingcommonestobjgrid' },
                {
                    name: 'Gkh.RealityObject.Register.StructElem.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'missingcommonestobjgrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.RealityObject.Field.Points_View', applyTo: 'b4savebutton', selector: 'structelementpanel' },
                { name: 'Gkh.RealityObject.Field.Points_Edit', applyTo: '[name=Points]', selector: 'structelementpanel' },
                {
                    name: 'Gkh.RealityObject.Field.Points_View',
                    applyTo: '[name=Points]',
                    selector: 'structelementpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.RealityObject.Register.StructElem.Field.Condition_Edit', applyTo: '[name=Condition]', selector: 'rostructeleditwin' },
                {
                    name: 'Gkh.RealityObject.Register.StructElem.Field.Condition_View',
                    applyTo: '[name=Condition]',
                    selector: 'rostructeleditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.RealityObject.Register.StructElem.Field.LastOverhaulYear_Edit', applyTo: '[name=LastOverhaulYear]', selector: 'rostructeleditwin' },
                {
                    name: 'Gkh.RealityObject.Register.StructElem.Field.LastOverhaulYear_View',
                    applyTo: '[name=LastOverhaulYear]',
                    selector: 'rostructeleditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.RealityObject.Register.StructElem.Field.Wearout_Edit', applyTo: '[name=Wearout]', selector: 'rostructeleditwin' },
                {
                    name: 'Gkh.RealityObject.Register.StructElem.Field.Wearout_View',
                    applyTo: '[name=Wearout]',
                    selector: 'rostructeleditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.RealityObject.Register.StructElem.Field.WearoutAct_Edit', applyTo: '[name=WearoutActual]', selector: 'rostructeleditwin' },
                {
                    name: 'Gkh.RealityObject.Register.StructElem.Field.WearoutAct_View',
                    applyTo: '[name=WearoutActual]',
                    selector: 'rostructeleditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.StructElem.Field.WearoutAct_View',
                    applyTo: 'gridcolumn[dataIndex=WearoutActual]',
                    selector: 'structelementgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.StructElem.Field.Wearout_View',
                    applyTo: 'gridcolumn[dataIndex=Wearout]',
                    selector: 'structelementgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.RealityObject.Register.StructElem.Field.Volume_Edit', applyTo: '[name=Volume]', selector: 'rostructeleditwin' },
                {
                    name: 'Gkh.RealityObject.Register.StructElem.Field.Volume_View',
                    applyTo: '[name=Volume]',
                    selector: 'rostructeleditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.RealityObject.Register.StructElem.Field.ElementName_Edit', applyTo: '[name=ElementName]', selector: 'rostructeleditwin' },
                {
                    name: 'Gkh.RealityObject.Register.StructElem.Field.ElementName_View',
                    applyTo: '[name=ElementName]',
                    selector: 'rostructeleditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.RealityObject.Register.StructElem.Field.SystemType_Edit', applyTo: '[name=SystemType]', selector: 'rostructeleditwin' },
                {
                    name: 'Gkh.RealityObject.Register.StructElem.Field.SystemType_View',
                    applyTo: '[name=SystemType]',
                    selector: 'rostructeleditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.RealityObject.Register.StructElem.Field.NetworkLength_Edit', applyTo: '[name=NetworkLength]', selector: 'rostructeleditwin' },
                {
                    name: 'Gkh.RealityObject.Register.StructElem.Field.NetworkLength_View',
                    applyTo: '[name=NetworkLength]',
                    selector: 'rostructeleditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.RealityObject.Register.StructElem.Field.NetworkPower_Edit', applyTo: '[name=NetworkPower]', selector: 'rostructeleditwin' },
                {
                    name: 'Gkh.RealityObject.Register.StructElem.Field.NetworkPower_View',
                    applyTo: '[name=NetworkPower]',
                    selector: 'rostructeleditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'roStructElStateTransferAspect',
            gridSelector: 'structelementgrid',
            menuSelector: 'structelementgridStateMenu',
            stateType: 'ovrhl_ro_struct_el'
        },
        {
            xtype: 'statecontextbuttonaspect',
            name: 'realObjStructElStateAspect',
            stateButtonSelector: 'rostructeleditwin #eledit-state',
            listeners: {
                transfersuccess: function () {
                    var me = this,
                        win = me.componentQuery('rostructeleditwin'),
                        model = me.controller.getModel('realityobj.StructuralElement'),
                        rec = win.getRecord();

                    model.load(rec.getId(), {
                        success: function (record) {
                            me.setStateData(record.get('Id'), record.get('State'));
                            me.controller.updateGrid();
                        }
                    });
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'structelworkaspect',
            modelName: 'realityobj.StructuralElementWork',
            gridSelector: 'realityobjStructElWorkGrid',
            editFormSelector: 'robjectStructElWorkEditWindow',
            editWindowView: 'realityobj.structelement.WorkEditWindow',
            getRecordBeforeSave: function(record) {
                record.set('StructuralElement', this.controller.params.structElId);
                return record;
            }
        },
        {
            xtype: 'gkheditpanel',
            name: 'roStructElementEditPanelAspect',
            editPanelSelector: 'structelementpanel',
            modelName: 'RealityObject'
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'missingCommonEstObjAspect',
            gridSelector: 'missingcommonestobjgrid',
            storeName: 'realityobj.MissingCeo',
            modelName: 'realityobj.MissingCeo',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#missingCommonEstObjMultiSelectWindow',
            storeSelect: 'realityobj.CeoSelect',
            storeSelected: 'realityobj.CeoSelected',
            titleSelectWindow: 'Выбор ООИ',
            titleGridSelect: 'ООИ',
            titleGridSelected: 'Выбранные ООИ',
            columnsGridSelect: [
                { header: 'Объект общего имущества', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Объект общего имущества', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            onBeforeLoad: function (store, operation) {
                var me = this;
                operation.params.realObjId = me.controller.getContextValue(me.controller.getMainView(), 'realityObjectId');
            },
            listeners: {
                getdata: function(asp, records) {
                    var me = this;
                    var recordIds = [];

                    records.each(function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddMissingCeo', 'RealityObjectMissingCeo', {
                            commonEstObjIds: Ext.encode(recordIds),
                            realObjId: me.controller.getContextValue(me.controller.getMainView(), 'realityObjectId')
                        })).next(function() {
                            asp.controller.getStore('realityobj.MissingCeo').load();
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранено!', 'Объекты общего имущества сохранены успешно');
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать объект общего имущества ');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function() {
        var me = this;
        me.control({
            'structelementgrid b4addbutton': {
                 click: { fn: me.addStructEl, scope: me }
            },

            'structelementgrid b4updatebutton': {
                 click: { fn: me.updateGrid, scope: me }
            },

            'structelementgrid': {
                 rowaction: { fn: me.rowaction, scope: me }
            },

            'structelselect b4closebutton': {
                 click: { fn: me.closeSelectElWin, scope: me }
            },

            'structelselect b4savebutton': {
                 click: { fn: me.saveStructElems, scope: me }
            },

            'structelselect button[cmd="Expand"]': {
                 click: { fn: me.expandAll, scope: me }
            },

            'rostructeleditwin b4savebutton': {
                 click: { fn: me.saveStructEl, scope: me }
            },

            'rostructeleditwin b4closebutton': {
                 click: { fn: me.closeStructElWin, scope: me }
            },

            'structelselect #btnUpdateTree': {
                 click: { fn: me.updateTree, scope: me }
            },

            'structelementgrid toolbar label[name="info"]': {
                beforerender: me.updateInfoLabel,
                afterrender: me.attachTooltip
            },

            'structelselect toolbar checkbox[name="Detailed"]': {
                change: me.onChBoxOnlyReqsChanged
            },

            'structelselect toolbar checkbox[name="OnlyRequired"]': {
                change: me.onChBoxOnlyReqsChanged
            },

            'structelselect treepanel': {
                load: me.onTreepanelLoad
            },

            'structelselect textfield[name="Find"]': {
                keydown: { fn: me.findFieldKeyAction, scope: me }
            },

            'rostructelhistorygrid b4updatebutton': {
                    click: function (btn) {
                        btn.up('rostructelhistorygrid').getStore().load();
                    }
            },
            'rostructelhistorygrid': {
                itemdblclick: { fn: me.onItemDblClickHistoryGrid, scope: me }
            },
            'rostructelhistorygrid b4editcolumn': {
                click: { fn: me.editHistoryGridBtnClick, scope: me }
            }
        });

        me.getStore('realityobj.StructuralElement').on('beforeload', me.onBeforeLoad, me);
        me.getStore('realityobj.StructuralElement').on('load', me.onStructElementStoreLoad, me);
        me.getStore('realityobj.StructuralElementWork').on('beforeload', me.onWorkBeforeLoad, me);
        me.getStore('realityobj.MissingCeo').on('beforeload', me.onMissingCeoBeforeLoad, me);
        me.callParent(arguments);
    },


    index: function (id) {
        var me = this,
            view,
            store;

        view = me.getMainView() || Ext.widget('structelementpanel');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        store = me.getStore('realityobj.StructuralElement');

        me.getAspect('roStructElementEditPanelAspect').setData(id);
        me.getAspect('structelstatepermission').setPermissionsByRecord({ getId: function() { return id; } });

        if (store.currentPage) {
            store.currentPage = 1;
        }

        me.getStore('realityobj.MissingCeo').load();
        store.load();
        view.down('rostructelhistorygrid').getStore().filter('realityObjectId', id);
    },

    onMissingCeoBeforeLoad: function (store, operation) {
        var me = this;
            operation.params.realObjId = me.getContextValue(me.getMainView(), 'realityObjectId');
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
            operation.params.objectId = me.getContextValue(me.getMainView(), 'realityObjectId');
    },
    onWorkBeforeLoad: function(store, operation) {
        if (this.params) {
            operation.params.structElId = this.params.structElId;
        }
    },

    onStructElementStoreLoad: function() {
        var me = this;
        me.updateInfoLabel(me.getElGridInfoLabel());
    },

    updateTree: function(btn) {
        var win = btn.up('structelselect'),
            treePanel = win.down('treepanel'),
            store = treePanel.getStore();

        store.load();
    },

    clearFilter: function(treePanel) {
        var view = treePanel.getView();

        treePanel.collapseAll();
        treePanel.getRootNode().cascadeBy(function(tree, view) {
            var uiNode = view.getNodeByRecord(this);
            if (uiNode) {
                Ext.get(uiNode).setDisplayed('table-row');
            }
        }, null, [this, view]);
    },

    filterBy: function(treePanel, text, by) {
        var view = treePanel.getView(),
            nodesAndParents = [];

        // Find the nodes which match the search term, expand them.
        // Then add them and their parents to nodesAndParents.
        treePanel.getRootNode().cascadeBy(function(tree) {
            var currNode = this;
            if (currNode && currNode.get(by) && currNode.get(by).toString().toLowerCase().indexOf(text.toLowerCase()) > -1) {
                treePanel.expandPath(currNode.getPath());

                if (currNode.hasChildNodes()) {
                    currNode.eachChild(function(child) {
                        nodesAndParents.push(child.id);
                    });
                }
                while (currNode.parentNode) {
                    nodesAndParents.push(currNode.id);
                    currNode = currNode.parentNode;
                }
            }
        }, null, [treePanel, view]);

        // Hide all of the nodes which aren't in nodesAndParents
        treePanel.getRootNode().cascadeBy(function(tree) {
            var uiNode = view.getNodeByRecord(this);
            if (uiNode) {
                if (!Ext.Array.contains(nodesAndParents, this.id)) {
                    Ext.get(uiNode).setDisplayed('none');
                } else {
                    Ext.get(uiNode).setDisplayed('table-row');
                }
            }
        });
    },

    rowaction: function(grid, action, record) {
        switch (action.toLowerCase()) {
            case 'edit':
                this.editRecord(record);
                break;
            case 'delete':
                this.deleteRecord(record);
                break;
        }
    },

    deleteRecord: function(record) {
        var me = this;

        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
            if (result === 'yes') {
                var model = this.getModel('realityobj.StructuralElement');

                var rec = new model({ Id: record.getId() });
                rec.destroy()
                    .next(function(result) {
                        me.updateGrid();
                    }, this)
                    .error(function(result) {
                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                    }, this);
            }
        }, me);
    },

    updateGrid: function(btn) {
        this.getStore('realityobj.StructuralElement').load();
    },

    getStructElSelectWindow: function() {
        var win = Ext.ComponentQuery.query('realityObjStructElSelectWindowTree')[0];

        if (win && !win.getBox().width) {
            win = win.destroy();
        }

        if (!win) {

            win = this.getView('realityobj.structelement.TreeMultiSelect').create({
                constrain: true,
                modal: false,
                objectId: this.getContextValue(this.getMainView(), 'realityObjectId'),
                closeAction: 'destroy',
                renderTo: B4.getBody().getActiveTab().getEl()
            });
        }

        return win;
    },

    getStructElEditWindow: function (addFields, name, multiple, isEngineeringNetwork) {
        var me = this,
            win = Ext.ComponentQuery.query('robjectStructelemEditWindow')[0];

        if (win && !win.getBox().width) {
            win = win.destroy();
        }

        if (!win) {
            win = me.getView('realityobj.structelement.EditWindow').create({
                constrain: true,
                modal: false,
                engineeringNetwork: isEngineeringNetwork,
                fields: addFields,
                showNameEditor: multiple,
                closeAction: 'destroy',
                structElName: name,
                renderTo: B4.getBody().getActiveTab().getEl(),
                ctxKey: me.getCurrentContextKey()
            });
        }

        return win;
    },

    addStructEl: function(btn) {
        //Получаем Фомру выбора Структурного элемента
        var win = this.getStructElSelectWindow();
        if (win) {
            win.show();
        }
    },

    onTreeStoreBeforeLoad: function(store, operation) {
        operation.params.onlyreq = this.getChBoxOnlyRequired().getValue();
    },

    closeSelectElWin: function(btn) {
        btn.up('structelselect').close();
    },

    closeStructElWin: function(btn) {
        btn.up('rostructeleditwin').close();
    },

    findFieldKeyAction: function(field, e) {
        if (e.getKey() === Ext.EventObject.ENTER) {
            this.params.findValue = field.getValue();
            this.getTreeGrid().getStore().load();
        }
    },

    saveStructEl: function(btn) {
        var me = this,
            win = btn.up('rostructeleditwin'),
            rec,
            dynamicFields,
            atrValues = [];

        if (!win.getForm().isValid()) {
            Ext.Msg.alert('Ошибка!', 'Проверьте правильность заполнения формы!');
            return;
        }

        dynamicFields = win.getForm().getFields().filterBy(function(item, key) { return item.dynamicField; });

        dynamicFields.each(function(item, index, len) {
            atrValues.push({
                Id: item.valId,
                Attribute: item.name,
                Value: item.getValue()
            });
        });
        var hasUploads = win.getForm().hasUpload();     
        win.getForm().updateRecord();
        rec = win.getRecord();
        rec.set('Values', atrValues);
        if (!rec.getId()) {
            saverecordNotUploaded(rec, win);
        }
        else if (hasUploads) {
            me.saverecordUploaded(rec, win);
        }
    },

    saverecordUploaded: function (rec, win) {
        var me = this,
            form = win.getForm();
            form.submit({
            url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
            params: {
                records: Ext.encode([rec.getData()])
            },
            success: function () {
                me.unmask();
                win.close();
                B4.QuickMsg.msg('Успешно', 'Успешно сохранено!', 'success');
            },
            failure: function (frm, action) {
                me.unmask();
                Ext.Msg.alert('Ошибка сохранения!', action.result.message);
            }
        });
    },

    saverecordNotUploaded: function (rec, win) {
        var me = this;
        rec.save({
            Id: rec.getId()
        }).next(function (resp) {
            me.params.structElId = resp.record.getId();
            me.getStore('realityobj.StructuralElement').load();
            win.close();
        }).error(function (result) {
            Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
        });
    },

    selectStructEl: function(btn) {
        var me = this,
            selectWin = btn.up('structelselect'),
            treePanel = selectWin.down('treepanel'),
            checked = treePanel.getChecked(),
            structElName,
            selectId,
            editWin,
            model,
            rec;

        if (checked.length < 1) {
            Ext.Msg.alert('Ошибка!', 'Необходимо выбрать конструктивный элемент!');
            return;
        }

        selectId = checked[0].data.id;
        structElName = checked[0].data.text;
        model = this.getModel('realityobj.StructuralElement');
        rec = new model({
            Id: 0,
            StructuralElement: selectId,
            RealityObject: me.getContextValue(me.getMainView(), 'realityObjectId')
        });

        B4.Ajax.request({
            url: B4.Url.action('GetAttributes', 'StructuralElement'),
            params: {
                element: selectId
            }
        }).next(function(resp) {
            var response = Ext.decode(resp.responseText),
                addFields = [];

            Ext.each(response.Attributes.data, function (item) {
                addFields.push(me.makeField(item));
            });

            editWin = me.getStructElEditWindow(addFields, structElName);

            editWin.loadRecord(rec);

            selectWin.close();

            me.getStore('realityobj.StructuralElementWork').removeAll();
            editWin.show();
        }).error(function() {
            Ext.Msg.alert('Ошибка!', 'При получении атрибутов произошла ошибка!');
        });
    },

    editRecord: function(rec) {
        var me = this,
            sId = rec.getId(),
            editWin;

        // Сохраняем значение текущего редактируемого элемента дома (для аспекта)
        this.params.structElId = sId;

        B4.Ajax.request({
            url: B4.Url.action('GetAttributes', 'StructuralElement'),
            params: {
                roelement: sId
            }
        }).next(function(resp) {
            var response = Ext.decode(resp.responseText),
                addFields = [],
                attributes = [],
                isEngineeringNetwork = false;

            if (response.Group) {
                isEngineeringNetwork = response.Group.IsEngineeringNetwork;
            }

            attributes = response.Attributes.data;

            Ext.each(attributes, function (item) {
                addFields.push(me.makeField(item));
                if (item.Hint && item.Hint !== '') {
                    addFields.push(me.makeHintLabel(item.Hint));
                }
            });

            editWin = me.getStructElEditWindow(addFields, rec.get('ElementName'), rec.get('Multiple'), isEngineeringNetwork);
            editWin.loadRecord(rec);
            editWin.show();

            me.getAspect('realObjStructElStateAspect').setStateData(rec.get('Id'), rec.get('State'));
        }).error(function() {
            Ext.Msg.alert('Ошибка!', 'При получении атрибутов произошла ошибка!');
        });
    },

    makeField: function(item) {
        var config;
        switch (item.AttributeType) {
            case 0:
                config = {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    allowDecimals: false
                };
                break;
            case 1:
                config = {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    allowDecimals: true
                };
                break;
            case 2:
                config = {
                    xtype: 'textfield'
                };
                break;
            case 3:
                config = {
                    xtype: 'checkbox',
                    value: item.Value === 'True',
                    checked: item.Value === 'True'
                };
                break;
            default:
                config = {
                    xtype: 'textfield'
                };
                break;
        }

        return Ext.apply({
            xtype: 'textfield',
            name: item.Id,
            fieldLabel: item.Name,
            anchor: '100%',
            allowBlank: !item.IsNeeded,
            dynamicField: true,
            valId: item.ValueId,
            value: item.Value
        }, config);
    },

    makeHintLabel: function(hintText) {
        return {
            xtype: 'label',
            text: hintText,
            style: 'color: #6e6efa; display: block;',
            margin: '0 0 5 155'
        };
    },

    validateStructElem: function(record) {
        return !Ext.isEmpty(record.get('Capacity')) && !Ext.isEmpty(record.get('LastYear'));
    },

    beforeStructElemsSave: function() {
    },

    saveStructElems: function (btn) {
        var me = this,
            grid = btn.up('structelselect').down('treepanel'),
            root = grid.getRootNode(),
            store = grid.getStore(),
            records = [];

        root.cascadeBy(function(record) {
            if (record.get('checked')) {
                records.push(record);
            }
        });

        B4.Ajax.request({
            url: B4.Url.action('IsRequiredStructElAdded', 'RealityObjectStructuralElement'),
            method: 'POST',
            timeout: 5 * 60 * 1000,
            params: {
                robjId: me.params.realityObjectId
            }
        }).next(function(response) {
            var resp;

            var countExceeds = false;
            Ext.each(records,
                function(rec) {
                    var count = rec.get('Count');
                    if (count > 50) {
                        countExceeds = true;
                        return;
                    }
                });

            if (!countExceeds) {
                try {
                    resp = Ext.JSON.decode(response.responseText);
                    if (!resp.data
                        .IsAdded &&
                        !me.validateRequiredGroups(store, resp.data.AddedStructElRequiredGroups)) {
                        Ext.Msg.confirm('Сообщение!',
                            'Необходимо выбрать хотя бы один элемент из обязательной группы. Сохранить выбранные конструктивные элементы?',
                            function(result) {
                                if (result === 'yes') {
                                    me.createRealObjStructElems(records, grid);
                                }
                            },
                            me);
                    } else {
                        me.createRealObjStructElems(records, grid);
                    }
                } catch (e) {
                    throw new Error(e);
                }
            } else {
                Ext.Msg.alert('Ошибка!', 'Введено слишком большое количество! Максимальное количество - 50');
            }
        }).error(function() {
            Ext.Msg.alert('Ошибка!', 'При выполнении запроса произошла ошибка!');
        });
    },

    createRealObjStructElems: function(records, grid) {

        var me = this,
            store = grid.getStore(),
            roStructEls = [];
        Ext.each(records, function(rec) {
            var count = rec.get('Count');
            if (!rec.get('multiple')) {
                roStructEls.push({
                    RealityObject: store.realityObjectId,
                    StructuralElement: rec.get('ElemId'),
                    Volume: rec.get('Capacity'),
                    LastOverhaulYear: rec.get('LastYear'),
                    Wearout: rec.get('Wearout'),
                    WearoutActual: rec.get('Wearout')
                });
            } else {
                for (var i = 1; i <= count; i++) {
                    roStructEls.push({
                        RealityObject: store.realityObjectId,
                        StructuralElement: rec.get('ElemId'),
                        Volume: rec.get('Capacity'),
                        LastOverhaulYear: rec.get('LastYear'),
                        Wearout: rec.get('Wearout'),
                        WearoutActual: rec.get('Wearout'),
                        Name: rec.get('text') + ' ' + i
                    });
                }
            }
        });

        B4.Ajax.request({
            url: B4.Url.action('Create', 'realityObjectStructuralElement'),
            method: 'POST',
            params: {
                records: Ext.JSON.encode(roStructEls)
            }
        }).next(function(response) {
            grid.up('window').close();
            me.getMainView().down('structelementgrid').getStore().load();
        }).error(function(response) {
            Ext.Msg.alert('Ошибка', response.message);
        });
    },

    expandAll: function(btn) {
        var selectWin = btn.up('structelselect'),
            treePanel = selectWin.down('treepanel'),
            chkbox = selectWin.down('checkbox[name="OnlyRequired"]');

        treePanel.expandAll();

        this.onChBoxOnlyReqsChanged(chkbox, chkbox.getValue());
    },

    validateRequiredGroups: function(treestore, addedgroups) {
        var me = this,
            root = treestore.getRootNode(),
            groups = [],
            valid = true;

        root.cascadeBy(function(node) {
            if (node.get('type') === 'group'
                && node.get('required')
                && !node.get('added')
                && (Ext.Array.indexOf(addedgroups, node.get('groupid')) === -1)) {
                groups.push(node);
            }
        });

        Ext.each(groups, function(g) {
            valid = me.hasCheckedElem(g);
            return valid;
        });

        return valid;
    },

    hasCheckedElem: function(groupNode) {
        var result = groupNode.findChildBy(function(node) {
            return node.get('checked');
        });

        if (result) {
            return true;
        }
        return false;
    },

    attachTooltip: function(lbl) {
        var me = this;
        lbl.getEl().on('mouseenter', me.showInfoToolTip);
        lbl.getEl().on('mouseleave', me.hideInfoToolTip);
    },

    updateInfoLabel: function(lbl) {
        var me = this;

        B4.Ajax.request({
            url: B4.Url.action('IsRequiredStructElAdded', 'RealityObjectStructuralElement'),
            method: 'POST',
            timeout: 5 * 60 * 1000,
            params: {
                robjId: me.getContextValue(me.getMainView(), 'realityObjectId')
            }
        }).next(function(response) {
            var resp;
            try {
                resp = Ext.JSON.decode(response.responseText);
                lbl.setVisible(!resp.data.IsAdded);
            } catch(e) {
                throw new Error(e);
            }
        }).error(function() {
            throw new Error('Ошибка при запросе');
        });
    },

    showInfoToolTip: function(e, t) {
        var me = this;
        if (!me.infoToolTip) {
            me.infoToolTip = Ext.create('Ext.tip.ToolTip', {
                target: t,
                html: 'Для добавления обязательных конструктивных элементов нажмите' +
                    ' <b>кнопку Добавить</b> и выберите в каждой группе хотя бы один конструктивный элемент'
            });
        }
        me.show();
    },

    hideInfoToolTip: function() {
        var me = this;
        if (!me.infoToolTip) {
            me.hide();
        }
    },

    onTreepanelLoad: function() {

        var cb = Ext.ComponentQuery.query('structelselect toolbar checkbox[name="OnlyRequired"]')[0];

        if (cb) {
            var treepanel = Ext.ComponentQuery.query('structelselect treepanel')[0];
            treepanel.disable();
            treepanel.filter(function(node) {
                return node.get('required') === cb.value;
            }, function() {
                treepanel.enable();
            });
        }
    },

    onChBoxOnlyReqsChanged: function(ch, newVal) {
        var treepanel = ch.up('structelselect').down('treepanel');
        treepanel.disable();
        treepanel.filter(function(node) {
            return node.get('required') === newVal;
        }, function() {
            treepanel.enable();
        });
    },

    onItemDblClickHistoryGrid: function (view, record) {
        var me = this,
            detailWindow = Ext.ComponentQuery.query('rostructelhistorydetwindow')[0];

        if (detailWindow) {
            detailWindow.show();
        } else {
            detailWindow = Ext.create('B4.view.realityobj.structelement.HistoryDetailWindow',
            {
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy',
                ctxKey: me.getCurrentContextKey()
            });
            detailWindow.show();
        }

        detailWindow.down('rostructelhistorydetgrid').getStore().filter('logEntityId', record.get('Id'));
    },

    editHistoryGridBtnClick: function (gridView, rowIndex, colIndex, el, e, rec) {
        this.onItemDblClickHistoryGrid(gridView, rec);
    }
});