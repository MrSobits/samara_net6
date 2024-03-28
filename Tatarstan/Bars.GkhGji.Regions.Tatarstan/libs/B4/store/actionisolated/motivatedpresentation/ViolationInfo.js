Ext.define('B4.store.actionisolated.motivatedpresentation.ViolationInfo', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        url: B4.Url.action('GetViolationInfoList', 'MotivatedPresentation')
    },
    fields: [
        'Address',
        'Violations'
    ]
});