Ext.define('B4.aspects.permission.TypeWorkCr', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.typeworkcrstateperm',

    permissions: [
            { name: 'GkhCr.TypeWorkCr.Edit', applyTo: 'b4savebutton', selector: 'workscreditpanel' },
            { name: 'GkhCr.TypeWorkCr.Field.FinanceSource_Edit', applyTo: '#tfFinanceSource', selector: 'workscreditpanel' },
            { name: 'GkhCr.TypeWorkCr.Field.Year_Edit', applyTo: '#nfYearRepair', selector: 'workscreditpanel' },
            { name: 'GkhCr.TypeWorkCr.Field.Volume_Edit', applyTo: '#nfVolume', selector: 'workscreditpanel' },
            { name: 'GkhCr.TypeWorkCr.Field.Sum_Edit', applyTo: '#nfSum', selector: 'workscreditpanel' }
    ]
});