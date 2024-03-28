Ext.define('B4.view.actcheck.DefinitionEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    minHeight: 300,
    bodyPadding: 5,
    itemId: 'actCheckDefinitionEditWindow',
    title: 'Форма определения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.view.Control.GkhButtonPrint',
        'B4.store.dict.Inspector',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        
        'B4.enums.TypeDefinitionAct'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 180,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        allowBlank: false,
                        labelAlign: 'right',
                        flex: 1,
                        labelWidth: 180
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNum',
                            fieldLabel: 'Номер',
                            allowBlank: true,
                            maxLength: 50
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата',
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'combobox',
                    floating: false,
                    editable: false,
                    name: 'TypeDefinition',
                    fieldLabel: 'Тип определения',
                    displayField: 'Display',
                    valueField: 'Value',
                    store: B4.enums.TypeDefinitionAct.getStore(),
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Inspector',
                    textProperty: 'Fio',
                    name: 'IssuedDefinition',
                    fieldLabel: 'ДЛ, вынесшее определение',
                    columns: [
                        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'ExecutionDate',
                    fieldLabel: 'Дата исполнения',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл'
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    maxLength: 500,
                    flex: 1
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