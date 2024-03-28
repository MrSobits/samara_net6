Ext.define('B4.view.dict.realityobjectoutdoorprogram.CopyWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.outdoorprogramcopywindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'hbox',
        align: 'stretch',
        pack: 'start'
    },
    width: 800,
    height: 450,
    maxWidth: 900,
    minWidth: 500,
    minHeight: 500,
    autoScroll: true,
    bodyPadding: 5,
    title: 'Копирование программы',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Period',

        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.enums.TypeVisibilityProgramCr',
        'B4.enums.TypeProgramCr',
        'B4.enums.TypeProgramStateCr'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    bodyPadding: 5,
                    title: 'Текущая программа',
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 90
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'Code',
                            fieldLabel: 'Код',
                            readOnly: true
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Period',
                            fieldLabel: 'Период',
                            readOnly: true
                        },
                        {
                            xtype: 'combobox',
                            fieldLabel: 'Видимость',
                            store: B4.enums.TypeVisibilityProgramCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TypeVisibilityProgram',
                            readOnly: true
                        },
                        {
                            xtype: 'combobox',
                            fieldLabel: 'Тип',
                            store: B4.enums.TypeProgramCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TypeProgram',
                            readOnly: true
                        },
                        {
                            xtype: 'combobox',
                            fieldLabel: 'Состояние',
                            store: B4.enums.TypeProgramStateCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TypeProgramState',
                            readOnly: true
                        },
                        {
                            xtype: 'checkbox',
                            name: 'IsNotAddOutdoor',
                            boxLabel: 'Не доступно добавление дворов',
                            margin: '0 0 2 95',
                            readOnly: true
                        },
                        {
                            xtype: 'textarea',
                            name: 'Description',
                            fieldLabel: 'Примечание',
                            height: 60,
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    bodyPadding: 5,
                    title: 'Новая программа',
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 90
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Наименование',
                            name: 'NewName',
                            allowBlank: false,
                            maxLength: 255
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Код',
                            name: 'NewCode',
                            maxLength: 255
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'NewPeriod',
                            fieldLabel: 'Период',
                            store: 'B4.store.dict.Period',
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'combobox',
                            name: 'NewTypeVisibilityProgram',
                            editable: false,
                            fieldLabel: 'Видимость',
                            store: B4.enums.TypeVisibilityProgramCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            value: 30
                        },
                        {
                            xtype: 'combobox',
                            name: 'NewTypeProgram',
                            editable: false,
                            fieldLabel: 'Тип',
                            store: B4.enums.TypeProgramCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                        },
                        {
                            xtype: 'combobox',
                            name: 'NewTypeProgramState',
                            editable: false,
                            fieldLabel: 'Состояние',
                            store: B4.enums.TypeProgramStateCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            value: 10
                        },
                        {
                            xtype: 'checkbox',
                            name: 'NewIsNotAddOutdoor',
                            boxLabel: 'Не доступно добавление дворов',
                            margin: '0 0 2 95'
                        },
                        {
                            xtype: 'textarea',
                            name: 'NewDescription',
                            fieldLabel: 'Примечание',
                            height: 60,
                            maxLength: 255
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    name: 'copyProgramButton',
                                    text: 'Копировать',
                                    tooltip: 'Копировать',
                                    iconCls: 'icon-accept'
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