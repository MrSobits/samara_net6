Ext.define('B4.view.longtermprobject.propertyownerdecision.AddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.longtermdecisionaddwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 700,
    minWidth: 700,
    minHeight: 170,
    maxHeight: 170,
    bodyPadding: 5,
    title: 'Выбор решения',
    closable: false,
    requires: [
        'B4.enums.PropertyOwnerDecisionType',
        'B4.enums.MethodFormFundCr',
        'B4.enums.MoOrganizationForm',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right'
            },
            items: [
                //{
                //    xtype: 'container',
                //    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; line-height: 16px; padding: 0px 10px 10px 30px;',
                //    html: '<span style="display: table-cell">' +
                //        '<span class="im-info" style="vertical-align: top;">' +
                //        '</span>' +
                //        '</span>' +
                //        '<span style="display: table-cell">' +
                //        '       Текущий договор управления можно посмотреть в разделе "Управление домом"  ' +
                //        '</span>'
                //},
                {
                    xtype: 'combobox',
                    editable: false,
                    floating: false,
                    name: 'PropertyOwnerDecisionType',
                    fieldLabel: 'Наименование решения',
                    displayField: 'Display',
                    store: B4.enums.PropertyOwnerDecisionType.getStore(),
                    valueField: 'Value',
                    allowBlank: false
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
                    allowBlank: false,
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