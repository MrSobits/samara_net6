﻿Ext.define('B4.view.wizard.export.publicPropertyContractData.PublicPropertyContractDataParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.store.integrations.houseManagement.PublicPropertyContract',
        'B4.model.integrations.houseManagement.PublicPropertyContract'
    ],
    layout: 'hbox',
    items: [
    {
        xtype: 'b4selectfield',
        itemId: 'contractsSelect',
        flex:1,
        padding: '10 5 0 10',
        labelWidth: 130,
        labelAlign: 'right',
        textProperty: 'DocNum',
        selectionMode: 'MULTI',
        fieldLabel: 'Договоры на пользование общим имуществом',
        store: 'B4.store.integrations.houseManagement.PublicPropertyContract',
        model: 'B4.model.integrations.houseManagement.PublicPropertyContract',
        columns: [
            {
                text: 'Адрес дома',
                dataIndex: 'Address',
                flex: 1,
                filter: { xtype: 'textfield' }
            },
            {
                xtype: 'datecolumn',
                text: 'Дата начала',
                dataIndex: 'DateStart',
                format: 'd.m.Y',
                flex: 1,
                filter: {
                    xtype: 'datefield',
                    operand: CondExpr.operands.eq
                }
            },
            {
                xtype: 'datecolumn',
                text: 'Дата окончания',
                dataIndex: 'DateEnd',
                format: 'd.m.Y',
                flex: 1,
                filter: {
                    xtype: 'datefield',
                    operand: CondExpr.operands.eq
                }
            },
            {
                text: 'Номер договора',
                dataIndex: 'ContractNumber',
                flex: 1,
                filter: {xtype:'textfield'}
            },
            //{
            //    text: 'Стороны договора',
            //    dataIndex: 'Organization',
            //    flex: 1,
            //    filter: { xtype: 'textfield' }
            //}
        ]
    }],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('#contractsSelect').setDisabled(multiSuppliers);
    },

    firstInit: function () {
        var me = this,
            sf = me.wizard.down('#contractsSelect');
        
        sf.on('change', function (field, newValue, oldValue, eOpts) {
            me.fireEvent('selectionchange', me);
        }, me);

        sf.on('beforeload', function (sf, opts) {
            opts.params.dataSupplierId = me.wizard.dataSupplierIds[0];
        }, me);
    },

    getParams: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;
        return {
            selectedList: multiSuppliers ? 'ALL' : me.wizard.down('#contractsSelect').getValue()
        };
    }
});