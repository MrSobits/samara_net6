Ext.define('B4.view.activitytsj.AddWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 600,
    bodyPadding: 5,
    itemId: 'activityTsjAddWindow',
    title: 'Деятельность ТСЖ и ЖСК',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.ManagingOrganization',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    itemId: 'sfTsj',
                    name: 'ManagingOrganization',
                    fieldLabel: 'ТСЖ/ЖСК',
                    isGetOnlyIdProperty: false,
                    store: 'B4.store.ManagingOrganization',
                    textProperty: 'ContragentName',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        { text: 'Наименование УО', dataIndex: 'ContragentShortName', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListWithoutPaging'
                            }
                        },
                        { text: 'ИНН', dataIndex: 'ContragentInn', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'textfield',
                    itemId: 'tfJuridicalAddress',
                    name: 'ContragentJuridicalAddress',
                    fieldLabel: 'Юридический адрес',
                    readOnly: true
                },
                {
                    xtype: 'textfield',
                    itemId: 'tfMailingAddress',
                    name: 'ContragentMailingAddress',
                    fieldLabel: 'Почтовый адрес',
                    readOnly: true
                },
                {
                    xtype: 'textfield',
                    itemId: 'tfInn',
                    name: 'ContragentInn',
                    fieldLabel: 'ИНН',
                    readOnly: true
                },
                {
                    xtype: 'textfield',
                    itemId: 'tfKpp',
                    name: 'ContragentKpp',
                    fieldLabel: 'КПП',
                    readOnly: true
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