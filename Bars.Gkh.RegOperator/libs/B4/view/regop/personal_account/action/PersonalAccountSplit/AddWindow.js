Ext.define('B4.view.regop.personal_account.action.PersonalAccountSplit.AddWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.personalaccountsplitaddwindow',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.form.SelectField',

		'B4.enums.regop.PersonalAccountOwnerType',
		'B4.enums.RoomOwnershipType'
    ],

    closable: false,
    width: 450,
    bodyPadding: 10,
    title: 'Карточка абонента',

    initComponent: function() {
        var me = this;

		Ext.applyIf(me, {
			defaults: {
				labelAlign: 'right',
				labelWidth: 130,
				anchor: '100%'
			},
			items: [
				{
					xtype: 'b4combobox',
					name: 'OwnerType',
					fieldLabel: 'Тип абонента',
					editable: false,
					allowBlank: false,
					items: B4.enums.regop.PersonalAccountOwnerType.getItems()
				},
				{
					xtype: 'b4combobox',
					name: 'OwnershipTypeNewLs',
					fieldLabel: 'Тип собственности',
					editable: false,
					allowBlank: true,
					items: B4.enums.RoomOwnershipType.getItems()
				},
				{
					xtype: 'container',
					layout: 'hbox',
					defaults: {
						labelAlign: 'right',
						labelWidth: 130
					},

					items: [
						{
							xtype: 'datefield',
							name: 'OpenDate',
							fieldLabel: 'Дата открытия',
							readOnly: true,
							format: 'd.m.Y',
							flex: 1
						},
						{
							xtype: 'button',
							action: 'selectOwner',
							text: 'Выбрать из абонентов',
							iconCls: 'icon-add',
							margin: '0 0 0 5px',
							width: 140
						}
					]
				},
				{
					// блоа данных по физику
					xtype: 'container',
					name: 'Individual',
					hidden: true,
					margin: 0,
					padding: 0,
					layout: 'form',
					defaults: {
						labelAlign: 'right',
						labelWidth: 130,
						anchor: '100%'
					},
					items: [
						{
							xtype: 'textfield',
							name: 'FirstName',
							fieldLabel: 'Имя',
							required: true,
							allowBlank: false
						},
						{
							xtype: 'textfield',
							name: 'Surname',
							fieldLabel: 'Фамилия',
							required: true,
							allowBlank: false
						},
						{
							xtype: 'textfield',
							name: 'SecondName',
							fieldLabel: 'Отчество'
						}
					]
				},
				{
					// блоа данных по юрику
					xtype: 'container',
					name: 'Legal',
					hidden: true,
					margin: 0,
					padding: 0,
					layout: 'form',
					defaults: {
						labelAlign: 'right',
						labelWidth: 130,
						anchor: '100%'
					},
					items: [
						{
							xtype: 'b4selectfield',
							name: 'Contragent',
							store: 'B4.store.Contragent',
							fieldLabel: 'Контрагент',
							editable: false,
							required: true,
							allowBlank: false,
							columns: [
								{ text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
								{ text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
								{ text: 'КПП', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
							]
						},
						{
							xtype: 'textfield',
							name: 'Inn',
							fieldLabel: 'ИНН',
							readOnly: true
						},
						{
							xtype: 'textfield',
							name: 'Kpp',
							fieldLabel: 'КПП',
							readOnly: true
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
                            xtype: 'b4savebutton',
                            text: 'Продолжить'
                        }, '->', {
                            xtype: 'b4closebutton',
                            listeners: {
                                click: function(btn) {
                                    btn.up('window').close();
                                }
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});