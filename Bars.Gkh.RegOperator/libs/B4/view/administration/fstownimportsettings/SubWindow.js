Ext.define('B4.view.administration.fstownimportsettings.SubWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.administration.fsTownImportSettings',
        'B4.store.administration.fsTownProperty'
    ],

    mixins: ['B4.mixins.window.ModalMask'],

    width: 600,

    bodyPadding: 3,

    alias: 'widget.fstownimportsettingssubwindow',

    title: 'Добавить запись',

    initComponent: function () {
        var me = this,
            cln,
            store = Ext.create('B4.store.administration.fsTownImportSettings');

        if (!me.phantomColumn) {
            me.itemId = "headerWindow";
            cln = {
                xtype: 'textfield',
                allowBlank: false,
                name: 'Index',
                fieldLabel: 'Номер строки'
            };
        } else {
            me.itemId = "dataWindow";
            cln = {
                xtype: 'textfield',
                allowBlank: false,
                name: 'Index',
                fieldLabel: 'Номер столбца'
            };
        }

        var useFile = {
            xtype: 'checkbox',
            boxLabel: 'Использовать наименование файла',
            name: 'UseFilename',
            handler: function(cb, checked) {
                var index = cb.up('window').down('[name="Index"]');

                index.setDisabled(checked === true);
            }
        };

        Ext.applyIf(me, {
            store: store,
            defaults: {
                labelAlign: 'top',
                anchor: '100%',
                margin: '8 5'
            },
            items: [
                {
                    xtype: 'checkbox',
                    name: 'Required',
                    boxLabel: 'Обязательное'
                },
                cln,
                {
                    xtype: 'textareafield',
                    name: 'Regex',
                    fieldLabel: 'Регулярное выражение'
                },
                {
                    xtype: 'textfield',
                    name: 'Format',
                    fieldLabel: 'Формат преобразования строки'
                },
                {
                    xtype: 'checkbox',
                    boxLabel: 'Использовать значение из регулярного выражения',
                    name: 'GetValueFromRegex'
                },
                {
                    xtype: 'b4combobox',
                    fieldLabel: 'Поле',
                    allowBlank: false,
                    url: '/FsImportSetup/GetObjectMeta',
                    name: 'PropertyName',
                    valueField: 'PropertyName',
                    displayField: 'DisplayName',
                    listeners: {
                        change: function (field, val) {
                            var obj = {};
                            obj[field.valueField] = val;
                            field.setValue(obj);
                        }
                    }
                },
                {
                    xtype: 'b4selectfield',
                    width: 565,
                    name: 'PaymentAgent',
                    fieldLabel: 'Код платежного агента из системы',
                    store: 'B4.store.PaymentAgent',
                    textProperty: 'Code',
                    editable: false,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'CtrMunicipality',
                            flex: 1,
                            text: 'Муниципальный район',
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
                            xtype: 'gridcolumn',
                            dataIndex: 'CtrName',
                            flex: 2,
                            text: 'Контрагент',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'CtrInn',
                            width: 80,
                            text: 'ИНН',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'CtrKpp',
                            width: 80,
                            text: 'КПП',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'CtrOgrn',
                            width: 80,
                            text: 'ОГРН',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Code',
                            width: 80,
                            text: 'Идентификатор',
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'RegexSuccessValue',
                    fieldLabel: 'Значение поля при совпадении'
                },
                {
                    xtype: 'textareafield',
                    name: 'ErrorText',
                    fieldLabel: 'Текст ошибки'
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
                        { xtype: 'tbfill' },
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

        if (!me.phantomColumn) {
            me.items = [useFile].concat(me.items);
        }

        me.callParent(arguments);
    }
});