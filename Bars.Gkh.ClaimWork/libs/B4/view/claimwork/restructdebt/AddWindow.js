Ext.define('B4.view.claimwork.restructdebt.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],

    bodyPadding: 10,
    resizable: false,
    closeAction: 'destroy',
    overflowY: 'auto',

    title: 'Добавление графика реструктуризации',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.MonthPicker',
        'B4.form.SelectField',
        'B4.model.claimwork.AccountDetail',
    ],

    alias: 'widget.restructdebtaddwindow',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.base.Store', {
                model: 'B4.model.claimwork.AccountDetail',
                autoLoad: false,
                proxy: Ext.create('B4.base.Proxy', {
                    controllerName: 'RestructDebtSchedule',
                    listAction: 'ListAccountInfo',
                    createAction: '',
                    readAction: '',
                    updateAction: '',
                    destroyAction: ''
                })
            });

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                width: 500,
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'PersonalAccount',
                    fieldLabel: 'Номер лицевого счета',
                    store: store,
                    selectionMode: 'SINGLE',
                    flex: 1,
                    idProperty: 'AccountId',
                    textProperty: 'PersonalAccountNum',
                    columns: [
                        {
                            dataIndex: 'PersonalAccountNum',
                            header: 'Наименование',
                            flex: 1.5,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            dataIndex: 'RoomAddress',
                            header: 'Адрес',
                            flex: 2,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            dataIndex: 'Municipality',
                            header: 'Муниципальный район',
                            flex: 2,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    windowCfg: {
                        width: 800,
                        title: 'Выбор лицевого счета'
                    }

                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    name: 'TotalDebtSum',
                    readOnly: false,
                    allowBlank: true,
                    fieldLabel: 'Сумма долга лицевого счета'
                },
                {
                    xtype: 'b4monthpicker',
                    name: 'StartDate',
                    fieldLabel: 'Дата с',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4monthpicker',
                    name: 'EndDate',
                    fieldLabel: 'Дата по',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'numberfield',
                    name: 'PaymentDay',
                    minValue: 1,
                    maxValue: 31,
                    fieldLabel: 'Допустимый срок оплаты'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
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
                            items: [
                                {
                                    xtype: 'b4closebutton',
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