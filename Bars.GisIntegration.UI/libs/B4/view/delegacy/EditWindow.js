Ext.define('B4.view.delegacy.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.delegacyeditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    bodyPadding: 5,
    title: 'Делегирование',
    closeAction: 'hide',

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.RisContragent'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'OperatorIS',
                    store: 'B4.store.RisContragent',
                    textProperty: 'FullName',
                    fieldLabel: 'Оператор ИС',
                    allowBlank: false,
                    editable: false,
                    columns: [
                        {
                            text: 'Наименование',
                            flex: 2,
                            dataIndex: 'FullName',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'ОГРН',
                            flex: 1,
                            dataIndex: 'Ogrn',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Юридический адрес',
                            flex: 3,
                            dataIndex: 'JuridicalAddress',
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'InformationProvider',
                    store: 'B4.store.RisContragent',
                    textProperty: 'FullName',
                    fieldLabel: 'Поставщик информации',
                    allowBlank: false,
                    editable: false,
                    columns: [
                        {
                            text: 'Наименование',
                            flex: 2,
                            dataIndex: 'FullName',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'ОГРН',
                            flex: 1,
                            dataIndex: 'Ogrn',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Юридический адрес',
                            flex: 3,
                            dataIndex: 'JuridicalAddress',
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    flex: 1,
                    border: false,
                    layout: { type: 'hbox', align: 'stretch' },
                    defaults: {
                        labelWidth: 140,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'StartDate',
                            fieldLabel: 'Дата начала',
                            format: 'd.m.Y',
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            name: 'EndDate',
                            fieldLabel: 'Дата окончания',
                            format: 'd.m.Y',
                            allowBlank: false
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
                            items: [ { xtype: 'b4savebutton' } ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [ { xtype: 'b4closebutton' } ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});