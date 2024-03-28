Ext.define('B4.view.SelectWindow.SingleSelectWindow', {
    extend: 'Ext.window.Window',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.ux.grid.Panel',
        'B4.ux.grid.selection.CheckboxModel',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    itemId: 'singleSelectWindow',
    closeAction: 'hide',
    height: 500,
    width: 900,
    layout: 'fit',
    mixins: ['B4.mixins.window.ModalMask'],
    maximizable: true,
    trackResetOnLoad: true,
    title: 'Выбор элементов',
    titleGridSelect: 'Элементы для выбора',

    storeSelect: null,
    columnsGridSelect: [],

    //#region extending
    leftGridConfig: {},
    leftTopToolbarConfig: {},
    toolbarItems: [],
    isPaginable: true,
    //#endregion

    initComponent: function () {
        var me = this,
            isExistDelColumn = false;

        if (!me.leftTopToolbarConfig) {
            me.leftTopToolbarConfig = {
                xtype: 'toolbar',
                dock: 'top',
                items: [
                    {
                        xtype: 'buttongroup',
                        columns: 2,
                        items: [
                            {
                                xtype: 'b4updatebutton'
                            }
                        ]
                    }
                ]
            };
        }

        var dockedItems = [me.leftTopToolbarConfig];
        if (me.isPaginable) {
            dockedItems.push(
            {
                xtype: 'b4pagingtoolbar',
                displayInfo: true,
                store: me.storeSelect,
                dock: 'bottom'
            });
        }

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    border: false,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        Ext.merge({
                            xtype: 'b4grid',
                            flex: 1,
                            allowDeselect: true,
                            rowLines: false,
                            itemId: 'multiSelectGrid',
                            title: me.titleGridSelect,
                            style: 'border-right: solid 1px #99bce8;',
                            border: false,
                            // необходимо для того чтобы неработали восстановления для грида посколкьу колонки показываются и скрываются динамически
                            provideStateId: Ext.emptyFn,
                            stateful: false,
                            store: me.storeSelect,
                            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {
                                mode: 'SINGLE',
                                checkOnly: true,
                                ignoreRightMouseSelection: true,
                            }),
                            columns: me.columnsGridSelect,
                            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                            viewConfig: {
                                loadMask: true
                            },
                            dockedItems: dockedItems
                        }, me.leftGridConfig),
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
                            columns: 1 + me.toolbarItems.length,
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    text: 'Применить'
                                }
                            ].concat(me.toolbarItems)
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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