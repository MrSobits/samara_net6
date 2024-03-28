Ext.define('B4.view.longtermprobject.propertyownerdecision.OwnerAccountEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.longtermdecisionowneraccountwindow',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        
        'B4.enums.PropertyOwnerDecisionType',
        'B4.enums.MoOrganizationForm',
        'B4.enums.OwnerAccountDecisionType',
        'B4.form.SelectField'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    bodyPadding: 5,
    title: 'Решение',
    closable: false,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; line-height: 16px; padding: 0px 10px 10px 30px;',
                    html: '<span style="display: table-cell">' +
                                '<span class="im-info" style="vertical-align: top;">' +
                                '</span>' +
                            '</span>' +
                            '<span style="display: table-cell">' +
                                '       Текущий договор управления можно посмотреть в разделе "Управление домом"  ' +
                            '</span>'
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    floating: false,
                    name: 'PropertyOwnerDecisionType',
                    fieldLabel: 'Наименование решения',
                    displayField: 'Display',
                    store: B4.enums.PropertyOwnerDecisionType.getStore(),
                    valueField: 'Value',
                    readOnly: true
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    floating: false,
                    name: 'MoOrganizationForm',
                    fieldLabel: 'Способ управления',
                    displayField: 'Display',
                    store: B4.enums.MoOrganizationForm.getStore(),
                    valueField: 'Value',
                    readOnly: true
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 200,
                        flex: 1,
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    items: [
                        {
                            name: 'DateStart',
                            allowBlank: false,
                            fieldLabel: 'Дата начала действия'
                        },
                        {
                            name: 'DateEnd',
                            fieldLabel: 'Дата окончания действия'
                        }
                    ]
                },
                {
                    xtype: 'combobox',
                    name: 'OwnerAccountType',
                    fieldLabel: 'Владелец специального счета',
                    displayField: 'Display',
                    store: B4.enums.OwnerAccountDecisionType.getStore(),
                    valueField: 'Value',
                    flex: 1
                },
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Наименование',
                    name: 'Contragent',
                    store: 'B4.store.OwnerAccountContragent',
                    editable: false,
                    itemId: 'sfContragent',
                    allowBlank: true,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    textProperty: 'Name'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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