Ext.define('B4.view.dict.municipality.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'fit',
    width: 650,
    //height: 415,
    minHeight: 415,
    maximizable: true,
    resizable: true,
    itemId: 'municipalityEditWindow',
    title: 'Муниципальное образование',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.municipality.SourceFinancingGrid'
    ],

    initComponent: function () {
        var me = this;

        this.cbCity = Ext.create('B4.form.ComboBox', {
            storeAutoLoad: false,
            itemId: 'municipalityFiasComboBox',
            labelAlign: 'right',
            fieldLabel: 'Муниципальное образование',
            emptyText: 'Введите наименование адресного объекта...',
            anchor: '100%',
            typeAhead: false,
            fields: ['GuidId', 'Code', 'Name', 'AddressName', 'PostCode'],
            url: '/Fias/GetPlacesList',
            mode: 'remote',
            valueField: 'GuidId',
            displayField: 'AddressName',
            triggerAction: 'query',
            minChars: 2,
            autoSelect: true,
            queryDelay: 500,
            queryParam: 'filter',
            loadingText: 'Загрузка...',
            hideTrigger: true,
            triggersConfig: [{ iconCls: 'x-form-clear-trigger', qtip: 'Очистить' }],
            selectOnFocus: false,
            listeners: {
                triggerclick: {
                    fn: function () {
                        this.cbCity.clearValue();
                    },
                    scope: this
                }
            }
        });

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    layout: 'vbox',
                    border: false,
                    margins: -1,
                    height: 500,
                    items: [
                        {
                            xtype: 'panel',
                            flex: 1,
                            margins: -1,
                            border: false,
                            frame: true,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Общие сведения',
                            padding: '10 10 0 10',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Name',
                                    fieldLabel: 'Наименование',
                                    labelAlign: 'right',
                                    labelWidth: 110,
                                    maxLength: 300,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'fieldset',
                                    flex: 1,
                                    title: 'Реквизиты',
                                    layout: { type: 'vbox', align: 'stretch' },
                                    defaults: {
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Group',
                                            fieldLabel: 'Группа',
                                            maxLength: 30
                                        },
                                        this.cbCity,
                                        {
                                            xtype: 'textfield',
                                            name: 'Cut',
                                            fieldLabel: 'Сокращение',
                                            maxLength: 10
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Code',
                                            fieldLabel: 'Код',
                                            maxLength: 30
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'FederalNumber',
                                            fieldLabel: 'Федеральный номер',
                                            maxLength: 30
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Okato',
                                            fieldLabel: 'ОКАТО',
                                            maxLength: 30
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Oktmo',
                                            fieldLabel: 'OKTMO',
                                            maxLength: 30
                                        },
                                        {
                                            xtype: 'textareafield',
                                            padding: '10 0 0 0',
                                            height: 50,
                                            name: 'Description',
                                            fieldLabel: 'Описание/ комментарий',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'checkbox',
                                            name: 'CheckCertificateValidity',
                                            boxLabel: 'Проверять корректность сертификата ЭЦП',
                                            margin: '5 0 5 105'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'sourcefingrid',
                            margins: -1,
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