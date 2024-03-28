/*

    Поле, с возможностью выбора значения из формы, представленного в виде дерева элементов

*/

Ext.define('B4.form.TreeSelectField', {
    extend: 'Ext.form.field.Trigger',

    alias: 'widget.treeselectfield',

    trigger1Cls: 'x-form-search-trigger',
    trigger2Cls: 'x-form-clear-trigger',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',

        'B4.form.SearchField'
    ],

    /**
    * @cfg {Boolean} allowBlank
    * Флаг: разрешено ли пустое значение поля
    */
    allowBlank: false,

    fieldSelector: null,
    multiSelect: false,
    store: null,

    idProperty: 'id',
    textProperty: 'text',

    windowSelector: null,
    titleWindow: null,
    windowWidth: 550,
    windowHeight: 400,

    editable: false,
    rootVisible: false,
    animate: true,
    autoScroll: true,
    useArrows: true,
    containerScroll: true,
    loadMask: true,

    columns: null,

    /**
    * @cfg {Bool} isGetOnlyIdProperty return only idProperty value
    */
    isGetOnlyIdProperty: true,

    initComponent: function () {
        this.callParent(arguments);
    },

    onStoreBeforeLoad: function (store, operation) {
        var me = this, options = {};
        me.fireEvent('beforeload', me, options, store);
        Ext.apply(operation, options);
    },

    getSelectField: function () {
        return this;
    },

    onTrigger1Click: function () {
        // Создаем необходимые контролы - окно и панель с деревом в нем
        var me = this,
            store = me.store;

        if (!store) {
            store = Ext.StoreMgr.lookup('ext-empty-store');
        } else if (Ext.isString(store)) {
            store = Ext.StoreMgr.lookup(store);
            if (!store) {
                store = me.store = Ext.create(me.store);
            }
            me.store = store;
        } else {
            me.store.load();
        }

        if (!Ext.isEmpty(store) && Ext.isFunction(store.on)) {
            store.on('beforeload', me.onStoreBeforeLoad, me);
        }

        if (!me.treePanel) {
            if (me.multiSelect) {
                me.treePanel = Ext.create('Ext.tree.Panel', {
                    region: 'west',
                    rootVisible: me.rootVisible,
                    animate: me.animate,
                    autoScroll: me.autoScroll,
                    useArrows: me.useArrows,
                    containerScroll: me.containerScroll,
                    loadMask: me.loadMask,
                    treetype: 'parttree',
                    columns: me.columns,
                    viewConfig: {
                        loadMask: true
                    },
                    store: me.store,
                    listeners: {
                        itemdblclick: me.onSelectItem,
                        checkchange: function (node, checked) {
                            me.checkChildren(node, checked);
                            me.walkThroughParents(node);
                        },
                        scope: me
                    }
                });
            } else {
                me.treePanel = Ext.create('Ext.tree.Panel', {
                    region: 'west',
                    rootVisible: me.rootVisible,
                    animate: me.animate,
                    autoScroll: me.autoScroll,
                    useArrows: me.useArrows,
                    containerScroll: me.containerScroll,
                    loadMask: me.loadMask,
                    columns: me.columns,
                    treetype: 'parttree',
                    viewConfig: {
                        loadMask: true
                    },
                    store: me.store,
                    listeners: {
                        itemdblclick: me.onSelectItem,
                        checkchange: function (node, checked) {
                            var tree = me.treePanel;

                            if (checked) {
                                var checkedNodes = tree.getChecked();

                                for (var i = 0; i < checkedNodes.length; i++) {
                                    checkedNodes[i].set('checked', false);
                                }

                                node.set('checked', true);
                            }
                        },
                        scope: me
                    }
                });
            }

        }

        if (!me.selectWindow) {
            me.selectWindow = Ext.create('Ext.window.Window', {
                layout: 'fit',
                width: me.windowWidth,
                height: me.windowHeight,
                title: me.titleWindow,
                constrain: true,
                modal: false,
                closeAction: 'destroy',
                renderTo: B4.getBody().getActiveTab().getEl(),
                items: [
                    me.treePanel
                ],
                tbar: [
                    {
                        flex: 1,
                        xtype: 'b4searchfield',
                        name: 'tfSearch',
                        tooltip: 'Найти элемент',
                        emptyText: 'Поиск',
                        enableKeyEvents: true
                    },
                    {
                        xtype: 'b4updatebutton',
                        tooltip: 'Обновить',
                        handler: me.onUpdateClick,
                        scope: me
                    },
                    {
                        xtype: 'b4savebutton',
                        tooltip: 'Выбрать',
                        text: 'Выбрать',
                        handler: me.onSelectItem,
                        scope: me
                    },
                    {
                        xtype: 'b4closebutton',
                        text: 'Закрыть',
                        tooltip: 'Закрыть',
                        handler: me.onSelectWindowClose,
                        scope: me
                    }
                ],
                listeners: {
                    close: function () {
                        delete me.treePanel;
                        delete me.selectWindow;
                    }
                }
            });

            me.store.sorters.clear();
        }

        var tfSearch = me.selectWindow.down('textfield[name="tfSearch"]');

        tfSearch.on('keypress', me.onSearchWork, me);
        tfSearch.on('onclear', me.clearFilter, me);


        me.fireEvent('beforeshowselectwindow', me, me.selectWindow);
        me.selectWindow.show();
    },

    checkChildren: function (node, check) {
        node.cascadeBy(function (n) {
            n.set('checked', check);
        });
    },

    walkThroughParents: function () {
        this.checkParentNodes();
    },

    checkParentNodes: function (beginWith) {
        var me = this,
            tree = me.treePanel,
            rootNode = beginWith || tree.getRootNode();

        var fn = function (node) {
            if (me.allChildrenAreChecked(node)) {
                node.set('checked', true);
            }
            else {
                if (node.isLeaf()) {
                    node.set('checked', node.data.checked === true);
                }
                else {
                    node.set('checked', false);
                }
            }
            for (var i = 0; i < node.childNodes.length; ++i) {
                fn(node.childNodes[i]);
            }
        };
        fn(rootNode);
    },

    allChildrenAreChecked: function (node) {
        var allChildrenAreChecked = false;
        var fn = function (child) {
            allChildrenAreChecked = child.data.checked === true;

            if (child.isLeaf()) {
                return allChildrenAreChecked;
            }

            for (var i = 0; i < child.childNodes.length; ++i) {
                if (fn(child.childNodes[i]) === false) {
                    return false;
                }
            }
        };

        fn(node);

        return allChildrenAreChecked;
    },

    /**
    * Очищаем поле
    */
    onTrigger2Click: function () {
        var me = this;
        me.setValue(null);
        me.updateDisplayedText();
    },

    onSelectWindowClose: function () {
        delete this.treePanel;
        this.selectWindow.close();
        delete this.selectWindow;
    },

    destroy: function () {
        var me = this;
        if (me.store && Ext.isObject(me.store)) {
            me.store.un('beforeload', me.onStoreBeforeLoad);
        }

        if (me.selectWindow) {
            me.selectWindow.destroy();
        }

        me.callParent(arguments);
    },

    onCloseWindowHandler: function () {
        var form = this.getForm();
        if (form) {
            form.close();
        }
    },

    updateDisplayedText: function (data) {
        var me = this,
            text;

        if (Ext.isString(data)) {
            text = data;
        }
        else {
            text = data && data[me.textProperty] ? data[me.textProperty] : '';
            if (Ext.isEmpty(text) && Ext.isArray(data)) {
                text = Ext.Array.map(data, function (record) { return record[me.textProperty]; }).join();
            }
        }

        me.setRawValue.call(me, text);
    },

    /**
    * Устанавливаем значение поля. 
    * @param {Object} data Новое значение
    * @return {B4.form.SelectField} this
    */
    setValue: function (data) {
        var me = this,
            oldValue = me.getValue(),
            isValid = me.getErrors() != '';

        // Если пришел объект с вьюмодели
        if (data && data.Name && data.Id) {
            data[me.textProperty] = data.Name;
            data[me.idProperty] = data.Id;
        }
        me.value = data;
        me.updateDisplayedText(data);

        me.fireEvent('validitychange', me, isValid);
        me.fireEvent('change', me, data, oldValue);
        me.validate();
        return me;
    },

    /**
    * Возвращает значение поля. 
    * @return {Object} this.value
    */
    getValue: function () {
        var me = this;

        if (Ext.isObject(me.value)) {
            return me.isGetOnlyIdProperty ? me.value[me.idProperty] : me.value;
        }

        if (Ext.isArray(me.value)) {
            return Ext.Array.map(me.value, function (data) { return me.isGetOnlyIdProperty ? data[me.idProperty] : data; });
        }

        return me.value;
    },

    onUpdateClick: function () {
        var me = this;
        me.treePanel.store.load();
    },

    onSelectItem: function () {
        debugger;
        var me = this,
            tree = me.treePanel,
            selection = tree.getSelectionModel();

        if (selection) {
            if (!tree.getSelectionModel().getSelection()) {
                return;
            }
            var data = tree.getSelectionModel().getSelection()[0].data;
            if (!data.leaf) {
                return;
            }

            var checkedNodes = tree.getChecked();
            for (var i = 0; i < checkedNodes.length; i++) {
                checkedNodes[i].set('checked', false);
            }

            me.setValue(data);
        }

        me.onSelectWindowClose();
    },

    onSearchWork: function (t, e) {
        var me = this,
            value = me.selectWindow.down('textfield[name="tfSearch"]').getValue();

        if (e.keyCode == 13) {
            if (value) {
                me.treePanel.getStore().load({
                    params: {
                        workName: value
                    }
                });
            }
        }
    },

    clearFilter: function() {
        this.treePanel.getStore().load();
    }
});