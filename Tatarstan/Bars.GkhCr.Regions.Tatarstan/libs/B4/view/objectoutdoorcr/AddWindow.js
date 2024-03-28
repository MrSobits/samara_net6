Ext.define('B4.view.objectoutdoorcr.AddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.objectoutdoorcraddwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    title: 'Объект программы благоустройства дворов',

    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',

        'B4.store.realityobj.RealityObjectOutdoor',
        'B4.store.dict.RealityObjectOutdoorProgram',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 140,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObjectOutdoorProgram',
                    fieldLabel: 'Программа благоустройства',
                    flex: 1,
                    textProperty: 'Name',
                    anchor: '100%',
                    store: 'B4.store.dict.RealityObjectOutdoorProgram',
                    columns: [
                        {
                            text: 'Программа', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' }
                        }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObjectOutdoor',
                    fieldLabel: 'Объект',
                    textProperty: 'Name',
                    anchor: '100%',
                    store: 'B4.store.realityobj.RealityObjectOutdoor',
                    editable: false,
                    columns: [
                        {
                            text: 'Муниципальный район', dataIndex: 'Municipality',
                            flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListMoAreaWithoutPaging'
                            }
                        },
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Код',
                            dataIndex: 'Code',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Жилые дома двора',
                            dataIndex: 'RealityObjects',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    allowBlank: false
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