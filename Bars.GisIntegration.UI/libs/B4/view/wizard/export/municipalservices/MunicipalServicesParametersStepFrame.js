Ext.define('B4.view.wizard.export.municipalservices.MunicipalServicesParametersStepFrame', {
    extend: 'B4.view.wizard.preparedata.ParametersStepFrame',
    requires: [
        'B4.form.SelectField',
        'B4.ux.grid.plugin.HeaderFilters'
    ],
    layout: 'hbox',
    mixins: ['B4.mixins.window.ModalMask'],
    items: [
    {
        xtype: 'b4selectfield',
        name: 'MunicipalServices',
        flex: 1,
        padding: '10 5 0 10',
        labelWidth: 130,
        labelAlign: 'right',
        textProperty: 'Name',
        selectionMode: 'MULTI',
        fieldLabel: 'Коммунальные услуги',
        store: 'B4.store.integrations.nsi.MunicipalService',
        model: 'B4.model.integrations.nsi.MunicipalService',
        columns: [
            {
                text: 'Наименование',
                dataIndex: 'Name',
                flex: 1,
                filter: { xtype: 'textfield' }
            },
            {
                text: 'Ед. измерения',
                dataIndex: 'UnitMeasure',
                flex: 1,
                filter: { xtype: 'textfield' }
            }
        ]
    }],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('b4selectfield[name=MunicipalServices]').setDisabled(multiSuppliers);
    },

    firstInit: function () {
        var me = this,
            sf = me.wizard.down('b4selectfield[name=MunicipalServices]');
        
        sf.on('change', function () {
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
            selectedList: multiSuppliers ? 'ALL' : me.wizard.down('b4selectfield[name=MunicipalServices]').getValue()
        };
    }
});