Ext.define('B4.view.belaymanorgactivity.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
       'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.ManagingOrganization',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    maximizable: true,
    resizable: true,
    width: 650,
    height: 690,
    bodyPadding: 5,
    itemId: 'belayManOrgActivityEditWindow',
    title: 'Страхование деятельности',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'ManagingOrganization',
                    fieldLabel: 'Управляющая организация',
                    labelWidth: 160,
                    padding: '0 0 5 0',
                    labelAlign: 'right',
                    store: 'B4.store.ManagingOrganization',
                    textProperty: 'ContragentName',
                    allowBlank: false,
                    editable: false,
                    columns: [
                        { text: 'Муниципальный район', dataIndex: 'Municipality', flex: 1,
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
                        { text: 'Наименование УО', dataIndex: 'ContragentShortName', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'ИНН', dataIndex: 'ContragentInn', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'belaypolicygrid',
                    margins: -1,
                    flex:1
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