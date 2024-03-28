Ext.define('B4.aspects.fieldrequirement.realityobj.Image', {
    extend: 'B4.aspects.FieldRequirementAspect',
    alias: 'widget.realityobjimagerequirement',
    
    init: function () {

        this.requirements = [
            {
                name: 'Gkh.RealityObject.Image.Fields.File_Rqrd',
                applyTo: 'b4filefield[name=File]',
                selector: 'realityobjimageeditwindow',
                applyBy: function (component, required) {
                    if (component) {

                        component.allowBlank = !required;

                        if (component.getValue && Ext.isEmpty(component.getValue())) {

                            if (!component.allowBlank) {
                                component.markInvalid('Это поле обязательно для заполнения');
                            } else {
                                component.clearInvalid();
                            }

                        }
                    }
                }
            }
        ];

        this.callParent(arguments);
    }
});