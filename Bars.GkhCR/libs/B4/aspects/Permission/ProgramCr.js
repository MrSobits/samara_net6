Ext.define('B4.aspects.permission.ProgramCr', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.programcrperm',

    permissions: [
        { name: 'GkhCr.ProgramCr.Create', applyTo: 'b4addbutton', selector: '#programCrGrid' },
        { name: 'GkhCr.ProgramCr.Edit', applyTo: 'b4savebutton', selector: '#programCrEditWindow' },
        { name: 'GkhCr.ProgramCr.Delete', applyTo: 'b4deletecolumn', selector: '#programCrGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ProgramCr.ChangeJournal.View', applyTo: '[panelName=ChangeJournal]', selector: 'programCrEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.tab.show();
                else component.tab.hide();
            }
        },
        {
            name: 'GkhCr.ProgramCr.AddWorkFromLongProgram.View', applyTo: '[name=AddWorkFromLongProgram]', selector: 'programCrEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ProgramCr.Field.NormativeDoc_View', applyTo: '[name=NormativeDoc]', selector: 'programCrEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ProgramCr.Field.File_View', applyTo: '[name=File]', selector: 'programCrEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.ProgramCr.Field.UseForReformaAndGisGkhReports', applyTo: '[name=UseForReformaAndGisGkhReports]', selector: 'programCrEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.ProgramCr.Field.NormativeDoc_Edit', applyTo: '[name=NormativeDoc]', selector: 'programCrEditWindow' },
        { name: 'GkhCr.ProgramCr.Field.File_Edit', applyTo: '[name=File]', selector: 'programCrEditWindow' }
    ]
});