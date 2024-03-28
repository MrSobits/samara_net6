// TODO : Заменить библиотеку
using System;
using System.IO;

namespace Bars.GkhGji.Utils.ExternalSignatureContainer
{
    // TODO: Заменить 
   // public class SimpleExternalSignatureContainer : IExternalSignatureContainer
    public class SimpleExternalSignatureContainer
    {
        private readonly byte[] _signedBytes;

        public SimpleExternalSignatureContainer(byte[] signedBytes)
        {
            _signedBytes = signedBytes;
        }

        public byte[] Sign(Stream data)
        {
            return _signedBytes;
        }
/*
        public void ModifySigningDictionary(PdfDictionary signDic)
        {
            throw new NotImplementedException();
        }*/
    }
}
