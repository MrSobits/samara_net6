Ext.define('B4.view.wizard.export.orgregistry.OrgRegistryParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.store.integrations.orgRegistry.Contragent',
        'B4.model.integrations.orgRegistry.Contragent'
    ],
    layout: 'hbox',
    items: [
    {
        xtype: 'b4selectfield',
        itemId: 'contragentsSelect',
        flex:1,
        padding: '10 5 0 0',
        labelWidth: 85,
        labelAlign: 'right',
        textProperty: 'Name',
        selectionMode: 'MULTI',
        fieldLabel: 'Контрагенты',
        store: 'B4.store.integrations.orgRegistry.Contragent',
        model: 'B4.model.integrations.orgRegistry.Contragent',
        columns: [
            {
                text: 'Наименование',
                dataIndex: 'Name',
                flex: 1,
                filter: { xtype: 'textfield' }
            },
            {
                text: 'ОГРН',
                dataIndex: 'Ogrn',
                flex: 1,
                filter: { xtype: 'textfield' }
            },
            {
                text: 'Юридический адрес',
                dataIndex: 'JuridicalAddress',
                flex: 2,
                filter: { xtype: 'textfield' }
            }
        ]
    }],

    firstInit: function () {
        var me = this,
            sf = me.wizard.down('#contragentsSelect');

        sf.on('change', function (field, newValue, oldValue, eOpts) {
            me.fireEvent('selectionchange', me);
        }, me);
    },

    getParams: function () {
        var me = this;

        return {
            selectedList: me.wizard.down('#contragentsSelect').getValue()
        };
    }
});