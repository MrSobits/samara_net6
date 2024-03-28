Ext.define('B4.view.decision.CrWorkTime', {
    extend: 'Ext.form.Panel',

    decisionTypeCode: '',

    defaults: {
        labelWidth: 200
    },
    border: false,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    minHeight: 200,

    initComponent: function() {
        Ext.apply(this, {
            items: [
                {
                    xtype: 'hidden',
                    name: 'byTypeCode',
                    value: true
                },
                {
                    xtype: 'datefield',
                    name: 'StartDate',
                    fieldLabel: 'Дата ввода в действие решения',
                    hideTrigger: (this.saveable === false)
                }
            ]
        });

        this.callParent(arguments);
    },

    afterShow: function() {
        this.add({
            xtype: 'grid',
            flex: 1,
            columns: [
                { text: 'Вид работы', dataIndex: 'Work', flex: 1 },
                { text: 'Установленная норма', dataIndex: 'Norm', width: 100 },
                { text: 'Принятое решение', dataIndex: 'Decision', width: 100 }
            ]
        });
    }
});