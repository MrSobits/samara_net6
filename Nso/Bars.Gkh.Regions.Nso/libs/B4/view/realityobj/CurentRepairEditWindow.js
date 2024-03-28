Ext.define('B4.view.realityobj.CurentRepairEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.realityobjcurrrepaireditwin',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    minWidth: 500,
    minHeight: 320,
    maxHeight: 320,
    bodyPadding: 5,
    title: 'Текущий ремонт',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.Builder'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 180
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'WorkKindName',
                    fieldLabel: 'Вид работы',
                    readOnly: true
                },
                {
                    xtype: 'datefield',
                    name: 'PlanDate',
                    fieldLabel: 'Плановая дата',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'PlanSum',
                    fieldLabel: 'План на сумму',
                    decimalSeparator: ',',
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'PlanWork',
                    fieldLabel: 'План объем работ',
                    decimalSeparator: ',',
                    minValue: 0
                },
                {
                    xtype: 'datefield',
                    name: 'FactDate',
                    fieldLabel: 'Факт дата',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'FactSum',
                    fieldLabel: 'Факт на сумму',
                    decimalSeparator: ',',
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'FactWork',
                    fieldLabel: 'Факт объем работ',
                    decimalSeparator: ',',
                    minValue: 0
                },
                {
                    xtype: 'textfield',
                    name: 'UnitMeasure',
                    fieldLabel: 'Единица измерения',
                    maxLength: 50
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'Builder',
                    fieldLabel: 'Подрядная организация',
                    store: 'B4.store.Builder',
                    textProperty: 'ContragentName',
                    columns: [
                        {
                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListWithoutPaging'
                            }
                        },
                        { text: 'Подрядная организация', dataIndex: 'ContragentName', flex: 1, filter: { xtype: 'textfield' } }
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
                            columns: 2,
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});