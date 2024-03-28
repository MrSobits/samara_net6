Ext.define('B4.aspects.permission.typeworkcr.EstimateCalculation', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.estimatecalculationtypeworkcrstateperm',

    permissions: [
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.Edit', applyTo: 'b4savebutton', selector: 'estimatecalcwin' },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.TypeWorkCr', applyTo: '#sfTypeWorkCr', selector: 'estimatecalcwin' },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.EstimateDocument', applyTo: '#fsEstimateDocument', selector: 'estimatecalcwin' },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.ResourceStatmentDocument', applyTo: '#fsResourceStatmentDocument', selector: 'estimatecalcwin' },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.FileEstimateDocument', applyTo: '#fsFileEstimateDocument', selector: 'estimatecalcwin' },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.TotalDirectCost', applyTo: '#dfTotalDirectCost', selector: 'estimatecalcwin' },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.OverheadSum', applyTo: '#dfOverheadSum', selector: 'estimatecalcwin' },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.EstimateProfit', applyTo: '#dfEstimateProfit', selector: 'estimatecalcwin' },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.Nds', applyTo: '#dfNds', selector: 'estimatecalcwin' },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.OtherCost', applyTo: '#dfOtherCost', selector: 'estimatecalcwin' },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.Field.TotalEstimate', applyTo: '#dfTotalEstimate', selector: 'estimatecalcwin' },

        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.Estimate.View', applyTo: 'estimategrid', selector: 'estimatecalcwin' },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.Estimate.Edit', applyTo: '#btnSaveRecs', selector: 'estimategrid' },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.Estimate.Delete', applyTo: 'b4deletecolumn', selector: 'estimategrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.ResStat.IsSumWithoutNds', applyTo: '[name=IsSumWithoutNds]', selector: 'resstatgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.ResStat.View', applyTo: 'resstatgrid', selector: 'estimatecalcwin' },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.ResStat.Edit', applyTo: '#btnSaveRecs', selector: 'resstatgrid' },
        { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.ResStat.Delete', applyTo: 'b4deletecolumn', selector: 'resstatgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});