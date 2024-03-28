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
        'B4.form.SaveCloseToolbar'
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
                    allowBlank: false
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    floating: false,
                    name: 'MethodFormFund',
                    fieldLabel: 'Способ формирования фонда',
                    displayField: 'Display',
                    store: Ext.create('Ext.data.Store', {
                        fields: ['Value', 'Display'],
                        data: [
                            { "Value": 10, "Display": "На счете регионального оператора" },
                            { "Value": 20, "Display": "На специальном счете" }
                        ]
                    }),
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
                    xtype: 'b4saveclosetoolbar'
                }
            ]
        });

        me.callParent(arguments);
    }
});