Ext.define('B4.aspects.permission.dict.ResettlementProgram', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.resettlementprogramdictperm',

    permissions: [
        { name: 'Gkh.Dictionaries.ResettlementProgram.Create', applyTo: 'b4addbutton', selector: '#resettlementProgramGrid' },
        { name: 'Gkh.Dictionaries.ResettlementProgram.Edit', applyTo: 'b4savebutton', selector: '#resettlementProgramEditWindow' },
        { name: 'Gkh.Dictionaries.ResettlementProgram.Delete', applyTo: 'b4deletecolumn', selector: '#resettlementProgramGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});