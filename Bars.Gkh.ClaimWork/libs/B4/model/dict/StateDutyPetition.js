Ext.define('B4.model.dict.StateDutyPetition', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id' },
        { name: 'StateDuty' },
        { name: 'PetitionToCourtType', mapping: 'PetitionToCourtType.ShortName' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'StateDutyPetition'
    }
});