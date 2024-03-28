Ext.define('B4.aspects.permission.specialobjectcr.Qualification', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.qualificationspecialobjectcrperm',

    permissions: [
        { name: 'GkhCr.SpecialObjectCr.Register.Qualification.Create', applyTo: 'b4addbutton', selector: 'specialobjectcrqualgrid' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.Qualification.Edit',
            applyTo: 'b4savebutton',
            selector: 'specialobjectcrqualeditwindow'
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.Qualification.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'specialobjectcrqualgrid',
            applyBy: function(component, allowed) {
                if (allowed)
                    component.show();
                else
                    component.hide();
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.Qualification.Edit',
            applyTo: 'b4deletecolumn',
            selector: 'specialobjectcrqualvoicemembergrid',
            applyBy: function(component, allowed) {
                if (allowed)
                    component.show();
                else
                    component.hide();
            }
        },
        { name: 'GkhCr.SpecialObjectCr.Register.Qualification.Edit', applyTo: 'b4savebutton', selector: 'specialobjectcrqualvoicemembergrid' }
    ]
});