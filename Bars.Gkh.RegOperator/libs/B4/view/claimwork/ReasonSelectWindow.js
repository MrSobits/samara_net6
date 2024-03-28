Ext.define('B4.view.claimwork.ReasonSelectWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.enums.DebtorState', 
        'B4.form.ComboBox',
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    bodyPadding: 10,
    itemId: 'clwStopReasonEditWindow',
    title: 'Выбор основания',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        var currDebtorState = B4.enums.DebtorState.getItemsWithEmpty([null, '-']);
        var newDebtorState = [];
        Ext.iterate(currDebtorState, function (val, key) {
            if (key == 11 || key == 12)
                newDebtorState.push(val);
        });

        Ext.applyIf(me, {
            items: [
                                
                {
                    xtype: 'b4combobox',
                    items: newDebtorState,
                    name: 'newState',
                    fieldLabel: 'Основание',
                    editable: false,
                    operand: CondExpr.operands.eq,
                    allowBlank: false,
                    valueField: 'Value',
                    itemId: 'cbDebtorState',
                    displayField: 'Display'
                },                
                {
                    xtype: 'button',
                    text: 'Приостановить',
                    tooltip: 'Приостановить ПИР по выбранной причине',
                    iconCls: 'icon-accept',
                    width: 70,
                    itemId: 'btnAccept'
                }
            ]
        });

        me.callParent(arguments);
    }
});