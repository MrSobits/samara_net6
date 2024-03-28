Ext.define('B4.view.longtermprobject.propertyownerdecision.MinAmountEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.longtermdecisionminamountwindow',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.PropertyOwnerDecisionType',
        'B4.enums.MoOrganizationForm',
        'B4.form.SelectField'
    ],

    modal: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    height: 250,
    minHeight: 250,
    bodyPadding: 5,
    title: 'Решение',
    closable: false,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 200
            },
            items: [
                {
                    xtype: 'container',
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; text-align: center; line-height: 16px; padding: 5px 5px 5px 30px;',
                    html: '<span style="display: table-cell">' +
                                '<span class="im-info" style="vertical-align: top;">' +
                                '</span>' +
                            '</span>' +
                            '<span style="display: table-cell; text-align: center;">' +
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
                        xtype: 'numberfield',
                        labelAlign: 'right',
                        labelWidth: 200,
                        flex: 1,
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        decimalSeparator: ',',
                        minValue: 0
                    },
                    items: [
                        {
                            name: 'SizeOfPaymentOwners',
                            fieldLabel: 'Размер вноса, установленный собственниками (кв.м.)',
                            allowBlank: false
                        },
                        {
                            name: 'SizeOfPaymentSubject',
                            fieldLabel: 'Размер вноса, установленный субъектом (кв.м.)',
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        labelAlign: 'right',
                        labelWidth: 200,
                        flex: 1
                    },
                    items: [
                        {
                            name: 'PaymentDateStart',
                            fieldLabel: 'Дата начала действия взноса',
                            allowBlank: false
                        },
                        {
                            name: 'PaymentDateEnd',
                            fieldLabel: 'Дата окончания действия взноса'
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