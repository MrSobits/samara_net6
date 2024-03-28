Ext.define('B4.view.otherservice.editwindow.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    width: 800,
    height: 600,
    bodyPadding: 5,
    title: 'Прочие услуги',
    alias: 'widget.otherserviceeditwindow',
    layout: 'border',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.Control.GkhDecimalField',
        'B4.store.dict.UnitMeasure',
        'B4.view.otherservice.editwindow.ProviderGrid',
        'B4.view.otherservice.editwindow.TariffGrid',
        'B4.store.service.ContragentForProvider'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    region: 'north',
                    height: 230,
                    defaults: {
                        labelWidth: 190,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items:
                        [
                            {
                                xtype: 'fieldset',
                                itemId: 'providerFieldSet',
                                defaults: {
                                    labelWidth: 190,
                                    anchor: '100%',
                                    labelAlign: 'right'
                                },
                                title: 'Поставщик',
                                items: [
                                    {
                                        xtype: 'b4selectfield',
                                        name: 'UnitMeasure',
                                        flex: 1,
                                        fieldLabel: 'Ед. измерения',
                                        anchor: '100%',
                                        editable: false,
                                        store: 'B4.store.dict.UnitMeasure'
                                    },
                                    {
                                        xtype: 'container',
                                        anchor: '100%',
                                        defaults: {
                                            labelWidth: 190,
                                            labelAlign: 'right'
                                        },
                                        layout: {
                                            type: 'hbox',
                                            pack: 'start'
                                        },
                                        items: [
                                            {
                                                xtype: 'b4selectfield',
                                                name: 'Provider',
                                                fieldLabel: 'Поставщик',
                                                isGetOnlyIdProperty: false,
                                                allowBlank: false,
                                                editable: false,

                                                store: 'B4.store.service.ContragentForProvider',
                                                readOnly: true,
                                                flex: 1,
                                                margins: '0 5 5 0',
                                                columns: [
                                                    {
                                                        text: 'Муниципальное образование',
                                                        dataIndex: 'Municipality',
                                                        flex: 1,
                                                        filter: { xtype: 'textfield' }
                                                    },
                                                    {
                                                        text: 'Наименование',
                                                        dataIndex: 'Name',
                                                        flex: 1,
                                                        filter: { xtype: 'textfield' }
                                                    },
                                                    { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                                ]
                                            },
                                            {
                                                xtype: 'button',
                                                name: 'changeProviderButton',
                                                text: 'Изменить',
                                                width: 120,
                                                margins: '0 0 5 0'
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'tbCommunalGrids',
                    region: 'center',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'tariffgrid',
                            margins: -1
                        },
                        {
                            xtype: 'providergrid',
                            margins: -1
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'tbfill'
                                },
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
    },

    closeWindow: function () {
        this.close();
    },
});
