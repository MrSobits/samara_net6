namespace Bars.Gkh.Domain.Impl
{
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using Authentification;

    public class X509ChainValidator : IX509CertificateValidator
    {
        private IGkhUserManager _userManager;

        public X509ChainValidator(IGkhUserManager userManager)
        {
            _userManager = userManager;
        }

        public bool Validate(X509Certificate2 certificate)
        {
            if (NeedToCheckCertificate())
            {
                return certificate.Verify();
            }

            return true;
        }

        private bool NeedToCheckCertificate()
        {
            var mos = _userManager.GetActiveOperatorMunicipalities();

            return mos.Any(x => x.CheckCertificateValidity);
        }
    }
}