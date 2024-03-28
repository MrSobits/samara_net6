Ext.define('B4.view.regop.personal_account.action.MergeAccount', {
    extend: 'B4.view.regop.personal_account.action.BaseAccountWindow',

    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.ux.grid.column.AreaShare',
        'B4.form.field.AreaShareField',
        'B4.store.regop.personal_account.InMemoryRoomAccountStore',
        'B4.ux.button.Close'
    ],

    alias: 'widget.mergeaccountwin',

    modal: true,
    store: null,

    width: 450,
    minHeight: 300,
    title: 'Слияние',
    bodyPadding: 0,
    layout: 'fit',
    initComponent: function() {
        var me = this;
        Ext.apply(me, {
            items: [
            {
                xtype: 'form',
                unstyled: true,
                border: false,
                layout: { type: 'vbox', align: 'stretch' },
                defaults: {
                    labelWidth: 150
                },
                items: [
                    {
                        xtype: 'hidden',
                        name: 'MergeInfos'
                    },
                    {
                        xtype: 'textfield',
                        name: 'Reason',
                        fieldLabel: 'Причина',
                        allowBlank: true,
                        margin: '2px'
                    },
                    {
                        xtype: 'b4filefield',
                        name: 'Document',
                        fieldLabel: 'Документ-основание',
                        margin: '2px'
                    },
                    {
                        xtype: 'gridpanel',
                        border: false,
                        name: 'mergegrid',
                        store: Ext.create('B4.store.regop.personal_account.InMemoryRoomAccountStore'),
                        columns: [
                            {
                                text: 'Лицевые счета',
                                dataIndex: 'PersonalAccountNum',
                                flex: 1
                            },
                            {
                                text: 'Текущая доля собственности',
                                dataIndex: 'AreaShare',
                                xtype: 'areasharecolumn',
                                flex: 1
                            },
                            {
                                text: 'Новая доля собственности',
                                dataIndex: 'NewShare',
                                xtype: 'areasharecolumn',
                                flex: 1,
                                editor: {
                                    xtype: 'areasharefield',
                                    allowBlank: false
                                }
                            }
                        ],
                        plugins: [
                            Ext.create('Ext.grid.plugin.CellEditing', {
                                clicksToEdit: 1
                            })
                        ]
                    }
                ]
            }]
        });
        me.callParent(arguments);

        me.down('b4savebutton').setText('Применить');
    }
});