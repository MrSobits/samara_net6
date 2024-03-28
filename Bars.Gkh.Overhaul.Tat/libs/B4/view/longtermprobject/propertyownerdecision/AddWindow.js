Ext.define('B4.view.longtermprobject.propertyownerdecision.AddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.longtermdecisionaddwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    minWidth: 600,
    minHeight: 195,
    maxHeight: 195,
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
                labelWidth: 180,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; line-height: 16px; padding: 0px 10px 10px 30px;',
                    html:   '<span style="display: table-cell">' +
                                '<span class="im-info" style="vertical-align: top;">' +
                                '</span>' +
                            '</span>' +
                            '<span style="display: table-cell;">' +
                                '       Текущий договор управления можно посмотреть в разделе "Управление домом"  ' +
                            '</span>'
                },
                {
                    xtype: 'b4combobox',
                    editable: false,
                    floating: false,
                    allowBlank: false,
                    displayField: 'Name',
                    valueField: 'Id',
                    name: 'PropertyOwnerDecisionType',
                    fieldLabel: 'Наименование решения',
                    url: '/PropertyOwnerDecisionWork/PropertyOwnerDecisionTypeList'
                },
                {
                    xtype: 'b4combobox',
                    editable: false,
                    floating: false,
                    allowBlank: false,
                    valueField: 'Value',
                    displayField: 'Display',
                    name: 'MethodFormFund',
                    fieldLabel: 'Способ формирования фонда',
                    url: '/PropertyOwnerDecisionWork/MethodFormFundCrTypeList'
                    //store: Ext.create('Ext.data.Store', {
                    //    fields: ['Value', 'Display'],
                    //    data: [
                    //        { "Value": 10, "Display": "На счете регионального оператора" },
                    //        { "Value": 20, "Display": "На специальном счете" }
                    //    ]
                    //})
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    floating: false,
                    allowBlank: false,
                    valueField: 'Value',
                    displayField: 'Display',
                    name: 'MoOrganizationForm',
                    fieldLabel: 'Способ управления',
                    store: B4.enums.MoOrganizationForm.getStore(),
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