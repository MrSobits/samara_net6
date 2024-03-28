Ext.define('B4.store.actionisolated.motivatedpresentation.InspectionInfo', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        url: B4.Url.action('GetInspectionInfoList', 'MotivatedPresentation')
    },
    fields: [
        'DateStart',
        'DateEnd',
        'Address',
        'InspectionNumber',
        'TimeVisitStart',
        'TimeVisitEnd',
        'InspectionId'
    ]
});