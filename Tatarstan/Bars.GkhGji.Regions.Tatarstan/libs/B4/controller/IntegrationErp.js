Ext.define('B4.controller.IntegrationErp', {
    extend: 'B4.controller.BaseIntegration',

    models: [
        'integrationerp.TatarstanDisposal'
    ],

    stores: [
        'integrationerp.TatarstanDisposal'
    ],

    views: [
        'integrationerp.TatarstanDisposalGrid',
        'integrationerp.TatarstanDisposalWindow'
    ],

    mainView: 'integrationerp.TatarstanDisposalGrid',
    mainViewSelector: 'tatarstandisposalgrid',

    editFormSelector: 'tatarstandisposalwindow',
    editWindowView: 'integrationerp.TatarstanDisposalWindow',
    modelAndStoreName: 'integrationerp.TatarstanDisposal'
});