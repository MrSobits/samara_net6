Ext.define('B4.view.regop.personal_account.PersonalAccountChargeGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [

        'B4.form.ComboBox',

        'B4.store.regop.personal_account.PersonalAccountCharge'
    ],

    title: 'Начисления',

    alias: 'widget.pachargegrid',

    store: 'regop.personal_account.PersonalAccountCharge',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            columns: [
                {
                    text: 'Лицевой счет',
                    dataIndex: 'BasePersonalAccount',
                    flex: 1,
                    renderer: function (value) {
                        if (value) {
                            return value.PersonalAccountNum;
                        }
                        return value;
                    }
                },
                { text: 'Дата начисления', dataIndex: 'ChargeDate', flex: 1 },
                { text: 'Начислено по тарифу', dataIndex: 'ChargeTariff', flex: 1 },
                { text: 'Пени', dataIndex: 'Penalty', flex: 1 },
                { text: 'Перерасчет', dataIndex: 'Recalc', flex: 1 },
                { text: 'Начислено всего', dataIndex: 'Sum', flex: 1 }
            ],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);
    }
});

