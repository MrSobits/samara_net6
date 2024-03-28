Ext.define('B4.view.dict.municipalitytree.UnionMoWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.unionMoWindow',
    mixins: ['B4.mixins.window.ModalMask'],
    width: 600,
    height: 160,
    bodyPadding: 5,
    closeAction: 'destroy',
    trackResetOnLoad: true,
    title: 'Объединение МО',
    layout: { type: 'vbox', align: 'stretch' },
    closable: false,

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.form.SelectField',
        'B4.form.TreeSelectField',
        'B4.store.dict.MunicipalityTree'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right',
                        labelWidth: 120
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'CopyFrom',
                            editable: false,
                            fieldLabel: 'Скопировать с',
                            store: 'B4.store.dict.municipality.MoArea',
                            textProperty: 'Name',
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            maxLength: 300
                        },
                        {
                            xtype: 'treeselectfield',
                            name: 'UnionMos',
                            fieldLabel: 'МО для объединения',
                            titleWindow: 'Выбор МО',
                            store: 'B4.store.dict.MunicipalitySelectTree',
                            editable: false,
                            disabled: true,
                            allowBlank: true,
                            onTrigger1Click: function () {
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
                                            itemappend: { fn: me.onNodeAppend, scope: me },
                                            checkchange: function (node, checked) {
                                                me.checkChildren(node, checked);
                                                me.walkThroughParents(node);
                                            }
                                        },
                                        scope: me
                                    }
                                    );
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
                                                enableKeyEvents: true,
                                                listeners: {
                                                    scope: me,
                                                    specialkey: function (f, e) {
                                                        if (e.getKey() == e.ENTER) {
                                                            var val = me.selectWindow.down('textfield[name="tfSearch"]').getValue(),
                                                                treeStore = me.treePanel.store;

                                                            treeStore.load();
                                                        }
                                                    }
                                                }
                                            },
                                            {
                                                xtype: 'b4updatebutton',
                                                tooltip: 'Обновить',
                                                handler: function () {
                                                    var val = me.selectWindow.down('textfield[name="tfSearch"]').getValue(),
                                                        treeStore = me.treePanel.store;

                                                    treeStore.load({
                                                        params: {
                                                            search: val
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
                                            },
                                            scope: me
                                        }
                                    });

                                    me.store.sorters.clear();
                                }

                                me.selectWindow.show();
                            },

                            setValue: function (data) {

                                var me = this,
                                    oldValue = me.getValue(),
                                    isValid = me.getErrors() != '';

                                if (me.store && data) {
                                    var array = data.split(',');

                                    if (array.length > 0) {

                                        if (typeof me.store != 'object') {
                                            me.store = Ext.create(me.store);
                                            me.store.load();
                                        }

                                        var text = me.store.getById(array[0]);

                                        for (var i = 1; i < array.length; i++) {
                                            text += ',' + me.store.getById(array[i]).raw.text;
                                        }

                                        data = text;
                                    }
                                }

                                me.value = data;
                                me.updateDisplayedText(data);

                                me.fireEvent('validitychange', me, isValid);
                                me.fireEvent('change', me, data, oldValue);
                                me.validate();
                                return me;
                            },

                            onNodeAppend: function (asp, node) {

                                var me = this;
                                if (me.value) {

                                    var valStr = me.value.toString();
                                    var vals = valStr.replace(/ /g, '').split(',');
                                    var id = node.raw.id.toString();

                                    if (vals.indexOf(id) >= 0) {
                                        node.set('checked', true);
                                    }
                                }

                            },

                            onSelectItem: function () {

                                var me = this,
                                    tree = me.treePanel,
                                    selection = tree.getSelectionModel();

                                var checkedNodes = tree.getChecked();

                                if (checkedNodes.length > 0) {
                                    if (!tree.getSelectionModel().getSelection()[0]) {
                                        return;
                                    }

                                    checkedNodes = Ext.Array.filter(checkedNodes, function (node) {
                                        return node.get('id') !== 'root';
                                    });

                                    var ids = checkedNodes[0].raw.id;
                                    var text = checkedNodes[0].raw.Name;

                                    for (var i = 1; i < checkedNodes.length; i++) {

                                        ids += ',' + checkedNodes[i].raw.id;
                                        text += ',' + checkedNodes[i].raw.text;
                                    }

                                    me.value = ids;
                                    me.setRawValue.call(me, text);
                                }

                                me.onSelectWindowClose();
                            }
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    action: 'SaveBtn',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: {
                                            fn: function (btn) {
                                                btn.up('window').close();
                                            }
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
