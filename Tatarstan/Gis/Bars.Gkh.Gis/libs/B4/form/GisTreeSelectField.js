/*

    Поле, с возможностью выбора значения из формы, представленного в виде дерева элементов

*/

Ext.define('B4.form.GisTreeSelectField', {
    extend: 'Ext.form.field.Trigger',

    alias: 'widget.gistreeselectfield',

    trigger1Cls: 'x-form-search-trigger',
    trigger2Cls: 'x-form-clear-trigger',

    requires: [
    'B4.ux.button.Save',
    'B4.ux.button.Close',
    'B4.ux.button.Update'
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
            store.on('beforeload', me.onStoreBeforeLoad, me);            
        }
        me.store = store;


        if (!me.treePanel) {
            me.treePanel = Ext.create('Ext.tree.Panel', {
                region: 'west',
                rootVisible: me.rootVisible,
                animate: me.animate,
                autoScroll: me.autoScroll,
                useArrows: me.useArrows,
                containerScroll: me.containerScroll,
                loadMask: me.loadMask,
                treetype: 'parttree',
                viewConfig: {
                    loadMask: true
                },
                store: me.store,
                listeners: {
                    itemdblclick: me.onSelectItem,
                    checkchange: me.onCheckChange,
                    itemappend: me.onNodeAppend,
                    scope: me
                }
            });
            //подгрузка данных
            me.treePanel.getStore().load();
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
                        xtype: 'textfield',
                        name: 'tfSearch',
                        tooltip: 'Найти элемент',
                        emptyText: 'Поиск',
                        enableKeyEvents: true
                    },
                    {
                        xtype: 'b4updatebutton',
                        tooltip: 'Обновить',
                        handler: function () {
                            me.treePanel.store.load({
                                params: {
                                    workName: me.selectWindow.down('textfield[name="tfSearch"]').getValue()
                                }
                            });
                        },
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

    updateDisplayedText: function(data) {
        var me = this,
            text = '';
       
        if (data != null && me.store.getRootNode) {
            text = me.createDisplayedText(me.store.getRootNode());
            if (text != '') {
                text = text.substring(0, text.length - 1);
            }
        } else if (data != null) {
            Ext.each(data, function (i) {
                if (i && i.text) {
                    if (text !== '') {
                        text += ', ';
                    }
                    text += i.text;
                }
            });
        }

        me.setRawValue.call(me, text);
    },
    
    //отображаемые названия со скобками
    createDisplayedText: function (node) {
        var me = this,
            childrenNames = '',
            text = '';

        Ext.each(node.childNodes, function (ch) {
            if (ch.isLeaf()) {
                if (ch.data.checked) {
                    childrenNames += (ch.data[me.textProperty] || '') + ',';
                }
            } else {
                var temp = me.createDisplayedText(ch);
                text += temp != '' ? temp + ',' : '';
            }
        });
        
        if (childrenNames != '') {
            childrenNames = childrenNames.substring(0, childrenNames.length - 1);
            text += node.data[me.textProperty] + '(' + childrenNames + ')';
        }

        return text;
    },

    /**
    * Устанавливаем значение поля. 
    * @param {Object} data Новое значение
    * @return {B4.form.SelectField} this
    */
    setValue: function (data) {
        var me = this,
            oldValue = me.getValue(),
            isValid = me.getErrors() != '',
            text = '';

        // Если пришел объект с вьюмодели
        Ext.each(data, function (i) {
            if (i && i.Name && i.Id) {
                i[me.textProperty] = i.Name;
                i[me.idProperty] = i.Id;
            }
        });
        
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
        } else if(me.value) {
            return me.isGetOnlyIdProperty ? me.value[me.idProperty] : me.value;
        }

        return me.value;
    },

    //запоминание выбранных узлов
    onSelectItem: function () {
        var me = this,
            tree = me.treePanel,
            //selection = tree.getSelectionModel(),
            checkedNodes = tree.getChecked(),
            data = [];            

        Ext.each(checkedNodes, function (i) {
            data.push(i.getData());            
        });        
        
        me.setValue(data);
        //me.setRawValue.call(me, text);

        //if (selection) {
        //    if (!tree.getSelectionModel().getSelection()) {
        //        return;
        //    }
        //    var data = tree.getSelectionModel().getSelection()[0].data;
        //    if (!data.leaf) {
        //        return;
        //    }            
        //    for (var i = 0; i < checkedNodes.length; i++) {
        //        checkedNodes[i].set("checked", false);
        //    }

        //    me.setValue(data);
        //}
        me.onSelectWindowClose();
    },

    onSearchWork: function (t, e) {
        var me = this,
            value = me.selectWindow.down('textfield[name="tfSearch"]').getValue();

        //todo убрать хард код
        if (e.keyCode == 13) {
            me.treePanel.getStore().load({
                params: {
                    workName: value
                }
            });
        }
    },
        
    //добавление узла - отмечаем ранее выбранные дочерние узлы
    onNodeAppend: function (asp, node) {
        var me = this,
            nodeIdValue = node.getData()[me.idProperty];
        if (me.value) {
            var idList = Ext.Array.map(me.value, function(i) {
                return i[me.idProperty];
            });

            if (idList.indexOf(nodeIdValue) >= 0) {
                node.set('checked', true);
            }
        }
    },
    
    //событие чека узла
    onCheckChange: function(node, checked) {
        var me = this;
        if (me.multiSelect) {
            me.checkChildren(node, checked);
            me.walkThroughParents(node);
        } else {
            Ext.each(me.treePanel.getChecked(), function(i) {
                if (checked && i != node) {
                    i.set("checked", false);
                }
            });
        }
    }
});