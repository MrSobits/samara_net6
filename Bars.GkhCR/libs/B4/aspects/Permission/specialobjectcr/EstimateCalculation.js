Ext.define('B4.aspects.permission.specialobjectcr.EstimateCalculation', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.estimatecalcspecialobjectcrstateperm',

    permissions: [ 
        { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.Edit', applyTo: 'b4savebutton', selector: 'specialobjectcrestimatecalcwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.TypeWorkCr', applyTo: 'b4selectfield[name=TypeWorkCr]', selector: 'specialobjectcrestimatecalcwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.EstimateDocument', applyTo: 'fieldset[name=EstimateDocument]', selector: 'specialobjectcrestimatecalcwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.ResourceStatmentDocument', applyTo: 'fieldset[name=ResourceStatmentDocument]', selector: 'specialobjectcrestimatecalcwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.FileEstimateDocument', applyTo: 'fieldset[name=FileEstimateDocument]', selector: 'specialobjectcrestimatecalcwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.TotalDirectCost', applyTo: 'gkhdecimalfield[name=TotalDirectCost]', selector: 'specialobjectcrestimatecalcwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.OverheadSum', applyTo: 'gkhdecimalfield[name=OverheadSum]', selector: 'specialobjectcrestimatecalcwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.EstimateProfit', applyTo: 'gkhdecimalfield[name=EstimateProfit]', selector: 'specialobjectcrestimatecalcwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.Nds', applyTo: 'gkhdecimalfield[name=Nds]', selector: 'specialobjectcrestimatecalcwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.OtherCost', applyTo: 'gkhdecimalfield[name=OtherCost]', selector: 'specialobjectcrestimatecalcwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.Field.TotalEstimate', applyTo: 'gkhdecimalfield[name=TotalEstimate]', selector: 'specialobjectcrestimatecalcwin' },

        { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.Estimate.View', applyTo: 'specialobjectcrestimategrid', selector: 'specialobjectcrestimatecalcwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.Estimate.Edit', applyTo: 'button[actionName=btnSaveRecs]', selector: 'specialobjectcrestimategrid' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.Estimate.Delete', applyTo: 'b4deletecolumn', selector: 'specialobjectcrestimategrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.ResStat.IsSumWithoutNds', applyTo: '[name=IsSumWithoutNds]', selector: 'specialobjectcrresstatgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.ResStat.View', applyTo: 'specialobjectcrresstatgrid', selector: 'specialobjectcrestimatecalcwin' },
        { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.ResStat.Edit', applyTo: 'button[actionName=btnSaveRecs]', selector: 'specialobjectcrresstatgrid' },
        {
            name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.ResStat.Delete', applyTo: 'b4deletecolumn', selector: 'specialobjectcrresstatgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});