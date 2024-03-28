Ext.define(
'B4.view.subprogram.EditWindow',
{
	extend: 'B4.form.Window',
	requires: [
		'B4.form.SelectField',
		'B4.ux.button.Close',
		'B4.ux.button.Save',
		'B4.form.EnumCombo',
		'B4.enums.TypeHouse',
		'B4.enums.ConditionHouse',
		'B4.enums.Condition',
		'B4.store.SubProgramCriterias'
	],
	mixins: [
		'B4.mixins.window.ModalMask'
	],
	layout: 'form',
	bodyPadding: 10,
	width: 600,
	itemId: 'subprogramEditWindow',
	title: 'Критерий',
	closeAction: 'hide',
	trackResetOnLoad: true,
	initComponent: function()
	{
		var me = this;
		Ext.applyIf(me,
			{
				defaults:
				{},
				items: [
                {
						xtype: 'container',
						layout: 'hbox',
						defaults:
						{
							labelWidth: 80,
							labelAlign: 'right',
						},
						items: [
						{
							xtype: 'textfield',
							name: 'Name',
							fieldLabel: 'Название',
							allowBlank: false,
							flex: 1
						}]
					},
				  {
					xtype: 'fieldset',
					defaults:
					{
						labelWidth: 250,
						anchor: '100%',
						labelAlign: 'right'
					},
					title: '',
					items: [					
					{
						xtype: 'container',
						layout: 'hbox',
						defaults:
						{
							labelWidth: 200,
							labelAlign: 'right',
						},
						items: [
						{
							xtype: 'checkbox',
							itemId: 'cbStatusUsed',
							name: 'IsStatusUsed',
							fieldLabel: 'Статус дома',
							allowBlank: false,
							editable: true,
						},
						{
							xtype: 'b4selectfield',
							name: 'Status',
							displayField: 'Name',
							itemId: 'daStatus',
							store: 'B4.store.RealityObjectStateStore',
							//selectionMode: 'MULTI',
							idProperty: 'Id',
							textProperty: 'Name',
							columns: [
							{
								text: 'Наименование',
								dataIndex: 'Name',
								flex: 1,
								filter:
								{
									xtype: 'textfield'
								}
							}],
							editable: false,
							allowBlank: true,
							valueField: 'Value',
							flex: 1,
                            disabled: true
						}]
					},
					{
						xtype: 'container',
						layout: 'hbox',
						defaults:
						{
							labelWidth: 200,
							labelAlign: 'right',
						},
						items: [
						{
							xtype: 'checkbox',
							itemId: 'cbTypeHouseUsed',
							name: 'IsTypeHouseUsed',
							fieldLabel: 'Тип дома',
							allowBlank: false,
							editable: true,
						},
						{
							xtype: 'b4enumcombo',
                            itemId: 'ecTypeHouse',
							name: 'TypeHouse',
							enumName: 'B4.enums.TypeHouse',
							includeEmpty: false,
							flex: 1,
                            disabled: true
						}]
					},
					{
						xtype: 'container',
						layout: 'hbox',
						defaults:
						{
							labelWidth: 200,
							labelAlign: 'right',
						},
						items: [
						{
							xtype: 'checkbox',
							itemId: 'cbConditionHouseUsed',
							name: 'IsConditionHouseUsed',
							fieldLabel: 'Состояние дома',
							allowBlank: false,
							editable: true,
						},
						{
							xtype: 'combobox',
							name: 'ConditionHouse',
							displayField: 'Display',
							itemId: 'cbConditionHouse',
							store: B4.enums.ConditionHouse
								.getStore(),
							valueField: 'Value',
							allowBlank: true,
							editable: false,
							flex: 1,
							disabled: true
						}]
					},
					{
						xtype: 'container',
						layout: 'hbox',
						defaults:
						{
							labelWidth: 250,
							labelAlign: 'right',
						},
						items: [
						{
							xtype: 'checkbox',
							itemId: 'cbNumberApartmentsUsed',
							name: 'IsNumberApartmentsUsed',
							fieldLabel: 'Количество квартир в доме',
							allowBlank: false,
							editable: true,
						},
						{
							xtype: 'combobox',
							name: 'NumberApartmentsCondition',
							displayField: 'Display',
							itemId: 'cbNumberApartmentsCondition',
							flex: 0.1,
							store: B4.enums.Condition
								.getStore(),
							valueField: 'Value',
							allowBlank: false,
							editable: false,
							disabled: true
						},
						{
							xtype: 'numberfield',
							name: 'NumberApartments',
							itemId: 'nfNumberApartments',
							allowDecimals: false,
							minValue: 1,
							flex: 0.3,
							allowBlank: false,
							disabled: true,
						}, ]
					},
					{
						xtype: 'container',
						layout: 'hbox',
						defaults:
						{
							labelWidth: 250,
							labelAlign: 'right',
						},
						items: [
						{
							xtype: 'checkbox',
							itemId: 'cbYearRepairUsed',
							name: 'IsYearRepairUsed',
							fieldLabel: 'Год последнего капитального ремонта КЭ',
							allowBlank: false,
							disabled: false,
							editable: true
						},
						{
							xtype: 'combobox',
							name: 'YearRepairCondition',
							displayField: 'Display',
							itemId: 'cbYearRepairCondition',
							flex: 0.1,
							store: B4.enums.Condition
								.getStore(),
							valueField: 'Value',
							allowBlank: false,
							editable: false,
							disabled: true
						},
						{
							xtype: 'numberfield',
							name: 'YearRepair',
							itemId: 'nfYearRepair',
							allowDecimals: false,
							minValue: 1,
							flex: 0.3,
							allowBlank: false,
							disabled: true
						}, ]
					},
					{
						xtype: 'container',
						layout: 'hbox',
						defaults:
						{
							labelWidth: 250,
							labelAlign: 'right',
						},
						items: [
						{
							xtype: 'checkbox',
							itemId: 'cbIsRepairNotAdvisableUsed',
							name: 'IsRepairNotAdvisableUsed',
							fieldLabel: 'Ремонт нецелесообразен',
							allowBlank: false,
							editable: true,
						},
						{
							xtype: 'checkbox',
							itemId: 'cbRepairNotAdvisable',
							name: 'RepairNotAdvisable',
							fieldLabel: 'Да/нет',
							allowBlank: false,
							disabled: true,
							editable: true,
						}, ]
					},
					{
						xtype: 'container',
						layout: 'hbox',
						defaults:
						{
							labelWidth: 250,
							labelAlign: 'right',
						},
						items: [
						{
							xtype: 'checkbox',
							itemId: 'cbIsNotInvolvedCrUsed',
							name: 'IsNotInvolvedCrUsed',
							fieldLabel: 'Дом не участвует в программе КР',
							allowBlank: false,
							editable: true,
						},
						{
							xtype: 'checkbox',
							itemId: 'cbNotInvolvedCr',
							name: 'NotInvolvedCr',
							fieldLabel: 'Да/нет',
							allowBlank: false,
							disabled: true,
							editable: true,
						}, ]
					},
					{
						xtype: 'container',
						layout: 'hbox',
						defaults:
						{
							labelWidth: 250,
							labelAlign: 'right',
						},
						items: [
						{
							xtype: 'checkbox',
							itemId: 'cbStructuralElementCountUsed',
							name: 'IsStructuralElementCountUsed',
							fieldLabel: 'Количество КЭ',
							allowBlank: false,
							disabled: false,
							editable: true
						},
						{
							xtype: 'combobox',
							name: 'StructuralElementCountCondition',
							displayField: 'Display',
							itemId: 'cbStructuralElementCountCondition',
							flex: 0.1,
							store: B4.enums.Condition
								.getStore(),
							valueField: 'Value',
							allowBlank: false,
							editable: false,
							disabled: true
						},
						{
							xtype: 'numberfield',
							name: 'StructuralElementCount',
							itemId: 'nfStructuralElementCount',
							allowDecimals: false,
							minValue: 1,
							flex: 0.3,
							allowBlank: false,
							disabled: true,
						}]
					},
                    {
						xtype: 'container',
						layout: 'hbox',
						defaults:
						{
							labelWidth: 250,
							labelAlign: 'right',
						},
						items: [
						{
							xtype: 'checkbox',
							itemId: 'cbFloorCountUsed',
							name: 'IsFloorCountUsed',
							fieldLabel: 'Количество этажей',
							allowBlank: false,
							disabled: false,
							editable: true
						},
						{
							xtype: 'combobox',
							name: 'FloorCountCondition',
							displayField: 'Display',
							itemId: 'cbFloorCountCondition',
							flex: 0.1,
							store: B4.enums.Condition.getStore(),
							valueField: 'Value',
							allowBlank: false,
							editable: false,
							disabled: true
						},
						{
							xtype: 'numberfield',
							name: 'FloorCount',
							itemId: 'nfFloorCount',
							allowDecimals: false,
							minValue: 1,
							flex: 0.3,
							allowBlank: false,
							disabled: true,
						}]
					},
                    {
						xtype: 'container',
						layout: 'hbox',
						defaults:
						{
							labelWidth: 250,
							labelAlign: 'right',
						},
						items: [
						{
							xtype: 'checkbox',
							itemId: 'cbLifetimeUsed',
							name: 'IsLifetimeUsed',
							fieldLabel: 'Срок службы',
							allowBlank: false,
							disabled: false,
							editable: true
						},
						{
							xtype: 'combobox',
							name: 'LifetimeCondition',
							displayField: 'Display',
							itemId: 'cbLifetimeCondition',
							flex: 0.1,
							store: B4.enums.Condition.getStore(),
							valueField: 'Value',
							allowBlank: false,
							editable: false,
							disabled: true
						},
						{
							xtype: 'numberfield',
							name: 'Lifetime',
							itemId: 'nfLifetime',
							allowDecimals: false,
							minValue: 1,
							flex: 0.3,
							allowBlank: false,
							disabled: true,
						}]
					}]
				}],
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
						xtype: 'b4savebutton'
					}]
				},
				{
					xtype: 'tbfill'
				},
				{
					xtype: 'buttongroup',
					columns: 2,
					items: [
					{
						xtype: 'b4closebutton'
					}]
				}]
			}]
		});
		me.callParent(arguments);
}
});