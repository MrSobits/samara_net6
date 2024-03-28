Ext.define('B4.view.longtermprobject.propertyownerdecision.MinAmountEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.longtermdecisionminamountwindow',
    requires: [
        'B4.form.SaveCloseToolbar',
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
    minHeight: 245,
    maxHeight: 245,
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
                        '<span style="display: table-cell;">' +
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
                        xtype: 'datefield',
                        labelAlign: 'right',
                        format: 'd.m.Y',
                        labelWidth: 200,
                        flex: 1
                    },
                    items: [
                        {
                            name: 'PaymentDateStart',
                            allowBlank: false,
                            fieldLabel: 'Дата начала действия'
                        },
                        {
                            name: 'PaymentDateEnd',
                            fieldLabel: 'Дата окончания действия'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 200,
                        flex: 1,
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        decimalSeparator: ',',
                        minValue: 0,
                        xtype: 'numberfield'
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
                }
            ],
            dockedItems: [
                {
                    xtype: 'b4saveclosetoolbar'
                }
            ]
        });

        me.callParent(arguments);
    }
});