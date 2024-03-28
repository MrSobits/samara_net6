Ext.define('B4.aspects.fieldrequirement.outdoor.Image', {
    extend: 'B4.aspects.FieldRequirementAspect',
    alias: 'widget.outdoorimagerequirement',

    init: function () {

        this.requirements = [
            
            { name: 'Gkh.RealityObjectOutdoor.Image.DateImage_Rqrd', applyTo: '[name=DateImage]', selector: 'outdoorimageeditwindow' },
            { name: 'Gkh.RealityObjectOutdoor.Image.Name_Rqrd', applyTo: '[name=Name]', selector: 'outdoorimageeditwindow' },
            { name: 'Gkh.RealityObjectOutdoor.Image.ImagesGroup_Rqrd', applyTo: '[name=ImagesGroup]', selector: 'outdoorimageeditwindow' },
            { name: 'Gkh.RealityObjectOutdoor.Image.Period_Rqrd', applyTo: '[name=Period]', selector: 'outdoorimageeditwindow' },
            { name: 'Gkh.RealityObjectOutdoor.Image.WorkCr_Rqrd', applyTo: '[name=WorkCr]', selector: 'outdoorimageeditwindow' },
            { name: 'Gkh.RealityObjectOutdoor.Image.File_Rqrd', applyTo: '[name=File]', selector: 'outdoorimageeditwindow' },
            { name: 'Gkh.RealityObjectOutdoor.Image.Description_Rqrd', applyTo: '[name=Description]', selector: 'outdoorimageeditwindow' }
        ];

        this.callParent(arguments);
    }
});