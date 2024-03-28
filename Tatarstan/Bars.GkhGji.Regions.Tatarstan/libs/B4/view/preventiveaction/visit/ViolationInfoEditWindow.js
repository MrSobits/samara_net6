Ext.define('B4.view.preventiveaction.visit.ViolationInfoEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 900,
    minHeight: 500,
    maxHeight: 500,
    autoScroll: true,
    bodyPadding: 5,
    itemId: 'visitViolationInfoEditWindow',
    title: 'Форма добавления нарушения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.RealityObject',
        'B4.view.preventiveaction.visit.ViolationGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObject',
                    itemId: 'sfRealityObject',
                    fieldLabel: 'Место выявления',
                    store: 'B4.store.RealityObject',
                    textProperty: 'FullAddress',
                    allowBlank: false,
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
                        {
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'visitviolationgrid',
                    flex: 1
                },
                {
                    xtype: 'textarea',
                    padding: '5 0 0 0',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    labelWidth: 80,
                    maxLength: 2000
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
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