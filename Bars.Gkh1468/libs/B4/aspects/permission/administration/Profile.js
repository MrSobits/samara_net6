Ext.define('B4.aspects.permission.administration.Profile', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.profileadminperm',

    permissions: [
        { name: 'Administration.Profile.Edit', applyTo: 'b4savebutton', selector: '#profileSettingEditPanel' }
    ]
});