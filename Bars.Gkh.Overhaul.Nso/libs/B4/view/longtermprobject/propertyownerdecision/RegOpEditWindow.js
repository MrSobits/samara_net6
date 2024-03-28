Ext.define('B4.view.longtermprobject.propertyownerdecision.RegOpEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.longtermdecisionregopwindow',
    requires: [
        'B4.form.SaveCloseToolbar',
        'B4.enums.PropertyOwnerDecisionType',
        'B4.enums.MethodFormFundCr',
        'B4.enums.MoOrganizationForm',
        'B4.form.SelectField'
    ],

    modal: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    height: 220,
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
                    name: 'MethodFormFund',
                    fieldLabel: 'Способ формирования фонда',
                    displayField: 'Display',
                    store: B4.enums.MethodFormFundCr.getStore(),
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