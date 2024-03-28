Ext.define('B4.view.SelectFieldCommonWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.selectfieldcommonwindow',
    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Выбор элемента',
    selectionMode: 'SINGLE',
    closable: true,
    modal: true,

    store: null,
    columns: null,

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.addEvents('acceptform');
    },

    initComponent: function () {
        var me = this,
            selModel = Ext.create('Ext.selection.CheckboxModel', {
                mode: me.selectionMode,
                checkOnly: me.selectionMode === 'MULTI',
                multiPageSelection: {},
                getSelected: function () {
                    return this.multiPageSelection;
                },
                listeners: {
                    selectionchange: function (selectionModel, selectedRecords) {
                        if (selectedRecords.length === 0 && this.store.loading === true && this.store.currentPage !== this.page) {
                            return;
                        }

                        if (this.store.loading === true) {
                            this.multiPageSelection = {};
                            return;
                        }

                        this.store.data.each(function (i) {
                            Ext.Object.each(this.getSelected(), function (property, value) {
                                if (i.id === value.id) {
                                    delete this.multiPageSelection[property];
                                }
                            }, this);
                        }, this);

                        if (me.selectionMode.toUpperCase() === 'SINGLE') {
                            Ext.each(selectedRecords, function (i) {
                                this.multiPageSelection[0] = i;
                            }, this);
                        } else {
                            Ext.each(selectedRecords, function (i) {
                                if (!Ext.Object.getKey(this.multiPageSelection, i))
                                    this.multiPageSelection[Ext.Object.getSize(this.multiPageSelection)] = i;
                            }, this);
                        }
                    },
                    buffer: 5
                }
            });
        
        me.gridView = Ext.widget(
            {
                xtype: 'gridpanel',
                title: null,
                border: false,
                columnLines: true,
                columns: me.columns,
                store: me.store,
                selModel: selModel,
                plugins: [ Ext.create('B4.ux.grid.plugin.HeaderFilters') ],
                dockedItems: [
                    {
                        xtype: 'b4pagingtoolbar',
                        displayInfo: true,
                        store: me.store,
                        dock: 'bottom'
                    }
                ]
            }
        );

        Ext.applyIf(me, {
            height: 500,
            width: 600,
            constrain: true,
            layout: 'fit',
            items: [ me.gridView ],
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
                                    text: 'Выбрать',
                                    iconCls: 'icon-accept',
                                    handler: me.onSelectValue,
                                    scope: me
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Закрыть',
                                    iconCls: "icon-decline",
                                    handler: me.selectWindowClose,
                                    scope: me
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    /**
     * Обработка события выбора значения
     */
    onSelectValue: function () {
        var me = this,
            rec = me.gridView.getSelectionModel().getSelected();

        if (!rec || rec.length === 0) {
            Ext.Msg.alert('Ошибка', 'Необходимо выбрать запись!');
            return;
        }

        if (me.selectionMode.toUpperCase() === 'SINGLE') {
            rec = rec[0];
            if (Ext.isEmpty(rec)) {
                Ext.Msg.alert('Ошибка', 'Необходимо выбрать запись!');
                return;
            }
            me.fireEvent('acceptform', me, rec.getData());
        }
        else {
            var data = [];
            for (var i in rec) {
                data.push(rec[i].getData());
            }
            me.fireEvent('acceptform', me, data);
        }
    },

    /**
     * Обработка события закрытия окна выбора
     */
    selectWindowClose: function () {
        delete this.gridView;
        delete this.store;
        this.close();
    }
});