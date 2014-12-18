using System.Security.Cryptography;

namespace QuikSharp.Starter
{
    internal static class RsaUtilities
    {
        public static string GenKeySaveInContainer(string containerName)
        {
            // Create the CspParameters object and set the key container 
            // name used to store the RSA key pair.
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = containerName;
            cp.KeyNumber = 1;

            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container.
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024, cp);

            return rsa.ToXmlString(true);
        }

        public static string GetKeyFromContainer(string containerName, bool includePrivateParameters = true)
        {
            // Create the CspParameters object and set the key container 
            // name used to store the RSA key pair.
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = containerName;
            cp.KeyNumber = 1;
            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container.
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024, cp);

            return rsa.ToXmlString(includePrivateParameters);
        }

        public static RSACryptoServiceProvider GetRSAProviderFromContainer(string containerName, bool machineStore = false)
        {
            // Create the CspParameters object and set the key container 
            // name used to store the RSA key pair.
            CspParameters cp = new CspParameters();
            if (machineStore) cp.Flags |= CspProviderFlags.UseMachineKeyStore;
            cp.KeyContainerName = containerName;

            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container.
            return new RSACryptoServiceProvider(2048, cp);
        }

        public static bool DeleteKeyFromContainer(string containerName)
        {
            // Create the CspParameters object and set the key container 
            // name used to store the RSA key pair.
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = containerName;
            cp.KeyNumber = 1;
            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container.
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024, cp);

            // Delete the key entry in the container.
            rsa.PersistKeyInCsp = false;

            // Call Clear to release resources and delete the key from the container.
            rsa.Clear();
            return true;
        }


        public static bool DeleteProviderFromContainer(string containerName, bool machineStore = false)
        {
            // Create the CspParameters object and set the key container 
            // name used to store the RSA key pair.
            CspParameters cp = new CspParameters();
            if (machineStore) cp.Flags |= CspProviderFlags.UseMachineKeyStore;
            cp.KeyContainerName = containerName;
            //cp.KeyNumber = (int)KeyNumber.Signature;

            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container.
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024, cp);

            // Delete the key entry in the container.
            rsa.PersistKeyInCsp = false;

            // Call Clear to release resources and delete the key from the container.
            rsa.Clear();
            return true;
        }
    
    
    }
}