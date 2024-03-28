Ext.define('B4.view.actionisolated.actactionisolated.DefinitionEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.actactionisolateddefinitioneditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    minHeight: 400,
    bodyPadding: 5,
    itemId: 'actActionIsolatedDefinitionEditWindow',
    title: 'Форма определения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.store.actactionisolated.DefenitionRealityObjectForSelect',
        'B4.enums.ActActionIsolatedDefinitionType',
        'B4.form.SelectField'
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
                    xtype: 'b4selectfield',
                    name: 'RealityObject',
                    textProperty: 'Address',
                    store: 'B4.store.actactionisolated.DefenitionRealityObjectForSelect',
                    fieldLabel: 'Адрес дома',
                    columns: [
                        { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    editable: false,
                    allowBlank: false
                },
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
                            name: 'Number',
                            fieldLabel: 'Номер',
                            maxLength: 50,
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            name: 'Date',
                            fieldLabel: 'Дата',
                            format: 'd.m.Y',
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'DefinitionType',
                    enumName: 'B4.enums.ActActionIsolatedDefinitionType',
                    allowBlank: false,
                    fieldLabel: 'Тип определения'
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Inspector',
                    textProperty: 'Fio',
                    name: 'Official',
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