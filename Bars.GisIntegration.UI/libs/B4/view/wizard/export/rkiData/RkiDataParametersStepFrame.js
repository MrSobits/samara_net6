Ext.define('B4.view.wizard.export.rkiData.RkiDataParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.store.integrations.infrastructure.Rki',
        'B4.model.integrations.infrastructure.Rki',
        'B4.ux.grid.filter.YesNo'
    ],
    layout: 'anchor',
    mixins: ['B4.mixins.window.ModalMask'],
    items: [
        {
            xtype: 'b4selectfield',
            name: 'Rki',
            anchor: '100%',
            margin: 5,
            modalWindow: true,
            textProperty: 'Name',
            selectionMode: 'MULTI',
            fieldLabel: 'ОКИ',
            store: 'B4.store.integrations.infrastructure.Rki',
            model: 'B4.model.integrations.infrastructure.Rki',
            columns: [
                {
                    text: 'Наименование',
                    dataIndex: 'Name',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'МО / Адрес',
                    dataIndex: 'Address',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Сфера ОКИ',
                    dataIndex: 'TypeGroupName',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Вид объекта',
                    dataIndex: 'TypeName',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ]
        }
    ],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('b4selectfield[name=Rki]').setDisabled(multiSuppliers);
    },

    firstInit: function () {
        var me = this,
            sf = me.wizard.down('b4selectfield[name=Rki]');

        sf.on('beforeload', function (sf, opts) {
            opts.params.dataSupplierId = me.wizard.dataSupplierIds[0];
        }, me);
    },

    getParams: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;
        return {
            selectedRecords: multiSuppliers ? 'ALL' : me.wizard.down('b4selectfield[name=Rki]').getValue()
        };
    }
});