Ext.define('B4.controller.report.HouseTechPassportReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.HouseTechPassportPanel',
    mainViewSelector: '#houseTechPassportPanel',

    requires: [        
        'B4.form.ComboBox'
    ],

    stores: [        
        'B4.ux.button.Update'
    ],
    
    refs: [       
        {
            ref: 'HouseField',
            selector: '#houseTechPassportPanel #sfHouse'
        }
    ],
   
    validateParams: function () {
        var house = this.getHouseField();
        if (!house.isValid()) {
            return "Не указан жилой дом";
        }
        
        return true;
    },

    getParams: function () {
        var house = this.getHouseField();        
        return {            
            house: (house ? house.getValue() : null)
        };
    }
});