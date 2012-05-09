using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;
using TheChallenge.Helpers.Encryption;

namespace Test
{
    class CryptoTest : nspec
    {
        void describe_Crypto()
        {
            context["encrypt data"] = () =>
            {
                it["should encrypt and decrypt same data"] = () =>
                    Crypto.DecryptStringAES(Crypto.EncryptStringAES("hello world")).should_be("hello world");
            };
        }
        
    }
}
