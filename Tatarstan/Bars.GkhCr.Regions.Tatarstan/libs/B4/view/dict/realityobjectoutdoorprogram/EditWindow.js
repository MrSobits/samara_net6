Ext.define('B4.view.dict.realityobjectoutdoorprogram.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.outdoorprogrameditwindow',
    mixins: ['B4.mixins.window.ModalMask'],

    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },

    width: 650,
    height: 600,
    maxWidth: 700,
    minWidth: 500,
    minHeight: 550,
    autoScroll: true,
    bodyPadding: 5,
    title: 'Программа капитального ремонта',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Period',
        'B4.store.dict.NormativeDoc',
        'B4.form.FileField',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.config.ContragentSelectField',

        'B4.enums.TypeVisibilityProgramCr',
        'B4.enums.TypeProgramCr',
        'B4.enums.TypeProgramStateCr'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 120
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            allowBlank: false,
                            maxLength: 255
                        },
                        {
                            xtype: 'textfield',
                            name: 'Code',
                            fieldLabel: 'Код',
                            maxLength: 255
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Period',
                            fieldLabel: 'Период',
                            store: 'B4.store.dict.Period',
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'combobox',
                            editable: false,
                            fieldLabel: 'Видимость',
                            store: B4.enums.TypeVisibilityProgramCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TypeVisibilityProgram'
                        },
                        {
                            xtype: 'combobox',
                            editable: false,
                            fieldLabel: 'Тип',
                            store: B4.enums.TypeProgramCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TypeProgram'
                        },
                        {
                            xtype: 'contragentselectfield',
                            name: 'GovernmentCustomer',
                            fieldLabel: 'Государственный заказчик',
                        },
                        {
                            xtype: 'combobox',
                            editable: false,
                            fieldLabel: 'Состояние',
                            store: B4.enums.TypeProgramStateCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TypeProgramState'
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    allowBlank: false,
                                    maxLength: 255
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата документа',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocumentDepartment',
                            fieldLabel: 'Орган принявший документ',
                            maxLength: 255,
                            allowBlank: false
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'NormativeDoc',
                            fieldLabel: 'Постановление об утверждении КП',
                            store: 'B4.store.dict.NormativeDoc',
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'File',
                            fieldLabel: 'Файл'
                        },
                        {
                            xtype: 'checkbox',
                            name: 'IsNotAddOutdoor',
                            boxLabel: 'Недоступно добавление дворов',
                            margin: '0 0 2 125'
                        },
                        {
                            xtype: 'textarea',
                            name: 'Description',
                            fieldLabel: 'Примечание',
                            height: 60,
                            maxLength: 255
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    minHeight: 200,
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'outdoorprogramchangejournalgrid',
                            flex: 1
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
                            xtype: 'buttongroup',
                            columns: 2,
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