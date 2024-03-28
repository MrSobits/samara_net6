Ext.define('B4.aspects.permission.rapidresponsesystem.AppealRequirement', {
    extend: 'B4.aspects.FieldRequirementAspect',

    alias: 'widget.appealrequirement',
    
    requirements: [
        { name: 'CitizenAppealModule.RapidResponseSystem.Field.ResponseDate', applyTo: '[name=ResponseDate]', selector: '#appealResponsePanel' },
        { name: 'CitizenAppealModule.RapidResponseSystem.Field.Theme', applyTo: '[name=Theme]', selector: '#appealResponsePanel' },
        { name: 'CitizenAppealModule.RapidResponseSystem.Field.Response', applyTo: '[name=Response]', selector: '#appealResponsePanel' },
        { name: 'CitizenAppealModule.RapidResponseSystem.Field.ResponseFile', applyTo: '[name=ResponseFile]', selector: '#appealResponsePanel' },
        { name: 'CitizenAppealModule.RapidResponseSystem.Field.CarriedOutWork', applyTo: '[name=CarriedOutWork]', selector: '#appealResponsePanel' },
    ]
});