Ext.define('B4.aspects.fieldrequirement.realityobj.Block', {
    extend: 'B4.aspects.FieldRequirementAspect',
    alias: 'widget.realityobjblockrequirement',
    
    init: function() {
        this.requirements = [
            
            { name: 'Gkh.RealityObject.Block.Fields.Number_Rqrd', applyTo: '[name=Number]', selector: 'blockeditwindow' },
            { name: 'Gkh.RealityObject.Block.Fields.AreaLiving_Rqrd', applyTo: '[name=AreaLiving]', selector: 'blockeditwindow' },
            { name: 'Gkh.RealityObject.Block.Fields.AreaTotal_Rqrd', applyTo: '[name=AreaTotal]', selector: 'blockeditwindow' },
            { name: 'Gkh.RealityObject.Block.Fields.CadastralNumber_Rqrd', applyTo: '[name=CadastralNumber]', selector: 'blockeditwindow' }
        ];

        this.callParent(arguments);
    }
});