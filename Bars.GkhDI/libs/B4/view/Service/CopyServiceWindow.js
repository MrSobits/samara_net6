Ext.define('B4.view.service.CopyServiceWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 800,
    height: 600,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    title: 'Копирование услуги',
    itemId: 'copyServiceWindow',
    layout: 'fit',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.Control.GkhDecimalField',
        'B4.store.dict.PeriodDi',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.SelectField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4grid',
                    title: 'Дома',
                    store: 'menu.ManagingOrgRealityObjDataMenuServ',
                    itemId: 'realityObjCopyServGrid',
                    columnLines: true,
                    sortableColumns: false,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'AddressName',
                            flex: 1,
                            text: 'Адрес'
                        }
                    ],
                    viewConfig: {
                        loadMask: true
                    },
                    dockedItems: [
                        {
                            xtype: 'toolbar',
                            dock: 'top',
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    columns: 2,
                                    items: [
                                        {
                                            xtype: 'b4addbutton'
                                        },
                                        {
                                            xtype: 'button',
                                            itemId: 'btnCopyServClear',
                                            text: 'Удалить все',
                                            tooltip: 'Удалить все',
                                            iconCls: 'icon-decline'
                                        }
                                    ]
                                }
                            ]
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'button',
                                    itemId: 'copyServiceButton',
                                    text: 'Копировать',
                                    tooltip: 'Копировать',
                                    iconCls: 'icon-add'
                                },
                                {
                                    xtype: 'tbfill'
                                },
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
