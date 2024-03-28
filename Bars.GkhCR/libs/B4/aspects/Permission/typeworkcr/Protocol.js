Ext.define('B4.aspects.permission.typeworkcr.Protocol', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.protocoltypeworkcrperm',

    permissions: [
        { name: 'GkhCr.TypeWorkCr.Register.Protocol.Create', applyTo: 'b4addbutton', selector: '#protocolCrGrid' },
        { name: 'GkhCr.TypeWorkCr.Register.Protocol.Edit', applyTo: 'b4savebutton', selector: 'objectcrprotocolwin' },
        { name: 'GkhCr.TypeWorkCr.Register.Protocol.Delete', applyTo: 'b4deletecolumn', selector: '#protocolCrGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        
        { name: 'GkhCr.TypeWorkCr.Register.Protocol.SumActVerificationOfCosts', applyTo: '#nfSumActVerificationOfCosts', selector: 'objectcrprotocolwin' },
        {
            name: 'GkhCr.TypeWorkCr.Register.Protocol.TypeWork.View', applyTo: 'tab[text="Виды работ"]', selector: 'objectcrprotocolwin',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});