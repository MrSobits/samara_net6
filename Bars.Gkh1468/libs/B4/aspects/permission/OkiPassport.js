Ext.define('B4.aspects.permission.OkiPassport', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.okipassportperm',

    permissions: [
        { name: 'Gkh1468.Dictionaries.Municipality.Create', applyTo: 'b4addbutton', selector: '#municipalityGrid' },
        { name: 'Gkh1468.Dictionaries.Municipality.Edit', applyTo: 'b4savebutton', selector: '#municipalityEditWindow' }
    ]
});