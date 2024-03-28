Ext.define('B4.view.StateTransfer.RuleEditWindow', {
    extend: 'B4.form.Window',

    layout: 'form',
    width: 600,
    bodyPadding: 5,
    itemId: 'stateTransferRuleEditWindow',
    title: 'Правило перехода статуса',
    trackResetOnLoad: true,
    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.Role',
        'B4.store.StatefulEntity',
        'B4.store.FiltredTransfer',
        'B4.store.Rule',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: { labelWidth: 100, labelAlign: 'right', anchor: '100%' },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Role',
                    itemId: 'stateTransferRole',
                    fieldLabel: 'Роль',
                    store: 'B4.store.Role',
                    allowBlank: false,
                    editable: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Object',
                    itemId: 'stateTransferObject',
                    fieldLabel: 'Тип объекта',
                    idProperty: 'TypeId',
                    store: 'B4.store.StatefulEntity',
                    allowBlank: false,
                    editable: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'StateTransfer',
                    itemId: 'stateTransfer',
                    fieldLabel: 'Переход',
                    store: 'B4.store.FiltredTransfer',
                    allowBlank: false,
                    editable: false,
                    disabled: true,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'RuleId',
                    itemId: 'stateTransferRule',
                    fieldLabel: 'Правило',
                    store: 'B4.store.Rule',
                    allowBlank: false,
                    editable: false,
                    disabled: true,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Описание', dataIndex: 'Description', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        { xtype: 'buttongroup', columns: 1, items: [{ xtype: 'b4savebutton' }] },
                        { xtype: 'tbfill' },
                        { xtype: 'buttongroup', columns: 1, items: [{ xtype: 'b4closebutton' }] }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});