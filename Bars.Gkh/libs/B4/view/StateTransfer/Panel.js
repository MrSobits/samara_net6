Ext.define('B4.view.StateTransfer.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    layout: 'fit',
    title: 'Переходы статусов',
    itemId: 'fiasTransferPanel',

    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Save',
        'B4.store.Role'
    ],

    initComponent: function() {
        var me = this,
            roleStore = Ext.create('B4.store.Role');

        roleStore.load();

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'border',
                    items:
                    [
                        {
                            xtype: 'panel',
                            region: 'north',
                            layout: 'fit',
                            title: 'Выберите сущность и роль для редактирования последовательности статусов',
                            split: false,
                            bodyStyle: Gkh.bodyStyle,
                            collapsible: false,
                            items: [
                                {
                                    xtype: 'container',
                                    defaults: {
                                        padding: '5 5 0 5',
                                        width: 500
                                    },
                                    anchor: '100%',

                                    layout: {
                                        pack: 'start',
                                        type: 'vbox'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4combobox',
                                            itemId: 'cbRole',
                                            labelAlign: 'right',
                                            flex: 1,
                                            editable: false,
                                            fieldLabel: 'Роль',
                                            store: roleStore,
                                            pageSize: 30,
                                            triggerAction: 'all'
                                        },
                                        {
                                            xtype: 'b4combobox',
                                            itemId: 'cbType',
                                            labelAlign: 'right',
                                            editable: false,
                                            flex: 1,
                                            fieldLabel: 'Тип объекта',
                                            fields: ['TypeId', 'Name'],
                                            valueField: 'TypeId',
                                            displayField: 'Name',
                                            queryMode: 'local',
                                            triggerAction: 'all',
                                            store: {
                                                fields: [
                                                    { name: 'TypeId', useNull: false },
                                                    { name: 'Name' }
                                                ]
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
                                            items: [
                                                { xtype: 'b4savebutton', itemId: 'saveBtn' }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            region: 'center',
                            layout: 'fit',
                            items: [
                                Ext.create('B4.view.StateTransfer.Grid')
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});