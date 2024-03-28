Ext.define('B4.controller.dict.ViolationFeatureGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'dict.ViolationFeatureGji',
        'dict.FeatureViolGji'
    ],

    stores: [
        'dict.ViolationFeatureGji',
        'dict.FeatureViolGji',
        'dict.ViolationGjiForSelect',
        'dict.ViolationGjiForSelected'
    ],

    views: [
        'dict.violationfeaturegji.ViolationFeatureGrid',
        'dict.violationfeaturegji.ViolationGroupsTree',
        'dict.violationfeaturegji.ViolationsGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'violationFeatureGjiGrid'
        },
        {
            ref: 'treeView',
            selector: 'violationgroupstree'
        },
        {
            ref: 'violGrid',
            selector: 'violationsgrid'
        }
    ],

    aspects: [

        {
            // Аспект массового добавления нарушений 
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'violationFeatureGridMultiSelectAspect',
            gridSelector: 'violationFeatureGjiGrid violationsgrid',
            storeName: 'dict.ViolationFeatureGji',
            modelName: 'dict.ViolationFeatureGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#violationFeatureGridMultiSelectWindow',
            storeSelect: 'dict.ViolationGjiForSelect',
            storeSelected: 'dict.ViolationGjiForSelected',
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            columnsGridSelect: [
                { header: 'Пункт НПД', dataIndex: 'NormDocNum', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Текст нарушения', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Текст нарушения', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds.length > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddFeatureViolations', 'ViolationFeatureGji', {
                            featureId: asp.controller.getContextValue('featureId'),
                            violationIds: recordIds
                        })).next(function () {
                            asp.controller.unmask();
                            asp.getGrid().getStore().load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhGji.Dict.ViolationGroup.Create', applyTo: 'b4addbutton', selector: 'violationFeatureGjiGrid' },
                { name: 'GkhGji.Dict.ViolationGroup.Edit', applyTo: 'b4savebutton', selector: '#violationFeatureGridMultiSelectWindow' },
                {
                    name: 'GkhGji.Dict.ViolationGroup.Delete', applyTo: 'b4deletecolumn', selector: 'violationFeatureGjiGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhGji.Dict.ViolationGroup.Violation.Create', applyTo: 'b4addbutton', selector: 'violationsgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhGji.Dict.ViolationGroup.Violation.Delete', applyTo: 'b4deletecolumn', selector: 'violationsgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('violationFeatureGjiGrid'),
            addSubGroupButton = view.down('violationgroupstree b4addbutton [action=addsubgroup]');

        me.bindContext(view);
        me.application.deployView(view);
        me.loadTreeData();

        addSubGroupButton.hide();

        B4.Ajax.request(B4.Url.action('GetParamByKey', 'GjiParams',
            { key: 'ViolationLevelCount' }))
            .next(function (resp) {
                var obj = Ext.decode(resp.responseText);
                me.setContextValue('violLevelCount', obj.data);

                if (obj.data == 1) {
                    addSubGroupButton.hide();
                } else {
                    addSubGroupButton.show();
                }
            });
    },

    init: function () {
        var me = this;

        me.control({
            'violationgroupstree': {
                itemcontextmenu: me.onTreeContextMenu,
                endcelledit: me.goEditSave,
                itemclick: me.onFeatureSelect
            },
            'violationgroupstree b4updatebutton': {
                click: me.loadTreeData
            },
            'violationgroupstree b4addbutton [action=addgroup]': {
                click: me.addGroup
            },
            'violationgroupstree b4addbutton [action=addsubgroup]': {
                click: me.addSubGroup
            },
            'violationgroupstree button[action=delete]': {
                click: me.deleteGroupOrSub
            },
            'violationsgrid': {
                render: {
                    fn: me.onViolGridRender,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    onViolGridRender: function (grid) {
        var me = this;
        grid.getStore().on('beforeload', function (store, operation) {
            operation.params = operation.params || {};
            operation.params.featureId = me.getContextValue('featureId');
        });
    },

    onFeatureSelect: function (view, node) {
        var me = this,
            grid = me.getViolGrid(),
            store = grid.getStore(),
            addButton = grid.down('b4addbutton'),
            violLevelCount = me.getContextValue('violLevelCount');
        grid.setDisabled(false);
        me.setContextValue('featureId', node.get('Id'));

        addButton.setDisabled(violLevelCount == null || (violLevelCount == 2 && node.get('depth') == 1));

        store.load();
    },

    loadTreeData: function (treepanel) {
        var me = this,
            treepanel = me.getTreeView();

        me.mask('Загрузка...', treepanel);
        B4.Ajax.request({
            url: B4.Url.action('GetTree', 'FeatureViolGji')
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText),
                rootNode = treepanel.getRootNode();
            me.unmask(treepanel);
            rootNode.removeAll();
            rootNode.appendChild(json);
        }).error(function () {
            me.unmask(treepanel);
        });
    },

    onTreeContextMenu: function (treepanel, record, na, nb, e) {
        var me = this,
            menu;
        e.preventDefault();

        menu = Ext.create('Ext.menu.Menu', {
            plain: true,
            items: [
                {
                    text: 'Добавить группу',
                    iconCls: 'icon-add',
                    listeners: {
                        click: {
                            fn: me.addGroup,
                            scope: me
                        }
                    }
                },
                {
                    text: 'Добавить подгруппу',
                    iconCls: 'icon-add',
                    listeners: {
                        click: {
                            fn: me.addSubGroup,
                            scope: me
                        }
                    }
                },
                {
                    text: 'Удалить',
                    iconCls: 'icon-cross',
                    listeners: {
                        click: {
                            fn: me.deleteGroupOrSub,
                            scope: me
                        }
                    }
                }
            ]
        });

        menu.showAt(e.xy);
    },

    addGroup: function () {
        var me = this,
            tree = me.getTreeView(),
            root = tree.getRootNode(),
            newnode = root.appendChild({
                Children: [],
                Name: "Новая группа",
                IsActual: true
            });

        tree.getView().focusRow(newnode);
        tree.getSelectionModel().select(newnode);
        me.goSaveChanges(newnode, 'create');
    },

    addSubGroup: function () {
        var me = this,
            tree = me.getTreeView(),
            node = tree.getSelectionModel().getLastFocused(),
            newnode, newrecord;

        if (!node) {
            B4.QuickMsg.msg('Информация', 'Сначала нужно выбрать группу', 'info');
            return false;
        }

        if (node.get('parentId') == "root") {
            newnode = {
                leaf: true,
                Parent: node.get('Id'),
                Name: "Новая подгруппа"
            };

            newrecord = node.appendChild(newnode);
            tree.getView().focusRow(newrecord);
            tree.getSelectionModel().select(newrecord);
            node.expand();
            me.goSaveChanges(newrecord, 'create', node.get('Id'));
        } else {
            B4.QuickMsg.msg('Информация', 'Подргуппу можно добавлять только для группы', 'info');
        }
    },

    deleteGroupOrSub: function () {
        var me = this,
            tree = me.getTreeView(),
            node = tree.getSelectionModel().getLastFocused(),
            callback = function() {
                node.remove(true);
                B4.QuickMsg.msg('Удаление', 'Запись удалена', 'success');
            };

        if (!node.childNodes.length) {
            me.goSaveChanges(node, 'delete', null, callback);

        } else {
            B4.QuickMsg.msg('Информация', 'Нельзя удалять группу, если у нее имеется подгруппы', 'info');
        }
    },

    goEditSave: function (record, action) {
        var me = this,
            callback = function () {
                B4.QuickMsg.msg('Сохранение', 'Запись сохранена', 'success');
            };

        me.goSaveChanges(record, action, record.get('Parent') || 0, callback);
    },
    goSaveChanges: function (record, action, parentId, callback) {
        var me = this,
            records = [];

        if (action === 'delete') {
            records = [record.get('Id')];
        } else {
            records = [{ Id: record.get('Id') || 0,
                Name: record.get('Name'),
                Code: record.get('Code'),
                Parent: record.get('Parent') || parentId || 0,
                IsActual: record.get('IsActual') }];
        }

        B4.Ajax.request({
            url: B4.Url.action(action, 'FeatureViolGji'),
            params: {
                records: Ext.JSON.encode(records)
            }
        }).next(function (resp) {
            var data = Ext.JSON.decode(resp.responseText).data[0];
            record.set('Id', data.Id);
            record.set('Parent', data.Parent ? data.Parent.Id : 0);

            if (callback && Ext.isFunction(callback)) {
                callback.call();
            }

        }).error(function (e) {
            Ext.Msg.alert('Ошибка удаления!', e.message || e);
        });
    }

});