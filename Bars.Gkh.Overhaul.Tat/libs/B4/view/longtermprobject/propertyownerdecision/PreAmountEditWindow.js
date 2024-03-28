Ext.define('B4.view.longtermprobject.propertyownerdecision.PreAmountEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.longtermdecisionpreamountwindow',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.PropertyOwnerDecisionType',
        'B4.enums.MoOrganizationForm',
        'B4.form.SelectField'
    ],

    modal: true,
    layout: {type: 'vbox', align: 'stretch'},
    width: 700,
    height: 200,
    minHeight: 200,
    maxHeight: 200,
    bodyPadding: 5,
    title: 'Решение',
    closable: false,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 180,
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
                        labelWidth: 180,
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
                    xtype: 'numberfield',
                    name: 'AccumulatedAmount',
                    fieldLabel: 'Накопленная сумма на КР (до 01.04.2014):',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalPrecision: 2,
                    decimalSeparator: ','
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