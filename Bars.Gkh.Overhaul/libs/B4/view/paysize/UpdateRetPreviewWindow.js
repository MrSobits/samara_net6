Ext.define('B4.view.paysize.UpdateRetPreviewWindow', {
    extend: 'Ext.window.Window',

    alias: 'widget.updateretpreviewwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.view.paysize.UpdateRetPreviewGrid'
    ],

    modal: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 800,
    minWidth: 700,
    maxWidth: 900,
    minHeight: 665,
    maxHeight: 665,
    bodyPadding: 5,
    title: 'Перечень обновляемых домов',
    closeAction: 'destroy',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'updateretpreviewgrid',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Подтвердить обновление',
                                    action: 'UpdateRoTypes'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        'click': function () {
                                            me.close();
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