Ext.define('B4.view.wastecollection.AddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.wastecollectionplaceaddwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    minHeight: 180,
    maxHeight: 180,
    width: 500,
    minWidth: 500,
    bodyPadding: 5,
    title: 'Форма добавления площадки сбора ТБО и ЖБО',

    requires: [
        'B4.form.SelectField',
        'B4.store.wastecollection.WasteCollectionPlace',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.form.ComboBox',
        'B4.enums.TypeWaste',
        'B4.enums.TypeWasteCollectionPlace',
        'B4.view.wastecollection.Grid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                allowBlank: false,
                editable: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.RealityObject',
                    textProperty: 'Address',
                    name: 'RealityObject',
                    fieldLabel: 'Адрес площадки сбора БО',
                    columns: [
                        {
                            text: 'Муниципальное образование',
                            dataIndex: 'Municipality',
                            flex: 1,
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
                        }]
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'TypeWaste',
                    fieldLabel: 'Тип хранимых БО',
                    enumName: 'B4.enums.TypeWaste',
                    includeEmpty: false,
                    enumItems: [],
                    hideTrigger: false
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'TypeWasteCollectionPlace',
                    fieldLabel: 'Тип объекта',
                    enumName: 'B4.enums.TypeWasteCollectionPlace',
                    includeEmpty: false,
                    enumItems: [],
                    hideTrigger: false
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