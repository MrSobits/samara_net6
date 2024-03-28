Ext.define('B4.controller.IntegrationErknm', {
    extend: 'B4.controller.BaseIntegration',

    requires: [
        'B4.aspects.ButtonDataExport'
    ],

    models: [
        'integrationerknm.Document'
    ],

    stores: [
        'integrationerknm.Document'
    ],

    views: [
        'integrationerknm.DocumentGrid',
        'integrationerknm.DocumentWindow'
    ],

    mainView: 'integrationerknm.DocumentGrid',
    mainViewSelector: 'integrationerknmgrid',

    editFormSelector: 'integrationerknmdocumentwindow',
    editWindowView: 'integrationerknm.DocumentWindow',
    modelAndStoreName: 'integrationerknm.Document',

    constructor: function (config) {
        var me = this;

        me.aspects.push(
            {
                xtype: 'b4buttondataexportaspect',
                name: 'buttonExportAspect',
                gridSelector: 'integrationerknmgrid',
                buttonSelector: 'integrationerknmgrid #btnExport',
                controllerName: 'ErknmIntegration',
                actionName: 'ExcelFileExport'
            }
        )

        me.callParent(arguments);
    }
});