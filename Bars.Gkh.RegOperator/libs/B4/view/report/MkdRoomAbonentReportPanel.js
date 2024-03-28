Ext.define('B4.view.report.MkdRoomAbonentReportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.mkdRoomAbonentReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.MunicipalityForSelect'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
            {
                xtype: 'b4selectfield',
                name: 'Municipality',
                fieldLabel: 'Муниципальное образование',
                itemId: 'sfMunicipality',
                allowBlank: false,
                selectionMode: 'MULTI',
                store: 'B4.store.dict.MunicipalityForSelect',
                onSelectAll: function () {
                    var me = this,
                        oldValue = me.getValue(),
                        isValid = me.getErrors() != '';

                    me.updateDisplayedText('Выбраны все');
                    me.value = 'All';

                    me.fireEvent('validitychange', me, isValid);
                    me.fireEvent('change', me, 'All', oldValue);
                    me.validate();

                    me.selectWindow.hide();
                }
            }
        ]
        });

        me.callParent(arguments);
    }
});