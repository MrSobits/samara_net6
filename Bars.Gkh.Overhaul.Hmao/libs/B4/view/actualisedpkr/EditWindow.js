Ext.define('B4.view.actualisedpkr.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.enums.TypeHouse',
        'B4.enums.ConditionHouse',
        'B4.enums.Condition',
        'B4.store.RealityObjectStateStore',
        'B4.store.SEStateStore'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    bodyPadding: 10,
    width: 600,
    itemId: 'actualisedpkrEditWindow',
    title: 'Условия отбора в актуальную программу',
    closeAction: 'destroy',
    trackResetOnLoad: true,
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {},
            items:
          [{
                xtype: 'container',
                layout: 'hbox',
                defaults: {
                    labelWidth: 150,
                    labelAlign: 'right',
                },
                items: [{
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    allowBlank: false,
                    name: 'DateStart',
                    fieldLabel: 'Дата начала действия условий',
                    itemId: 'dfDateStart',
                    flex: 0.5,
                },
                {
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    allowBlank: false,
                    name: 'DateEnd',
                    fieldLabel: 'Дата прекращения действия условий',
                    itemId: 'dfDateEnd',
                    flex: 0.5,
                },
                ]
            },
            {
                xtype: 'container',
                layout: 'hbox',
                defaults: {
                    labelWidth: 200,
                    labelAlign: 'right',
                },
                items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Status',
                    fieldLabel: 'Статус МКД',
                    displayField: 'Name',
                    itemId: 'daStatuses',
                    store: 'B4.store.RealityObjectStateStore',
                    //selectionMode: 'MULTI',
                    idProperty: 'Id',
                    textProperty: 'Name',
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
                    editable: false,
                    allowBlank: true,
                    valueField: 'Value',
                    flex: 1
                }]
            },
            {
                xtype: 'container',
                layout: 'hbox',
                defaults: {
                    labelWidth: 200,
                    labelAlign: 'right',
                },
                items: [{
                    xtype: 'b4enumcombo',
                    name: 'TypeHouse',
                    enumName: 'B4.enums.TypeHouse',
                    fieldLabel: 'Тип дома',
                    includeEmpty: false,
                    flex: 1
                }]
            },
            {
                xtype: 'container',
                layout: 'hbox',
                defaults: {
                    labelWidth: 200,
                    labelAlign: 'right',
                },
                items: [{
                    xtype: 'combobox',
                    name: 'ConditionHouse',
                    fieldLabel: 'Состояние дома',
                    displayField: 'Display',
                    itemId: 'cbConditionHouse',                    
                    store: B4.enums.ConditionHouse.getStore(),
                    valueField: 'Value',
                    allowBlank: true,
                    editable: false,
                    flex: 1
                }]
            },
            {
                xtype: 'container',
                layout: 'hbox',
                defaults: {
                    labelWidth: 250,
                    labelAlign: 'right',
                },
                items: [{
                    xtype: 'checkbox',
                    itemId: 'cbNumberApartments',
                    name: 'IsNumberApartments',
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
                    store: B4.enums.Condition.getStore(),
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
                },
                ]
            },
            {
                xtype: 'container',
                layout: 'hbox',
                defaults: {
                    labelWidth: 250,
                    labelAlign: 'right',
                },
                items: [{
                    xtype: 'checkbox',
                    itemId: 'cbYearRepair',
                    name: 'IsYearRepair',
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
                    store: B4.enums.Condition.getStore(),
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
                },
                ]
            },
            {
                xtype: 'container',
                layout: 'hbox',
                defaults: {
                    labelWidth: 250,
                    labelAlign: 'right',
                },
                items: [{
                    xtype: 'checkbox',
                    itemId: 'cbRepairAdvisable',
                    name: 'CheckRepairAdvisable',
                    fieldLabel: 'Ремонт целесообразен',
                    allowBlank: false,
                    disabled: false,
                    editable: true
                },]
            },
            {
                xtype: 'container',
                layout: 'hbox',
                defaults: {
                    labelWidth: 250,
                    labelAlign: 'right',
                },
                items: [{
                    xtype: 'checkbox',
                    itemId: 'cbInvolvedCr',
                    name: 'CheckInvolvedCr',
                    fieldLabel: 'Дом участвует в КР',
                    allowBlank: false,
                    disabled: false,
                    editable: true
                },]
            },
            {
                xtype: 'container',
                layout: 'hbox',
                defaults: {
                    labelWidth: 250,
                    labelAlign: 'right',
                },
                items: [{
                    xtype: 'checkbox',
                    itemId: 'cbStructuralElementCount',
                    name: 'IsStructuralElementCount',
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
                    store: B4.enums.Condition.getStore(),
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
                },
                ]
            },
            {
                xtype: 'container',
                layout: 'hbox',
                defaults: {
                    labelWidth: 200,
                    labelAlign: 'right',
                },
                items: [
                    {
                        xtype: 'b4selectfield',
                        name: 'SEStatus',
                        fieldLabel: 'Статус KЭ',
                        displayField: 'Name',
                        itemId: 'daSEStatus',
                        store: 'B4.store.SEStateStore',
                        //selectionMode: 'MULTI',
                        idProperty: 'Id',
                        textProperty: 'Name',
                        columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
                        editable: false,
                        allowBlank: true,
                        valueField: 'Value',
                        flex: 1
                    }]
            },
            ],
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                    xtype: 'buttongroup',
                    columns: 2,
                    items: [{
                        xtype: 'b4savebutton'
                    }]
                },
                {
                    xtype: 'tbfill'
                },
                {
                    xtype: 'buttongroup',
                    columns: 2,
                    items: [{
                        xtype: 'b4closebutton'
                    }]
                }
                ]
            }]
        });

        me.callParent(arguments);
    }
});