Ext.define('B4.view.dict.paymentdocinfo.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.paymentdocinfoeditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 800,
    height: 300,
    bodyPadding: 5,
    title: 'Редактирование',
    requires: [
        'B4.enums.FundFormationType',
        'B4.form.EnumCombo',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.form.TreeSelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.button.Delete',
        'B4.view.Control.GkhDecimalField',
        'B4.view.Control.GkhIntField',
        'B4.store.dict.municipality.MoArea',
        'B4.store.realityobj.ByLocalityGuid',
        'B4.store.dict.MunicipalityTree',
        'B4.store.regop.Fias'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
            {
                border: false,
                bodyStyle: Gkh.bodyStyle,
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                items: [
                    {
                        border: false,
                        bodyStyle: Gkh.bodyStyle,
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        defaults: {
                            labelWidth: 200,
                            labelAlign: 'right'
                        },
                        items: [
                            {
                                xtype: 'datefield',
                                name: 'DateStart',
                                allowBlank: false,
                                fieldLabel: 'Дата действия с',
                                format: 'd.m.Y'
                            },
                            {
                                xtype: 'b4enumcombo',
                                name: 'FundFormationType',
                                fieldLabel: 'Способ формирования фонда',
                                enumName: 'B4.enums.FundFormationType',
                                value: 0
                            },
                            {
                                xtype: 'b4selectfield',
                                name: 'Municipality',
                                fieldLabel: 'Муниципальный район',
                                store: 'B4.store.dict.municipality.MoArea',
                                editable: false,
                                columns: [
                                    { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                ]
                            },
                            {
                                xtype: 'b4selectfield',
                                name: 'LocalityName',
                                isGetOnlyIdProperty: false,
                                fieldLabel: 'Населенный пункт',
                                store: 'B4.store.regop.Fias',
                                model: 'B4.model.regop.Fias',
                                idProperty: 'AOGuid',
                                textProperty: 'FormalName',
                                columns: [
                                    {
                                        header: 'Тип',
                                        flex: 1,
                                        dataIndex: 'ShortName',
                                        filter: { xtype: 'textfield' }
                                    },
                                    {
                                        header: 'Наименование',
                                        flex: 3,
                                        dataIndex: 'FormalName',
                                        filter: { xtype: 'textfield' }
                                    }
                                ]
                            }
                        ]
                    },
                    {
                        border: false,
                        bodyStyle: Gkh.bodyStyle,
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        defaults: {
                            labelWidth: 200,
                            labelAlign: 'right'
                        },
                        items: [
                            {
                                xtype: 'datefield',
                                name: 'DateEnd',
                                fieldLabel: 'Дата действия по',
                                format: 'd.m.Y'
                            },
                            {
                                xtype: 'checkbox',
                                name: 'IsForRegion',
                                fieldLabel: 'Для региона'
                            },
                            {
                                xtype: 'treeselectfield',
                                name: 'MoSettlement',
                                fieldLabel: 'Муниципальное образование',
                                editable: false,
                                allowBlank: true,
                                titleWindow: 'Выбор муниципального образования',
                                store: 'B4.store.dict.MunicipalityTree'
                            },
                            {
                                xtype: 'b4selectfield',
                                store: 'B4.store.realityobj.ByLocalityGuid',
                                textProperty: 'Address',
                                editable: false,
                                name: 'RealityObject',
                                fieldLabel: 'Жилой дом',
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
                                    }
                                ]
                            }
                        ]
                    }
                ]
            },
            {
                xtype: 'fieldset',
                title: 'Информационное поле',
                layout: 'fit',
                flex: 1,
                items: {
                    xtype: 'textarea',
                    name: 'Information',
                    maxLength: 4000
                }
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
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'b4deletebutton'
                                }
                            ]
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