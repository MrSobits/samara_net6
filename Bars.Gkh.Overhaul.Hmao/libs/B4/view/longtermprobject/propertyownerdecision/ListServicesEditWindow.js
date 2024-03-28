Ext.define('B4.view.longtermprobject.propertyownerdecision.ListServicesEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.longtermdecisionlistserviceswindow',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.PropertyOwnerDecisionType',
        'B4.enums.MoOrganizationForm',
        'B4.form.SelectField',
        'B4.view.longtermprobject.propertyownerdecision.work.ListServicesWorksGrid'
    ],

    modal: true,
    layout: 'form',
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
                        labelWidth: 200,
                        flex: 1,
                        xtype: 'datefield',
                        labelAlign: 'right',
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
                    xtype: 'listservicesworksgrid',
                    height: 400
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