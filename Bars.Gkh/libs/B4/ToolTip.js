// Override for adding tooltips to form fields
Ext.override(Ext.form.Field, {
    afterRender: function () {
        this.callParent(arguments);
        try {
            if (this.qtipText) {
                create_tooltip_fields(this, this.qtipText);
            }
        } catch (e) { }
    }
});

function create_tooltip_fields(field, tooltipText) {
    Ext.QuickTips.register({
        target: field.getEl(),
        title: '',
        text: '<span style="">' + tooltipText + '</span>',
        enabled: true,
        trackMouse: true
    });
    var label = findLabel(field);
    if (label) {
        Ext.QuickTips.register({
            target: label,
            title: '',
            text: '<span style="">' + tooltipText + '</span>',
            enabled: true,
            trackMouse: true
        });
    }
}

var findLabel = function (field) {

    var wrapDiv = null;
    var label = null;

    // find form-element and label
    wrapDiv = field.getEl().up('div.x-form-element');
    if (wrapDiv)
        label = wrapDiv.child('label');

    if (label)
        return label;

    // find form-item and label
    wrapDiv = field.getEl().up('div.x-form-item');
    if (wrapDiv)
        label = wrapDiv.child('label');

    if (label)
        return label;
}