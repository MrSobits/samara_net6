Ext.define('B4.aspects.permission.dict.ZonalInspection', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.zonalinspectiondictperm',

    permissions: [
        { name: 'Gkh.Dictionaries.ZonalInspection.Create', applyTo: 'b4addbutton', selector: '#zonalInspectionGrid' },
        { name: 'Gkh.Dictionaries.ZonalInspection.Edit', applyTo: 'b4savebutton', selector: '#zonalInspectionEditWindow' },
        {
            name: 'Gkh.Dictionaries.ZonalInspection.Delete', applyTo: 'b4deletecolumn', selector: '#zonalInspectionGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },

        { name: 'Gkh.Dictionaries.ZonalInspection.Create', applyTo: 'b4addbutton', selector: '#zonalInspectionMunicipalityGrid' },
        {
            name: 'Gkh.Dictionaries.ZonalInspection.Delete', applyTo: 'b4deletecolumn', selector: '#zonalInspectionMunicipalityGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },

        { name: 'Gkh.Dictionaries.ZonalInspection.Create', applyTo: 'b4addbutton', selector: '#zonalInspectionInspectorsGrid' },
        {
            name: 'Gkh.Dictionaries.ZonalInspection.Delete', applyTo: 'b4deletecolumn', selector: '#zonalInspectionInspectorsGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.Dictionaries.ZonalInspection.Fields.IndexOfGji', applyTo: '[name=IndexOfGji]', selector: '#zonalInspectionEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        }
    ]
});