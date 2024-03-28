Ext.define('B4.DisposalTextValues', {

    singleton: true,
    loadedCss: {},

    //Именительный падеж 
    getSubjectiveCase: function () {
        return 'Распоряжение';
    },

    //Именительный падеж Приказ напроверку предписания 
    getSubjectiveForPrescriptionCase: function () {
        return 'Распоряжение на проверку предписания';
    },

    //Именительный множественный падеж 
    getSubjectiveManyCase: function () {
        return 'Распоряжения';
    },

    //Родительный множественный падеж 
    getGenetiveManyCase: function () {
        return 'Распоряжений';
    },
    
    //Дательный падеж 
    getDativeCase: function () {
        return 'Распоряжению';
    }
});