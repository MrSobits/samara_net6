Ext.define('B4.view.dict.programcr.CopyWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
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
    itemId: 'programCrCopyWindow',
    title: 'Копирование программы',
    closeAction: 'hide',
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
                            allowBlank: false,
                            maxLength: 300,
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'Code',
                            fieldLabel: 'Код',
                            maxLength: 50,
                            readOnly: true
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Period',
                            fieldLabel: 'Период',
                            store: 'B4.store.dict.Period',
                            allowBlank: false,
                            editable: false,
                            readOnly: true
                        },
                        {
                            xtype: 'combobox', editable: false,
                            fieldLabel: 'Видимость',
                            store: B4.enums.TypeVisibilityProgramCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TypeVisibilityProgramCr',
                            readOnly: true
                        },
                        {
                            xtype: 'combobox', editable: false,
                            fieldLabel: 'Тип',
                            store: B4.enums.TypeProgramCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TypeProgramCr',
                            readOnly: true
                        },
                        {
                            xtype: 'combobox', editable: false,
                            fieldLabel: 'Состояние',
                            store: B4.enums.TypeProgramStateCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TypeProgramStateCr',
                            readOnly: true
                        },
                        {
                            xtype: 'checkbox',
                            name: 'UsedInExport',
                            boxLabel: 'Используется при экспорте',
                            margins: '0 0 2 95',
                            readOnly: true
                        },
                        {
                            xtype: 'checkbox',
                            name: 'NotAddHome',
                            boxLabel: 'Не доступно добавление домов',
                            margin: '0 0 2 95',
                            readOnly: true
                        },
                        {
                            xtype: 'checkbox',
                            name: 'MatchFl',
                            boxLabel: 'Соответствует ФЗ',
                            margin: '0 0 2 95',
                            readOnly: true
                        },
                        {
                            xtype: 'checkbox',
                            name: 'ForSpecialAccount',
                            boxLabel: 'Для специальных счетов',
                            margin: '5 5 10 95',
                        },
                        {
                            xtype: 'textarea',
                            name: 'Description',
                            fieldLabel: 'Примечание',
                            height: 60,
                            maxLength: 500,
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
                            itemId: 'tfName',
                            fieldLabel: 'Наименование',
                            allowBlank: false,
                            maxLength: 300
                        },
                        {
                            xtype: 'textfield',
                            itemId: 'tfCode',
                            fieldLabel: 'Код',
                            maxLength: 50
                        },
                        {
                            xtype: 'b4selectfield',
                            itemId: 'sflPeriod',
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
                            itemId: 'cbTypeVisibilityProgramCr',
                            value: 30
                        },
                        {
                            xtype: 'combobox',
                            editable: false,
                            fieldLabel: 'Тип',
                            store: B4.enums.TypeProgramCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            itemId: 'cbTypeProgramCr'
                        },
                        {
                            xtype: 'combobox',
                            editable: false,
                            fieldLabel: 'Состояние',
                            store: B4.enums.TypeProgramStateCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            itemId: 'cbTypeProgramStateCr',
                            value: 10
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'chbUsedInExport',
                            boxLabel: 'Используется при экспорте',
                            margins: '0 0 0 95'
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'chbNotAddHome',
                            boxLabel: 'Не доступно добавление домов',
                            margin: '0 0 2 95'
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'chbMatchFl',
                            boxLabel: 'Соответствует ФЗ',
                            margin: '0 0 2 95'
                        },
                        {
                            xtype: 'checkbox',
                            name: 'chbForSpecialAccount',
                            boxLabel: 'Для специальных счетов',
                            margin: '0 0 2 95'
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'chbCopyWithoutAttachments',
                            boxLabel: 'Копировать без файлов вложений',
                            checked: true,
                            margin: '0 0 10 95'
                        },
                        {
                            xtype: 'textarea',
                            itemId: 'taDescription',
                            fieldLabel: 'Примечание',
                            height: 60,
                            maxLength: 500
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
                                    xtype: 'button',
                                    itemId: 'copyProgramButton',
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