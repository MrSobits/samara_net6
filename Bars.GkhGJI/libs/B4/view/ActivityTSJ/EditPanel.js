Ext.define('B4.view.activitytsj.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    itemId: 'activityTsjEditPanel',
    title: 'Общие сведения',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.manorg.Grid',
        'B4.store.ManagingOrganization',
        'B4.store.ActivityTsj',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Общие сведения',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        anchor: '100%'
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
                            allowBlank: false,
                            editable: false,
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
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});