using System;

// Start with default bitmap w/ 16pixel correspond to 48 color bytes. Each byte will hide 2 bits of the given input
// the maximum input is 96bits = 12 bytes. Input is taken from the command line argument
class Steganography
{
    public static void Main()
    {
        string[] args = Environment.GetCommandLineArgs();
        //Console.WriteLine("args : {0}", string.Join(", ", args));
        string inputStr = args[1];
        byte[] bmpBytes = new byte[] {
        0x42,0x4D,0x4C,0x00,0x00,0x00,0x00,0x00,
        0x00,0x00,0x1A,0x00,0x00,0x00,0x0C,0x00,
        0x00,0x00,0x04,0x00,0x04,0x00,0x01,0x00,
        0x18,0x00,0x00,0x00,0xFF,0xFF,0xFF,0xFF,
        0x00,0x00,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
        0xFF,0x00,0x00,0x00,0xFF,0xFF,0xFF,0x00,
        0x00,0x00,0xFF,0x00,0x00,0xFF,0xFF,0xFF,
        0xFF,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0xFF,
        0xFF,0x00,0x00,0x00,0xFF,0xFF,0xFF,0x00,
        0x00,0x00
        };
        // get input into an array of bits
        inputStr = inputStr.Replace(" ", string.Empty);
        byte[] inputBytes = StringToByteArray(inputStr);
        string[] inputBitStrArr = new string[inputBytes.Length];
        for(int i = 0; i < inputBytes.Length; i++) {
            inputBitStrArr[i] = Convert.ToString(inputBytes[i],2).PadLeft(8,'0');
        }
        string inputBitStr =  string.Join(string.Empty, inputBitStrArr); //inputBytes.Select( b => Convert.ToString(b, 2).PadLeft(8, '0')));
        //Console.WriteLine(inputBitStr);
        //string inputBits = Convert.ToString(Convert.ToByte(inputStr,16),2);
        int startImgIndex = 26; // Header bytes from 0-25
        int startInputIndex = 0;
        for (int i = startImgIndex; i < bmpBytes.Length; i++) {
            if (startInputIndex >= inputBitStr.Length) {
                break;
            }
            // get 2bits of the input at a time and XOR it with the current bmpBytes 
            byte input = Convert.ToByte(inputBitStr.Substring(startInputIndex, 2), 2);
            byte output = Convert.ToByte(bmpBytes[i] ^ input);
            //Console.WriteLine("XORing {0} x {1} = {2}", input, bmpBytes[i], output);
            bmpBytes[i] = output;
            startInputIndex += 2;
        }
        Console.WriteLine(ByteArrayToString(bmpBytes));
    }

    public static string ByteArrayToString(byte[] ba)
    {
      return BitConverter.ToString(ba).Replace("-"," ");
    }

    public static byte[] StringToByteArray(String hex)
    {
      int NumberChars = hex.Length;
      byte[] bytes = new byte[NumberChars / 2];
      for (int i = 0; i < NumberChars; i += 2)
        bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
      return bytes;
    }
}
