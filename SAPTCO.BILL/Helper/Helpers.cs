using System;

namespace SAPTCO.BILL.Helper
{
    public static class Helpers
    {
        public static string GetTLV(int tag, string value)
        {
            string tagHexa = string.Concat("0", tag.ToString("X"));
            string lengthHexa = string.Concat(value.Length <= 15 ? "0" : "", value.Length.ToString("X"));
            string valueHexa = HexadecimalEncoding.ToHexString(value);

            return string.Concat(tagHexa, lengthHexa, valueHexa);
        }

        public static string FromHexaToBase64(string inputHex)
        {
            inputHex = inputHex.Replace("-", "");

            byte[] resultantArray = new byte[inputHex.Length / 2];

            for (int i = 0; i < resultantArray.Length; i++)
            {
                resultantArray[i] = Convert.ToByte(inputHex.Substring(i * 2, 2), 16);
            }

            string base64encoded = Convert.ToBase64String(resultantArray);

            return base64encoded;
        }
    }
}