/* Переопределяем системную панель главного меню, чтобы отображать скролл, если список меню не помещается по высоте на экран */
Ext.define('B4.view.desktop.menu.Main', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.mainmenu',

    requires: [
        'B4.store.MenuStore',
        'B4.view.desktop.menu.FirstLevelItem',
        'B4.view.desktop.menu.FirstLevel',
        'B4.view.desktop.menu.Right'
    ],

    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    /**
    * @cfg {B4.store.MenuItemStore/String} store
    * Store, по которому будет строиться меню
    */

    /**
     * @private
     * @cfg {Ext.panel.Panel} rightPanel
     * Панель, содержащая иконки подменю, должен иметь cardLayout
     */

    /**
     * @private
     * @cfg {Ext.panel.Panel} leftPanel
     * Панель, содержащая верхний уровень меню
     */

    /**
     * Свойство, по которому отслеживается построено ли меню.
     * @private
     */
    isBuilt: false,


    initComponent: function () {
        var me = this,
            store = me.store,
            rightPanel = me.rightPanel = Ext.create('Ext.panel.Panel', {
                flex: 2,
                ui: 'b4accardion',
                layout: {
                    type: 'card',
                    align: 'stretch'
                }
            }),
            leftPanel = me.leftPanel = {
                xtype: 'panel',
                layout: 'vbox',
                autoScroll: true,
                ui: 'b4-lt-wrap',
                minWidth: 280,
                items: [{
                    xtype: 'menufirstlevel',
                    cardPanel: rightPanel,
                    minWidth: 250
                }],
                dockedItems: [{
                    dock: 'bottom',
                    xtype: 'container',
                    cls: 'b4desktop-exit-btn-cnt',
                    height: 105,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'button',
                            itemId: 'logoutBt',
                            ui: 'b4logout',
                            html: '<span class="b4logout" style="margin-right: 6px;"></span><span style="position: relative; top: 1px;">Выйти из системы</span>',
                            margin: '10px 50px 20px 50px'
                        },
                        {
                            xtype: 'combo',
                            ui: 'b4search',
                            store: 'SearchIndex',
                            typeAhead: false,
                            hideLabel: true,
                            hideTrigger: true,
                            width: 274,
                            placeHolder: 'Поиск',
                            queryMode: 'local',
                            listConfig: {
                                itemId:'searchList',
                                loadingText: 'Поиск...',
                                emptyText: '<div class="b4search-item-inner"><span class="text-wrap"><span class="title">Нет совпадений</span></span></div>',
                                ui: 'b4searchItem',
                                height: 'auto',
                                maxHeight: 550,

                                getInnerTpl: function () {
                                    return '<a href="javascript:void(0);" class="b4search-item-wrap">' +
                                                '<div class="b4search-item-inner">' +
                                                    '<div class="row">' +
                                                        '<div class="i-hold">' +
                                                            '<span class="icon-big-{iconCls}"></span>' +
                                                        '</div>' +
                                                        '<div class="text-wrap">' +
                                                            '<div class="title">{text}</div>' +
                                                            '<div class="text">{parent}</div>' +
                                                        '</div>' +
                                                    '</div>' +
                                                '</div>' +
                                            '</a>';
                                },
                                listeners: {
                                    itemclick: function (me, record, item, index, e, eOpts) {
                                        console.log(me);
                                        me.fireEvent('itemtap', record);
                                    }
                                }
                            }
                        }]
                }]
            };

        // Store routine
        if (Ext.isString(store)) {
            store = me.store = Ext.StoreMgr.lookup(store);
        } else if (!store || Ext.isObject(store) && !store.isStore) {
            store = me.store = new Ext.data.TreeStore(Ext.apply({
                root: me.root,
                fields: me.fields,
                model: me.model,
                folderSort: me.folderSort
            }, store));
        } else if (me.root) {
            store = me.store = Ext.data.StoreManager.lookup(store);
            store.setRootNode(me.root);
            if (me.folderSort !== undefined) {
                store.folderSort = me.folderSort;
                store.sort();
            }
        }
        // Привязываем событие load у store к событию storeload у this.
        // При загрузке this.store автоматически вызовется событие this.storeload.
        // Обработка этого события ниже в блоке listeners
        me.relayEvents(store, ['load'], 'store');

        me.items = [leftPanel, rightPanel];

        me.callParent();

    },

    afterRender: function () {
        this.callParent(arguments);
        if (this.store.isStore) {
            if (this.store.getRootNode().getChildAt(0)) {
                this.build(this.store);
            } else { 
                if (!this.store.isLoading()) {
                    this.store.load();
                }
            }
        }
    },

    listeners: {
        storeload: function (store, node, records, successful, eOpts) {
            this.build(store);
        }
    },

    addToSearchIndex: function (index, item, parent) {
        var me = this;
        if (item.leaf) {
            index.add(Ext.apply(item, {'parent': parent}));
        } else {
            Ext.each(item.children, function (child, ind, children) {
                me.addToSearchIndex(index, child, parent);
            });
        }
    },

    build: function (store) {
        var me = this;
        if (!me.isBuilt) {
            store.getRootNode().eachChild(function (record) {
                var subStore = Ext.create('B4.store.MenuStore', { data: record.data.children });
                var card = { xtype: 'menuright', store: subStore };
                card = me.rightPanel.add(card);
                me.down('menufirstlevel').add(Ext.apply({
                    xtype: 'menufirstlevelitem',
                    topLevel: me.down('menufirstlevel'),
                    card: card
                }, record.raw));
                var searchIndex = Ext.getStore('SearchIndex');
                me.addToSearchIndex(searchIndex, record.raw, record.get('text'));
            });
        }
        me.isBuilt = true;
    }
});