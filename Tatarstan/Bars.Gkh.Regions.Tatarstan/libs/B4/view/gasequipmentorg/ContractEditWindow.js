Ext.define('B4.view.gasequipmentorg.ContractEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.gasequipmentorgcontracteditwindow',

    requires: [
        'B4.form.SelectField',
        'B4.view.realityobj.Grid',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.store.gasequipmentorg.Contract',
        'B4.form.FileField'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    closeAction: 'destroy',
    layout: { type: 'vbox', align: 'stretch' },
    minWidth: 800,
    maxWidth: 800,
    width: 800,
    height: 200,
    bodyPadding: 3,
    
    title: 'Договор с жилым домом',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 160
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObject',
                    itemId: 'sfRealityObject',
                    fieldLabel: 'Жилой дом',
                    labelWidth: 150,
                   
                    store: 'B4.store.RealityObject',
                    textProperty: 'Address',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        {
                            text: 'Муниципальный район',
                            dataIndex: 'Municipality',
                            flex: 1,
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
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNum',
                            fieldLabel: 'Номер документа',
                            labelWidth: 150,
                            maxLength: 300
                        }                        
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'StartDate',
                            fieldLabel: 'Дата  начала предоставления услуги',
                            labelWidth: 150,
                            format: 'd.m.Y',
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            name: 'EndDate',
                            fieldLabel: 'Дата  окончания предоставления услуги',
                            labelWidth: 170,
                            format: 'd.m.Y',
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    labelWidth: 150
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});