// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.HexConvert
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  public class HexConvert
  {
    private static char[] hexDigits = new char[16]
    {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F'
    };

    public static byte[] ToBytesNoSwap(string hex)
    {
      byte num = 0;
      byte[] bytesNoSwap = (byte[]) null;
      if (0 < hex.Length)
      {
        string upper = hex.ToUpper();
        int length = upper.Length / 2;
        if (upper.Length % 2 == 1)
        {
          ++length;
          upper.Insert(0, "0");
        }
        bytesNoSwap = new byte[length];
        int index = 0;
        for (int startIndex = 0; startIndex < upper.Length; ++startIndex)
        {
          byte byteVal1 = 0;
          byte byteVal2 = 0;
          if (PviMarshal.HexCharToByte(System.Convert.ToChar(upper.Substring(startIndex, 1)), ref byteVal1))
          {
            char hexChar = '0';
            if (upper.Length > startIndex + 1)
              hexChar = System.Convert.ToChar(upper.Substring(startIndex + 1, 1));
            PviMarshal.HexCharToByte(hexChar, ref byteVal2);
            bytesNoSwap[index] = (byte) ((uint) byteVal1 * 16U + (uint) byteVal2);
            ++index;
            ++startIndex;
          }
          else
            num = (byte) 0;
        }
      }
      return bytesNoSwap;
    }

    public static byte[] ToBytes(string hex)
    {
      string upper = hex.ToUpper();
      int length1 = upper.Length / 2;
      if (upper.Length % 2 == 1)
        ++length1;
      byte[] numArray = new byte[length1];
      int length2 = 0;
      for (int index = 0; index < upper.Length; ++index)
      {
        byte byteVal1 = 0;
        byte byteVal2 = 0;
        if (PviMarshal.HexCharToByte(System.Convert.ToChar(upper.Substring(upper.Length - 1 - index, 1)), ref byteVal1))
        {
          char hexChar = '0';
          if (upper.Length - 2 - index >= 0)
            hexChar = System.Convert.ToChar(upper.Substring(upper.Length - 2 - index, 1));
          while (!PviMarshal.HexCharToByte(hexChar, ref byteVal2))
          {
            ++index;
            byteVal2 = (byte) 0;
            if (upper.Length - 2 - index >= 0)
              hexChar = System.Convert.ToChar(upper.Substring(upper.Length - 2 - index, 1));
          }
          numArray[length2] = (byte) ((uint) byteVal1 + (uint) byteVal2 * 16U);
          ++length2;
          ++index;
        }
        else
          byteVal1 = (byte) 0;
      }
      byte[] bytes = new byte[length2];
      int index1;
      for (index1 = 0; index1 < bytes.Length / 2; ++index1)
      {
        byte num = numArray[index1];
        bytes[index1] = numArray[bytes.Length - index1 - 1];
        bytes[bytes.Length - index1 - 1] = num;
      }
      if (bytes.Length % 2 == 1)
        bytes[index1] = numArray[index1];
      return bytes;
    }

    public static string ToHexString(byte[] bytes)
    {
      char[] chArray = new char[bytes.Length * 2];
      for (int index = 0; index < bytes.Length; ++index)
      {
        int num = (int) bytes[index];
        chArray[index * 2] = HexConvert.hexDigits[num >> 4];
        chArray[index * 2 + 1] = HexConvert.hexDigits[num & 15];
      }
      return new string(chArray);
    }
  }
}
