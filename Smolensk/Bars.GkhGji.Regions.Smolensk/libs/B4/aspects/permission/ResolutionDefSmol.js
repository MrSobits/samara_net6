Ext.define('B4.aspects.permission.ResolutionDefSmol', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.resolutiondefsmolperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */

    //поля только в регионе Смоленска
        { name: 'GkhGji.DocumentsGji.Resolution.Register.Definition.DescriptionSet', applyTo: '[name=DescriptionSet]', selector: '#resolutionDefinitionEditWindow' },
        { name: 'GkhGji.DocumentsGji.Resolution.Register.Definition.DefinitionResult', applyTo: '[name=DefinitionResult]', selector: '#resolutionDefinitionEditWindow' }

    ]
});