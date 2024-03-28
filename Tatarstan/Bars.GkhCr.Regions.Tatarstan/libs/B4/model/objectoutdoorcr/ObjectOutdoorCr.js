Ext.define('B4.model.objectoutdoorcr.ObjectOutdoorCr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ObjectOutdoorCr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DateEndBuilder' },
        { name: 'DateStartWork' },
        { name: 'DateAcceptGji' },
        { name: 'DateStopWorkGji' },
        { name: 'DateGjiReg' },
        { name: 'DateEndWork' },
        { name: 'СommissioningDate' },
        { name: 'SumDevolopmentPsd' },
        { name: 'SumSmr' },
        { name: 'SumSmrApproved' },
        { name: 'Description' },
        { name: 'MaxAmount' },
        { name: 'FactAmountSpent' },
        { name: 'FactStartDate' },
        { name: 'FactEndDate' },
        { name: 'WarrantyEndDate' },
        { name: 'RealityObjects' },
        { name: 'GjiNum' },
        { name: 'Municipality' },
        { name: 'MunicipalityName' },
        { name: 'RealityObjectOutdoorProgram', defaultValue: null },
        { name: 'OutdoorProgramName' },
        { name: 'BeforeDeleteOutdoorProgramName' },
        { name: 'BeforeDeleteRealityObjectOutdoorProgram', defaultValue: null  },
        { name: 'RealityObjectOutdoor', defaultValue: null },
        { name: 'RealityObjectOutdoorName' },
        { name: 'RealityObjectOutdoorCode' },
        { name: 'State', defaultValue: null }
    ]
});