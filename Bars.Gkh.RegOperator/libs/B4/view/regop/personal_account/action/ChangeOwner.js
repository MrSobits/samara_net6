Ext.define('B4.view.regop.personal_account.action.ChangeOwner', {
    extend: 'B4.view.regop.personal_account.action.BaseAccountWindow',

    requires: [
         'B4.form.SelectField',
         'B4.store.regop.owner.PersonalAccountOwner',
         'B4.enums.regop.PersonalAccountOwnerType',
         'B4.ux.grid.column.Enum',
		 'B4.form.FileField',

		 'B4.enums.RoomOwnershipType'
    ],

    alias: 'widget.changeownerwin',

    modal: true,

    width: 500,
    title: 'Смена абонента',

    initComponent: function() {
        var me = this;
        Ext.apply(me, {
            layout: {
                type: 'form'
                //, align: 'stretch'
            },
            items: [{
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
                        name: 'AccountId'
                    },
                    {
                        xtype: 'textfield',
                        labelWidth: 150,
                        fieldLabel: 'Текущий владелец',
                        name: 'CurrentOwner',
                        readOnly: true
                    },
                    {
                        xtype: 'textfield',
                        labelWidth: 150,
                        fieldLabel: 'Форма собственности текущего владельца',
                        name: 'CurrentOwnershipType',
                        readOnly: true
                    },
                    {
                        xtype: 'b4selectfield',
                        store: 'B4.store.regop.owner.PersonalAccountOwner',
                        labelWidth: 150,
                        editable: false,
                        name: 'NewOwner',
                        fieldLabel: 'Новый владелец',
                        allowBlank: false,
                        columns: [
                            {
                                xtype: 'b4enumcolumn',
                                header: 'Тип абонента',
                                enumName: 'B4.enums.regop.PersonalAccountOwnerType', // перечисление
                                filter: true, // создать фильтр
                                dataIndex: 'OwnerType'
                            },
                            {
                                xtype: 'gridcolumn',
                                header: 'ФИО/Наименование',
                                dataIndex: 'Name',
                                flex: 1,
                                filter: { xtype: 'textfield' }
                            },
                            {
                                xtype: 'gridcolumn',
                                header: 'ИНН',
                                dataIndex: 'Inn',
                                flex: 1,
                                filter: { xtype: 'textfield' }
                            },
                            {
                                xtype: 'gridcolumn',
                                header: 'КПП',
                                dataIndex: 'Kpp',
                                flex: 1,
                                filter: { xtype: 'textfield' }
                            }
                        ]
					},
					{
						xtype: 'b4enumcombo',
						name: 'NewOwnershipType',
						fieldLabel: 'Тип собственности нового владельца',
						enumName: 'B4.enums.RoomOwnershipType',
						allowBlank: false,
					},
                    {
						xtype: 'checkboxfield',
						name: 'NewLSCheckBox',
                        inputValue: 'true',
						fieldLabel: 'Создать новый лицевой счет'
					},
                    {
                        xtype: 'datefield',
                        name: 'ActualFrom',
                        fieldLabel: 'Дата начала действия',
                        allowBlank: false
                    },
                    {
                        xtype: 'b4filefield',
                        name: 'Document',
                        fieldLabel: 'Документ-основание'
                    },
                    {
                        xtype: 'textarea',
                        name: 'Reason',
                        fieldLabel: 'Причина',
                        allowBlank: true
                    }
                ]
            }]
        });
        me.callParent(arguments);
    }
    
});