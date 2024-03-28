Ext.define('B4.view.wizard.export.capitalrepair.CapitalRepairContractsStepFrame', {
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
             name: 'CrPlan',
             flex: 1,
             anchor: '100%',
             labelWidth: 60,
             labelAlign: 'right',
             modalWindow: true,
             textProperty: 'Name',
             itemId: 'CrPlanSelect',
             selectionMode: 'MULTI',
             fieldLabel: 'Планы',
             store: 'B4.store.integrations.capitalrepair.CrPlan',
             model: 'B4.model.integrations.capitalrepair.CrPlan',
             columns: [
                 {
                     text: 'Наименование',
                     dataIndex: 'Name',
                     flex: 1,
                     filter: { xtype: 'textfield' }
                 },
                 {
                     text: 'Муниципальное образование',
                     dataIndex: 'MunicipalityName',
                     flex: 1,
                     filter: { xtype: 'textfield' }
                 },
                 {
                     type: 'datecolumn',
                     format: 'd.m.Y',
                     text: 'Месяц и год начала',
                     dataIndex: 'StartMonthYear',
                     flex: 0.5,
                     filter: { xtype: 'datefield', format: 'd.m.Y' }
                 }
             ]
         }
    ],

    init: function () {
        var me = this,
            multiSuppliers = me.wizard.dataSupplierIds && me.wizard.dataSupplierIds.length > 1;

        me.wizard.down('#CrPlanSelect').setDisabled(multiSuppliers);
    },

    firstInit: function () {
        var me = this,
            sf = me.wizard.down('#CrPlanSelect');
        
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
            selectedList: multiSuppliers ? 'ALL' : me.wizard.down('#CrPlanSelect').getValue()
        };
    }
});