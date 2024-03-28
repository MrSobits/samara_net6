Ext.define('B4.DisposalTextValues', {

    singleton: true,
    loadedCss: {},

    //Именительный падеж 
    getSubjectiveCase: function () {
        return 'Приказ';
    },

    //Именительный падеж Приказ напроверку предписания 
    getSubjectiveForPrescriptionCase: function () {
        return 'Приказ на проверку предписания';
    },

    //Именительный множественный падеж 
    getSubjectiveManyCase: function () {
        return 'Приказы';
    },

    //Родительный множественный падеж 
    getGenetiveManyCase: function () {
        return 'Приказов';
    },
    
    //Дательный падеж 
    getDativeCase: function () {
        return 'Приказу';
    }
});