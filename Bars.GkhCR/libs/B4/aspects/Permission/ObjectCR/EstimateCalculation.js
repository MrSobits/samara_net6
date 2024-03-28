Ext.define('B4.aspects.permission.objectcr.EstimateCalculation', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.estimatecalculationobjectcrstateperm',

    permissions: [
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.Edit', applyTo: 'b4savebutton', selector: 'estimatecalcwin' },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.Field.TypeWorkCr', applyTo: '#sfTypeWorkCr', selector: 'estimatecalcwin' },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.Field.EstimateDocument', applyTo: '#fsEstimateDocument', selector: 'estimatecalcwin' },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.Field.ResourceStatmentDocument', applyTo: '#fsResourceStatmentDocument', selector: 'estimatecalcwin' },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.Field.FileEstimateDocument', applyTo: '#fsFileEstimateDocument', selector: 'estimatecalcwin' },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.Field.TotalDirectCost', applyTo: '#dfTotalDirectCost', selector: 'estimatecalcwin' },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.Field.OverheadSum', applyTo: '#dfOverheadSum', selector: 'estimatecalcwin' },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.Field.EstimateProfit', applyTo: '#dfEstimateProfit', selector: 'estimatecalcwin' },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.Field.Nds', applyTo: '#dfNds', selector: 'estimatecalcwin' },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.Field.OtherCost', applyTo: '#dfOtherCost', selector: 'estimatecalcwin' },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.Field.TotalEstimate', applyTo: '#dfTotalEstimate', selector: 'estimatecalcwin' },

        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.Estimate.View', applyTo: 'estimategrid', selector: 'estimatecalcwin' },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.Estimate.Edit', applyTo: '#btnSaveRecs', selector: 'estimategrid' },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.Estimate.Delete', applyTo: 'b4deletecolumn', selector: 'estimategrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.ResStat.IsSumWithoutNds', applyTo: '[name=IsSumWithoutNds]', selector: 'resstatgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.ResStat.View', applyTo: 'resstatgrid', selector: 'estimatecalcwin' },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.ResStat.Edit', applyTo: '#btnSaveRecs', selector: 'resstatgrid' },
        { name: 'GkhCr.ObjectCr.Register.EstimateCalculation.ResStat.Delete', applyTo: 'b4deletecolumn', selector: 'resstatgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});