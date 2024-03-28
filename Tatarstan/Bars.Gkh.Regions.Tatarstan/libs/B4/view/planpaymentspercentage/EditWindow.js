Ext.define('B4.view.planpaymentspercentage.EditWindow',
{
    extend: 'B4.form.Window',

    alias: 'widget.planpaymentspercentagewindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    bodyPadding: 5,

    title: 'Добавление процент по оплате',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeServiceGis',
        'B4.view.Control.GkhDecimalField'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me,
        {
            items: [
                {
                    xtype: 'fieldset',
                    border: null,
                    defaults: {
                        labelWidth: 120,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'hidden',
                            name: 'Id'
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'PublicServiceOrg',
                            fieldLabel: 'Ресурсоснабжающая организации',
                            editable: false,
                            allowBlank: false,
                            store: 'B4.store.PublicServiceOrg',
                            textProperty: 'ContragentName',
                            columns: [
                                {
                                    text: 'Контрагент',
                                    dataIndex: 'ContragentName',
                                    flex: 1,
                                    filter: {
                                        xtype:
                                            'textfield'
                                    }
                                },
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            listeners: {
                                beforeload: function(asp, operation, store) {
                                    operation.params.operatorHasContragent = true;
                                }
                            }
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.dict.Service',
                            selectionMode: 'SINGLE',
                            windowCfg: { modal: true },
                            textProperty: 'Name',
                            labelAlign: 'right',
                            fieldLabel: 'Услуга',
                            editable: false,
                            allowBlank: false,
                            columns: [
                                {
                                    text: 'Код',
                                    dataIndex: 'Code',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'Наименование',
                                    dataIndex: 'Name',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                }
                            ],
                            name: 'Service',
                            listeners: {
                                beforeload: function(asp, options, store) {
                                    options.params.typeGroupServiceDi = B4.enums.TypeServiceGis.Communal;
                                }
                            }
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Resource',
                            fieldLabel: 'Ресурс',
                            editable: false,
                            allowBlank: false,
                            store: 'B4.store.dict.CommunalResource',
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Percentage',
                            fieldLabel: 'Процент оплаты',
                            minValue: 0,
                            hideTrigger: true,
                            allowBlank: false,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateStart',
                            fieldLabel: 'Начало действия',
                            format: 'd.m.Y',
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateEnd',
                            fieldLabel: 'Конец действия',
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